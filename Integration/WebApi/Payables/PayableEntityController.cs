/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : PayableEntityController                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive payable entities depending of their types.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Orders.Adapters;

using Empiria.Payments.Payables.Adapters;
using Empiria.Payments.Payables.Services;

using Empiria.Payments.Adapters;
using Empiria.Payments.UseCases;

namespace Empiria.Payments.Payables.WebApi {

  /// <summary>Web API used to retrive payable entities depending of their types.</summary>
  public class PayableEntityController : WebApiController {

    #region Query web apis

    [HttpPost]
    [Route("v2/payments-management/payables/search")]   // ToDo: Remove this deprecated route in future versions.
    public CollectionModel SearchPaymentOrders([FromBody] PaymentOrdersQuery query) {

      using (var usecases = PaymentOrderUseCases.UseCaseInteractor()) {
        FixedList<PaymentOrderDescriptor> paymentOrders = usecases.SearchPaymentOrders(query);

        return new CollectionModel(base.Request, paymentOrders);
      }
    }


    [HttpGet]
    [Route("v2/payments-management/payment-order-types")]  // ToDo: Remove on next version
    [Route("v2/payments-management/payables/payable-types")]
    public CollectionModel GetPayableEntityTypes() {

      using (var services = PayableEntityServices.ServiceInteractor()) {
        FixedList<NamedEntityDto> payableEntityTypes = services.GetPayableEntityTypes();

        return new CollectionModel(Request, payableEntityTypes);
      }
    }


    [HttpPost]
    [Route("v8/order-management/orders/{orderUID:guid}/request-payment")]
    public SingleObjectModel RequestOrderPayment([FromUri] string orderUID,
                                                 [FromBody] PaymentOrderFields fields) {

      using (var services = PayableEntityServices.ServiceInteractor()) {

        OrderHolderDto order = services.RequestPayment(orderUID, fields);

        return new SingleObjectModel(base.Request, order);
      }
    }


    [HttpPost]
    [Route("v2/payments-management/payable-entities/search")]
    public CollectionModel SearchPayableEntities([FromBody] PayableEntitiesQuery query) {

      base.RequireBody(query);

      using (var services = PayableEntityServices.ServiceInteractor()) {
        FixedList<PayableEntityDto> payableEntities = services.SearchPayableEntities(query);

        return new CollectionModel(Request, payableEntities);
      }
    }

    #endregion Query web apis

  }  // class PayableEntityController

}  // namespace Empiria.Payments.Payables.WebApi
