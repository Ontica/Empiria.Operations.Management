/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Extension methods                       *
*  Type     : FixedAssetTransactionQueryExtensions       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for FixedAssetTransactionQuery interface adapter.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Inventory.FixedAssets.Adapters {

  /// <summary>Extension methods for FixedAssetTransactionQuery interface adapter.</summary>
  static internal class FixedAssetTransactionQueryExtensions {

    #region Extension methods

    static internal void EnsureIsValid(this FixedAssetTransactionQuery query) {
      // no-op
    }


    static internal string MapToFilterString(this FixedAssetTransactionQuery query) {
      string transactionTypeFilter = BuildTransactionTypeFilter(query.TransactionTypeUID);
      string basePartyFilter = BuildBasePartyFilter(query.BasePartyUID);
      string operationSourceFilter = BuildOperationSourceFilter(query.OperationSourceUID);

      string tagsFilter = BuildTagsFilter(query.Tags);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string statusFilter = BuildStatusFilter(query.Status);

      var filter = new Filter(transactionTypeFilter);

      filter.AppendAnd(basePartyFilter);
      filter.AppendAnd(operationSourceFilter);
      filter.AppendAnd(tagsFilter);
      filter.AppendAnd(keywordsFilter);
      filter.AppendAnd(statusFilter);

      return filter.ToString();
    }


    static internal string MapToSortString(this FixedAssetTransactionQuery query) {
      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      } else {
        return "OMS_TXN_NUMBER, OMS_TXN_APPLICATION_DATE, OMS_TXN_REQUESTED_TIME";
      }
    }

    #endregion Extension methods

    #region Helpers

    static private string BuildBasePartyFilter(string basePartyUID) {
      if (basePartyUID.Length == 0) {
        return string.Empty;
      }

      var baseParty = Party.Parse(basePartyUID);

      return $"OMS_TXN_BASE_PARTY_ID = {baseParty.Id}";
    }


    static private string BuildOperationSourceFilter(string operationSourceUID) {
      if (operationSourceUID.Length == 0) {
        return string.Empty;
      }

      var operationSource = OperationSource.Parse(operationSourceUID);

      return $"OMS_TXN_SOURCE_ID = {operationSource.Id}";
    }


    static private string BuildTransactionTypeFilter(string transactionTypeUID) {
      if (transactionTypeUID.Length == 0) {
        return string.Empty;
      }

      var transactionType = FixedAssetTransactionType.Parse(transactionTypeUID);

      return $"OMS_TXN_TYPE_ID = {transactionType.Id}";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("OMS_TXN_KEYWORDS", keywords);
    }


    static private string BuildStatusFilter(TransactionStatus status) {
      if (status == TransactionStatus.All) {
        return "OMS_TXN_STATUS <> 'X' ";
      }

      return $"OMS_TXN_STATUS = '{(char) status}'";
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

  }  // class FixedAssetTransactionQueryExtensions

}  // namespace Empiria.Budgeting.Transactions.Adapters
