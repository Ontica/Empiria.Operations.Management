/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Fields DTO                              *
*  Type     : OrderItemFields                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract DTO fields structure used for update order items information.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Products;

namespace Empiria.Orders {

  /// <summary>Abstract DTO fields structure used for update order items information.</summary>
  abstract public class OrderItemFields {

    public string ProductUID {
      get; set;
    } = string.Empty;


    public string ProductCode {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string Justification {
      get; set;
    } = string.Empty;


    public string ProductUnitUID {
      get; set;
    } = string.Empty;


    public decimal RequestedQty {
      get; set;
    }


    public decimal Quantity {
      get; set;
    } = 1;


    public decimal UnitPrice {
      get; set;
    }

    [Newtonsoft.Json.JsonProperty(PropertyName = "Total")]
    public decimal Subtotal {
      get; set;
    }

    public decimal Discount {
      get; set;
    }

    public decimal PenaltyDiscount {
      get; set;
    }

    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public string BudgetUID {
      get; set;
    } = string.Empty;


    public string BudgetAccountUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; set;
    } = string.Empty;


    public string RequisitionUID {
      get; set;
    } = string.Empty;


    public string RequisitionItemUID {
      get; set;
    } = string.Empty;


    public string RelatedItemUID {
      get; set;
    } = string.Empty;


    public string OriginCountryUID {
      get; set;
    } = string.Empty;


    public DateTime? StartDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public DateTime? EndDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public DateTime RequiredTime {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public string ProductName {
      get; set;
    } = string.Empty;


    public string ContractItemUID {
      get; set;
    } = string.Empty;


    public string RequestedByUID {
      get; set;
    } = string.Empty;


    public string ResponsibleUID {
      get; set;
    } = string.Empty;


    public string BeneficiaryUID {
      get; set;
    } = string.Empty;


    public string ReceivedByUID {
      get; set;
    } = string.Empty;


    public string LocationUID {
      get; set;
    } = string.Empty;


    public virtual void EnsureValid() {
      ProductUID = Patcher.CleanUID(ProductUID);
      ProductUnitUID = Patcher.CleanUID(ProductUnitUID);
      RequestedByUID = Patcher.CleanUID(RequestedByUID);
      BudgetAccountUID = Patcher.CleanUID(BudgetAccountUID);
      Description = Patcher.PatchClean(Description, string.Empty);

      Assertion.Require(ProductUID.Length != 0 || BudgetAccountUID.Length != 0 || Description.Length != 0,
                        "Necesito se proporcione el producto, su cuenta presupuestal o su descripción.");

      if (Subtotal > 0 && Quantity == 0) {
        Quantity = Subtotal;
        UnitPrice = 1;
      }

      Assertion.Require(ProductUnitUID, "Se requiere proporcionar la unidad de medida.");

      var productUnit = ProductUnit.Parse(ProductUnitUID);

      if (productUnit.MoneyBased) {
        Assertion.Require(UnitPrice == 1m, "El precio unitario debe ser igual a uno.");
        Quantity = Subtotal;

      } else {
        Assertion.Require(UnitPrice > 0, "El precio unitario debe ser mayor a cero.");
      }

      Assertion.Require(Quantity > 0, "Necesito se proporcione la cantidad mínima.");

      Assertion.Require(UnitPrice > 0, "El precio unitario debe ser mayor a cero.");
      Assertion.Require(Discount >= 0, "El descuento no puede ser negativo.");
      Assertion.Require(PenaltyDiscount >= 0, "El descuento por penalización no puede ser negativo.");

      if (!StartDate.HasValue) {
        StartDate = ExecutionServer.DateMaxValue;
      }
      if (!EndDate.HasValue) {
        EndDate = ExecutionServer.DateMaxValue;
      }

      Assertion.Require(StartDate.Value <= EndDate.Value,
                        $"La fecha final del período o vigencia debe ser " +
                        $"posterior a la fecha inicial.");
    }

  }  // class OrderItemFields

}  // namespace Empiria.Orders
