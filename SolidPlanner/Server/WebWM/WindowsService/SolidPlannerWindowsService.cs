using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using WebMonk;
using WebWM.Mvc.Layouts;

namespace WebWM.WindowsService;

public class SolidPlannerWindowsService : BackgroundService
{
    #region Constructors
    public SolidPlannerWindowsService(ILogger<SolidPlannerWindowsService> logger)
    {
        Logger = logger;
    }
    #endregion

    #region Overrides
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //set working path to match the location of the executable
        var strExeFilePath = Assembly.GetExecutingAssembly().Location;
        var strWorkPath = Path.GetDirectoryName(strExeFilePath);
        Directory.SetCurrentDirectory(strWorkPath!);

        try
        {
            var webServer = new WebServer(Program.HttpPort, $"http://localhost:{Program.HttpPort}")
            {
                LoginUrl = "/Auth/Login",
                DefaultLayout = new MasterMvcLayout()
            };

            await webServer.RunAsync(stoppingToken, null, false, false);
        }
        catch (OperationCanceledException)
        {
            // When the stopping token is canceled, for example, a call made from services.msc,
            // we shouldn't exit with a non-zero exit code. In other words, this is expected...
        }
        catch (Exception ex)
        {
            // Terminates this process and returns an exit code to the operating system.
            // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
            // performs one of two scenarios:
            // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
            // 2. When set to "StopHost": will cleanly stop the host, and log errors.
            //
            // In order for the Windows Service Management system to leverage configured
            // recovery options, we need to terminate the process with a non-zero exit code.

            Logger?.LogError(ex, ex.Message);
            Environment.Exit(1);
        }
    }
    #endregion

    #region Properties
    public static ILogger<SolidPlannerWindowsService>? Logger { get; private set; }
    #endregion
}