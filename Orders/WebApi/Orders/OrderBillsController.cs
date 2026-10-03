/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                            Component : Web Api Layer                         *
*  Assembly : Empiria.Orders.WebApi.dll                    Pattern   : Web api Controller                    *
*  Type     : OrderBillsController                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update orders bills.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Documents;
using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Orders.UseCases;

using Empiria.Billing;
using Empiria.Billing.Adapters;

namespace Empiria.Orders.WebApi {

  /// <summary>Web API used to retrieve and update orders bills.</summary>
  public class OrderBillsController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v8/order-management/orders/bill-types")]
    public CollectionModel GetBillTypes() {

      using (var usecases = OrderBillsUseCases.UseCaseInteractor()) {

        FixedList<DocumentProduct> billTypes = usecases.GetBillTypes();

        var mapped = billTypes.Select(x => BillTypeDto.MapToBillTypeDto(x))
                              .ToFixedList();

        return new CollectionModel(base.Request, mapped);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v8/order-management/orders/{orderUID:guid}/bills")]
    public SingleObjectModel AddBill([FromUri] string orderUID) {

      var order = Order.Parse(orderUID);

      DocumentFields documentFields = GetFormDataFromHttpRequest<DocumentFields>("document");

      var documentProduct = DocumentProduct.Parse(documentFields.DocumentProductUID);

      InputFileCollection inputFiles = GetAllInputFilesFromHttpRequest();

      using (var usecases = OrderBillsUseCases.UseCaseInteractor()) {

        Bill bill = usecases.AddBillToOrder(order, documentProduct, documentFields, inputFiles);

        return new SingleObjectModel(Request, BillMapper.MapToBillDto(bill));
      }
    }


    [HttpGet]
    [Route("v8/order-management/orders/{orderUID:guid}/bills")]
    public CollectionModel GetBills([FromUri] string orderUID) {

      var order = Order.Parse(orderUID);

      using (var usecases = OrderBillsUseCases.UseCaseInteractor()) {

        FixedList<Bill> bills = usecases.GetBillsFor(order);

        return new CollectionModel(this.Request, BillMapper.MapToBillDto(bills));
      }
    }


    [HttpDelete]
    [Route("v8/order-management/orders/{orderUID:guid}/bills/{billUID:guid}")]
    public NoDataModel RemoveBill([FromUri] string orderUID,
                                  [FromUri] string billUID) {

      var order = Order.Parse(orderUID);

      Bill bill = Bill.Parse(billUID);

      using (var usecases = OrderBillsUseCases.UseCaseInteractor()) {

        usecases.RemoveBillFromOrder(order, bill);

        return new NoDataModel(this.Request);
      }
    }

    #endregion Command web apis

  }  // class OrderBillsController

}  // namespace Empiria.Orders.WebApi
