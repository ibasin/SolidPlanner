using System;
using Supermodel.Client.Backend.Auth;
using Supermodel.Encryptor;

namespace BatchApiClientWM.Supermodel.Auth;

public class SolidPlannerSecureAuthHeaderGenerator : BasicAuthHeaderGenerator
{
    #region Constructors
    public SolidPlannerSecureAuthHeaderGenerator(string username, string password) : base(username, password){}
    #endregion

    #region Overrides
    public override AuthHeader CreateAuthHeader()
    {
            var dateTimeSalt = HashAgent.Generate5MinTimeStampSalt(DateTime.UtcNow);
            var secretTokenHashSalt = HashAgent.GenerateGuidSalt();
            var secretTokenHash = HashAgent.HashPasswordSHA256(SecretToken + dateTimeSalt, secretTokenHashSalt);
            var authHeader = HttpAuthAgent.CreateSMCustomEncryptedAuthHeader(Key, Username, Password, secretTokenHash, secretTokenHashSalt);
            authHeader.HeaderName = HeaderName;
            return authHeader;
        }
    #endregion

    #region Shared Constants
    public static readonly byte[] Key = { 0x38, 0xB3, 0x63, 0x71, 0x3C, 0x2B, 0x82, 0x7E, 0xB1, 0x6E, 0xB7, 0x7A, 0x74, 0x27, 0x3D, 0x34 };
    public static readonly string HeaderName = "X-SolidPlanner-Authorization";
    // ReSharper disable StringLiteralTypo
    public static readonly string SecretToken = "pBkBuzj3KSbq97wdE0XYjlsM7LKoJr+l8K69LDPGO56GfeihQFsiE0+pc55o3YcvP4z46IYlPwyizVc/O351WQ";
    // ReSharper restore StringLiteralTypo
    #endregion
}