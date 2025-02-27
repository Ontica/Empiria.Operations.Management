/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Use cases Layer                         *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : FixedAssetUseCases                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for fixed assets management.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Contacts;

using Empiria.HumanResources;

using Empiria.Inventory.FixedAssets.Data;
using Empiria.Inventory.FixedAssets.Adapters;

namespace Empiria.Inventory.FixedAssets.UseCases {

  /// <summary>Use cases for fixed assets management.</summary>
  public class FixedAssetUseCases : UseCase {

    #region Constructors and parsers

    protected FixedAssetUseCases() {
      // no-op
    }

    static public FixedAssetUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FixedAssetUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedAssetHolderDto GetFixedAsset(string fixedAssetUID) {
      Assertion.Require(fixedAssetUID, nameof(fixedAssetUID));

      var fixedAsset = FixedAsset.Parse(fixedAssetUID);

      return FixedAssetMapper.Map(fixedAsset);
    }


    public FixedList<NamedEntityDto> GetFixedAssetKeepers(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<Parties.Person> employees = Employment.GetEmployees();

      return employees.MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> GetFixedAssetTypes() {
      var fixedAssetsTypes = FixedAssetType.GetList();

      return fixedAssetsTypes.MapToNamedEntityList();
    }


    public FixedList<FixedAssetDescriptor> SearchFixedAssets(FixedAssetsQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();
      string orderBy = query.MapToSortString();

      FixedList<FixedAsset> fixedAssets = FixedAssetsData.GetFixedAssets(filter, orderBy);

      return FixedAssetMapper.Map(fixedAssets);
    }

    #endregion Use cases

  }  // class FixedAssetUseCases

}  // namespace Empiria.Inventory.FixedAssets.UseCases
