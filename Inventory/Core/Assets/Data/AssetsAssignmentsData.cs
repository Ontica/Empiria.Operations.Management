/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Data Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data service                            *
*  Type     : AssetsAssignmentsData                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read methods for asset assignments.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

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
using Empiria.Parties;

namespace Empiria.Inventory.Assets.Data {

  /// <summary>Provides data read methods for asset assignments.</summary>
  static internal class AssetsAssignmentsData {

    #region Methods

    static internal FixedList<Asset> GetAssets(AssetAssignment assignment) {
      var sql = "SELECT * FROM vw_OMS_Assets " +
                $"WHERE Last_Asgmt_Txn_Id = {assignment.Transaction.Id} AND " +
                $"Asset_Status <> 'X' " +
                "ORDER BY Asset_No";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Asset>(op);
    }


    static internal FixedList<AssetTransaction> GetTransactionsFor(Person assignedTo) {
      var sql = "SELECT * FROM OMS_Assets_Transactions " +
                $"WHERE (ASSET_TXN_ASSIGNED_TO_ID = {assignedTo.Id} OR " +
                $"ASSET_TXN_RELEASED_BY_ID = {assignedTo.Id}) AND " +
                $"ASSET_TXN_STATUS <> 'X' " +
                $"ORDER BY ASSET_TXN_NO DESC";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<AssetTransaction>(op);
    }


    static internal FixedList<AssetAssignment> SearchAssignments(string filter, string sortBy) {
      var sql = "SELECT DISTINCT Last_Asgmt_Txn_Id, Last_Asgmt_Location_Id " +
                "FROM vw_OMS_Assets";

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
