/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                            Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : AssetsDocumentsController                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update asset's documents.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Documents;
using Empiria.Documents.Services;

namespace Empiria.Inventory.Assets.WebApi {

  /// <summary>Web API used to retrive and update asset's documents.</summary>
  public class AssetsDocumentsController : WebApiController {

    #region Command web apis

    [HttpDelete]
    [Route("v2/assets/{assetUID:guid}/documents/{documentUID:guid}")]
    public NoDataModel RemoveAssetDocument([FromUri] string assetUID,
                                           [FromUri] string documentUID) {

      var asset = Asset.Parse(assetUID);
      var document = Document.Parse(documentUID);

      DocumentServices.RemoveDocument(asset, document);

      return new NoDataModel(this.Request);
    }


    [HttpPost]
    [Route("v2/assets/{assetUID:guid}/documents")]
    public SingleObjectModel StoreAssetDocument([FromUri] string assetUID) {

      var asset = Asset.Parse(assetUID);

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      InputFile documentFile = base.GetInputFileFromHttpRequest();

      var document = DocumentServices.StoreDocument(documentFile, asset, fields);

      return new SingleObjectModel(base.Request, document);
    }


    [HttpPut, HttpPatch]
    [Route("v2/assets/{assetUID:guid}/documents/{documentUID:guid}")]
    public SingleObjectModel UpdateAssetDocument([FromUri] string assetUID,
                                                 [FromUri] string documentUID,
                                                 [FromBody] DocumentFields fields) {
      base.RequireBody(fields);

      var asset = Asset.Parse(assetUID);
      var document = Document.Parse(documentUID);

      var documentDto = DocumentServices.UpdateDocument(asset, document, fields);

      return new SingleObjectModel(base.Request, documentDto);
    }

    #endregion Command web apis

  }  // class AssetsDocumentsController

}  // namespace Empiria.Inventory.Assets.WebApi
