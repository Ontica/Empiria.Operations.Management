/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Data Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data service                            *
*  Type     : AssetsTransactionsData                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for assets transactions instances.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Data;
using Empiria.Parties;

namespace Empiria.Inventory.Assets.Data {

  /// <summary>Provides data read and write methods for assets transactions instances.</summary>
  static internal class AssetsTransactionsData {

    #region Methods

    static internal void Clean(AssetTransaction txn) {
      if (txn.IsEmptyInstance) {
        return;
      }
      var sql = "UPDATE OMS_ASSETS_TRANSACTIONS " +
                $"SET ASSET_TXN_UID = '{Guid.NewGuid().ToString()}', " +
                $"ASSET_TXN_KEYWORDS = '{txn.Keywords}', " +
                $"ASSET_TXN_POSTED_BY_ID = 152 " +
                $"WHERE ASSET_TXN_ID = {txn.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    static internal void Clean(AssetTransactionEntry entry) {
      if (entry.IsEmptyInstance) {
        return;
      }
      var sql = "UPDATE OMS_ASSETS_ENTRIES " +
                $"SET ASSET_ENTRY_UID = '{Guid.NewGuid().ToString()}', " +
                $"ASSET_ENTRY_KEYWORDS = '{entry.Keywords}', " +
                $"ASSET_ENTRY_POSTED_BY_ID = 152 " +
                $"WHERE ASSET_ENTRY_ID = {entry.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    static internal string GenerateNextTransactionNo(AssetTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));

      string transactionPrefix = transaction.AssetTransactionType.Prefix;

      Assertion.Require(transactionPrefix,
          $"Undetermined asset transaction prefix for {transaction.AssetTransactionType.DisplayName}.");

      int year = transaction.ApplicationDate.Year;

      string prefix = $"{year}-AF-{transactionPrefix}";

      string sql = "SELECT MAX(ASSET_TXN_NO) " +
                   "FROM OMS_ASSETS_TRANSACTIONS " +
                   $"WHERE ASSET_TXN_NO LIKE '{prefix}-%'";

      string lastUniqueID = DataReader.GetScalar(DataOperation.Parse(sql), string.Empty);

      if (lastUniqueID.Length != 0) {

        int consecutive = int.Parse(lastUniqueID.Split('-')[3]) + 1;

        return $"{prefix}-{consecutive:00000}";

      } else {
        return $"{prefix}-00001";
      }
    }


    static internal FixedList<AssetTransaction> GetTransactions(Asset asset) {
      var sql = "SELECT * FROM OMS_Assets_Transactions " +
                "WHERE Asset_TXN_ID IN " +
                    $"(SELECT Asset_Entry_TXN_ID FROM OMS_Assets_Entries" +
                    $" WHERE Asset_Entry_Asset_ID = {asset.Id}" +
                    $" AND Asset_Entry_Status <> 'X') " +
         $"AND Asset_TXN_Status <> 'X' " +
         $"ORDER BY Asset_TXN_No";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<AssetTransaction>(op);
    }


    static internal FixedList<Person> GetTransactionsAssignees(string keywords) {
      var sql = "SELECT DISTINCT * FROM Parties " +
                "WHERE Party_ID IN " +
                    "(SELECT Asset_TXN_Assigned_To_ID" +
                    " FROM OMS_Assets_Transactions" +
                    " WHERE Asset_TXN_Status <> 'X') " +
                "{(KEYWORDS.FILTER}} " +
                "ORDER BY Party_Name";

      if (keywords.Length != 0) {
        sql = sql.Replace("{(KEYWORDS.FILTER}}",
                          $"AND {SearchExpression.ParseAndLikeKeywords("PARTY_KEYWORDS", keywords)}");
      } else {
        sql = sql.Replace("{(KEYWORDS.FILTER}}", string.Empty);
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Person>(op);
    }


    static internal FixedList<Person> GetTransactionsManagers(string keywords) {
      var sql = "SELECT DISTINCT * FROM Parties " +
                "WHERE Party_ID IN (SELECT Asset_TXN_Mgr_ID " +
                                   "FROM OMS_Assets_Transactions " +
                                   "WHERE Asset_TXN_Status <> 'X') " +
                "{(KEYWORDS.FILTER}} " +
                "ORDER BY Party_Name";

      if (keywords.Length != 0) {
        sql = sql.Replace("{(KEYWORDS.FILTER}}",
                          $"AND {SearchExpression.ParseAndLikeKeywords("PARTY_KEYWORDS", keywords)}");
      } else {
        sql = sql.Replace("{(KEYWORDS.FILTER}}", string.Empty);
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Person>(op);
    }


    static internal List<AssetTransactionEntry> GetTransactionEntries(AssetTransaction transaction) {
      var sql = "SELECT * FROM OMS_Assets_Entries " +
               $"WHERE Asset_Entry_TXN_ID = {transaction.Id} AND " +
                     $"Asset_Entry_Status <> 'X' " +
               $"ORDER BY Asset_Entry_ID";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<AssetTransactionEntry>(op);
    }


    static internal FixedList<AssetTransaction> SearchTransactions(string filter, string sortBy) {
      var sql = "SELECT * FROM OMS_Assets_Transactions";

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

      var op = DataOperation.Parse("write_OMS_Asset_Transaction", o.Id, o.UID, o.AssetTransactionType.Id,
        o.TransactionNo, o.Description, EmpiriaString.Tagging(o.Identificators), EmpiriaString.Tagging(o.Tags),
        o.AssignedTo.Id, o.AssignedToOrgUnit.Id, o.ReleasedBy.Id, o.ReleasedByOrgUnit.Id, o.BaseLocation.Id,
        o.OperationSource.Id, o.ApplicationDate, o.AppliedBy.Id, o.RecordingDate, o.RecordedBy.Id,
        o.AuthorizationTime, o.AuthorizedBy.Id, o.RequestedTime, o.RequestedBy.Id,
        extensionData, o.Keywords, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteAssetTransactionEntry(AssetTransactionEntry o,
                                                    string operationData,
                                                    string extensionData) {

      var op = DataOperation.Parse("write_OMS_Asset_Entry", o.Id, o.UID,
        o.AssetTransactionEntryType.Id, o.Transaction.Id,
        o.Asset.Id, o.Description, o.OperationId, operationData, extensionData,
        o.Keywords, o.Position, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    #endregion Methods

  }  // class AssetsTransactionsData

}  // namespace Empiria.Inventory.Assets.Data
