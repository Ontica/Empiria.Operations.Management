/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Query Type Extensions                   *
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

      string assignedToFilter = BuildAssignedToFilter(query.AssignedToUID);
      string assignedToOrgUnitFilter = BuildAssignedToOrgUnitFilter(query.AssignedToOrgUnitUID);
      string locationFilter = BuildLocationFilter(query);
      string managerFilter = BuildManagerFilter(query.ManagerUID);
      string managerOrgUnitFilter = BuildManagerOrgUnitFilter(query.ManagerOrgUnitUID);
      string statusFilter = BuildStatusFilter(query.Status);
      string assetTypeFilter = BuildAssetTypeFilter(query.AssetTypeUID);
      string assetNoFilter = BuildAssetNoFilter(query.AssetNo);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);

      var filter = new Filter(assignedToFilter);

      filter.AppendAnd(assignedToOrgUnitFilter);
      filter.AppendAnd(locationFilter);
      filter.AppendAnd(managerFilter);
      filter.AppendAnd(managerOrgUnitFilter);
      filter.AppendAnd(statusFilter);
      filter.AppendAnd(assetTypeFilter);
      filter.AppendAnd(assetNoFilter);
      filter.AppendAnd(keywordsFilter);

      return filter.ToString();
    }


    static internal string MapToSortString(this AssetsQuery query) {

      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      }

      return "SKU_NO";
    }

    #endregion Extension Methods

    #region Helpers

    static private string BuildAssetNoFilter(string assetNo) {
      if (assetNo.Length == 0) {
        return string.Empty;
      }

      return $"SKU_NO LIKE '%{assetNo}%'";
    }


    static private string BuildAssetTypeFilter(string assetTypeUID) {
      if (assetTypeUID.Length == 0) {
        return string.Empty;
      }

      var assetType = AssetType.Parse(assetTypeUID);

      return $"ASSET_TYPE_ID = {assetType.Id}";
    }


    static private string BuildAssignedToFilter(string assignedToUID) {
      if (assignedToUID.Length == 0) {
        return string.Empty;
      }

      var assignedTo = Person.Parse(assignedToUID);

      return $"ASSET_ASSIGNED_TO_ID = {assignedTo.Id}";
    }


    static private string BuildAssignedToOrgUnitFilter(string assignedToOrgUnitUID) {
      if (assignedToOrgUnitUID.Length == 0) {
        return string.Empty;
      }

      var assignedToOrgUnit = OrganizationalUnit.Parse(assignedToOrgUnitUID);

      return $"ASSET_ASSIGNED_TO_ORG_UNIT_ID = {assignedToOrgUnit.Id}";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }

      return SearchExpression.ParseAndLike("ASSET_KEYWORDS", keywords);
    }


    static private string BuildLocationFilter(AssetsQuery query) {
      if (query.BuildingUID.Length == 0) {
        return string.Empty;
      }

      Location location;

      if (query.PlaceUID.Length != 0) {
        location = Location.Parse(query.PlaceUID);

        return $"ASSET_LOCATION_ID = {location.Id}";
      }

      if (query.FloorUID.Length != 0) {
        location = Location.Parse(query.FloorUID);
      } else {
        location = Location.Parse(query.BuildingUID);
      }

      FixedList<Location> locations = location.GetAllChildren();

      var locationIds = locations.Select(x => x.Id).ToFixedList().ToArray();

      return SearchExpression.ParseInSet("ASSET_LOCATION_ID", locationIds);
    }


    static private string BuildManagerFilter(string managerUID) {
      if (managerUID.Length == 0) {
        return string.Empty;
      }

      var manager = Person.Parse(managerUID);

      return $"ASSET_MGR_ID = {manager.Id}";
    }


    static private string BuildManagerOrgUnitFilter(string managerOrgUnitUID) {
      if (managerOrgUnitUID.Length == 0) {
        return string.Empty;
      }

      var managerOrgUnit = OrganizationalUnit.Parse(managerOrgUnitUID);

      return $"ASSET_MGR_ORG_UNIT_ID = {managerOrgUnit.Id}";
    }


    static private string BuildStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return "ASSET_STATUS <> 'X'";
      }

      return $"ASSET_STATUS = '{(char) status}'";
    }

    #endregion Helpers

  }  // class AssetsQueryExtensions

} // namespace Empiria.Inventory.Assets.Adapters
