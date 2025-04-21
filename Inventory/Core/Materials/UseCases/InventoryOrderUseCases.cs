/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrderUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to manage inventory order.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.Data;
using Empiria.Orders;
using Empiria.Services;
using Empiria.StateEnums;
using iText.StyledXmlParser.Jsoup.Nodes;

namespace Empiria.Inventory.UseCases {

  /// <summary>Use cases to manage inventory order.</summary>
  public class InventoryOrderUseCases : UseCase {

    #region Constructors and parsers

    protected InventoryOrderUseCases() {
      // no-op
    }

    static public InventoryOrderUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<InventoryOrderUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases


    public InventoryHolderDto CreateInventoryEntry(string orderUID, string orderItemUID,
                                                    InventoryEntryFields fields) {

      var order = InventoryOrderData.GetInventoryOrderByUID(orderUID);
      var orderItem = InventoryOrderData.GetInventoryOrderItemByUID(orderItemUID);
      
      var inventoryEntry = new InventoryEntry(order, orderItem);

      inventoryEntry.Update(fields, orderItemUID);

      inventoryEntry.Save();

      return GetInventoryOrderByUID(orderUID);
    }


    public InventoryHolderDto GetInventoryOrderByUID(string orderUID) {

      InventoryOrder order = InventoryOrderData.GetInventoryOrderByUID(orderUID);
      
      FixedList<InventoryOrderItem> items = GetInventoryOrderItemsByOrder(order.OrderId);
      order.Items = items;

      InventoryOrderActions actions = GetActions(items);

      return InventoryOrderMapper.MapToHolderDto(order, actions);
    }


    private InventoryOrderActions GetActions(FixedList<InventoryOrderItem> items) {

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


    private FixedList<InventoryOrderItem> GetInventoryOrderItemsByOrder(int orderId) {

      FixedList<InventoryOrderItem> orderItems =
          InventoryOrderData.GetInventoryOrderItemsByOrder(orderId);

      foreach (var item in orderItems) {

        FixedList<InventoryEntry> entries = InventoryOrderData.GetInventoryEntriesByOrderItemId(item);
        item.Entries = entries;
      }

      return orderItems;
    }


    static public LocationEntry GetLocationEntryById(int locationId) {

      return InventoryOrderData.GetLocationEntryById(locationId);
    }


    static public ProductEntry GetProductEntryById(int productId) {

      return InventoryOrderData.GetProductEntryById(productId);
    }


    public InventoryOrderDataDto SearchInventoryOrder(InventoryOrderQuery query) {

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      var orders = InventoryOrderData.SearchInventoryOrders(filter, sort);

      return InventoryOrderMapper.InventoryOrderDataDto(orders, query);
    }


    public InventoryHolderDto DeleteInventoryEntry(string orderUID, string itemUID, string entryUID) {

      InventoryOrder order = InventoryOrderData.GetInventoryOrderByUID(orderUID);
      InventoryOrderItem orderItem = InventoryOrderData.GetInventoryOrderItemByUID(itemUID);
      InventoryEntry entry = InventoryOrderData.GetInventoryEntryByUID(entryUID);
      
      Assertion.Require(order.OrderId == entry.OrderId && orderItem.OrderId == entry.OrderId,
                        $"El registro de inventario no coincide con la orden!");

      InventoryOrderData.DeleteEntryStatus(order.OrderId, orderItem.OrderItemId,
                                           entry.InventoryEntryId, InventoryStatus.Deleted);

      return GetInventoryOrderByUID(orderUID);
    }


    public InventoryHolderDto CloseInventoryEntries(string orderUID) {

      var order = InventoryOrderData.GetInventoryOrderByUID(orderUID);
      var orderItems = InventoryOrderData.GetInventoryOrderItemsByOrder(order.OrderId);

      EnsureIsValidToClose(orderItems);
      
      InventoryOrderData.UpdateEntriesStatusByOrder(order.OrderId, InventoryStatus.Cerrado);

      return GetInventoryOrderByUID(orderUID);
    }


    private void EnsureIsValidToClose(FixedList<InventoryOrderItem> orderItems) {

      foreach (var item in orderItems) {

        var entries = InventoryOrderData.GetInventoryEntriesByOrderItemId(item);
        var entriesQuantity = entries.Sum(x => x.InputQuantity);

        Assertion.Require(item.ProductQuantity == entriesQuantity, "Faltan productos por asignar.");
      }

    }


    #endregion Use cases

    #region Private methods


    #endregion Private methods

  } // class InventoryOrderUseCases

} // namespace Empiria.Inventory.UseCases
