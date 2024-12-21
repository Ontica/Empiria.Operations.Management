/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                      Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : FixedAssetsTransactionsDocumentsController   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update fixed assets transaction's documents.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Documents;
using Empiria.Documents.Services;

namespace Empiria.Inventory.FixedAssets.WebApi {

  /// <summary>Web API used to retrive and update fixed assets transaction's documents.</summary>
  public class FixedAssetsTransactionsDocumentsController : WebApiController {

    #region Command web apis

    [HttpDelete]
    [Route("v2/fixed-assets/transactions/{transactionUID:guid}/documents/{documentUID:guid}")]
    public NoDataModel RemoveFixedAssetTransactionDocument([FromUri] string transactionUID,
                                                           [FromUri] string documentUID) {

      var transaction = FixedAssetTransaction.Parse(transactionUID);
      var document = Document.Parse(documentUID);

      DocumentServices.RemoveDocument(transaction, document);

      return new NoDataModel(this.Request);
    }


    [HttpPost]
    [Route("v2/fixed-assets/transactions/{transactionUID:guid}/documents")]
    public SingleObjectModel StoreFixedAssetTransactionDocument([FromUri] string transactionUID) {

      var transaction = FixedAssetTransaction.Parse(transactionUID);

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      InputFile documentFile = base.GetInputFileFromHttpRequest();

      var document = DocumentServices.StoreDocument(documentFile, transaction, fields);

      return new SingleObjectModel(base.Request, document);
    }


    [HttpPut, HttpPatch]
    [Route("v2/fixed-assets/transactions/{transactionUID:guid}/documents/{documentUID:guid}")]
    public SingleObjectModel UpdateFixedAssetTransactionDocument([FromUri] string transactionUID,
                                                                 [FromUri] string documentUID,
                                                                 [FromBody] DocumentFields fields) {
      base.RequireBody(fields);

      var transaction = FixedAssetTransaction.Parse(transactionUID);
      var document = Document.Parse(documentUID);

      var documentDto = DocumentServices.UpdateDocument(transaction, document, fields);

      return new SingleObjectModel(base.Request, documentDto);
    }

    #endregion Command web apis

  }  // class FixedAssetsTransactionsDocumentsController

}  // namespace Empiria.Inventory.FixedAssets.WebApi
