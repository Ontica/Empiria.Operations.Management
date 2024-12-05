﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Fields DTO                              *
*  Type     : ContractItemFields                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO fields structure used for update contracts information.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>DTO fields structure used for update contracts item information.</summary>
  public class ContractItemFields {

    public string ProductUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string UnitMeasureUID {
      get; set;
    } = string.Empty;


    public decimal FromQuantity {
      get; set;
    }


    public decimal ToQuantity {
      get; set;
    }


    public decimal UnitPrice {
      get; set;
    }


    public string ProjectUID {
      get; set;
    } = string.Empty;


    public string PaymentPeriodicityUID {
      get; set;
    } = string.Empty;


    public string BudgetAccountUID {
      get; set;
    } = string.Empty;


    public string DocumentTypeUID {
      get; set;
    } = string.Empty;


    public decimal Total {
      get; set;
    }


    internal void EnsureValid() {
      Assertion.Require(ProductUID, "Se requiere del número de producto.");
      Assertion.Require(Description, "Necesito el nombre del contrato.");
      Assertion.Require(UnitMeasureUID, "Necesito la unidad de medida.");
      Assertion.Require(UnitPrice > 0, "Necesito el precio unitario.");
      Assertion.Require(FromQuantity > 0, "Necesito la cantidad de medida inicial.");
      Assertion.Require(ToQuantity > 0, "Necesito la cantidad de medida final.");

    }

  }  // class ContractItemFields

}  // namespace Empiria.Procurement.Contracts.Adapters
