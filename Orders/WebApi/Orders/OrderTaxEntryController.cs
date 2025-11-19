/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                             Component : Web Api Layer                        *
*  Assembly : Empiria.Orders.WebApi.dll                     Pattern   : Web Api Controller                   *
*  Type     : OrderTaxEntryController                       License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web API used to retrive and update order tax entries.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Orders.Adapters;
using Empiria.Orders.UseCases;

namespace Empiria.Orders.WebApi {

  /// <summary>Web API used to retrive and update order tax entries.</summary>
  public class OrderTaxEntryController : WebApiController {

    #region Web apis

    [HttpPost]
    [Route("v8/order-management/orders/{orderUID:guid}/tax-entries")]
    public SingleObjectModel AddTaxEntry([FromUri] string orderUID,
                                         [FromBody] OrderTaxEntryFields fields) {

      fields.OrderUID = orderUID;

      using (var usecases = OrderTaxesUseCases.UseCaseInteractor()) {

        OrderTaxEntryDto taxEntry = usecases.AddTaxEntry(fields);

        return new SingleObjectModel(this.Request, taxEntry);
      }
    }


    [HttpDelete]
    [Route("v8/order-management/orders/{orderUID:guid}/tax-entries/{taxEntryUID:guid}")]
    public NoDataModel RemoveTaxEntry([FromUri] string orderUID,
                                      [FromUri] string taxEntryUID) {

      using (var usecases = OrderTaxesUseCases.UseCaseInteractor()) {
        OrderTaxEntryDto taxEntry = usecases.RemoveTaxEntry(orderUID, taxEntryUID);

        return new NoDataModel(this.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v8/order-management/orders/{orderUID:guid}/tax-entries/{taxEntryUID:guid}")]
    public SingleObjectModel UpdateOrderTaxEntry([FromUri] string orderUID,
                                                 [FromUri] string taxEntryUID,
                                                 [FromBody] OrderTaxEntryFields fields) {

      fields.UID = taxEntryUID;
      fields.OrderUID = orderUID;

      using (var usecases = OrderTaxesUseCases.UseCaseInteractor()) {
        OrderTaxEntryDto taxEntry = usecases.UpdateTaxEntry(fields);

        return new SingleObjectModel(this.Request, taxEntry);
      }
    }

    #endregion Web apis

  }  // class OrderTaxEntryController

}  // namespace Empiria.Orders.WebApi
