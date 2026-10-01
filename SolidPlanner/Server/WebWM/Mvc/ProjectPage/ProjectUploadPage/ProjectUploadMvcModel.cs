using Domain.Entities;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace WebWM.Mvc.ProjectPage.ProjectUploadPage;

public class ProjectUploadMvcModel : Bs4.MvcModelForEntity<Project>
{
    #region Overrides
    public override string Label => "";

    //public override async Task<T> MapToCustomAsync<T>(T other)
    //{
    //    var project = CastToEntity(other);

    //    await project.LoadFromCsvAsync(Encoding.UTF8.GetString(CsvFile.BinaryContent!));
    //    project.Name = "";

    //    return base.MapToCustomAsync(other);
    //}
    public override Task MapFromCustomAsync<T>(T other)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Properties
    public Bs4.BinaryFileMvcModel CsvFile { get; set; } = new();
    #endregion
}
