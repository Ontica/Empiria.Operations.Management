﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Data Layer                              *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Data service                            *
*  Type     : ContractOrdersData                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for contract's supply orders.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Procurement.Contracts.Data {

  /// <summary>Provides data read and write methods for contract's supply orders.</summary>
  static internal class ContractOrdersData {

    #region Methods

    static internal FixedList<ContractOrder> GetContractOrders(Contract contract) {
      var sql = "SELECT * FROM OMS_ORDERS " +
                $"WHERE ORDER_CONTRACT_ID = {contract.Id} AND " +
                 "ORDER_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<ContractOrder>(op);
    }


    static internal void WriteOrder(ContractOrder o, string extensionData) {
      var op = DataOperation.Parse("write_OMS_Order",
                     o.Id, o.UID, o.OrderType.Id, o.Category.Id, o.OrderNo, o.Description,
                     string.Join(" ", o.Identificators), string.Join(" ", o.Tags),
                     o.RequestedBy.Id, o.Responsible.Id, o.Beneficiary.Id, o.Provider.Id,
                     o.Budget.Id, o.RequisitionId, o.Contract.Id, o.Project.Id, o.Currency.Id,
                     o.Source.Id, (char) o.Priority, extensionData, o.Keywords,
                     o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class ContractOrdersData

}  // namespace Empiria.Procurement.Contracts.Data
