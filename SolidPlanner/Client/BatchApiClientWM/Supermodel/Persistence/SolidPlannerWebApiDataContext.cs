using Supermodel.Client.Backend.DataContext.WebApi;

namespace BatchApiClientWM.Supermodel.Persistence;

public class SolidPlannerWebApiDataContext: WebApiDataContext
{
    #region Overrides
    //public override string BaseUrl => "http://192.168.210.1:46482/api/"; //this one is for WM's specific IP
    public override string BaseUrl => "http://localhost:46482/api/"; //this one is for WM through localhost

    // set timeout to 10 min, so we can debug if starting server and client simultaneously 
    //protected override HttpClient CreateHttpClient()
    //{
    //    var httpClient = base.CreateHttpClient();
    //    httpClient.Timeout = new TimeSpan(0, 10, 0);
    //    return httpClient;
    //}
    #endregion
}