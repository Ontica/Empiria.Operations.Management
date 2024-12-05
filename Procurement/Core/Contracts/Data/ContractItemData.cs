/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Data Layer                              *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Data service                            *
*  Type     : ContractItemData                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for contract item instances.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Data;

namespace Empiria.Procurement.Contracts.Data {

  /// <summary>Provides data read and write methods for contract item instances.</summary>
  static internal class ContractItemData {

    static internal List<ContractItem> GetContractItems(Contract contract) {
      Assertion.Require(contract, nameof(contract));

      var sql = "SELECT * FROM OMS_CONTRACT_ITEMS " +
                $"WHERE CONTRACT_ITEM_CONTRACT_ID = {contract.Id} AND " +
                $"CONTRACT_ITEM_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<ContractItem>(op);
    }


    static internal void WriteContractItem(ContractItem o, string extensionData) {
      var op = DataOperation.Parse("write_OMS_Contract_Item",
                  o.Id, o.UID, o.ContractItemType.Id, o.Contract.Id,
                  o.Product.Id, o.Description, o.ProductUnit.Id,
                  o.UnitPrice, o.MinQuantity, o.MaxQuantity,
                  o.BudgetAccount.Id, o.Project.Id, o.Supplier.Id,
                  o.PeriodicityRule.ToString(), extensionData, o.Keywords,
                  o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class ContractData

}  // namespace Empiria.Procurement.Contracts.Data
