using System;
using System.Threading.Tasks;
using BatchApiClientWM.Supermodel.Auth;
using BatchApiClientWM.Supermodel.Persistence;
using Supermodel.ApiClient.Models;
using Supermodel.Client.Backend.UnitOfWork;

namespace BatchApiClientWM;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
            
        //var authHeaderGenerator = new BasicAuthHeaderGenerator("supermodel@noblis.org", "1234");
        var authHeaderGenerator = new SolidPlannerSecureAuthHeaderGenerator("supermodel@noblis.org", "1234");

        await using (new UnitOfWork<SolidPlannerWebApiDataContext>())
        {
            UnitOfWorkContext.AuthHeader = authHeaderGenerator.CreateAuthHeader();
                
            var user = new SolidPlannerUserUpdatePassword
            {
                Id = 1,
                OldPassword = "1234",
                NewPassword = "12345"
            };
            user.Update(); //we do update because we need to "attach" the model, make it managed
        }
        Console.WriteLine("User with id=1's password updated to '12345'");
    }
}