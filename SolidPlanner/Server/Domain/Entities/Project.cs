using Domain.Supermodel.Persistence;
using Microsoft.EntityFrameworkCore;
using Supermodel.DataAnnotations.Validations;
using Supermodel.Persistence.Entities;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CsvMaker.CsvString;
using CsvMaker.Extensions;
using Supermodel.ReflectionMapper;

namespace Domain.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Project : Entity
{
    #region Embedded Types
    public record struct Estimate(double BestCase, double WorstCase, double Expected);
    #endregion

    #region Validaton
    public override async Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
    {
        var vrl = await base.ValidateAsync(validationContext);

        await using (new UnitOfWorkIfNoAmbientContext<DataContext>(MustBeWritable.No))
        {
            var repo = LinqRepoFactory.Create<Project>();
            if (repo.Items.Any(x => x.Name == Name && x.Id != Id)) vrl.AddValidationResult(this, $"Project Name '{Name}' already used", x => x.Name);
        }

        return vrl;
    }
    #endregion


    #region Oevrrides
    protected override void DeleteInternal()
    {
        foreach (var projectTask in ProjectTasks) projectTask.Delete();
        base.DeleteInternal();
    }
    #endregion

    #region Methods
    public Estimate CalcEstimate(int? start = null, int? end = null)
    {
        var subproject = ProjectTasks.Where(x => !x.Ignore && (start == null || x.SequenceNumber >= start) && (end == null || x.SequenceNumber <= end)).ToArray();

        var projectSigma = Math.Sqrt(subproject.Sum(x => Math.Pow(x.CalcTaskSigma(), 2)));
        
        var expected = subproject.Sum(x => x.Expected);
        var bestCase = expected - projectSigma;
        var worstCase = expected + projectSigma;

        return new Estimate(bestCase, worstCase, expected);
    }
    public async Task<string> SaveToCsvAsync()
    {
        var projectTasksCsvModels = new List<ProjectTaskCsvModel>();
        await projectTasksCsvModels.MapFromAsync(ProjectTasks.ToList());
        var csv = projectTasksCsvModels.ToCsv();
        
        //Append totals
        var estimate = CalcEstimate();
        csv.Append($"\nBest Case:,{estimate.BestCase:F1}\nWorstCase:,{estimate.WorstCase:F1}\nExpected:,{estimate.Expected:F1}\n");

        return csv.ToString();
    }
    public async Task LoadFromCsvAsync(string csvFile)
    {
        csvFile = RemoveSummaryAndBlankLinesFromCsv(csvFile);

        var projectTasksCsvModels = new List<ProjectTaskCsvModel>();
        projectTasksCsvModels.ReadCsv(new CsvStringReader(csvFile));
        
        foreach (var projectTask in ProjectTasks) projectTask.Delete();
        ProjectTasks.Clear();

        await projectTasksCsvModels.MapToAsync(ProjectTasks);
    }
    private string RemoveSummaryAndBlankLinesFromCsv(string csv)
    {
        csv = csv.Replace("\r", "");
        var lines = csv.Split('\n');
        lines = lines.Where(x => x.Trim().Length > 0).ToArray();
        var index = Array.FindIndex(lines, x => x.StartsWith("Best Case:"));
        if (index < 0) return csv;

        return string.Join('\n', lines.Take(index));
    }
    #endregion

    #region Properties
    [Required, MaxLength(200)] public string Name { get; set; } = "";
    public virtual List<ProjectTask> ProjectTasks { get; set; } = new();
    #endregion
}