/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Orders Management Integration                 Component : Web Api Layer                        *
*  Assembly : Empiria.Operations.Integration.WebApi.dll     Pattern   : Web Api Controller                   *
*  Type     : OrderTypeController                           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web api used to handle order types and their data.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Operations.Integration.Orders.UseCases;

namespace Empiria.Operations.Integration.Orders.WebApi {

  /// <summary>Web api used to handle order types and their data.</summary>
  public class OrderTypeController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v8/order-management/order-types/{orderTypeUID}/categories")]
    public CollectionModel GetOrderTypeCategories([FromUri] string orderTypeUID) {

      using (var usecases = OrderTypeUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> categories = usecases.GetOrderTypeCategories(orderTypeUID);

        return new CollectionModel(base.Request, categories);
      }
    }

    #endregion Web Apis

  }  // class OrderTypeController

}  // namespace Empiria.Operations.Orders.WebApi
