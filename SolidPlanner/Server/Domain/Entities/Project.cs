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

        var projectSigma = Math.Sqrt(subproject.Sum(x => Math.Pow(x.CalcSigma(), 2)));
        
        var expected = subproject.Sum(x => x.Expected);
        var bestCase = expected - projectSigma;
        var worstCase = expected + projectSigma;

        return new Estimate(bestCase, worstCase, expected);
    }
    #endregion

    #region Properties
    [Required, MaxLength(200)] public string Name { get; set; } = "";
    public virtual List<ProjectTask> ProjectTasks { get; set; } = new();
    #endregion
}