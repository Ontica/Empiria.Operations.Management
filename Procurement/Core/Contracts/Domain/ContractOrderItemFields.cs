/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Information Holder                      *
*  Type     : ContractOrderItemFields                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to update procurment contract order's items.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders;

namespace Empiria.Procurement.Contracts {

  /// <summary>Input fields DTO used to update procurment contract's order items.</summary>
  public class ContractOrderItemFields : PayableOrderItemFields {

    public override void EnsureValid() {
      base.EnsureValid();

      Assertion.Require(ContractItemUID, nameof(ContractItemUID));
    }

  }  // class ContractOrderItemFields

}  // namespace Empiria.Procurement.Contracts
