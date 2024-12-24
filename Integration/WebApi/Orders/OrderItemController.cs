/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Orders Management Integration                 Component : Web Api Layer                        *
*  Assembly : Empiria.Operations.Integration.WebApi.dll     Pattern   : Web Api Controller                   *
*  Type     : OrderController                               License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web api used to handle integrated orders items edition.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Orders.Adapters;

using Empiria.Procurement.Contracts;
using Empiria.Procurement.Contracts.Adapters;

using Empiria.Operations.Integration.Orders.UseCases;


namespace Empiria.Operations.Integration.Orders.WebApi {

  /// <summary>Web api used to handle integrated orders items edition.</summary>
  public class OrderItemController : WebApiController {

    #region Web Apis


    [HttpPost]
    [Route("v8/order-management/orders/{orderUID:guid}/items")]
    public SingleObjectModel CreateOrderItem([FromUri] string orderUID,
                                             [FromBody] ContractOrderItemFields fields) {

      base.RequireBody(fields);

      using (var usecases = ContractOrderItemUseCases.UseCaseInteractor()) {
        ContractOrderItemDto orderItem = usecases.CreateOrderItem(orderUID, fields);

        return new SingleObjectModel(base.Request, orderItem);
      }
    }


    [HttpDelete]
    [Route("v8/order-management/orders/{orderUID:guid}/items/{orderItemUID:guid}")]
    public NoDataModel RemoveOrderItem([FromUri] string orderUID,
                                       [FromUri] string orderItemUID) {

      using (var usecases = ContractOrderItemUseCases.UseCaseInteractor()) {
        _ = usecases.DeleteOrderItem(orderUID, orderItemUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v8/order-management/orders/{orderUID:guid}/items/{orderItemUID:guid}")]
    public SingleObjectModel UpdateOrderItem([FromUri] string orderUID,
                                             [FromUri] string orderItemUID,
                                             [FromBody] ContractOrderItemFields fields) {

      base.RequireBody(fields);

      using (var usecases = ContractOrderItemUseCases.UseCaseInteractor()) {
        ContractOrderItemDto orderItem = usecases.UpdateOrderItem(orderUID, orderItemUID, fields);

        return new SingleObjectModel(base.Request, orderItem);
      }
    }

    #endregion Web Apis

  }  // class OrderItemController

}  // namespace Empiria.Operations.Orders.WebApi
