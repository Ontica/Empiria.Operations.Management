/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                            Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Query Api Controller                  *
*  Type     : AssetsController                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve assets data.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Inventory.Assets.Adapters;
using Empiria.Inventory.Assets.UseCases;

namespace Empiria.Inventory.Assets.WebApi {

  /// <summary>Web API used to retrieve assets data.</summary>
  public class AssetsController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/assets/{assetUID:guid}")]
    public SingleObjectModel GetAsset([FromUri] string assetUID) {

      using (var usecases = AssetUseCases.UseCaseInteractor()) {
        AssetHolderDto asset = usecases.GetAsset(assetUID);

        return new SingleObjectModel(base.Request, asset);
      }
    }


    [HttpGet]
    [Route("v2/assets/assignees")]
    public CollectionModel GetAssetsAssignees([FromUri] string keywords = "") {

      using (var usecases = AssetUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> assignees = usecases.GetAssetsAssignees(keywords);

        return new CollectionModel(base.Request, assignees);
      }
    }


    [HttpGet]
    [Route("v2/assets/types")]
    public CollectionModel GetAssetTypes() {

      using (var usecases = AssetUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> assetTypes = usecases.GetAssetTypes();

        return new CollectionModel(base.Request, assetTypes);
      }
    }


    [HttpPost]
    [Route("v2/assets/search")]
    public CollectionModel SearchAssets([FromBody] AssetsQuery query) {

      base.RequireBody(query);

      using (var usecases = AssetUseCases.UseCaseInteractor()) {
        FixedList<AssetDescriptor> assets = usecases.SearchAssets(query);

        return new CollectionModel(base.Request, assets);
      }
    }

    #endregion Web Apis

  }  // class AssetsController

}  // namespace Empiria.Inventory.Assets.WebApi
