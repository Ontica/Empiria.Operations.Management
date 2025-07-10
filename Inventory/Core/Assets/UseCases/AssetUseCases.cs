/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Use cases Layer                         *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : AssetUseCases                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for assets management.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Inventory.Assets.Adapters;
using Empiria.Inventory.Assets.Data;

namespace Empiria.Inventory.Assets.UseCases {

  /// <summary>Use cases for assets management.</summary>
  public class AssetUseCases : UseCase {

    #region Constructors and parsers

    protected AssetUseCases() {
      // no-op
    }

    static public AssetUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<AssetUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public AssetHolderDto GetAsset(string assetUID) {
      Assertion.Require(assetUID, nameof(assetUID));

      var asset = Asset.Parse(assetUID);

      return AssetMapper.Map(asset);
    }


    public FixedList<NamedEntityDto> GetAssetsAssignees(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<Parties.Person> assignees = AssetsData.GetAssetsAssignees(keywords);

      return assignees.MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> GetAssetTypes() {
      var assetsTypes = AssetType.GetList();

      return assetsTypes.MapToNamedEntityList();
    }


    public FixedList<Asset> SearchAssets(AssetsQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();
      string orderBy = query.MapToSortString();

      return AssetsData.SearchAssets(filter, orderBy);
    }

    #endregion Use cases

  }  // class AssetUseCases

}  // namespace Empiria.Inventory.Assets.UseCases
