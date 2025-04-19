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
using Empiria.Inventory.Adapters;
using Empiria.Inventory.Data;
using Empiria.Orders;
using Empiria.Services;
using Empiria.StateEnums;

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
      var orderItem = InventoryOrderData.GetInventoryOrderItemsByUID(orderItemUID);
      
      var inventoryEntry = new InventoryEntry(order, orderItem);

      inventoryEntry.Update(fields, orderItemUID);

      inventoryEntry.Save();

      return GetInventoryOrderByUID(orderUID);
    }


    public InventoryHolderDto GetInventoryOrderByUID(string orderUID) {

      var order = InventoryOrderData.GetInventoryOrderByUID(orderUID);
      order.Items = GetInventoryOrderItemsByOrder(order.OrderId);
      InventoryOrderActions actions = GetActions(order);

      return InventoryOrderMapper.MapToHolderDto(order, actions);
    }


    private InventoryOrderActions GetActions(InventoryOrder order) {

      return new InventoryOrderActions {
        CanEditEntries = order.Status == EntityStatus.Active ||
                         order.Status == EntityStatus.Pending ||
                         order.Status == EntityStatus.OnReview ?
                         true : false,
      };
    }

    private FixedList<InventoryOrderItem> GetInventoryOrderItemsByOrder(int orderId) {

      FixedList<InventoryOrderItem> orderItems =
          InventoryOrderData.GetInventoryOrderItemsByOrder(orderId);

      foreach (var item in orderItems) {

        item.Entries = InventoryOrderData.GetInventoryEntriesByOrderItemId(item);
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

    #endregion Use cases

    #region Private methods


    #endregion Private methods

  } // class InventoryOrderUseCases

} // namespace Empiria.Inventory.UseCases
