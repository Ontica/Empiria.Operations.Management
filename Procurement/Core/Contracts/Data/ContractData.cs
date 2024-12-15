/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Data Layer                              *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Data service                            *
*  Type     : ContractData                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for contract instances.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Procurement.Contracts.Data {

  /// <summary>Provides data read and write methods for contract instances.</summary>
  static internal class ContractData {

    #region Methods

    static internal FixedList<Contract> GetContracts(string filter, string sortBy) {
      var sql = "SELECT * FROM vw_OMS_CONTRACTS ";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Contract>(dataOperation);
    }


    static internal void WriteContract(Contract o, string extensionData) {
      var op = DataOperation.Parse("write_OMS_Contract",
                     o.Id, o.UID, o.ContractType.Id, o.ContractCategory.Id, o.ContractNo, o.Name,
                     o.Description, o.Currency.Id, o.FromDate, o.ToDate, o.SignDate,
                     o.ManagedByOrgUnit.Id, o.BudgetType.Id, o.Customer.Id, o.Supplier.Id,
                     extensionData, o.Keywords, o.Parent.Id,
                     o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

   #endregion Methods

  }  // class ContractData

}  // namespace Empiria.Procurement.Contracts.Data
