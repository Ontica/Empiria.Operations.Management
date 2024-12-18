/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Orders Management Integration                 Component : Web Api Layer                        *
*  Assembly : Empiria.Operations.Integration.WebApi.dll     Pattern   : Web Api Controller                   *
*  Type     : OrderManagementController                     License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web api used to handle integrated orders.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Orders.Adapters;
using Empiria.Operations.Integration.Orders.Adapters;

using Empiria.Operations.Integration.Orders.UseCases;

namespace Empiria.Operations.Integration.Orders.WebApi {

  /// <summary>Web api used to handle integrated orders.</summary>
  public class OrderManagementController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v8/order-management/orders/{orderUID:guid}/activate")]
    public SingleObjectModel ActivateOrder([FromUri] string orderUID) {

      using (var usecases = OrderManagementUseCases.UseCaseInteractor()) {
        OrderHolderDto order = usecases.ActivateOrder(orderUID);

        return new SingleObjectModel(base.Request, order);
      }
    }


    [HttpGet]
    [Route("v8/order-management/orders/{orderUID:guid}")]
    public SingleObjectModel GetOrder([FromUri] string orderUID) {

      using (var usecases = OrderManagementUseCases.UseCaseInteractor()) {
        OrderHolderDto order = usecases.GetOrder(orderUID);

        return new SingleObjectModel(base.Request, order);
      }
    }


    [HttpPost]
    [Route("v8/order-management/orders/search")]
    public CollectionModel SearchOrders([FromBody] OrdersQuery query) {

      base.RequireBody(query);

      using (var usecases = OrderManagementUseCases.UseCaseInteractor()) {
        FixedList<OrderDescriptor> orders = usecases.SearchOrders(query);

        return new CollectionModel(base.Request, orders);
      }
    }


    [HttpPost]
    [Route("v8/order-management/orders/{orderUID:guid}/suspend")]
    public SingleObjectModel SuspendOrder([FromUri] string orderUID) {

      using (var usecases = OrderManagementUseCases.UseCaseInteractor()) {
        OrderHolderDto order = usecases.SuspendOrder(orderUID);

        return new SingleObjectModel(base.Request, order);
      }
    }

    #endregion Web Apis

  }  // class OrderManagementController

}  // namespace Empiria.Operations.Orders.WebApi
