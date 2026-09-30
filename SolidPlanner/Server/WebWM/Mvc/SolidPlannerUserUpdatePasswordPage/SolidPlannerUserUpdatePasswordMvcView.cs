using Domain.Supermodel.Persistence;
using Supermodel.Presentation.WebMonk.Bootstrap4.Views;
using Supermodel.Presentation.WebMonk.Views.Interfaces;

namespace WebWM.Mvc.SolidPlannerUserUpdatePasswordPage;

public class SolidPlannerUserUpdatePasswordMvcView : CRUDMvcView<SolidPlannerUserUpdatePasswordMvcModel, DataContext>
{
    public override ListMode ListMode { get; } = ListMode.NoList;

    protected override bool ShowDefaultEditPageTitle => false;
    protected override string? EditPageTitle { get; } = "Update Password";
}