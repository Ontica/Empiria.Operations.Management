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

    public decimal Discount {
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


    public string ProviderUID {
      get; set;
    } = string.Empty;


    public string RequestedByUID {
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


    public DateTime StartDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public DateTime EndDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public DateTime RequiredTime {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public virtual void EnsureValid() {
      ProductUID = Patcher.CleanUID(ProductUID);
      BudgetAccountUID = Patcher.CleanUID(BudgetAccountUID);
      Description = Patcher.PatchClean(Description, string.Empty);

      Assertion.Require(ProductUID.Length != 0 || BudgetAccountUID.Length != 0 || Description.Length != 0,
                        "Necesito se proporcione el producto, su cuenta presupuestal o su descripción.");
      Assertion.Require(Quantity > 0, "Necesito se proporcione la cantidad mínima.");
      Assertion.Require(StartDate <= EndDate,
                        $"{nameof(StartDate)} must be less than or equal to {nameof(EndDate)}");
    }

  }  // class OrderItemFields

}  // namespace Empiria.Orders
