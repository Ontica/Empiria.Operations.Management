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
    
    
    internal static FixedList<InventoryEntry> GetInventoryEntriesByOrderItemId(InventoryOrderItem orderItem) {

      var sql = $"SELECT * FROM OMS_Inventory_Entries " +
                $"WHERE Inv_Entry_Status != 'X' " +
                $"AND Inv_Entry_Order_Id = {orderItem.OrderId} " +
                $"AND Inv_Entry_Order_Item_Id = {orderItem.OrderItemId} ";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<InventoryEntry>(op);
    }


    internal static FixedList<InventoryEntry> GetInventoryEntryByOrderId(int orderId) {

      var sql = $"SELECT * FROM OMS_Inventory_Entries " +
                $"WHERE Inv_Entry_Order_Id = {orderId} AND " +
                $"Inv_Entry_Status != 'X' ";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<InventoryEntry>(op);
    }


    internal static InventoryOrder GetInventoryOrderByUID(string orderUID) {

      var sql = $"SELECT * FROM OMS_Orders WHERE Order_UID = '{orderUID}'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<InventoryOrder>(op);
    }


    internal static FixedList<InventoryOrderItem> GetInventoryOrderItemsByOrder(int orderId) {

      var sql = $"SELECT * FROM OMS_Order_Items " +
                $"WHERE Order_Item_Status != 'X' " +
                $"AND Order_Item_Order_Id = {orderId}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<InventoryOrderItem>(op);
    }


    internal static InventoryOrderItem GetInventoryOrderItemByUID(string itemUID) {

      var sql = $"SELECT * FROM OMS_Order_Items WHERE Order_Item_UID = '{itemUID}'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<InventoryOrderItem>(op);
    }


    internal static LocationEntry GetLocationEntryById(int locationId) {

      var sql = $"SELECT * FROM Common_Storage " +
                $"WHERE Object_Type_Id = 275 AND Object_Id = '{locationId}'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<LocationEntry>(op);
    }


    internal static LocationEntry GetLocationEntryByName(string locationName) {

      try {

        var sql = $"SELECT * FROM Common_Storage " +
                $"WHERE Object_Type_Id = 275 AND Object_Name = '{locationName}'";

        var op = DataOperation.Parse(sql);

        return DataReader.GetPlainObject<LocationEntry>(op);

      } catch (Exception) {

        throw new Exception("Localización no encontrada.");
      }
    }


    internal static ProductEntry GetProductEntryById(int productId) {
      var sql = $"SELECT * FROM OMS_Products WHERE Product_Id = {productId}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<ProductEntry>(op);
    }


    internal static ProductEntry GetProductEntryByName(string productName) {

      try {

        var sql = $"SELECT * FROM OMS_Products WHERE Product_Name = '{productName}'";

        var op = DataOperation.Parse(sql);

        return DataReader.GetPlainObject<ProductEntry>(op);

      } catch (Exception ) {

        throw new Exception("Producto no coincide con el seleccionado.");
      }
      
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


    internal static void DeleteEntryStatus(int orderId, int orderItemId,
                                           int inventoryEntryId, InventoryStatus status) {
      
      string sql = $"UPDATE OMS_Inventory_Entries " +
                   $"SET Inv_Entry_Status = '{(char) status}' " +
                   $"WHERE Inv_Entry_Id = {inventoryEntryId} AND " +
                   $"Inv_Entry_Order_Id = {orderId} AND " +
                   $"Inv_Entry_Order_Item_Id = {orderItemId}";

      var dataOperation = DataOperation.Parse(sql);

      DataWriter.Execute(dataOperation);
    }


    internal static void UpdateEntriesStatusByOrder(int orderId, InventoryStatus status) {

      string sql = $"UPDATE OMS_Inventory_Entries " +
                   $"SET Inv_Entry_Status = '{(char) status}' " +
                   $"WHERE Inv_Entry_Order_Id = {orderId} AND " +
                   $"Inv_Entry_Status != 'X'";

      var dataOperation = DataOperation.Parse(sql);

      DataWriter.Execute(dataOperation);
    }

    internal static void WriteInventoryEntry(InventoryEntry entry) {

      var op = DataOperation.Parse("write_OMS_Inventory_Entry",
          entry.Id, entry.UID,
          entry.InventoryEntryTypeId,
          entry.OrderId,
          entry.OrderItemId,
          entry.ProductId,
          entry.SkuId,
          entry.LocationId, 
          entry.Observations,
          entry.UnitId, entry.InputQuantity,
          entry.InputCost, entry.OutputQuantity,
          entry.OutputCost, entry.EntryTime,
          entry.Tags, entry.ExtData,
          entry.Keywords, entry.PostedBy.Id,
          entry.PostingTime, (char) entry.Status);

      DataWriter.Execute(op);
    }


    internal static InventoryEntry GetInventoryEntryByUID(string entryUID) {

      var sql = $"SELECT * FROM OMS_Inventory_Entries WHERE Inv_Entry_Uid = '{entryUID}'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<InventoryEntry>(op);
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
