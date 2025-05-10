/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Information Holder                      *
*  Type     : InventoryUtility                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents an inventory utility.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;
using Empiria.Inventory.Adapters;
using Empiria.Orders;

namespace Empiria.Inventory {

  /// <summary>Represents an inventory utility.</summary>
  internal class InventoryUtility {

    #region Public methods

    static internal void EnsureIsValidToClose(FixedList<InventoryOrderItem> orderItems) {
      foreach (var item in orderItems) {

        OrderItem orderItem = OrderItem.Parse(item.OrderItemId);
        var entries = InventoryEntry.GetListFor(orderItem);

        var entriesQuantity = entries.Sum(x => x.InputQuantity);

        Assertion.Require(orderItem.Quantity == entriesQuantity, "Faltan productos por asignar.");
      }
    }


    internal InventoryOrderActions GetActions(FixedList<InventoryOrderItem> items) {
      bool existClosedEntries = false;

      foreach (var item in items) {
        foreach (var entry in item.Entries) {
          if (entry.Status == InventoryStatus.Cerrado) {
            existClosedEntries = true;
          }
        }
      }

      InventoryOrderActions actions = new InventoryOrderActions {
        CanEditEntries = existClosedEntries ? false : true
      };

      return actions;
    }


    internal void GetInventoryEntriesByItem(FixedList<InventoryOrderItem> items) {

      foreach (var item in items) {
        OrderItem orderItem = OrderItem.Parse(item.OrderItemId);
        item.Entries = InventoryEntry.GetListFor(orderItem);
      }
    }


    internal InventoryOrder MapToInventoryOrder(Order order) {

      return new InventoryOrder {
        ClosingTime = order.ClosingTime,
        InventoryOrderNo = order.OrderNo,
        InventoryOrderUID = order.UID,
        InventoryOrderTypeId = order.OrderType.Id,
        OrderId = order.Id,
        Order_Description = order.Description,
        PostedBy = order.PostedBy,
        PostingTime = order.PostingTime,
        Responsible = order.Responsible,
        Status = order.Status,
        Items = MapToInventoryItems(order.GetItems<OrderItem>())
      };
    }

    #endregion Public methods

    #region Private methods

    private InventoryOrderItem MapToInventoryItem(OrderItem x) {

      return new InventoryOrderItem {
        Description = x.Description,
        InventoryOrderItemUID = x.UID,
        ItemTypeId = x.OrderItemType.Id,
        OrderId = x.Order.Id,
        OrderItemId = x.Id,
        PostedBy = x.PostedBy,
        PostingTime = x.PostingTime,
        Product = x.Product,
        ProductQuantity = x.Quantity,
        ProductUnitId = x.ProductUnit.Id,
        Status = x.Status
      };
    }


    private FixedList<InventoryOrderItem> MapToInventoryItems(FixedList<OrderItem> orderItems) {

      return orderItems.Select(x => MapToInventoryItem(x))
                   .ToFixedList();
    }

    #endregion Private methods

  } // class InventoryUtility

} // namespace Empiria.Inventory
