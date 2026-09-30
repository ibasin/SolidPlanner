using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Supermodel.Persistence
{
    public static class DataSeeder
    {
        #region Methods
        public static Task SeedDataAsync()
        {
            var users = new[] 
            { 
                new SolidPlannerUser { FirstName = "Sample", LastName = "Account", Username="supermodel@noblis.org", Password="1234" },
            };
            foreach (var user in users) user.Add();

            return Task.CompletedTask;
        }
        #endregion
    }
}
