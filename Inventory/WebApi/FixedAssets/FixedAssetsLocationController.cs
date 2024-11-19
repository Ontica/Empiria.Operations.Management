/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                      Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Query Api Controller                  *
*  Type     : FixedAssetsLocationController                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve fixed assets location data.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Inventory.FixedAssets.UseCases;

namespace Empiria.Inventory.FixedAssets.WebApi {

  /// <summary>Web API used to retrieve fixed assets location data.</summary>
  public class FixedAssetsLocationController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/fixed-assets/locations/root")]
    public CollectionModel GetFixedAssetRootLocations() {

      using (var usecases = FixedAssetLocationUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> locations = usecases.GetRootLocations();

        return new CollectionModel(base.Request, locations);
      }
    }


    [HttpGet]
    [Route("v2/fixed-assets/locations/{locationUID}/children")]
    public CollectionModel GetFixedAssetLocationsList([FromUri] string locationUID) {

      using (var usecases = FixedAssetLocationUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> locations = usecases.GetLocationChildren(locationUID);

        return new CollectionModel(base.Request, locations);
      }
    }

    #endregion Web Apis

  }  // class FixedAssetsLocationController

}  // namespace Empiria.Inventory.FixedAssets.WebApi
