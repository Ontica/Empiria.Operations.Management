/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Fields DTO                              *
*  Type     : OrderItemFields                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract DTO fields structure used for update order items information.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders {

  /// <summary>Abstract DTO fields structure used for update order items information.</summary>
  abstract public class OrderItemFields {

    public string ProductUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string ProductUnitUID {
      get; set;
    } = string.Empty;


    public decimal Quantity {
      get; set;
    } = 1;

    public decimal UnitPrice {
      get; set;
    }

    public decimal Discount {
      get; set;
    }

    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public string BudgetAccountUID {
      get; set;
    } = string.Empty;


    public string RequestedByUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; set;
    } = string.Empty;


    public string ProviderUID {
      get; set;
    } = string.Empty;


    public virtual void EnsureValid() {
      Assertion.Require(ProductUID, "Necesito se proporcione el producto.");
      Assertion.Require(Quantity > 0, "Necesito se proporcione la cantidad mínima.");
    }

  }  // class OrderItemFields

}  // namespace Empiria.Orders
