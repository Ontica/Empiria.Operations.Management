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

      var taxEntry = new OrderTaxEntry(_order, fields.GetTaxType(), fields.Total);

      _taxEntries.Value.Add(taxEntry);

      return taxEntry;
    }


    internal FixedList<OrderTaxEntry> GetList() {
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
