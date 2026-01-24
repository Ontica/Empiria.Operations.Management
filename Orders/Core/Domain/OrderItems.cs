/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Separated aggregator                    *
*  Type     : OrderItems                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Collection of items for an order.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Orders.Data;

namespace Empiria.Orders {

  /// <summary>Collection of taxes for an order.</summary>
  public class OrderItems {

    private readonly Order _order;
    private Lazy<List<OrderItem>> _items = new Lazy<List<OrderItem>>();

    #region Constructors and parsers

    internal OrderItems(Order order) {
      Assertion.Require(order, nameof(order));

      _order = order;
      _items = new Lazy<List<OrderItem>>(() => OrdersData.GetOrderItems(_order));
    }

    #endregion Constructors and parsers

    #region Properties

    public int Count {
      get {
        return _items.Value.Count;
      }
    }


    public decimal Subtotal {
      get {
        return _items.Value.Sum(x => x.Subtotal);
      }
    }


    public decimal Discount {
      get {
        return _items.Value.Sum(x => x.Discount);
      }
    }


    public decimal Penalties {
      get {
        return _items.Value.Sum(x => x.PenaltyDiscount);
      }
    }


    public decimal DiscountsTotal {
      get {
        return Discount + Penalties;
      }
    }

    #endregion Properties

    #region Methods

    public virtual void Add(OrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));
      Assertion.Require(orderItem.Order.Equals(_order), "OrderItem.Order instance mismatch.");

      _items.Value.Add(orderItem);
      _order.Taxes.ApplyTaxes(orderItem);
    }


    public OrderItem Get(string orderItemUID) {
      var item = _items.Value.Find(x => x.UID == orderItemUID);

      Assertion.Require(item, $"Order item {orderItemUID} not found.");

      return item;
    }


    public FixedList<OrderItem> GetItems() {
      return _items.Value.ToFixedList();
    }


    public virtual void Remove(OrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      orderItem.Delete();

      _items.Value.Remove(orderItem);

      _order.Taxes.UnapplyTaxes(orderItem);
    }


    internal void Update(OrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      var existingItem = _items.Value.Find(x => x.Equals(orderItem));

      Assertion.Require(existingItem, "Order item not found in order.");

      _order.Taxes.ApplyTaxes(orderItem);
    }

    #endregion Methods

  }  // class OrderItems

}  // namespace Empiria.Orders
