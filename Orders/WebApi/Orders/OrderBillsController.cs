/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                            Component : Web Api Layer                         *
*  Assembly : Empiria.Orders.WebApi.dll                    Pattern   : Web api Controller                    *
*  Type     : OrderBillsController                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update orders bills.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.IO;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Documents;
using Empiria.Financial;
using Empiria.Storage;

using Empiria.Billing;
using Empiria.Billing.Adapters;
using Empiria.Billing.UseCases;

namespace Empiria.Orders.WebApi {

  /// <summary>Web API used to retrive and update orders bills.</summary>
  public class OrderBillsController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v8/order-management/orders/bill-types")]
    public CollectionModel GetBillTypes() {

      var billTypes = DocumentProduct.GetList<DocumentProduct>()
                     .FindAll(x => x.InternalCode.StartsWith("CFDI-"));

      return new CollectionModel(base.Request, billTypes.MapToNamedEntityList());
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v8/order-management/orders/{orderUID:guid}/bills")]
    public SingleObjectModel AddBill([FromUri] string orderUID) {

      var order = Order.Parse(orderUID);

      DocumentFields requestFields = GetFormDataFromHttpRequest<DocumentFields>("document");

      var documentProduct = DocumentProduct.Parse(requestFields.DocumentProductUID);

      InputFileCollection files = base.GetInputFilesFromHttpRequest(documentProduct.ApplicationContentType);

      Assertion.Require(files.ContainsKey("xml"), "Se requiere proprocionar el archivo XML del comprobante fiscal");

      InputFile xmlFile = files["xml"];
      InputFile pdfFile = files.ContainsKey("pdf") ? files["pdf"] : null;

      var xmlReader = new StreamReader(xmlFile.Stream);

      var xmlAsString = xmlReader.ReadToEnd();

      var usecases = BillUseCases.UseCaseInteractor();

      string billNo = usecases.ExtractBillNo(xmlAsString);

      var bill = Bill.TryParseWithBillNo(billNo);

      Assertion.Require(bill == null, $"El comprobante con folio '{billNo}' ya existe en el sistema.");

      bill = Bill.Parse(usecases.CreateBill(xmlAsString, (IPayableEntity) order).UID);

      var xmlDocument = DocumentServices.StoreDocument(xmlFile, bill, requestFields);

      var fields = new DocumentFields {
        UID = xmlDocument.UID,
        DocumentProductUID = requestFields.DocumentProductUID,
        Name = requestFields.Name,
        DocumentNo = bill.BillNo,
        DocumentDate = bill.IssueDate,
        SourcePartyUID = bill.IssuedBy.UID,
        TargetPartyUID = bill.IssuedTo.UID,
        Description = order.Description
      };

      DocumentServices.UpdateDocument(bill, xmlDocument, fields);

      if (pdfFile != null) {
        DocumentDto pdfDocument = DocumentServices.StoreDocument(pdfFile, bill, fields);

        fields.UID = pdfDocument.UID;
        DocumentServices.UpdateDocument(bill, pdfDocument, fields);
      }

      return new SingleObjectModel(Request, BillMapper.MapToBillDto(bill));
    }


    [HttpGet]
    [Route("v8/order-management/orders/{orderUID:guid}/bills")]
    public CollectionModel GetBills([FromUri] string orderUID) {

      var order = Order.Parse(orderUID);

      FixedList<Bill> bills = Bill.GetListFor((IPayableEntity) order);

      return new CollectionModel(this.Request, BillMapper.MapToBillDto(bills));
    }



    [HttpDelete]
    [Route("v8/order-management/orders/{orderUID:guid}/bills/{billUID:guid}")]
    public NoDataModel RemoveBill([FromUri] string orderUID,
                                  [FromUri] string billUID) {

      var order = Order.Parse(orderUID);

      Bill bill = Bill.Parse(billUID);

      bill.Delete();

      bill.Save();

      DocumentServices.RemoveAllDocuments(bill);

      return new NoDataModel(this.Request);
    }

    #endregion Command web apis

  }  // class OrderBillsController

}  // namespace Empiria.Orders.WebApi
