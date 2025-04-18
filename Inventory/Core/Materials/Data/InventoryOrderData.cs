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
using Empiria.Inventory.Adapters;
using Empiria.Orders;

namespace Empiria.Inventory.Data {

  /// <summary>Provides data read and write methods for inventory order instances.</summary>
  static internal class InventoryOrderData {
    
    internal static InventoryOrder GetInventoryOrderByUID(string orderUID) {

      var sql = $"SELECT * FROM OMS_Orders WHERE Order_UID = '{orderUID}'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<InventoryOrder>(op);
    }


    internal static FixedList<InventoryOrderItem> GetInventoryOrderItemsByOrder(int inventoryOrderId) {

      var sql = $"SELECT * FROM OMS_Order_Items WHERE Order_Item_Order_Id = {inventoryOrderId}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<InventoryOrderItem>(op);
    }

    internal static FixedList<InventoryOrder> SearchInventoryOrders(string filter, string sort) {

      var sql = "SELECT * FROM OMS_Orders";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }
      if (!string.IsNullOrWhiteSpace(sort)) {
        sql += $" ORDER BY {sort}";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<InventoryOrder>(op);
    }


    internal static void WriteInventoryEntry(InventoryEntry entry) {

      var op = DataOperation.Parse("write_OMS_Inventory_Entry",
          entry.Id, entry.UID,
          entry.InventoryEntryTypeId,
          //entry.Order.Id,
          entry.OrderItem,
          //entry.Product.Id,
          entry.SkuId,
          //entry.Location, 
          entry.ObservationNotes,
          entry.UnitId, entry.InputQuantity,
          entry.InputCost, entry.OutputQuantity,
          entry.OutputCost, entry.EntryTime,
          entry.Tags, entry.ExtData,
          entry.Keywords, entry.PostedBy.Id,
          entry.PostingTime, (char) entry.Status);

      DataWriter.Execute(op);
    }


    //internal static void WriteInventoryOrder(InventoryOrder order) {
      
    //  var op = DataOperation.Parse("write_OMS_Inventory_Order",
    //      order.Id, order.UID,
    //      order.InventoryOrderTypeId, order.InventoryOrderNo,
    //      order.Reference, order.Responsible,
    //      order.AssignedTo, order.Notes,
    //      order.InventoryOrderExtData, order.Keywords,
    //      order.ScheduledTime, order.ClosingTime,
    //      order.PostingTime, order.PostedBy,
    //      (char) order.Status);

    //  DataWriter.Execute(op);
    //}

  } // class InventoryOrderData

} // namespace Empiria.Inventory.Data
