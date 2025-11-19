/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                             Component : Web Api Layer                        *
*  Assembly : Empiria.Orders.WebApi.dll                     Pattern   : Web Api Controller                   *
*  Type     : OrderDocumentsController                      License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web API used to retrive and update order documents.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Documents;

namespace Empiria.Orders.WebApi {

  /// <summary>Web API used to retrive and update order documents.</summary>
  public class OrderDocumentsController : WebApiController {

    #region Web apis

    [HttpDelete]
    [Route("v8/order-management/orders/{orderUID:guid}/documents/{documentUID:guid}")]
    public NoDataModel RemoveOrderDocument([FromUri] string orderUID,
                                           [FromUri] string documentUID) {

      var order = Order.Parse(orderUID);
      var document = DocumentServices.GetDocument(documentUID);

      DocumentServices.RemoveDocument(order, document);

      return new NoDataModel(this.Request);
    }


    [HttpPost]
    [Route("v8/order-management/orders/{orderUID:guid}/documents")]
    public SingleObjectModel StoreOrderDocument([FromUri] string orderUID) {

      var order = Order.Parse(orderUID);

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      FixedList<DocumentDto> currentDocuments = DocumentServices.GetEntityDocuments(order);

      InputFile documentFile = base.GetInputFileFromHttpRequest();

      var document = DocumentServices.StoreDocument(documentFile, order, fields);

      return new SingleObjectModel(base.Request, document);
    }


    [HttpPut, HttpPatch]
    [Route("v8/order-management/orders/{orderUID:guid}/documents/{documentUID:guid}")]
    public SingleObjectModel UpdateOrderDocument([FromUri] string orderUID,
                                                 [FromUri] string documentUID,
                                                 [FromBody] DocumentFields fields) {
      base.RequireBody(fields);

      var order = Order.Parse(orderUID);
      var document = DocumentServices.GetDocument(documentUID);

      var documentDto = DocumentServices.UpdateDocument(order, document, fields);

      return new SingleObjectModel(base.Request, documentDto);
    }

    #endregion Web apis

  }  // class OrderDocumentsController

}  // namespace Empiria.Orders.WebApi
