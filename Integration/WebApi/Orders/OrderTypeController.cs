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

using Empiria.Orders;
using Empiria.Orders.UseCases;

using Empiria.Payments.Adapters;

namespace Empiria.Operations.Integration.Orders.WebApi {

  /// <summary>Web api used to handle order types and their data.</summary>
  public class OrderTypeController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v8/order-management/expenses-types")]
    public CollectionModel GetExpensesTypes() {

      using (var usecases = OrderTypeUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> types = usecases.GetExpensesTypes();

        return new CollectionModel(base.Request, types);
      }
    }


    [HttpGet]
    [Route("v8/order-management/order-types/{orderTypeUID}/categories")]
    public CollectionModel GetOrderTypeCategories([FromUri] string orderTypeUID) {

      using (var usecases = OrderTypeUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> categories = usecases.GetOrderTypeCategories(orderTypeUID);

        return new CollectionModel(base.Request, categories);
      }
    }


    [HttpGet]
    [Route("v2/payments-management/payment-order-types")]
    [Route("v8/order-management/order-types/payable")]
    public CollectionModel GetPayableOrderTypes() {

      using (var usecases = OrderTypeUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> payableOrderTypes = usecases.GetPayableOrderTypes();

        return new CollectionModel(Request, payableOrderTypes);
      }
    }


    /// <summary>Should be deprecated in future versions. Used in create payment orders editor.</summary>
    [HttpPost]
    [Route("v2/payments-management/payable-entities/search")]   // ToDo : Remove on next version
    public CollectionModel SearchPayableOrders([FromBody] PayableEntitiesQuery query) {

      base.RequireBody(query);

      var orders = BaseObject.GetFullList<PayableOrder>()
                             .ToFixedList()
                             .FindAll(x => x.Status != StateEnums.EntityStatus.Deleted);

      return new CollectionModel(base.Request, PayableEntityMapper.Map(orders));
    }

    #endregion Web Apis

  }  // class OrderTypeController



  /// <summary>Should be deprecated in future versions. Used in create payment orders editor. 
  /// Input query DTO used to retrieve payable entities.</summary>
  public class PayableEntitiesQuery {

    public string OrganizationalUnitUID {
      get; set;
    } = string.Empty;


    public string PayableEntityTypeUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;

  }  // class PayableEntitiesQuery

}  // namespace Empiria.Operations.Orders.WebApi
