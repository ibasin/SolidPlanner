using System;
using System.IO;
using Supermodel.Client.CodeGen;

namespace ModelGeneratorWM;

class Program
{
    static void Main()
    {
        var modelGenerator = new ModelGen([typeof(WebWM.Mvc.SolidPlannerUserUpdatePasswordPage.SolidPlannerUserUpdatePasswordMvcController).Assembly]);
        var sb = modelGenerator.GenerateModels();
        var code = sb.ToString();

        File.WriteAllText(@"..\..\..\..\..\Client\BatchApiClientWM\Supermodel\ModelsForRuntime\Supermodel.Mobile.ModelsForRuntime.cs", code);

        Console.WriteLine("All done!");
    }
}