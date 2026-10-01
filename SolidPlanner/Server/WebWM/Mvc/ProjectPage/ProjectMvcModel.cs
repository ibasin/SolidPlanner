using Domain.Entities;
using Supermodel.DataAnnotations.Attributes;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.ReflectionMapper;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using WebWM.Mvc.ProjectPage.ProjectTaskPage;

namespace WebWM.Mvc.ProjectPage;

public class ProjectMvcModel : Bs4.MvcModelForEntity<Project>
{
    #region Overrides
    public override string Label => Name.Value;

    public override async Task MapFromCustomAsync<T>(T other)
    {
        var project = CastToEntity(other);

        var estimate = project.CalcEstimate();
        BestCase.DoubleValue = estimate.BestCase;
        WorstCase.DoubleValue = estimate.WorstCase;
        Expected.DoubleValue = estimate.Expected;

        await base.MapFromCustomAsync(other);
    }
    public override async Task<T> MapToCustomAsync<T>(T other)
    {
        var project = CastToEntity(other);

        if (Id == 0 && !ProjectTasksCsvFile.IsEmpty)
        {
            var csvFile = Encoding.UTF8.GetString(ProjectTasksCsvFile.BinaryContent!);
            await project.LoadFromCsvAsync(csvFile);
        }

        return await base.MapToCustomAsync(other);
    }
    #endregion

    #region Properties
    [Required, ListColumn] public Bs4.TextBoxMvcModel Name { get; set; } = new();

    [NotRMapped, ListColumn, DisplayOnly] public Bs4.TextBoxMvcModel BestCase { get; set; } = new() { DisplayNumericFormat = "F1" };
    [NotRMapped, ListColumn, DisplayOnly] public Bs4.TextBoxMvcModel WorstCase { get; set; } = new() { DisplayNumericFormat = "F1" };
    [NotRMapped, ListColumn, DisplayOnly] public Bs4.TextBoxMvcModel Expected { get; set; } = new() { DisplayNumericFormat = "F1" };

    [NotRMapped, DisplayName("Project Tasks (.csv file)"), HtmlAttr("data-csv", "true")] public Bs4.BinaryFileMvcModel ProjectTasksCsvFile { get; set; } = new();

    [NotRMappedTo] public List<ProjectTaskMvcModel> ProjectTasks { get; set; } = new();
    #endregion
}
