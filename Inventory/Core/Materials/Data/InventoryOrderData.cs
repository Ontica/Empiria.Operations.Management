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
using Empiria.Orders;

namespace Empiria.Inventory.Data {

  /// <summary>Provides data read and write methods for inventory order instances.</summary>
  static internal class InventoryOrderData {

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

    internal static FixedList<InventoryOrderItem> GetInventoryOrderItems(InventoryOrder order) {

      string sql = $"SELECT * FROM OMS_Order_Items " +
                   $"WHERE Order_Item_Order_Id = {order.Id} AND " +
                   $"Order_Item_Status <> 'X' ";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<InventoryOrderItem>(op);
    }


    internal static FixedList<InventoryEntry> GetInventoryEntriesByOrderItem(InventoryOrderItem orderItem) {

      var sql = $"SELECT * FROM OMS_Inventory_Entries " +
                $"WHERE Inv_Entry_Status != 'X' " +
                $"AND Inv_Entry_Order_Id = {orderItem.Order.Id} " +
                $"AND Inv_Entry_Order_Item_Id = {orderItem.Id} ";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<InventoryEntry>(op);
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

      return DataReader.GetFixedList<InventoryOrder>(op);
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
          entry.Order.Id,
          entry.OrderItem.Id,
          entry.Product.Id,
          entry.Sku.Id,
          entry.Location.Id,
          entry.Observations,
          entry.Unit.Id, entry.InputQuantity,
          entry.InputCost, entry.OutputQuantity,
          entry.OutputCost, entry.EntryTime,
          entry.Tags, entry.ExtData,
          entry.Keywords, entry.PostedBy.Id,
          entry.PostingTime, (char) entry.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteOrder(InventoryOrder o, string extensionData) {
      var op = DataOperation.Parse("write_OMS_Order",
                     o.Id, o.UID, o.OrderType.Id, o.Category.Id, o.OrderNo, o.Description,
                     EmpiriaString.Tagging(o.Identificators), EmpiriaString.Tagging(o.Tags),
                     o.RequestedBy.Id, o.Responsible.Id, o.Beneficiary.Id, o.Provider.Id,
                     -1, o.RequisitionId, -1, o.Project.Id, o.Currency.Id,
                     o.Source.Id, (char) o.Priority, o.AuthorizationTime, o.AuthorizedBy.Id,
                     o.ClosingTime, o.ClosedBy.Id, extensionData, o.Keywords, o.Warehouse.Id, 
                     o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteOrderItem(InventoryOrderItem o, string extensionData) {
      var op = DataOperation.Parse("write_OMS_Order_Item",
                     o.Id, o.UID, o.OrderItemType.Id, o.Order.Id, o.Product.Id,
                     o.Description, o.ProductUnit.Id, o.Quantity, o.UnitPrice, o.Discount,
                     o.Currency.Id, -1, -1, o.RequestedBy.Id,
                     -1, o.Project.Id, o.Provider.Id, -1, extensionData,
                     o.Keywords, o.Location.Id, o.Position, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

  } // class InventoryOrderData

} // namespace Empiria.Inventory.Data
