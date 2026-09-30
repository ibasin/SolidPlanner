using WebMonk.HttpRequestHandlers;
using WebMonk.Rendering.Views;
using WebWM.Mvc.ProjectPage;

namespace WebWM.Mvc;

public class DefaultPathRedirectorHttpRequestHandler : DefaultPathRedirectorHttpRequestHandlerBase
{
    //public DefaultPathRedirectorHttpRequestHandler() : base("/Auth/Login") { } //alternative, non-strongly-typed version 
    
    #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    public DefaultPathRedirectorHttpRequestHandler() : base(Render.Helper.UrlForMvcAction<ProjectMvcController>(x => x.GetListAsync())) { }
    #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
}