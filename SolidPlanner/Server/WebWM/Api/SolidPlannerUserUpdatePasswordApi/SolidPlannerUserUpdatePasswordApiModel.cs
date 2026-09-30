using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Domain.Entities;
using Supermodel.DataAnnotations.Validations;
using Supermodel.Presentation.WebMonk.Models.Api;
using Supermodel.ReflectionMapper;

namespace WebWM.Api.SolidPlannerUserUpdatePasswordApi;

public class SolidPlannerUserUpdatePasswordApiModel : ApiModelForEntity<SolidPlannerUser>
{
    #region Overrides
    public override Task<T> MapToCustomAsync<T>(T other)
    {
            var user = CastToEntity(other);
            user.Password = NewPassword;
            return base.MapToCustomAsync(other);
        }
    public override Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
    {
            //we don't want to the domain-level validation to be called here (which called by the base method) because we don't fill that object completely
            var vr = new ValidationResultList();
            return Task.FromResult(vr);
        }
    #endregion
        
    #region Properties
    [Required, NotRMapped] public string OldPassword { get; set; } = "";
    [Required, NotRMapped] public string NewPassword { get; set; } = "";
    #endregion
}