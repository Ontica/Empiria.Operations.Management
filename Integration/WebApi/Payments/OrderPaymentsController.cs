/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Orders Management Integration                 Component : Web Api Layer                        *
*  Assembly : Empiria.Operations.Integration.WebApi.dll     Pattern   : Web Api Controller                   *
*  Type     : OrderPaymentsController                       License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web api used to handle order payment requests.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Payments;

using Empiria.Orders.Adapters;

using Empiria.Operations.Integration.Payments.UseCases;

namespace Empiria.Operations.Integration.Payments.WebApi {

  /// <summary>Web api used to handle order payment requests.</summary>
  public class OrderPaymentsController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v8/order-management/orders/{orderUID:guid}/request-payment")]
    public SingleObjectModel RequestOrderPayment([FromUri] string orderUID,
                                                 [FromBody] PaymentOrderFields fields) {

      using (var usecases = PaymentsProcurementUseCases.UseCaseInteractor()) {

        OrderHolderDto order = usecases.RequestPayment(orderUID, fields);

        return new SingleObjectModel(base.Request, order);
      }
    }

    #endregion Web Apis

  }  // class OrderPaymentsController

}  // namespace Empiria.Operations.Integration.Payments.WebApi
