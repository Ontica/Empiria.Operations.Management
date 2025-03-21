/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Data Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data service                            *
*  Type     : AssetsTransactionsData                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for assets transactions instances.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Data;
using Empiria.Parties;

namespace Empiria.Inventory.Assets.Data {

  /// <summary>Provides data read and write methods for assets transactions instances.</summary>
  static internal class AssetsTransactionsData {

    #region Methods

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


    static internal FixedList<Person> GetTransactionsManagers() {
      var sql = "SELECT DISTINCT * FROM PARTIES " +
          "WHERE PARTY_ID IN (SELECT ASSET_TXN_MGR_ID " +
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


    static internal void WriteAssetTransaction(AssetTransaction o, string extensionData) {

      var op = DataOperation.Parse("WRITE_OMS_ASSET_TRANSACTION", o.Id, o.UID, o.AssetTransactionType.Id,
        o.TransactionNo, o.Description, string.Join(" ", o.Identificators), string.Join(" ", o.Tags),
        o.Manager.Id, o.ManagerOrgUnit.Id, o.AssignedTo.Id, o.AssignedToOrgUnit.Id,
        o.Location.Id, o.OperationSource.Id, o.RequestedTime, o.RequestedBy.Id,
        o.ApplicationTime, o.AppliedBy.Id, o.RecordingTime, o.RecordedBy.Id,
        extensionData, o.Keywords, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteAssetTransactionEntry(AssetTransactionEntry o,
                                                    string operationData,
                                                    string extensionData) {

      var op = DataOperation.Parse("WRITE_OMS_ASSET_ENTRY", o.Id, o.UID, o.AssetTransactionEntryType.Id,
        o.Transaction.Id, o.Asset.Id, o.Description, o.OperationId, operationData, extensionData,
        o.Keywords, o.Position, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class AssetsTransactionsData

}  // namespace Empiria.Inventory.Assets.Data
