using System.Linq;
using Domain.Supermodel.Persistence;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.Presentation.WebMonk.Bootstrap4.Views;
using Supermodel.Presentation.WebMonk.Views.Interfaces;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebWM.Mvc.ProjectPage.ProjectTaskPage;

namespace WebWM.Mvc.ProjectPage;

public class ProjectMvcView : EnhancedCRUDMvcView<ProjectMvcModel, Bs4.DummySearchMvcModel, DataContext>
{
    public override ListMode ListMode => ListMode.MultiColumn;

    protected override IGenerateHtml? RenderChildren(ProjectMvcModel model)
    {
        return new Bs4.CRUDMultiColumnChildrenEditableList(model.ProjectTasks.OrderBy(x => x.SequenceNumber.IntValue), typeof(DataContext), typeof(ProjectTaskMvcController), model.Id, "Tasks");
    }
}
