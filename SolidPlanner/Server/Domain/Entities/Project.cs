using Supermodel.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain.Entities;

public class Project : Entity
{
    #region Embedded Types
    public record struct Estimate(int BestCase, int WorstCase, int Expected);
    #endregion

    #region Oevrrides
    protected override void DeleteInternal()
    {
        foreach (var projectTask in ProjectTasks) projectTask.Delete();
        base.DeleteInternal();
    }
    #endregion

    #region Methods
    public Estimate CalcEstimate(int? start = null, int? end = null)
    {
        var subproject = ProjectTasks.Where(x => !x.Ignore && (start == null || x.SequenceNumber >= start) && (end == null || x.SequenceNumber <= end)).ToArray();

        var sd = (int)Math.Round((subproject.Sum(x => x.BestCase) + subproject.Sum(x => x.WorstCase)) / 6.0);
        
        var expected = subproject.Sum(x => x.Expected);
        var bestCase = expected - sd;
        var worstCase = expected + sd;

        return new Estimate(bestCase, worstCase, expected);
    }
    #endregion

    #region Properties
    [Required] public string Name { get; set; } = "";
    public virtual List<ProjectTask> ProjectTasks { get; set; } = new();
    #endregion
}