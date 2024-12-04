/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Data Layer                              *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Data service                            *
*  Type     : ContractOrdersData                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for contract supply orders.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Procurement.Contracts.Data {

  /// <summary>Provides data read and write methods for contract supply orders.</summary>
  static internal class ContractOrdersData {

    #region Methods

    static internal void WriteContractOrder(ContractOrder o, string extensionData) {
      var op = DataOperation.Parse("write_OMS_Order",
                     o.Id, o.UID, o.OrderType.Id, o.OrderNo, o.Description,
                     o.Contract.Id, extensionData,
                     o.Keywords, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class ContractOrdersData

}  // namespace Empiria.Procurement.Contracts.Data
