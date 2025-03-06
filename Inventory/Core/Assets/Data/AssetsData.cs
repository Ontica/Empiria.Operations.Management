/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Data Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data service                            *
*  Type     : AssetsData                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for asset instances.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Data;
using Empiria.Parties;

namespace Empiria.Inventory.Assets.Data {

  /// <summary>Provides data read and write methods for asset instances.</summary>
  static internal class AssetsData {

    #region Methods

    static internal FixedList<Person> GetAssetsAssignees() {
      var sql = "SELECT DISTINCT * FROM PARTIES " +
                "WHERE PARTY_ID IN (SELECT ASSET_ASSIGNED_TO_ID " +
                                   "FROM OMS_ASSETS " +
                                   "WHERE ASSET_STATUS <> 'X') " +
                "ORDER BY PARTY_NAME";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Person>(op);
    }


    static internal FixedList<AssetTransaction> GetTransactions(Asset asset) {
      var sql = "SELECT * FROM OMS_ASSETS_TRANSACTIONS " +
         $"WHERE ASSET_TXN_ID IN " +
            $"(SELECT ASSET_ENTRY_TXN_ID FROM OMS_ASSETS_ENTRIES " +
              $"WHERE ASSET_ENTRY_ASSET_ID = {asset.Id} " +
              $"AND ASSET_ENTRY_STATUS <> 'X') " +
         $"AND ASSET_TXN_STATUS <> 'X' " +
         $"ORDER BY ASSET_TXN_NO";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<AssetTransaction>(op);
    }


    static internal FixedList<Person> GetTransactionsAssignees() {
      var sql = "SELECT DISTINCT * FROM PARTIES " +
                "WHERE PARTY_ID IN (SELECT ASSET_TXN_ASSIGNED_TO_ID " +
                                   "FROM OMS_ASSETS_TRANSACTIONS " +
                                   "WHERE ASSET_TXN_STATUS <> 'X') " +
                "ORDER BY PARTY_NAME";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Person>(op);
    }


    static internal List<AssetTransactionEntry> GetTransactionEntries(AssetTransaction transaction) {
      var sql = "SELECT * FROM OMS_ASSETS_ENTRIES " +
               $"WHERE ASSET_ENTRY_TXN_ID = {transaction.Id} AND " +
                     $"ASSET_ENTRY_STATUS <> 'X' " +
               $"ORDER BY ASSET_ENTRY_ID";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<AssetTransactionEntry>(op);
    }


    static internal FixedList<Asset> SearchAssets(string filter, string sortBy) {
      var sql = "SELECT OMS_ASSETS.* " +
                "FROM OMS_ASSETS INNER JOIN OMS_PRODUCTS_SKUS " +
                "ON OMS_ASSETS.ASSET_SKU_ID = OMS_PRODUCTS_SKUS.SKU_ID";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Asset>(op);
    }


    static internal FixedList<AssetTransaction> SearchTransactions(string filter, string sortBy) {
      var sql = "SELECT * FROM OMS_ASSETS_TRANSACTIONS";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<AssetTransaction>(op);
    }

    #endregion Methods

  }  // class AssetsData

}  // namespace Empiria.Inventory.Assets.Data
