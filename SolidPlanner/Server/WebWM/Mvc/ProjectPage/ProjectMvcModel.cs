using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Domain.Entities;
using Supermodel.DataAnnotations.Attributes;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.ReflectionMapper;

namespace WebWM.Mvc.ProjectPage;

public class ProjectMvcModel : Bs4.MvcModelForEntity<Project>
{
    #region Overrides
    public override string Label => Name.Value;

    public override Task MapFromCustomAsync<T>(T other)
    {
        var project = CastToEntity(other);

        var estimate = project.CalcEstimate();
        BestCase.IntValue = estimate.BestCase;
        WorstCase.IntValue = estimate.WorstCase;
        Expected.IntValue = estimate.Expected;

        return base.MapFromCustomAsync(other);
    }
    #endregion

    #region Properties
    [Required, ListColumn] public Bs4.TextBoxMvcModel Name { get; } = new();
    [NotRMapped, ListColumn, DisplayOnly] public Bs4.TextBoxMvcModel BestCase { get; } = new();
    [NotRMapped, ListColumn, DisplayOnly] public Bs4.TextBoxMvcModel WorstCase { get; } = new();
    [NotRMapped, ListColumn, DisplayOnly] public Bs4.TextBoxMvcModel Expected { get; } = new();
    #endregion
}
