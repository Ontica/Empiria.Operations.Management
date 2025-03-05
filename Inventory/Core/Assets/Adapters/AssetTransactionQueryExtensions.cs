/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Extension methods                       *
*  Type     : AssetTransactionQueryExtensions            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for AssetTransactionQuery interface adapter.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Extension methods for AssetTransactionQuery interface adapter.</summary>
  static internal class AssetTransactionQueryExtensions {

    #region Extension methods

    static internal void EnsureIsValid(this AssetTransactionQuery query) {
      // no-op
    }


    static internal string MapToFilterString(this AssetTransactionQuery query) {
      string transactionTypeFilter = BuildTransactionTypeFilter(query.TransactionTypeUID);
      string managerOrgUnitFilter = BuildManagerOrgUnitFilter(query.ManagerOrgUnitUID);
      string operationSourceFilter = BuildOperationSourceFilter(query.OperationSourceUID);

      string tagsFilter = BuildTagsFilter(query.Tags);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string statusFilter = BuildStatusFilter(query.Status);

      var filter = new Filter(transactionTypeFilter);

      filter.AppendAnd(managerOrgUnitFilter);
      filter.AppendAnd(operationSourceFilter);
      filter.AppendAnd(tagsFilter);
      filter.AppendAnd(keywordsFilter);
      filter.AppendAnd(statusFilter);

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

    static private string BuildManagerOrgUnitFilter(string managerOrgUnitUID) {
      if (managerOrgUnitUID.Length == 0) {
        return string.Empty;
      }

      var baseParty = OrganizationalUnit.Parse(managerOrgUnitUID);

      return $"ASSET_TXN_MGR_ORG_UNIT_ID = {baseParty.Id}";
    }


    static private string BuildOperationSourceFilter(string operationSourceUID) {
      if (operationSourceUID.Length == 0) {
        return string.Empty;
      }

      var operationSource = OperationSource.Parse(operationSourceUID);

      return $"ASSET_TXN_SOURCE_ID = {operationSource.Id}";
    }


    static private string BuildTransactionTypeFilter(string transactionTypeUID) {
      if (transactionTypeUID.Length == 0) {
        return string.Empty;
      }

      var transactionType = AssetTransactionType.Parse(transactionTypeUID);

      return $"ASSET_TXN_TYPE_ID = {transactionType.Id}";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("ASSET_TXN_KEYWORDS", keywords);
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

    #endregion Helpers

  }  // class AssetTransactionQueryExtensions

}  // namespace Empiria.Inventory.Assets.Adapters
