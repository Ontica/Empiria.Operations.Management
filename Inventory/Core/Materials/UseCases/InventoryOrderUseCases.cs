﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrderUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to manage inventory order.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


using System;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.Assets;
using Empiria.Inventory.Data;
using Empiria.Locations;
using Empiria.Orders;
using Empiria.Orders.Adapters;
using Empiria.Parties;
using Empiria.Products;
using Empiria.Services;
using Empiria.StateEnums;


namespace Empiria.Inventory.UseCases {

  /// <summary>Use cases to manage inventory order.</summary>
  public class InventoryOrderUseCases : UseCase {

    private const int INVENTORYORDERTYPEID = 4010;

    #region Constructors and parsers

    protected InventoryOrderUseCases() {
      // no-op
    }

    static public InventoryOrderUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<InventoryOrderUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public InventoryHolderDto CloseInventoryEntries(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);

      FixedList<InventoryOrderItem> orderItems = order.GetItems<InventoryOrderItem>();

      InventoryUtility.EnsureIsValidToClose(orderItems);

      InventoryOrderData.UpdateEntriesStatusByOrder(order.Id, InventoryStatus.Cerrado);

      return GetInventoryOrder(orderUID);
    }


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

      var inventoryEntry = new InventoryEntry(orderUID, orderItemUID);

      inventoryEntry.Update(fields, orderItemUID);

      inventoryEntry.Save();

