/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Data Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data service                            *
*  Type     : AssetsAssignmentsData                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read methods for asset assignments.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Inventory.Assets.Data {

  /// <summary>Provides data read methods for asset assignments.</summary>
  static internal class AssetsAssignmentsData {

    #region Methods

    static internal FixedList<Asset> GetAssets(AssetAssignment assignment) {
      var sql = "SELECT OMS_Assets.* FROM OMS_Assets " +
                $"WHERE Asset_Last_Asgmt_Txn_Id = {assignment.LastAssignment.Id} AND " +
                $"Asset_Status <> 'X' " +
                "ORDER BY Asset_No";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Asset>(op);
    }


    static internal FixedList<AssetAssignment> SearchAssignments(string filter, string sortBy) {
      var sql = "SELECT DISTINCT Asset_Last_Asgmt_Txn_Id, Asset_Location_Id " +
                "FROM OMS_Assets";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<AssetAssignment>(op)
                       .Sort((x, y) => $"{x.AssignedTo.Name}|{x.AssignedToOrgUnit.Code}|{x.Location.FullName}"
                                       .CompareTo($"{y.AssignedTo.Name}|{y.AssignedToOrgUnit.Code}|{y.Location.FullName}"));
    }

    #endregion Methods

  }  // class AssetsAssignmentsData

}  // namespace Empiria.Inventory.Assets.Data
