/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Type Extensions                         *
*  Type     : AssetsQueryExtensions                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for AssetsQuery type.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Locations;
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Extension methods for AssetsQuery type.</summary>
  static internal class AssetsQueryExtensions {

    #region Extension Methods

    static internal void EnsureIsValid(this AssetsQuery query) {
      // no - op
    }


    static internal string MapToFilterString(this AssetsQuery query) {

      string custodianOrgUnitFilter = BuildCustodianOrgUnitFilter(query.CustodianOrgUnitUID);

      string statusFilter = BuildStatusFilter(query.Status);

      string inventoryNoFilter = BuildInventoryNoFilter(query.InventoryNo);

      string assetTypeFilter = BuildAssetTypeFilter(query.FixedAssetTypeUID);

      string locationFilter = BuildLocationFilter(query);

      string keywordsFilter = BuildKeywordsFilter(query.Keywords);


      var filter = new Filter(custodianOrgUnitFilter);

      filter.AppendAnd(statusFilter);
      filter.AppendAnd(inventoryNoFilter);
      filter.AppendAnd(locationFilter);
      filter.AppendAnd(assetTypeFilter);
      filter.AppendAnd(keywordsFilter);

      return filter.ToString();
    }


    static internal string MapToSortString(this AssetsQuery query) {

      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      }

      return "FXD_ASST_INVENTORY_NO";

    }

    #endregion Extension Methods

    #region Helpers

    static private string BuildCustodianOrgUnitFilter(string custodianOrgUnitUID) {
      if (custodianOrgUnitUID == string.Empty) {
        return string.Empty;
      }

      var custodianOrgUnit = OrganizationalUnit.Parse(custodianOrgUnitUID);

      return $"FXD_ASST_CUSTODIAN_ORG_UNIT_ID = {custodianOrgUnit.Id}";
    }


    static private string BuildAssetTypeFilter(string assetTypeUID) {
      if (assetTypeUID == string.Empty) {
        return string.Empty;
      }

      var assetType = AssetType.Parse(assetTypeUID);

      return $"FXD_ASST_TYPE_ID = {assetType.Id}";
    }


    static private string BuildInventoryNoFilter(string inventoryNo) {
      if (inventoryNo == string.Empty) {
        return string.Empty;
      }

      return $"FXD_ASST_INVENTORY_NO LIKE '%{inventoryNo}%'";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords == string.Empty) {
        return string.Empty;
      }

      return SearchExpression.ParseAndLike("FXD_ASST_KEYWORDS", keywords);
    }


    static private string BuildLocationFilter(AssetsQuery query) {
      if (string.IsNullOrWhiteSpace(query.BuildingUID)) {
        return string.Empty;
      }

      Location location;

      if (query.PlaceUID.Length != 0) {
        location = Location.Parse(query.PlaceUID);

        return $"FXD_ASST_LOCATION_ID = {location.Id}";
      }

      if (query.FloorUID.Length != 0) {
        location = Location.Parse(query.FloorUID);
      } else {
        location = Location.Parse(query.BuildingUID);
      }

      FixedList<Location> locations = location.GetAllChildren();

      var locationIds = locations.Select(x => x.Id).ToFixedList().ToArray();

      return SearchExpression.ParseInSet("FXD_ASST_LOCATION_ID", locationIds);
    }


    static private string BuildStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return "FXD_ASST_STATUS <> 'X' ";
      }

      return $"FXD_ASST_STATUS = '{(char) status}'";
    }

    #endregion Helpers

  }  // class AssetsQueryExtensions

} // namespace Empiria.Inventory.Assets.Adapters
