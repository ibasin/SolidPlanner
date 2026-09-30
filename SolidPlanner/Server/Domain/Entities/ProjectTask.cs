using Domain.Supermodel.Persistence;
using Microsoft.EntityFrameworkCore;
using Supermodel.DataAnnotations.Validations;
using Supermodel.Persistence.Entities;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;

[Index(nameof(ParentProjectId), nameof(Name), IsUnique = true)]
[Index(nameof(ParentProjectId), nameof(SequenceNumber), IsUnique = true)]
public class ProjectTask : Entity
{
    #region Validation
    public override async Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
    {
        var vrl = await base.ValidateAsync(validationContext);

        await using (new UnitOfWorkIfNoAmbientContext<DataContext>(MustBeWritable.No))
        {
            if (BestCase < 0) vrl.AddValidationResult(this, "Best Case must be greater than or equal to 0", x => x.BestCase);
            if (WorstCase < 0) vrl.AddValidationResult(this, "Worst Case must be greater than or equal to 0", x => x.WorstCase);
            if (BestCase > WorstCase) vrl.AddValidationResult(this, "Best Case must be less than or equal to Worst Case", x => x.BestCase);

            var repo = LinqRepoFactory.Create<ProjectTask>();
            if (repo.Items.Any(x => x.ParentProjectId == ParentProjectId && x.SequenceNumber == SequenceNumber && x.Id != Id)) vrl.AddValidationResult(this, "Sequence Number must be unique", x => x.SequenceNumber);
            if (repo.Items.Any(x => x.ParentProjectId == ParentProjectId && x.Name == Name && x.Id != Id)) vrl.AddValidationResult(this, "Task Name must be unique", x => x.Name);
        }

        return vrl;
    }
    #endregion

    #region Methods
    public ProjectTask? GetNextTask()
    {
        return ParentProject!.ProjectTasks.OrderBy(x => x.SequenceNumber).FirstOrDefault(x => x.SequenceNumber > SequenceNumber);
    }
    public ProjectTask? GetPreviousTask()
    {
        return ParentProject!.ProjectTasks.OrderByDescending(x => x.SequenceNumber).FirstOrDefault(x => x.SequenceNumber < SequenceNumber);
    }
    #endregion

    #region Properties
    [Required] public virtual Project? ParentProject { get; set; };
    public long ParentProjectId { get; set; }

    [Required] public uint SequenceNumber { get; set; }

    [Required, MaxLength(200)] public string Name { get; set; } = "";
    public bool Ignore { get; set; }
    [Required] public int BestCase { get; set; }
    [Required] public int WorstCase { get; set; }
    [NotMapped] public int Expected => (int)Math.Round((BestCase + 3*(BestCase + WorstCase)/2.0 + 2*WorstCase) / 6.0);
    #endregion
}