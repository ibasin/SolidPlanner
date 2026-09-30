using Domain.Supermodel.Persistence;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.Presentation.WebMonk.Bootstrap4.Views;
using Supermodel.Presentation.WebMonk.Views.Interfaces;

namespace WebWM.Mvc.ProjectPage;

public class ProjectMvcView : EnhancedCRUDMvcView<ProjectMvcModel, Bs4.DummySearchMvcModel, DataContext>
{
    public override ListMode ListMode => ListMode.MultiColumn;
}
