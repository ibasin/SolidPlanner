using Domain.Entities;
using Supermodel.DataAnnotations.Attributes;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace WebWM.Mvc.ProjectPage.ProjectTaskPage;

public class ProjectTaskMvcModel : Bs4.ChildMvcModelForEntity<ProjectTask, Project>
{
    #region Overrides
    public override string Label => Name.Value;

    public override Project? GetParentEntity(ProjectTask entity)
    {
        return entity.ParentProject;
    }
    public override void SetParentEntity(ProjectTask entity, Project? parent)
    {
        entity.ParentProject = parent!;
    }
    protected override async Task<ProjectTask> CreateTempValidationEntityAsync()
    {
        var entity = await base.CreateTempValidationEntityAsync();
        if (ParentId != null) entity.ParentProjectId = ParentId.Value;
        return entity;
    }
    #endregion

    #region Properties
    [ListColumn] public Bs4.CheckboxMvcModel Ignore { get; set; } = new();
    [Required, ListColumn(Header="Sequence")] public Bs4.TextBoxMvcModel SequenceNumber { get; set; } = new();
    [Required, ListColumn] public Bs4.TextBoxMvcModel Name { get; set; } = new();
    [Required, ListColumn] public Bs4.TextBoxMvcModel BestCase { get; set; } = new();
    [Required, ListColumn] public Bs4.TextBoxMvcModel WorstCase { get; set; } = new();
    [NotMapped, ListColumn, DisplayOnly] public Bs4.TextBoxMvcModel Expected { get; set; } = new() { DisplayNumericFormat = "F1" };

    #endregion
}
