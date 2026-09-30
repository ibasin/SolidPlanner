using WebMonk.HttpRequestHandlers;
using WebMonk.Rendering.Views;
using WebWM.Mvc.AuthPage;

namespace WebWM.Mvc;

public class DefaultPathRedirectorHttpRequestHandler : DefaultPathRedirectorHttpRequestHandlerBase
{
    //public DefaultPathRedirectorHttpRequestHandler() : base("/Auth/Login") { } //alternative, non-strongly-typed version 
    public DefaultPathRedirectorHttpRequestHandler() : base(Render.Helper.UrlForMvcAction<AuthMvcController>(x => x.GetLogIn())) { }
}