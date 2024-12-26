/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Orders Management Integration                 Component : Web Api Layer                        *
*  Assembly : Empiria.Operations.Integration.WebApi.dll     Pattern   : Web Api Controller                   *
*  Type     : OrderController                               License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web api used to handle integrated orders.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

using Empiria.Orders;
using Empiria.Orders.Adapters;

using Empiria.Procurement.Contracts;

using Empiria.Operations.Integration.Orders.Adapters;
using Empiria.Operations.Integration.Orders.UseCases;

namespace Empiria.Operations.Integration.Orders.WebApi {

  /// <summary>Web api used to handle integrated orders.</summary>
  public class OrderController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v8/order-management/orders/{orderUID:guid}/activate")]
    public SingleObjectModel ActivateOrder([FromUri] string orderUID) {

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        OrderHolderDto order = usecases.ActivateOrder(orderUID);

        return new SingleObjectModel(base.Request, order);
      }
    }


    [HttpPost]
    [Route("v8/order-management/orders")]
    public SingleObjectModel CreateOrder([FromBody] object fields) {

      base.RequireBody(fields);

      OrderFields orderFields = MapToOrderFields(fields);

      OrderHolderDto order = OrderUseCaseSelector.CreateOrder(orderFields);

      return new SingleObjectModel(base.Request, order);
    }


    [HttpDelete]
    [Route("v8/order-management/orders/{orderUID:guid}")]
    public NoDataModel DeleteOrder([FromUri] string orderUID) {

      _ = OrderUseCaseSelector.DeleteOrder(orderUID);

      return new NoDataModel(base.Request);
    }


    [HttpGet]
    [Route("v8/order-management/orders/{orderUID:guid}")]
    public SingleObjectModel GetOrder([FromUri] string orderUID) {

      OrderHolderDto order = OrderUseCaseSelector.GetOrder(orderUID);

      return new SingleObjectModel(base.Request, order);
    }


    [HttpPost]
    [Route("v8/order-management/orders/search")]
    public CollectionModel SearchOrders([FromBody] OrdersQuery query) {

      base.RequireBody(query);

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        FixedList<OrderDescriptor> orders = usecases.SearchOrders(query);

        return new CollectionModel(base.Request, orders);
      }
    }


    [HttpPost]
    [Route("v8/order-management/orders/{orderUID:guid}/suspend")]
    public SingleObjectModel SuspendOrder([FromUri] string orderUID) {

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        OrderHolderDto order = usecases.SuspendOrder(orderUID);

        return new SingleObjectModel(base.Request, order);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v8/order-management/orders/{orderUID:guid}")]
    public SingleObjectModel UpdateOrder([FromUri] string orderUID,
                                         [FromBody] object fields) {

      base.RequireBody(fields);

      OrderFields orderFields = MapToOrderFields(fields);

      Assertion.Require(orderFields.UID.Length == 0 || orderFields.UID == orderUID,
                        "OrderUID mismatch.");

      orderFields.UID = orderUID;

      OrderHolderDto order = OrderUseCaseSelector.UpdateOrder(orderFields);

      return new SingleObjectModel(base.Request, order);
    }

    #endregion Web Apis

    #region Helpers

    private OrderFields MapToOrderFields(object fields) {

      base.RequireBody(fields);

      JsonObject json = base.GetJsonFromBody(fields);

      OrderType orderType = json.Get("orderTypeUID", OrderType.Empty);

      if (orderType.Equals(OrderType.Empty)) {
        throw Assertion.EnsureNoReachThisCode("orderTypeUID can not be empty.");

      } else if (orderType.Equals(OrderType.ContractOrder)) {
        return JsonConverter.ToObject<ContractOrderFields>(json.ToString());

      } else {
        return JsonConverter.ToObject<PayableOrderFields>(json.ToString());
      }
    }

    #endregion Helpers

  }  // class OrderController

}  // namespace Empiria.Operations.Orders.WebApi
