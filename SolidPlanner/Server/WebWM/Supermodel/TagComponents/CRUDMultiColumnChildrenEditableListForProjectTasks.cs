using Domain.Supermodel.Persistence;
using Supermodel.DataAnnotations;
using Supermodel.DataAnnotations.Exceptions;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.Presentation.WebMonk.Models;
using Supermodel.Presentation.WebMonk.Models.Mvc;
using Supermodel.ReflectionMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WebWM.Mvc.ProjectPage.ProjectTaskPage;

namespace WebWM.Supermodel.TagComponents;

#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
public class CRUDMultiColumnChildrenEditableListForProjectTasks(IEnumerable<IChildMvcModelForEntity> items, long parentId, string pageTitle) : Bs4.CRUDMultiColumnChildrenEditableList(items, typeof(DataContext), typeof(ProjectTaskMvcController), parentId, pageTitle)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    #region Overrides
    protected override async Task<IViewModelForEntity> GetNewItemAsync(Type iEnumerableType, Type dataContextType)
    {
        await using ((IAsyncDisposable)ReflectionHelper.CreateGenericType(typeof(UnitOfWork<>), dataContextType, ReadOnly.Yes))
        {
            if (!iEnumerableType.IsGenericType) throw new SupermodelException("!iEnumerableType.IsGenericType");

            var mvcModelItemType = iEnumerableType.GenericTypeArguments.First();
            var newMvcModelItem = (ProjectTaskMvcModel)ReflectionHelper.CreateType(mvcModelItemType);

            //Init mvc model if it requires async initialization
            if (newMvcModelItem is IAsyncInit iAsyncInit && !iAsyncInit.AsyncInitialized) await iAsyncInit.InitAsync().ConfigureAwait(false);

            var newEntityItem = newMvcModelItem.CreateEntity();
            newMvcModelItem = await newMvcModelItem.MapFromAsync(newEntityItem).ConfigureAwait(false);

            //This is to preset Sequence Number for new items
            var maxSequenceNumber = await LinqRepoFactory.Create<ProjectTask>().Items.Where(x => x.ParentProjectId == parentId).MaxAsync(x => (uint?)x.SequenceNumber) ?? 0;
            newMvcModelItem.SequenceNumber.UIntValue = maxSequenceNumber - maxSequenceNumber % 100 + 100;

            return newMvcModelItem;
        }
    }
    #endregion
}
