/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                            Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : AssetsTransactionsDocumentsController        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update assets transaction's documents.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Documents;
using Empiria.Documents.Services;

namespace Empiria.Inventory.Assets.WebApi {

  /// <summary>Web API used to retrive and update assets transaction's documents.</summary>
  public class AssetsTransactionsDocumentsController : WebApiController {

    #region Command web apis

    [HttpDelete]
    [Route("v2/assets/transactions/{transactionUID:guid}/documents/{documentUID:guid}")]
    public NoDataModel RemoveAssetTransactionDocument([FromUri] string transactionUID,
                                                      [FromUri] string documentUID) {

      var transaction = AssetTransaction.Parse(transactionUID);
      var document = Document.Parse(documentUID);

      DocumentServices.RemoveDocument(transaction, document);

      return new NoDataModel(this.Request);
    }


    [HttpPost]
    [Route("v2/assets/transactions/{transactionUID:guid}/documents")]
    public SingleObjectModel StoreAssetTransactionDocument([FromUri] string transactionUID) {

      var transaction = AssetTransaction.Parse(transactionUID);

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      InputFile documentFile = base.GetInputFileFromHttpRequest();

      var document = DocumentServices.StoreDocument(documentFile, transaction, fields);

      return new SingleObjectModel(base.Request, document);
    }


    [HttpPut, HttpPatch]
    [Route("v2/assets/transactions/{transactionUID:guid}/documents/{documentUID:guid}")]
    public SingleObjectModel UpdateAssetTransactionDocument([FromUri] string transactionUID,
                                                            [FromUri] string documentUID,
                                                            [FromBody] DocumentFields fields) {
      base.RequireBody(fields);

      var transaction = AssetTransaction.Parse(transactionUID);
      var document = Document.Parse(documentUID);

      var documentDto = DocumentServices.UpdateDocument(transaction, document, fields);

      return new SingleObjectModel(base.Request, documentDto);
    }

    #endregion Command web apis

  }  // class AssetsTransactionsDocumentsController

}  // namespace Empiria.Inventory.Assets.WebApi
