/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                         Component : Web Api                               *
*  Assembly : Empiria.Procurement.WebApi.dll               Pattern   : Web api Controller                    *
*  Type     : SupplierDocumentsController                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update supplier documents.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Parties;

using Empiria.Documents;

namespace Empiria.Procurement.Suppliers.WebApi {

  /// <summary>Web API used to retrieve and update supplier documents.</summary>
  public class SupplierDocumentsController : WebApiController {

    #region Command web apis

    [HttpDelete]
    [Route("v8/procurement/suppliers/{supplierUID:guid}/documents/{documentUID:guid}")]
    public NoDataModel RemoveSupplierDocument([FromUri] string supplierUID,
                                              [FromUri] string documentUID) {

      var supplier = Party.Parse(supplierUID);

      var document = DocumentServices.GetDocument(documentUID);

      DocumentServices.RemoveDocument(supplier, document);

      return new NoDataModel(this.Request);
    }


    [HttpPost]
    [Route("v8/procurement/suppliers/{supplierUID:guid}/documents")]
    public SingleObjectModel StoreSupplierDocument([FromUri] string supplierUID) {

      var supplier = Party.Parse(supplierUID);

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      InputFile documentFile = base.GetInputFileFromHttpRequest();

      var document = DocumentServices.StoreDocument(documentFile, supplier, fields);

      return new SingleObjectModel(base.Request, document);
    }


    [HttpPut, HttpPatch]
    [Route("v8/procurement/suppliers/{supplierUID:guid}/documents/{documentUID:guid}")]
    public SingleObjectModel UpdateSupplierDocument([FromUri] string supplierUID,
                                                    [FromUri] string documentUID,
                                                    [FromBody] DocumentFields fields) {
      base.RequireBody(fields);

      var supplier = Party.Parse(supplierUID);

      var document = DocumentServices.GetDocument(documentUID);

      var documentDto = DocumentServices.UpdateDocument(supplier, document, fields);

      return new SingleObjectModel(base.Request, documentDto);
    }

    #endregion Command web apis

  }  // class SupplierDocumentsController

}  // namespace Empiria.Procurement.Suppliers.WebApi
