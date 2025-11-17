/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Orders Management Integration                 Component : Web Api Layer                        *
*  Assembly : Empiria.Operations.Integration.WebApi.dll     Pattern   : Web Api Controller                   *
*  Type     : RequisitionController                         License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web api used to handle requsitions.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Orders.Adapters;
using Empiria.Orders.UseCases;

namespace Empiria.Operations.Integration.Orders.WebApi {

  /// <summary>Web api used to handle requsitions.</summary>
  public class RequisitionController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v8/order-management/requisitions/available")]
    public CollectionModel SearchRequisitions([FromBody] OrdersQuery query) {

      using (var usecases = RequisitionUseCases.UseCaseInteractor()) {
        var requisitions = usecases.SearchRequisitions(query);

        return new CollectionModel(base.Request, requisitions);
      }
    }

    #endregion Web Apis

  }  // class RequisitionController

}  // namespace Empiria.Operations.Integration.Orders.WebApi
