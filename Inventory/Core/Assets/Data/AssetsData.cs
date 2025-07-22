/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Data Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data service                            *
*  Type     : AssetsData                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for asset instances.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Data;
using Empiria.Parties;

namespace Empiria.Inventory.Assets.Data {

  /// <summary>Provides data read and write methods for asset instances.</summary>
  static internal class AssetsData {

    static internal void Clean(Asset asset) {
      if (asset.IsEmptyInstance) {
        return;
      }
      var sql = "UPDATE OMS_ASSETS " +
                $"SET ASSET_UID = '{Guid.NewGuid().ToString()}', " +
                $"ASSET_KEYWORDS = '{asset.Keywords}', " +
                $"ASSET_START_DATE = {DataCommonMethods.FormatSqlDbDate(new DateTime(2025, 07, 04))}, " +
                $"ASSET_POSTING_TIME = {DataCommonMethods.FormatSqlDbDate(new DateTime(2025, 06, 25))}, " +
                $"ASSET_END_DATE = {DataCommonMethods.FormatSqlDbDate(new DateTime(2078, 12, 31))}, " +
                $"ASSET_LAST_UPDATE = {DataCommonMethods.FormatSqlDbDate(new DateTime(2025, 07, 04))}, " +
                $"ASSET_POSTED_BY_ID = 152 " +
                $"WHERE ASSET_ID = {asset.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }

    #region Methods

    static internal FixedList<Person> GetAssetsAssignees(string keywords) {
      var sql = "SELECT DISTINCT * FROM Parties " +
                $"WHERE {SearchExpression.ParseAndLikeKeywords("PARTY_KEYWORDS", keywords)} " +
                "AND Party_ID IN (SELECT Last_Asgmt_Assigned_To_Id " +
                                   "FROM vw_OMS_Assets " +
                                   "WHERE Asset_Status <> 'X') " +
                "ORDER BY Party_Name";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Person>(op);
    }


    static internal FixedList<Asset> SearchAssets(string filter, string sortBy) {
      var sql = "SELECT * FROM vw_OMS_Assets";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Asset>(op);
    }


    static internal void WriteAsset(Asset o, string accountingData, string extensionData) {

      var op = DataOperation.Parse("write_OMS_Asset", o.Id, o.UID, o.AssetType.Id, o.AssetNo,
        o.SkuId, o.Description, EmpiriaString.Tagging(o.Identificators), EmpiriaString.Tagging(o.Tags),
        o.Manager.Id, o.ManagerOrgUnit.Id, o.CurrentLocation.Id, o.CurrentCondition, (int) o.InUse,
        o.LastAssignmentEntryId, accountingData, extensionData, o.Keywords,
        o.StartDate, o.EndDate, o.LastUpdate, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class AssetsData

}  // namespace Empiria.Inventory.Assets.Data
