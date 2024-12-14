/* Empiria Operations ****************************************************************************************
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

    #endregion Methods

  }  // class ContractOrdersData

}  // namespace Empiria.Procurement.Contracts.Data
