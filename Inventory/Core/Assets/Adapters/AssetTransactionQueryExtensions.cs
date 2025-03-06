/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Query extension methods                 *
*  Type     : AssetTransactionQueryExtensions            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for AssetTransactionQuery interface adapter.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Locations;

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Extension methods for AssetTransactionQuery interface adapter.</summary>
  static internal class AssetTransactionQueryExtensions {

    #region Extension methods

    static internal void EnsureIsValid(this AssetTransactionQuery query) {
      // no-op
    }


    static internal string MapToFilterString(this AssetTransactionQuery query) {
      string transactionTypeFilter = BuildTransactionTypeFilter(query.TransactionTypeUID);
      string assignedToFilter = BuildAssignedToFilter(query.AssignedToUID);
      string assignedToOrgUnitFilter = BuildAssignedToOrgUnitFilter(query.AssignedToOrgUnitUID);
      string locationFilter = BuildLocationFilter(query);
      string managerFilter = BuildManagerFilter(query.ManagerUID);
      string managerOrgUnitFilter = BuildManagerOrgUnitFilter(query.ManagerOrgUnitUID);
      string operationSourceFilter = BuildOperationSourceFilter(query.OperationSourceUID);
      string statusFilter = BuildStatusFilter(query.Status);

      string tagsFilter = BuildTagsFilter(query.Tags);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);

      var filter = new Filter(transactionTypeFilter);

      filter.AppendAnd(assignedToFilter);
      filter.AppendAnd(assignedToOrgUnitFilter);
      filter.AppendAnd(locationFilter);
      filter.AppendAnd(managerFilter);
      filter.AppendAnd(managerOrgUnitFilter);
      filter.AppendAnd(operationSourceFilter);
      filter.AppendAnd(statusFilter);
      filter.AppendAnd(tagsFilter);
      filter.AppendAnd(keywordsFilter);

      return filter.ToString();
    }

    static internal string MapToSortString(this AssetTransactionQuery query) {
      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      } else {
        return "ASSET_TXN_NO, ASSET_TXN_APPLICATION_TIME, ASSET_TXN_REQUESTED_TIME";
      }
    }

    #endregion Extension methods

    #region Helpers

    static private string BuildAssignedToFilter(string assignedToUID) {
      if (assignedToUID.Length == 0) {
        return string.Empty;
      }

      var assignedTo = Person.Parse(assignedToUID);

      return $"ASSET_TXN_ASSIGNED_TO_ID = {assignedTo.Id}";
    }


    static private string BuildAssignedToOrgUnitFilter(string assignedToOrgUnitUID) {
      if (assignedToOrgUnitUID.Length == 0) {
        return string.Empty;
      }

      var assginedToOrgUnit = OrganizationalUnit.Parse(assignedToOrgUnitUID);

      return $"ASSET_TXN_ASSIGNED_TO_ORG_UNIT_ID = {assginedToOrgUnit.Id}";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("ASSET_TXN_KEYWORDS", keywords);
    }


    static private string BuildLocationFilter(AssetTransactionQuery query) {
      if (query.BuildingUID.Length == 0) {
        return string.Empty;
      }

      Location location;

      if (query.PlaceUID.Length != 0) {
        location = Location.Parse(query.PlaceUID);

        return $"ASSET_TXN_LOCATION_ID = {location.Id}";
      }

      if (query.FloorUID.Length != 0) {
        location = Location.Parse(query.FloorUID);
      } else {
        location = Location.Parse(query.BuildingUID);
      }

      FixedList<Location> locations = location.GetAllChildren();

      var locationIds = locations.Select(x => x.Id).ToFixedList().ToArray();

      return SearchExpression.ParseInSet("ASSET_TXN_LOCATION_ID", locationIds);
    }


    static private string BuildManagerFilter(string managerUID) {
      if (managerUID.Length == 0) {
        return string.Empty;
      }

      var manager = Person.Parse(managerUID);

      return $"ASSET_TXN_MGR_ID = {manager.Id}";
    }


    static private string BuildManagerOrgUnitFilter(string managerOrgUnitUID) {
      if (managerOrgUnitUID.Length == 0) {
        return string.Empty;
      }

      var managerOrgUnit = OrganizationalUnit.Parse(managerOrgUnitUID);

      return $"ASSET_TXN_MGR_ORG_UNIT_ID = {managerOrgUnit.Id}";
    }


    static private string BuildOperationSourceFilter(string operationSourceUID) {
      if (operationSourceUID.Length == 0) {
        return string.Empty;
      }

      var operationSource = OperationSource.Parse(operationSourceUID);

      return $"ASSET_TXN_SOURCE_ID = {operationSource.Id}";
    }


    static private string BuildStatusFilter(TransactionStatus status) {
      if (status == TransactionStatus.All) {
        return "ASSET_TXN_STATUS <> 'X' ";
      }

      return $"ASSET_TXN_STATUS = '{(char) status}'";
    }


    static private string BuildTagsFilter(string[] tags) {
      if (tags.Length == 0) {
        return string.Empty;
      }

      return string.Empty;

      //var filter = SearchExpression.ParseOrLikeKeywords("PRODUCT_TAGS", string.Join(" ", tags));

      //return $"({filter})";
    }


    static private string BuildTransactionTypeFilter(string transactionTypeUID) {
      if (transactionTypeUID.Length == 0) {
        return string.Empty;
      }

      var transactionType = AssetTransactionType.Parse(transactionTypeUID);

      return $"ASSET_TXN_TYPE_ID = {transactionType.Id}";
    }

    #endregion Helpers

  }  // class AssetTransactionQueryExtensions

}  // namespace Empiria.Inventory.Assets.Adapters
