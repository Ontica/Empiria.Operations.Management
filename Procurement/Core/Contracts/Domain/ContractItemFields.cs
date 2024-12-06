/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Fields DTO                              *
*  Type     : ContractItemFields                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO fields structure used for update contracts information.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Procurement.Contracts {

  /// <summary>DTO fields structure used for update contracts item information.</summary>
  public class ContractItemFields {

    public string ContractItemTypeUID {
      get; set;
    } = string.Empty;


    public string ProductUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string ProductUnitUID {
      get; set;
    } = string.Empty;


    public decimal UnitPrice {
      get; set;
    }


    public decimal MinQuantity {
      get; set;
    }


    public decimal MaxQuantity {
      get; set;
    }

    public string RequisitionItemUID {
      get; set;
    } = string.Empty;


    public string RequesterOrgUnitUID {
      get; set;
    } = string.Empty;


    public string BudgetAccountUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; set;
    } = string.Empty;


    public string SupplierUID {
      get; set;
    } = string.Empty;


    public string PeriodicityTypeUID {
      get; set;
    } = string.Empty;


    internal void EnsureValid() {
      Assertion.Require(ProductUID, "Necesito se proporcione el producto.");
      Assertion.Require(ProductUnitUID, "Necesito la unidad de medida del producto.");
      Assertion.Require(UnitPrice > 0, "Necesito el precio unitario.");
      Assertion.Require(MinQuantity > 0, "Necesito se proporcione la cantidad mínima.");
      Assertion.Require(MaxQuantity > 0, "Necesito se proporcione la cantidad máxima.");
      Assertion.Require(MinQuantity <= MaxQuantity,
                       "La cantidad máxima no puede ser menor a la cantidad mínima.");
    }

  }  // class ContractItemFields

}  // namespace Empiria.Procurement.Contracts
