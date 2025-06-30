/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Use cases Layer                         *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : AssetAssignationUseCases                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for assets assignations.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Inventory.Assets.Adapters;
using Empiria.Inventory.Assets.Data;

namespace Empiria.Inventory.Assets.UseCases {

  /// <summary>Use cases for assets assignations.</summary>
  public class AssetAssignationUseCases : UseCase {

    #region Constructors and parsers

    protected AssetAssignationUseCases() {
      // no-op
    }

    static public AssetAssignationUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<AssetAssignationUseCases>();
    }


    #endregion Constructors and parsers

    #region Use cases

    public AssetAssignationHolder GetAssetAssignation(string assignationUID) {
      Assertion.Require(assignationUID, nameof(assignationUID));

      var assignation = AssetAssignation.Parse(assignationUID);

      return AssetAssignationMapper.Map(assignation);
    }


    public FixedList<AssetAssignationDescriptor> SearchAssetAssignations(AssetsAssignationsQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();

      string sort = query.MapToSortString();

      FixedList<AssetAssignation> assignations = AssetsAssignationsData.SearchAssignations(filter, sort);

      return AssetAssignationMapper.Map(assignations);
    }

    #endregion Use cases

  }  // class AssetAssignationUseCases

}  // namespace Empiria.Inventory.Assets.UseCases
