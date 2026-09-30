using WebMonk.Extensions;
using WebMonk.HttpRequestHandlers.Controllers;
using WebMonk.Results;

namespace WebWM.Mvc.HomePage;

public class HomeMvcController: MvcController
{
    public ActionResult GetIndex()
    {
            return new HomeMvcView().RenderIndex().ToHtmlResult();
        }
}