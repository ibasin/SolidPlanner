using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Persistence.EFCore;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;

namespace Batch;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();

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
        bool AskUserWhetherToReseedDb()
        {
            Console.Write("Do you want to re-seed the database (if not sure, answer no or press enter) [yes/no]? ");
            return Console.ReadLine()!.Trim().ToLower() == "yes";
        }
        await using (new UnitOfWork<DataContext>())
        {
            if (!await EFCoreUnitOfWorkContext.Database.CanConnectAsync() || AskUserWhetherToReseedDb())
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

        await using (new UnitOfWork<DataContext>())
        {
            var repo = RepoFactory.Create<SolidPlannerUser>();
            var user = await repo.GetByIdOrDefaultAsync(1);
            if (user == null)
            {
                Console.WriteLine("User with id=1 does not exist!");
            }
            else
            {
                user.Password = "12345";
            }
        }
        Console.WriteLine("User with id=1's password updated to '12345'");
    }
}