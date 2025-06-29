/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Data Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data service                            *
*  Type     : AssetsAssignationsData                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read methods for asset assignations.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Inventory.Assets.Data {

  /// <summary>Provides data read methods for asset assignations.</summary>
  static internal class AssetsAssignationsData {

    #region Methods

    static internal FixedList<AssetAssignation> SearchAssignations(string filter, string sortBy) {
      var sql = "SELECT DISTINCT Asset_Assigned_To_Id, Party_Name Asset_Assigned_To, " +
                  "Asset_Assigned_To_Org_Unit_Id, Asset_Location_Id " +
                "FROM OMS_Assets INNER JOIN OMS_Products_SKUS " +
                  "ON OMS_Assets.Asset_SKU_ID = OMS_Products_SKUS.SKU_ID " +
                "INNER JOIN Parties ON OMS_Assets.Asset_Assigned_To_Id = Parties.Party_Id";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<AssetAssignation>(op);
    }

    #endregion Methods

  }  // class AssetsAssignationsData

}  // namespace Empiria.Inventory.Assets.Data
