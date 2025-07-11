﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                            Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Query Api Controller                  *
*  Type     : AssetsAssignmentController                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve asset assignments data.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Inventory.Assets.Adapters;
using Empiria.Inventory.Assets.UseCases;

namespace Empiria.Inventory.Assets.WebApi {

  /// <summary>Web API used to retrieve asset assignments data.</summary>
  public class AssetsAssignmentController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/assets/assignments/{assignmentUID}")]
    public SingleObjectModel GetAssetAssignments([FromUri] string assignmentUID) {

      using (var usecases = AssetAssignmentUseCases.UseCaseInteractor()) {
        AssetAssignmentHolder assignment = usecases.GetAssetAssignment(assignmentUID);

        return new SingleObjectModel(base.Request, assignment);
      }
    }


    [HttpPost]
    [Route("v2/assets/assignments/search")]
    public CollectionModel SearchAssetsAssignments([FromBody] AssetsAssignmentsQuery query) {

      using (var usecases = AssetAssignmentUseCases.UseCaseInteractor()) {
        FixedList<AssetAssignmentDescriptor> assignments = usecases.SearchAssetsAssignments(query);

        return new CollectionModel(base.Request, assignments);
      }
    }

    #endregion Web Apis

  }  // class AssetsAssignmentController

}  // namespace Empiria.Inventory.Assets.WebApi
