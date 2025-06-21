/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Fields DTO                              *
*  Type     : PayableOrderItemFields                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO fields structure used for update payable order items.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders {

  /// <summary>DTO fields structure used for update payable order items.</summary>
  public class PayableOrderItemFields : OrderItemFields {

    public decimal UnitPrice {
      get; set;
    }

    public decimal Discount {
      get; set;
    }

    public string CurrencyUID {
      get; internal set;
    } = string.Empty;


    public string BudgetAccountUID {
      get; set;
    } = string.Empty;


    public override void EnsureValid() {
      base.EnsureValid();

      Assertion.Require(UnitPrice > 0, "El precio unitario debe ser mayor a cero.");
      Assertion.Require(Discount >= 0, "El descuento no puede ser negativo.");
      Assertion.Require(((Quantity * UnitPrice) - Discount) >= 0, "El total del concepto no puede ser negativo.");
      Assertion.Require(BudgetAccountUID, "Necesito la cuenta presupuestal a la que se aplicará el concepto.");
    }

  }  // class PayableOrderItemFields

}  // namespace Empiria.Orders
