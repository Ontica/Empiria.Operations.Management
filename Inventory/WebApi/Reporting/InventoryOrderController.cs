
using System.Web.Http;

using Empiria.Inventory.Reporting.Adapters;
using Empiria.Inventory.Reporting.UseCases;
using Empiria.WebApi;


namespace Empiria.Inventory.Reporting.WebApi {

  public class ReportingController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v8/order-management/inventory/finder")]
    public SingleObjectModel GetInventory([FromBody] FinderInventoryQuery query) {

      using (var usecases = ReportingUseCases.UseCaseInteractor()) {

       ReportingDataDto inventoryOrderDto = usecases.FinderInventory(query);

       return new SingleObjectModel(this.Request, inventoryOrderDto);
      }
    }

    #endregion Web Apis

  }
}
