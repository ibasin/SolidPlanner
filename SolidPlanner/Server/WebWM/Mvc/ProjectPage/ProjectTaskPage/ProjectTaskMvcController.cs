using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Presentation.WebMonk.Controllers.Mvc;

namespace WebWM.Mvc.ProjectPage.ProjectTaskPage;

public class ProjectTaskMvcController : InlineChildCRUDMvcController<ProjectTask, ProjectTaskMvcModel, Project, ProjectTaskMvcController, DataContext>
{
}
