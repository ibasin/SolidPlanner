using Supermodel.Presentation.WebMonk.Bootstrap4.Views;
using Domain.Supermodel.Persistence;
using Supermodel.Presentation.WebMonk.Views.Interfaces;

namespace WebWM.Mvc.ProjectPage.ProjectUploadPage;

public class ProjectUploadMvView : CRUDMvcView<ProjectUploadMvcModel, DataContext>
{
    #region Overrides
    public override ListMode ListMode => ListMode.NoList;
    #endregion
}
