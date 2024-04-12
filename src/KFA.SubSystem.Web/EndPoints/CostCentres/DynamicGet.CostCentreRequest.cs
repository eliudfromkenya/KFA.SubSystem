using KFA.SubSystem.UseCases.ModelCommandsAndQueries;

namespace KFA.SubSystem.Web.EndPoints.CostCentres;

public class DynamicGetCostCentreRequest
{
    public const string Route = "/cost_centres/dynamically";

    public ListParam? ListParam { get; init; } = null;
}

