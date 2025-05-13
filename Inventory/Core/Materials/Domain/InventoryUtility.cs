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

    static internal InventoryOrderActions GetActions(FixedList<InventoryOrderItem> items) {

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


    static internal void EnsureIsValidToClose(FixedList<InventoryOrderItem> orderItems) {
      foreach (var item in orderItems) {

        InventoryOrderItem orderItem = InventoryOrderItem.Parse(item.Id);
        var entries = InventoryEntry.GetListFor(orderItem);

        var entriesQuantity = entries.Sum(x => x.InputQuantity);

        Assertion.Require(orderItem.Quantity == entriesQuantity, "Faltan productos por asignar.");
      }
    }


    static internal InventoryOrder GetInventoryOrder(string orderUID) {

      InventoryOrder inventoryOrder = InventoryOrder.Parse(orderUID);

      inventoryOrder.Items = inventoryOrder.GetItems<InventoryOrderItem>();

      GetInventoryEntriesByItem(inventoryOrder.Items);

      return inventoryOrder;
    }


    #endregion Public methods

    #region Private methods

    static private void GetInventoryEntriesByItem(FixedList<InventoryOrderItem> items) {

      foreach (var item in items) {

        item.Entries = InventoryEntry.GetListFor(item);
      }
    }

    #endregion Private methods

  } // class InventoryUtility

} // namespace Empiria.Inventory
