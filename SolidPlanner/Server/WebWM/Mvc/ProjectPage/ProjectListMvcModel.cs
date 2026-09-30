using Domain.Entities;
using Supermodel.DataAnnotations.Attributes;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.ReflectionMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebWM.Mvc.ProjectPage.ProjectTaskPage;

namespace WebWM.Mvc.ProjectPage;

public class ProjectMvcModel : Bs4.MvcModelForEntity<Project>
{
    #region Overrides
    public override string Label => Name.Value;

    public override Task MapFromCustomAsync<T>(T other)
    {
        var project = CastToEntity(other);

        var estimate = project.CalcEstimate();
        BestCase = estimate.BestCase;
        WorstCase = estimate.WorstCase;
        Expected = estimate.Expected;

        return base.MapFromCustomAsync(other);
    }
    #endregion

    #region Properties
    [Required, ListColumn] public Bs4.TextBoxMvcModel Name { get; set; } = new();

    [NotRMapped, ListColumn, DisplayOnly] public int BestCase { get; set; }
    [NotRMapped, ListColumn, DisplayOnly] public int WorstCase { get; set; }
    [NotRMapped, ListColumn, DisplayOnly] public int Expected { get; set; }

    [NotRMappedTo] public List<ProjectTaskMvcModel> ProjectTasks { get; set; } = new();
    #endregion
}