      return GetInventoryOrder(orderUID);
    }


    public InventoryHolderDto CreateInventoryOrder(string warehouseUID, InventoryOrderFields fields) {
      Assertion.Require(warehouseUID, nameof(warehouseUID));
      Assertion.Require(fields, nameof(fields));

      var orderType = Orders.OrderType.Parse(INVENTORYORDERTYPEID);

      InventoryOrder order = new InventoryOrder(warehouseUID, orderType);

      order.Update(fields);

      order.Save();

      return GetInventoryOrder(order.UID);
    }


    public InventoryHolderDto CreateInventoryOrderItem(string orderUID, InventoryOrderItemFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(fields, nameof(fields));

      var order = InventoryOrder.Parse(orderUID);

      var location = CommonStorage.TryParseNamedKey<Location>(fields.Location);
      Assertion.Require(location, $"La ubicacion {fields.Location} no existe.");
      Assertion.Require(order.Warehouse == GetRootLocation(location),
                 $"La localización {fields.Location} no existe en el almacen {order.Warehouse.Name}");

      var product = Product.TryParseWithCode(fields.Product);
      Assertion.Require(product, $"El producto con clave {fields.Product} no existe.");

      var isnotexistProductinLocation = VerifyProductAndLocationInOrder(order.Id, product.Id, location.Id);
      Assertion.Require(isnotexistProductinLocation, $"Ya existe ese producto en esa localización {fields.Location}.");

      fields.ProductUID = product.UID;
      fields.Description = product.Description;
      fields.ProductUnitUID = product.BaseUnit.UID;      

      var orderItemType = Orders.OrderItemType.Parse(4059);
      InventoryOrderItem orderItem = new InventoryOrderItem(orderItemType, order, location);

      var position = GetItemPosition(order);
      fields.Position = position;

      orderItem.Update(fields);
      order.AddItem(orderItem);
      orderItem.Save();

      AddInventoryEntry(order, orderItem, fields);

      return GetInventoryOrder(order.UID);
    }


    public InventoryHolderDto DeleteInventoryEntry(string orderUID, string itemUID, string entryUID) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(itemUID, nameof(itemUID));
      Assertion.Require(entryUID, nameof(entryUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);
      InventoryOrderItem orderItem = InventoryOrderItem.Parse(itemUID);
      InventoryEntry entry = InventoryEntry.Parse(entryUID);

      Assertion.Require(order.Id == entry.Order.Id && orderItem.Order.Id == entry.Order.Id,
                        $"El registro de inventario no coincide con la orden!");

      InventoryOrderData.DeleteEntryStatus(order.Id, orderItem.Id,
                                           entry.Id, InventoryStatus.Deleted);

      return GetInventoryOrder(orderUID);
    }


    public void DeleteInventoryOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);

      order.Delete();
      order.Save();
    }


    public InventoryHolderDto DeleteInventoryOrderItem(string orderUID, string orderItemUID) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);

      var item = order.GetItem<InventoryOrderItem>(orderItemUID);

      order.RemoveItem(item);

      item.Save();

      return GetInventoryOrder(orderUID);
    }


    public InventoryHolderDto GetInventoryOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder inventoryOrder = InventoryUtility.GetInventoryOrder(orderUID);

      InventoryOrderActions actions = InventoryUtility.GetActions(inventoryOrder);

      return InventoryOrderMapper.MapToHolderDto(inventoryOrder, actions);
    }


    public InventoryEntryDto GetInventoryEntryByUID(string inventoryEntryUID) {
      Assertion.Require(inventoryEntryUID, nameof(inventoryEntryUID));

      InventoryEntry entry = InventoryEntry.Parse(inventoryEntryUID);

      return InventoryOrderMapper.MapToInventoryEntryDto(entry);
    }


    public FixedList<NamedEntityDto> GetOrderTypes() {
      return InventoryType.GetList().MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> GetWarehouses() {
      return CommonStorage.GetList<Location>().FindAll(x => x.Level == 1 && x.GetStatus<EntityStatus>() != EntityStatus.Deleted).MapToNamedEntityList();
    }


    public InventoryOrderDataDto SearchInventoryOrder(InventoryOrderQuery query) {
      Assertion.Require(query, nameof(query));

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      var orders = InventoryOrderData.SearchInventoryOrders(filter, sort);

      return InventoryOrderMapper.InventoryOrderDataDto(orders, query);
    }


    public FixedList<NamedEntityDto> GetPartiesByRol(string rol) {
      return Party.GetPartiesInRole(rol).MapToNamedEntityList();
    }


    public InventoryHolderDto UpdateInventoryOrder(string orderUID, InventoryOrderFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var order = InventoryOrder.Parse(orderUID);
      order.Update(fields);

      order.Save();

      return GetInventoryOrder(order.UID);
    }

    
    public InventoryHolderDto UpdateInventoryOrderItem(string orderUID, string orderItemUID,
                                               InventoryOrderItemFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));
      Assertion.Require(fields, nameof(fields));

      var product = Product.TryParseWithCode(fields.Product);
      Assertion.Require(product, "El producto no existe");

      var location = CommonStorage.TryParseNamedKey<Location>(fields.Location);
      Assertion.Require(location, $"La ubicacion {fields.Location} no existe.");

      fields.ProductUID = product.UID;
      fields.Description = product.Description;
      fields.ProductUnitUID = product.BaseUnit.UID;

      var order = InventoryOrder.Parse(orderUID);

      var item = order.GetItem<InventoryOrderItem>(orderItemUID);

      item.Update(fields);

      item.Save();

      return GetInventoryOrder(order.UID);
    }


    public InventoryHolderDto CloseInventoryOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);

      order.Close();
      order.Save();

      order.CloseItems();

      OutputInventoryEntriesVW(order);

      CloseInventoryEntries(order.UID);

      return GetInventoryOrder(order.UID);
    }

    #endregion Use cases

    #region Helpers

    private void AddInventoryEntry(InventoryOrder order, InventoryOrderItem orderItem, InventoryOrderItemFields fields) {
      var inventoryEntry = new InventoryEntry(order.UID, orderItem.UID);

      InventoryEntryFields entryFields = new InventoryEntryFields();

      entryFields.Product = fields.Product;
      entryFields.Quantity = fields.Quantity;
      entryFields.Location = fields.Location;

      inventoryEntry.Update(entryFields, orderItem.UID);

      inventoryEntry.Save();
    }


    private int GetItemPosition(InventoryOrder order) {
      if (order.Items.Count == 0) {
        return 1;
      } else {
        var allItems = InventoryOrderData.GetAllInventoryOrderItems(order);
        return allItems.Count + 1;
      }
    }


    private Location GetRootLocation(Location location) {
      var current = location;
      while (!current.IsRoot) {
        var parent = current.GetParent<Location>();
        current = parent;
      }

      return current;
    }


    public void OutputInventoryEntriesVW(InventoryOrder order) {
      
      foreach (var item in order.Items) {

        var inventoryEntry = new InventoryEntry(order, item);

        var price = InventoryOrderData.GetProductPriceFromVirtualWarehouse(item.Product.Id);
        inventoryEntry.OutputEntry(price);

        inventoryEntry.Save();
      }      
    }


    private bool VerifyProductAndLocationInOrder(int orderId, int productID, int locationID) {
      if (InventoryOrderData.VerifyProductAndLocationInOrder(orderId, productID, locationID) != 0) {
        return false;
      }
      return true;
    }

    #endregion Helpers

  } // class InventoryOrderUseCases

} // namespace Empiria.Inventory.UseCases
