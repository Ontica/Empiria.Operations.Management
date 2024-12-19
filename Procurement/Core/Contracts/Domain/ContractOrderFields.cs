/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Fields DTO                              *
*  Type     : ContractOrderFields                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used for update contract supply orders.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders;

namespace Empiria.Procurement.Contracts {

  /// <summary>Input fields DTO used for update contract supply orders.</summary>
  public class ContractOrderFields : PayableOrderFields {

    public string ContractUID {
      get; set;
    } = string.Empty;


    internal override void EnsureValid() {
      base.EnsureValid();

      Assertion.Require(ContractUID, nameof(ContractUID));
    }

  }  // class ContractOrderFields

}  // namespace Empiria.Procurement.Contracts.Adapters
