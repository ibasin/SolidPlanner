using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Domain.Supermodel.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Supermodel.Persistence.EFCore;
using Supermodel.Persistence.UnitOfWork;
using WebWM.Mvc.Layouts;
using WebWM.WindowsService;

namespace WebWM;

public class Program
{
    #region Program's starting point
    static async Task Main(params string[] args)
    {
        #region Run as a windows service if used with "/s" argument
        if (args is ["/s"])
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddWindowsService(options => { options.ServiceName = "SolidPlanner Web App"; });
            builder.Services.AddHostedService<SolidPlannerWindowsService>();

            var host = builder.Build();
            await host.RunAsync();
            return;
        }
        #endregion

        #region Migration database code (either use this or Re-seeding database code)
        //When creating a migration, uncomment the return statement below
        //Ignore the following message when running migration:
        //An error occurred while accessing the Microsoft.Extensions.Hosting services. Continuing without the application service provider. Error: The entry point exited without ever building an IHost.
        //return;

        //bool AskUserWhetherToMigrateDb()
        //{
        //    Console.Write("Do you want to migrate the database (if not sure, answer no or press enter) [yes/no]? ");
        //    return Console.ReadLine()!.Trim().ToLower() == "yes";
        //}
        //await using (new UnitOfWork<DataContext>())
        //{
        //    if (AskUserWhetherToMigrateDb())
        //    {
        //        Console.Write("Migrating the database... ");
        //        var sql = EmbeddedResource.ReadTextFileWithFileName(typeof(Program).Assembly, "MigrationScripts.script.sql");
        //        sql = sql.Replace("GO", "");
        //        await EFCoreUnitOfWorkContext.Database.ExecuteSqlAsync(FormattableStringFactory.Create(sql));
        //        Console.WriteLine("Done!");
        //        Console.WriteLine();
        //    }
        //}
        #endregion

        #region Re-seeding database code (either use this or Migration database code)
        //bool AskUserWhetherToReseedDb()
        //{
        //    Console.Write("Do you want to re-seed the database (if not sure, answer no or press enter) [yes/no]? ");
        //    return Console.ReadLine()!.Trim().ToLower() == "yes";
        //}
        await using (new UnitOfWork<DataContext>())
        {
            if (!await EFCoreUnitOfWorkContext.Database.CanConnectAsync() /*|| AskUserWhetherToReseedDb()*/)
            {
                Console.Write("Recreating the database... ");
                await EFCoreUnitOfWorkContext.Database.EnsureDeletedAsync();
                await EFCoreUnitOfWorkContext.Database.EnsureCreatedAsync();
                await UnitOfWorkContext.SeedDataAsync();
                Console.WriteLine("Done!");
                Console.WriteLine();
            }
        }
        #endregion

        #region Create Web Server and register global filters 
        var webServer = new SolidPlannerWebServer(HttpPort, $"http://localhost:{HttpPort}")
        {
            LoginUrl = "/Auth/Login",
            DefaultLayout = new MasterMvcLayout()
        };
        //webServer.GlobalFilters.Add(new ApiSecureAuthenticateAttribute());
        //webServer.GlobalFilters.Add(new ApiBasicAuthenticateAttribute());
        #endregion

        #region Start Web Server. If in Debug open the browser, otherwise just sit and listen for web requests
        await webServer.RunAsync("/");
        //if (Debugger.IsAttached)
        //{
        //    await webServer.RunAsync("/");
        //    //await webServer.RunAsync();
        //}
        //else
        //{
        //    await webServer.RunAsync();
        //}
        #endregion
    }
    #endregion

    #region Constants
    public const int HttpPort = 45244;
    #endregion
}