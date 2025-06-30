/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                            Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Query Api Controller                  *
*  Type     : AssetsAssignationController                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve asset assignations data.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Inventory.Assets.Adapters;
using Empiria.Inventory.Assets.UseCases;

namespace Empiria.Inventory.Assets.WebApi {

  /// <summary>Web API used to retrieve asset assignations data.</summary>
  public class AssetsAssignationController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/assets/assignations/{assignationUID}")]
    public SingleObjectModel GetAssetAssignation([FromUri] string assignationUID) {

      using (var usecases = AssetAssignationUseCases.UseCaseInteractor()) {
        AssetAssignationHolder assignation = usecases.GetAssetAssignation(assignationUID);

        return new SingleObjectModel(base.Request, assignation);
      }
    }


    [HttpPost]
    [Route("v2/assets/assignations/search")]
    public CollectionModel SearchAssetsAssignations([FromBody] AssetsAssignationsQuery query) {

      using (var usecases = AssetAssignationUseCases.UseCaseInteractor()) {
        FixedList<AssetAssignationDescriptor> assignations = usecases.SearchAssetAssignations(query);

        return new CollectionModel(base.Request, assignations);
      }
    }

    #endregion Web Apis

  }  // class AssetsAssignationController

}  // namespace Empiria.Inventory.Assets.WebApi
