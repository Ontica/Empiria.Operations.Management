/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Data Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data service                            *
*  Type     : FixedAssetsData                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for fixed asset instances.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using Empiria.Data;

namespace Empiria.Inventory.FixedAssets.Data {

  /// <summary>Provides data read and write methods for fixed asset instances.</summary>
  static internal class FixedAssetsData {

    #region Methods

    static internal FixedList<FixedAsset> GetFixedAssets(string filter, string sortBy) {
      var sql = "SELECT * FROM OMS_FIXED_ASSETS";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FixedAsset>(dataOperation);

    }


    static internal FixedList<FixedAssetTransaction> GetTransactions(FixedAsset fixedAsset) {
      var sql = "SELECT * FROM OMS_TRANSACTIONS " +
         $"WHERE OMS_TXN_ID IN " +
            $"(SELECT OMS_TXN_ENTRY_TXN_ID FROM OMS_TRANSACTION_ENTRIES " +
              $"WHERE OMS_TXN_ENTRY_OBJECT_ID = {fixedAsset.Id} " +
              $"AND OMS_TXN_ENTRY_STATUS <> 'X') " +
         $"AND OMS_TXN_STATUS <> 'X' " +
         $"ORDER BY OMS_TXN_NUMBER";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FixedAssetTransaction>(op);
    }


    static internal List<FixedAssetTransactionEntry> GetTransactionEntries(FixedAssetTransaction transaction) {
      var sql = "SELECT * FROM OMS_TRANSACTION_ENTRIES " +
         $"WHERE OMS_TXN_ENTRY_TXN_ID = {transaction.Id} AND " +
               $"OMS_TXN_ENTRY_STATUS <> 'X' " +
         $"ORDER BY OMS_TXN_ENTRY_ID";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<FixedAssetTransactionEntry>(op);
    }


    static internal FixedList<FixedAssetTransaction> SearchTransactions(string filter, string sort) {
      Assertion.Require(filter, nameof(filter));
      Assertion.Require(sort, nameof(sort));

      var sql = "SELECT * FROM OMS_TRANSACTIONS " +
               $"WHERE {filter} " +
               $"ORDER BY {sort}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FixedAssetTransaction>(op);
    }

    #endregion Methods

  }  // class FixedAssetsData

}  // namespace Empiria.Inventory.FixedAssets.Data
