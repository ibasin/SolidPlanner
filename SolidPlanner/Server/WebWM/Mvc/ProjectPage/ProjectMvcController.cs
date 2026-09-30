using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.Presentation.WebMonk.Controllers.Mvc;

namespace WebWM.Mvc.ProjectPage;

public class ProjectMvcController : EnhancedCRUDMvcController<Project, ProjectMvcModel, Bs4.DummySearchMvcModel, ProjectMvcView, DataContext>   
{

}
