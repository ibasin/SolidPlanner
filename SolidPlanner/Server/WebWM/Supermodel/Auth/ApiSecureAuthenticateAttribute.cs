using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Encryptor;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.WebMonk.Auth;

namespace WebWM.Supermodel.Auth;

public class ApiSecureAuthenticateAttribute: SupermodelAuthenticateAttributeBase
{
    #region Overrides
    protected override Task<List<Claim>> AuthenticateBasicAndGetClaimsAsync(string username, string password)
    {
        throw new InvalidOperationException(); 
    }
    protected override async Task<List<Claim>> AuthenticateEncryptedAndGetClaimsAsync(string[] args)
    {
        if (args.Length != 4) return new List<Claim>();

        await using (new UnitOfWork<DataContext>(ReadOnly.Yes))
        {
            var utcNow = DateTime.UtcNow;

            var username = args[0];
            var password = args[1];
            var secretTokenHash = args[2];
            var secretTokenHashSalt = args[3];

            var repo = LinqRepoFactory.Create<SolidPlannerUser>();
            var lowerCaseUsername =  username.ToLower();
            var user = repo.Items.SingleOrDefault(u => u.Username.ToLower() == lowerCaseUsername);

            var secretTokenValid = false;

            if (user != null && !string.IsNullOrEmpty(user.Username))
            {
                var dateTimeSalt = HashAgent.Generate5MinTimeStampSalt(utcNow.AddMinutes(-5));
                if (HashAgent.HashPasswordSHA256(SecretToken + dateTimeSalt, secretTokenHashSalt) == secretTokenHash) secretTokenValid = true;

                dateTimeSalt = HashAgent.Generate5MinTimeStampSalt(utcNow);
                if (HashAgent.HashPasswordSHA256(SecretToken + dateTimeSalt, secretTokenHashSalt) == secretTokenHash) secretTokenValid = true;

                dateTimeSalt = HashAgent.Generate5MinTimeStampSalt(utcNow.AddMinutes(5));
                if (HashAgent.HashPasswordSHA256(SecretToken + dateTimeSalt, secretTokenHashSalt) == secretTokenHash) secretTokenValid = true;
            }

            if (user != null && user.PasswordEquals(password) && secretTokenValid)
            {
                var claims = AuthClaimsHelper.CreateNewClaimsListWithIdAndLabel(user.Id, $"{user.FirstName} {user.LastName}");
                    
                //Add claims for the specific permissions following the example below
                //if (user.Admin) claims.Add(new Claim(ClaimTypes.Role, SolidPlannerUser.AdminRole, ClaimValueTypes.String));

                return claims;
            }
            else
            {
                return new List<Claim>();
            }
        }
    }
    #endregion

    #region Properties
    protected override byte[] EncryptionKey => Key;
    protected override string AuthHeaderName => HeaderName;
    #endregion

    #region Shared Constants
    public static readonly byte[] Key = { 0x38, 0xB3, 0x63, 0x71, 0x3C, 0x2B, 0x82, 0x7E, 0xB1, 0x6E, 0xB7, 0x7A, 0x74, 0x27, 0x3D, 0x34 };
    public static readonly string HeaderName = "X-SolidPlanner-Authorization";
    // ReSharper disable StringLiteralTypo
    public static readonly string SecretToken = "pBkBuzj3KSbq97wdE0XYjlsM7LKoJr+l8K69LDPGO56GfeihQFsiE0+pc55o3YcvP4z46IYlPwyizVc/O351WQ";
    // ReSharper restore StringLiteralTypo
    #endregion
}