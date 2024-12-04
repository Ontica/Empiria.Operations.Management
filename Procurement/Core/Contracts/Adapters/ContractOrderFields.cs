/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Fields DTO                              *
*  Type     : ContractOrderFields                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used for update contract supply orders information.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Input fields DTO used for update contract milestones information.</summary>
  public class ContractOrderFields {

    public string ContractUID {
      get; set;
    }

    internal void EnsureValid() {
      Assertion.Require(ContractUID, "Se requiere se proporcione el contrato.");
    }

  }  // class ContractOrderFields

}  // namespace Empiria.Procurement.Contracts.Adapters
