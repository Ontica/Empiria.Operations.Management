/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Data Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data service                            *
*  Type     : AssetsData                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for asset instances.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;
using Empiria.Parties;

namespace Empiria.Inventory.Assets.Data {

  /// <summary>Provides data read and write methods for asset instances.</summary>
  static internal class AssetsData {

    #region Methods

    static internal FixedList<Person> GetAssetsAssignees() {
      var sql = "SELECT DISTINCT * FROM Parties " +
                "WHERE Party_ID IN (SELECT Asset_Assigned_To_ID " +
                                   "FROM OMS_Assets " +
                                   "WHERE Asset_Status <> 'X') " +
                "ORDER BY Party_Name";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Person>(op);
    }


    static internal FixedList<Asset> SearchAssets(string filter, string sortBy) {
      var sql = "SELECT OMS_Assets.* " +
                "FROM OMS_Assets INNER JOIN OMS_Products_SKUS " +
                "ON OMS_Assets.Asset_SKU_ID = OMS_Products_SKUS.SKU_ID";

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

      var op = DataOperation.Parse("write_OMS_Asset", o.Id, o.UID, o.AssetType.Id,
        o.Sku.Id, o.Description, EmpiriaString.Tagging(o.Identificators), EmpiriaString.Tagging(o.Tags),
        o.Manager.Id, o.ManagerOrgUnit.Id, o.AssignedTo.Id, o.AssignedToOrgUnit.Id,
        o.Location.Id, o.Condition, accountingData, extensionData, o.Keywords,
        o.StartDate, o.EndDate, o.LastUpdate, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class AssetsData

}  // namespace Empiria.Inventory.Assets.Data
