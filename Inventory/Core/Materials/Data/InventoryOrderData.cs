/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Data Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data service                            *
*  Type     : InventoryOrderData                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for inventory order instances.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Data;

namespace Empiria.Inventory.Data {

  /// <summary>Provides data read and write methods for inventory order instances.</summary>
  static internal class InventoryOrderData {
    
    internal static void WriteInventoryEntry(InventoryEntry entry) {

      var op = DataOperation.Parse("write_OMS_Inventory_Entry",
          entry.Id, entry.UID,
          entry.InventoryEntryTypeId,
          5225,
          5,
          //entry.OrderItemId.Order.Id,
          //entry.OrderItem.Id,
          //entry.Product.Id,
          entry.Product,
          entry.SkuId,
          entry.LocationId, entry.ObservationNotes,
          entry.UnitId, entry.InputQuantity,
          entry.InputCost, entry.OutputQuantity,
          entry.OutputCost, entry.EntryTime,
          entry.Tags, entry.ExtData,
          entry.InventoryEntryKeywords, entry.PostedBy.Id,
          entry.PostingTime, (char) entry.Status);

      DataWriter.Execute(op);
    }


    internal static void WriteInventoryOrder(InventoryOrder order) {
      
      var op = DataOperation.Parse("write_OMS_Inventory_Order",
          order.Id, order.UID,
          order.InventoryOrderTypeId, order.InventoryOrderNo,
          order.Reference, order.Responsible,
          order.AssignedTo, order.Notes,
          order.InventoryOrderExtData, order.Keywords,
          order.ScheduledTime, order.ClosingTime,
          order.PostingTime, order.PostedBy,
          (char) order.Status);

      DataWriter.Execute(op);
    }

  } // class InventoryOrderData

} // namespace Empiria.Inventory.Data
