using CsvMaker.Models;

namespace Domain.Entities;

public class ProjectTaskCsvModel : CsvModel
{
    #region Properties
    public string Name { get; set; } = "";
    public int BestCase { get; set; }
    public int WorstCase { get; set; }
    public double Expected { get; set; }
    #endregion
}
