using System;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.Presentation.WebMonk.Controllers.Mvc;
using System.Text;
using System.Threading.Tasks;
using WebMonk.Results;

namespace WebWM.Mvc.ProjectPage;

public class ProjectMvcController : EnhancedCRUDMvcController<Project, ProjectMvcModel, Bs4.DummySearchMvcModel,
    ProjectMvcView, DataContext>
{
    #region Action Methods

    public async Task<ActionResult> DownloadCsvAsync(long id)
    {
        await using (new UnitOfWork<DataContext>(ReadOnly.Yes))
        {
            var project = await RepoFactory.Create<Project>().GetByIdAsync(id);
            var csvFile = await project.SaveToCsvAsync();
            var now = DateTime.Now;
            return new BinaryFileResult(Encoding.UTF8.GetBytes(csvFile), $"{project.Name}_{now.Month}-{now.Day}-{now.Year}.csv", "text/csv");
        }
    }
    #endregion
}
