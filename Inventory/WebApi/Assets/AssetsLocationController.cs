/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                            Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Query Api Controller                  *
*  Type     : AssetsLocationController                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve assets location data.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Inventory.Assets.UseCases;

namespace Empiria.Inventory.Assets.WebApi {

  /// <summary>Web API used to retrieve assets location data.</summary>
  public class AssetsLocationController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/fixed-assets/locations/root")]
    public CollectionModel GetAssetsRootLocations() {

      using (var usecases = AssetLocationUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> locations = usecases.GetRootLocations();

        return new CollectionModel(base.Request, locations);
      }
    }


    [HttpGet]
    [Route("v2/fixed-assets/locations/{locationUID}/children")]
    public CollectionModel GetAssetLocationsList([FromUri] string locationUID) {

      using (var usecases = AssetLocationUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> locations = usecases.GetLocationChildren(locationUID);

        return new CollectionModel(base.Request, locations);
      }
    }

    #endregion Web Apis

  }  // class AssetsLocationController

}  // namespace Empiria.Inventory.Assets.WebApi
