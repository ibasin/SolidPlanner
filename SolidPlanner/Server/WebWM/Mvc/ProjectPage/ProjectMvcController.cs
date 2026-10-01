using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.Presentation.WebMonk.Controllers.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMonk.Results;

namespace WebWM.Mvc.ProjectPage;

public class ProjectMvcController : EnhancedCRUDMvcController<Project, ProjectMvcModel, Bs4.DummySearchMvcModel, ProjectMvcView, DataContext>
{
    #region Action Methods
    public async Task<ActionResult> GetCsvAsync(long id)
    {
        await using (new UnitOfWork<DataContext>(ReadOnly.Yes))
        {
            var project = await RepoFactory.Create<Project>().GetByIdAsync(id);
            var csvFile = await project.SaveToCsvAsync();
            var now = DateTime.Now;
            return new BinaryFileResult(Encoding.UTF8.GetBytes(csvFile), $"{MakeSafeFileName(project.Name)}_{now.Month}-{now.Day}-{now.Year}.csv", "text/csv");
        }
    }
    #endregion

    #region Helper Methods
    private static string MakeSafeFileName(string input, char replacement = '_')
    {
        if (string.IsNullOrWhiteSpace(input)) throw new Exception();
            
        var invalidChars = Path.GetInvalidFileNameChars();
        return new string(input.Select(ch => invalidChars.Contains(ch) ? replacement : ch).ToArray());
    }
    #endregion
}
