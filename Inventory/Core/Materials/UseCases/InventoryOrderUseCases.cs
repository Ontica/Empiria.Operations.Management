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


    internal InventoryEntryDto CreateInventoryEntry(string orderUID, string orderItemUID,
                                                    InventoryEntryFields fields) {

      var inventoryEntry = new InventoryEntry(orderUID, orderItemUID, fields);

      inventoryEntry.Update();

      inventoryEntry.Save();

      return InventoryOrderMapper.MapToInventoryEntryDto(inventoryEntry);
    }


    public InventoryHolderDto GetInventoryOrderByUID(string orderUID) {

      var order = InventoryOrderData.GetInventoryOrderByUID(orderUID);
      order.Items = InventoryOrderData.GetInventoryOrderItemsByOrder(order.InventoryOrderId);

      return InventoryOrderMapper.MapToHolderDto(order);
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
