/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : OrderTaxes                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Collection of taxes for an order.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Financial;

using Empiria.Orders.Data;

namespace Empiria.Orders {

  /// <summary>Collection of taxes for an order.</summary>
  public class OrderTaxes {

    private readonly Order _order;
    private Lazy<List<OrderTaxEntry>> _taxEntries = new Lazy<List<OrderTaxEntry>>();

    #region Constructors and parsers

    internal OrderTaxes(Order order) {
      Assertion.Require(order, nameof(order));

      _order = order;
      _taxEntries = new Lazy<List<OrderTaxEntry>>(() => OrdersData.GetOrderTaxEntries(_order));
    }

    #endregion Constructors and parsers

    #region Properties

    public decimal Total {
      get {
        return _taxEntries.Value.Sum(x => x.Total);
      }
    }

    #endregion Properties

    #region Methods

    internal OrderTaxEntry AddTax(OrderTaxEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var taxEntry = _taxEntries.Value.Find(x => x.TaxType.UID == fields.TaxTypeUID);

      if (taxEntry != null) {
        taxEntry.Sum(fields.Total);

        return taxEntry;
      }

      taxEntry = new OrderTaxEntry(_order, fields.GetTaxType(), fields.Total);

      _taxEntries.Value.Add(taxEntry);

      return taxEntry;
    }


    internal void ApplyTaxes(OrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      FixedList<TaxType> taxTypes = TaxType.AppliableFor(orderItem.Product);

      foreach (var taxType in taxTypes) {
        var fields = new OrderTaxEntryFields {
          OrderUID = _order.UID,
          TaxTypeUID = taxType.UID,
          Total = orderItem.Subtotal * taxType.Rate
        };

        OrderTaxEntry taxEntry = AddTax(fields);
        taxEntry.Save();
      }
    }


    public FixedList<OrderTaxEntry> GetList() {
      return _taxEntries.Value.ToFixedList();
    }


    internal OrderTaxEntry GetTax(string taxEntryUID) {
      Assertion.Require(taxEntryUID, nameof(taxEntryUID));

      var taxEntry = _taxEntries.Value.Find(x => x.UID == taxEntryUID);

      Assertion.Require(taxEntry, $"Order tax entry not found for order {_order.OrderNo}.");

      return taxEntry;
    }


    internal OrderTaxEntry RemoveTax(string taxEntryUID) {
      Assertion.Require(taxEntryUID, nameof(taxEntryUID));

      OrderTaxEntry taxEntry = GetTax(taxEntryUID);

      taxEntry.Delete();

      _taxEntries.Value.Remove(taxEntry);

      return taxEntry;
    }


    internal void UnapplyTaxes(OrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      FixedList<TaxType> taxTypes = TaxType.AppliableFor(orderItem.Product);

      foreach (var taxType in taxTypes) {

        var taxEntry = _taxEntries.Value.Find(x => x.TaxType.Equals(taxType));

        if (taxEntry != null) {
          decimal amount = -1 * (orderItem.Subtotal * taxType.Rate);

          taxEntry.Sum(amount);
          taxEntry.Save();
        }
      }
    }


    internal OrderTaxEntry UpdateTax(OrderTaxEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      OrderTaxEntry taxEntry = GetTax(fields.UID);

      taxEntry.Update(fields.Total);

      return taxEntry;
    }

    #endregion Methods

  }  // class OrderTaxes

}  // namespace Empiria.Orders
