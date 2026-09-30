using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebMonk.Exceptions;
using WebMonk;
using WebWM.WindowsService;

namespace WebWM;

public class SolidPlannerWebServer : WebServer
{
    #region Constructors
    public SolidPlannerWebServer(int httpPort, string navigationBaseUrl, Assembly[]? appAssemblies = null)
        : base(httpPort, navigationBaseUrl, appAssemblies) { }
    #endregion

    #region Overrides
    protected override Task OnInternalServerErrorAsync(Exception ex)
    {
        if (Debugger.IsAttached) return base.OnInternalServerErrorAsync(ex);

        SolidPlannerWindowsService.Logger?.LogWarning(ex, ex.Message);

        return Task.CompletedTask;
    }
    protected override Task OnUnsupportedMediaTypeAsync(Exception415UnsupportedMediaType ex)
    {
        if (Debugger.IsAttached) return base.OnUnsupportedMediaTypeAsync(ex);

        SolidPlannerWindowsService.Logger?.LogWarning(ex, ex.Message);

        return Task.CompletedTask;
    }
    #endregion
}