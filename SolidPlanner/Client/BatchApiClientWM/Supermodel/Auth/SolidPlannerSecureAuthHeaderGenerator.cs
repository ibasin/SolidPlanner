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
    public static readonly byte[] Key = { 0x4E, 0xD0, 0xD2, 0x77, 0x51, 0xE4, 0xD9, 0x62, 0x45, 0x9D, 0xDB, 0x04, 0x55, 0x6E, 0x9B, 0x91 };
    public static readonly string HeaderName = "X-SolidPlanner-Authorization";
    // ReSharper disable StringLiteralTypo
    public static readonly string SecretToken = "SG4oXo/iYnlJ5FLtKh7zXMydry+flLxVNzzsEG+4Sxa+1um6LJpB8FN1os4W7mGwc8aQJIyIbhBs9wkY+CFHNg";
    // ReSharper restore StringLiteralTypo
    #endregion
}