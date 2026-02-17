/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Fields DTO                              *
*  Type     : ContractFields                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used for update contracts information.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Orders.Contracts {

  /// <summary>Input fields DTO used for update contracts information.</summary>
  public class ContractFields : OrderFields {

    public string ContractNo {
      get; set;
    } = string.Empty;


    public DateTime? SignDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public override void EnsureValid() {
      Assertion.Require(RequisitionUID, "Necesito se seleccione la requisición.");

      Assertion.Require(ContractNo.Length <= 36,
        "El número de contrato es muy largo. Favor de colocar su descripción en el campo correspondiente.");

      Assertion.Require(Name, "Necesito el nombre del contrato.");

      Assertion.Require(CurrencyUID, "Necesito la moneda del contrato.");
      Assertion.Require(Budgets.Length > 0, "Necesito se seleccione el presupuesto o presupuestos " +
                                            "asociados al contrato.");

      Assertion.Require(StartDate.HasValue, "Necesito la fecha de inicio del contrato.");
      Assertion.Require(EndDate.HasValue, "Necesito la fecha de término del contrato.");

      Assertion.Require(ProviderUID, "Necesito se proporcione al proveedor del contrato.");

      base.EnsureValid();
    }

  }  // class ContractFields

}  // namespace Empiria.Orders.Contracts
