/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                       Component : Data Layer                              *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Data service                            *
*  Type     : SuppliersData                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for Supplier instances.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

using Empiria.Parties;

namespace Empiria.Procurement.Suppliers.Data {

  /// <summary>Provides data read and write methods for Supplier instances.</summary>
  static internal class SuppliersData {

    static internal FixedList<Party> GetSuppliers(string filter, string sortBy) {
      var sql = "SELECT * FROM Parties";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Party>(op);
    }

  }  // class SuppliersData

}  // namespace Empiria.Procurement.Suppliers.Data
