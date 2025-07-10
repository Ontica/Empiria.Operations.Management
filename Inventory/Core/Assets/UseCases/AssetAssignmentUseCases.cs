/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Use cases Layer                         *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : AssetAssignmentUseCases                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for assets assignments.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Inventory.Assets.Adapters;
using Empiria.Inventory.Assets.Data;

namespace Empiria.Inventory.Assets.UseCases {

  /// <summary>Use cases for assets assignments.</summary>
  public class AssetAssignmentUseCases : UseCase {

    #region Constructors and parsers

    protected AssetAssignmentUseCases() {
      // no-op
    }

    static public AssetAssignmentUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<AssetAssignmentUseCases>();
    }


    #endregion Constructors and parsers

    #region Use cases

    public AssetAssignmentHolder GetAssetAssignment(string assignmentUID) {
      Assertion.Require(assignmentUID, nameof(assignmentUID));

      var assignment = AssetAssignment.Parse(assignmentUID);

      return AssetAssignmentMapper.Map(assignment);
    }


    public FixedList<AssetAssignment> SearchAssetsAssignments(AssetsAssignmentsQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();

      string sort = query.MapToSortString();

      return AssetsAssignmentsData.SearchAssignments(filter, sort);
    }

    #endregion Use cases

  }  // class AssetAssignmentUseCases

}  // namespace Empiria.Inventory.Assets.UseCases
