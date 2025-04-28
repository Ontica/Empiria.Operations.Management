/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrderUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to manage inventory order.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Locations;
using Empiria.Products;
using Empiria.Services;

using Empiria.Inventory.Adapters;
using Empiria.Inventory.Data;

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
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));
      Assertion.Require(fields, nameof(fields));

      ProductEntry productEntry = InventoryOrderData.GetProductEntryByName(fields.Product.Trim());
      LocationEntry locationEntry = InventoryOrderData.GetLocationEntryByName(fields.Location.Trim());

      fields.EnsureIsValid(productEntry.ProductId, orderItemUID);

      fields.ProductUID = Product.Parse(productEntry.ProductId).UID;
      fields.LocationUID = Location.Parse(locationEntry.LocationId).UID;

      var order = InventoryOrderData.GetInventoryOrderByUID(orderUID);
      var orderItem = InventoryOrderData.GetInventoryOrderItemByUID(orderItemUID);

      var inventoryEntry = new InventoryEntry(order, orderItem);

      inventoryEntry.Update(fields, orderItemUID);

      inventoryEntry.Save();

      return GetInventoryOrderByUID(orderUID);
    }


    public InventoryHolderDto GetInventoryOrderByUID(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

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


    public InventoryOrderDataDto SearchInventoryOrder(InventoryOrderQuery query) {
      Assertion.Require(query, nameof(query));

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      var orders = InventoryOrderData.SearchInventoryOrders(filter, sort);

      return InventoryOrderMapper.InventoryOrderDataDto(orders, query);
    }


    public InventoryHolderDto DeleteInventoryEntry(string orderUID, string itemUID, string entryUID) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(itemUID, nameof(itemUID));
      Assertion.Require(entryUID, nameof(entryUID));

      InventoryOrder order = InventoryOrderData.GetInventoryOrderByUID(orderUID);
      InventoryOrderItem orderItem = InventoryOrderData.GetInventoryOrderItemByUID(itemUID);
      InventoryEntry entry = InventoryOrderData.GetInventoryEntryByUID(entryUID);

      Assertion.Require(order.OrderId == entry.OrderId && orderItem.OrderId == entry.OrderId,
                        $"El registro de inventario no coincide con la orden!");


      InventoryOrderData.DeleteEntryStatus(order.OrderId, orderItem.OrderItemId,
                                           entry.Id, InventoryStatus.Deleted);

      return GetInventoryOrderByUID(orderUID);
    }


    public InventoryHolderDto CloseInventoryEntries(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = InventoryOrderData.GetInventoryOrderByUID(orderUID);
      var orderItems = InventoryOrderData.GetInventoryOrderItemsByOrder(order.OrderId);

      EnsureIsValidToClose(orderItems);

      InventoryOrderData.UpdateEntriesStatusByOrder(order.OrderId, InventoryStatus.Cerrado);

      return GetInventoryOrderByUID(orderUID);
    }

    #endregion Use cases

    #region Helpers

    private void EnsureIsValidToClose(FixedList<InventoryOrderItem> orderItems) {
      foreach (var item in orderItems) {

        var entries = InventoryOrderData.GetInventoryEntriesByOrderItemId(item);
        var entriesQuantity = entries.Sum(x => x.InputQuantity);

        Assertion.Require(item.ProductQuantity == entriesQuantity, "Faltan productos por asignar.");
      }
    }

    #endregion Helpers

  } // class InventoryOrderUseCases

} // namespace Empiria.Inventory.UseCases
