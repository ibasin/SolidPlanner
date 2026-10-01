using System;
using Domain.Supermodel.Persistence;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.Presentation.WebMonk.Bootstrap4.Views;
using Supermodel.Presentation.WebMonk.Models.Mvc;
using Supermodel.Presentation.WebMonk.Views.Interfaces;
using System.Linq;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebWM.Supermodel.TagComponents;

namespace WebWM.Mvc.ProjectPage;

public class ProjectMvcView : EnhancedCRUDMvcView<ProjectMvcModel, Bs4.DummySearchMvcModel, DataContext>
{
    #region Render Methods Overrides
    public override IGenerateHtml RenderList(ListWithCriteria<ProjectMvcModel, Bs4.DummySearchMvcModel> models, int totalCount)
    {
        var tags = base.RenderList(models, totalCount);

        var btnGroups = tags.Where(x => x.TagType == "div" && x.Attributes.KeyExistsAndContains("class", "btn-group")).ToArray();
        if (btnGroups.Length != models.Count) throw new Exception($"Expected {models.Count} btn-groups, but found {btnGroups.Length}");
        foreach (var btnGroup in btnGroups)
        {
            var aTag = (A)btnGroup[0];
            var url = aTag.Attributes["href"] ?? throw new Exception();
            
            var newUrl = url.Replace("/project/detail/", "/project/csv/");
            var newATag = new A(new { href = newUrl, @class = "btn btn-success" })
            {
                new Span(new { @class = "oi oi-data-transfer-download" })
            };
            btnGroup.Insert(1, newATag);
        }

        return tags;
    }
    #endregion

    #region Overrides
    public override ListMode ListMode => ListMode.MultiColumn;

    protected override IGenerateHtml? RenderChildren(ProjectMvcModel model)
    {
        return new Tags()
        {
            new CRUDMultiColumnChildrenEditableListForProjectTasks(model.ProjectTasks.OrderBy(x => x.SequenceNumber.IntValue), model.Id, "Tasks")
        };
    }
    #endregion
}
