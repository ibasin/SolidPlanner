using System.ComponentModel.DataAnnotations;
using Domain.Supermodel.Persistence;
using Supermodel.Persistence.Entities;

namespace Domain.Entities
{
    public class SolidPlannerUser : UserEntity<SolidPlannerUser, DataContext>
    {
        #region Properties
        [Required] public string FirstName { get; set; } = "";
        [Required] public string LastName { get; set; } = "";
        #endregion
    }
}
