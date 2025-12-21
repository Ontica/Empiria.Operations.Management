/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Input fields                            *
*  Type     : OrderTaxEntryFields                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields used to add or update an order tax entry.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

namespace Empiria.Orders {

  /// <summary>Input fields used to add or update an order tax entry.</summary>
  public class OrderTaxEntryFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string OrderUID {
      get; set;
    } = string.Empty;


    public string TaxTypeUID {
      get; set;
    } = string.Empty;

    public decimal Total {
      get; set;
    }


    internal void EnsureValid() {
      Assertion.Require(OrderUID, nameof(OrderUID));

      if (string.IsNullOrEmpty(UID)) {
        Assertion.Require(TaxTypeUID, nameof(TaxTypeUID));
      }

      Assertion.Require(Total > 0m, $"{nameof(Total)} must be non-negative.");
    }


    internal TaxType GetTaxType() {
      return TaxType.Parse(TaxTypeUID);
    }

  }  // class OrderTaxEntryFields

}  // namespace Empiria.Orders
