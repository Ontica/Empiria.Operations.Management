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
using Empiria.Inventory.Adapters;
using Empiria.Inventory.Data;
using Empiria.Json;
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
    static private readonly JsonObject config = ConfigurationData.Get<JsonObject>("Dates");


    #region Constructors and parsers

    protected InventoryOrderUseCases() {
      // no-op
    }

    static public InventoryOrderUseCases UseCaseInteractor() {
      var _fromDate = config.Get<DateTime>("importDate");

      return UseCase.CreateInstance<InventoryOrderUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public InventoryHolderDto CloseInventoryOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);

      order.Close();
      order.Save();

      order.CloseItems();

      OutputInventoryEntriesVW(order);

      var inventoryEntryUseCase = InventoryEntryUseCases.UseCaseInteractor();
      inventoryEntryUseCase.CloseInventoryEntries(order.UID);

      return GetInventoryOrder(order.UID);
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

      AddInventoryEntry(order, orderItem);

      return GetInventoryOrder(order.UID);
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

      InventoryOrderData.DeleteEntry(order.Id, item.Id);

      return GetInventoryOrder(orderUID);
    }


    public InventoryHolderDto GetInventoryOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder inventoryOrder = InventoryUtility.GetInventoryOrder(orderUID);

      InventoryOrderActions actions = InventoryUtility.GetActions(inventoryOrder);

      return InventoryOrderMapper.MapToHolderDto(inventoryOrder, actions);
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


    public void LoadOrderFromNK() {
      JsonObject config = ConfigurationData.Get<JsonObject>("ImportNK.Dates");
      var _fromDate = config.Get<DateTime>("importDate");

      LoadInputOrdersFromNK(_fromDate);
      LoadOutputOrdersFromNK(_fromDate);
    }


    public void LoadInputOrdersFromNK(DateTime fromDate) {
      var sut = Order.GetFullList<Order>().FindAll(x => x.ClosingTime == fromDate && x.OrderType.Id == 4005);

      string inventoryTypeUID = "a40c65bd-9a56-48eb-a8bf-f9245ecd3004";

      InventoryOrder inventoryOrder;
      for (int i = 0; i < 3; i++) {
        var order = sut[i];
        inventoryOrder = GenerateInventoryOrder(order, inventoryTypeUID);
        GenerateInventoryOrderItems(order, inventoryOrder);
      }
    }


    public void LoadOutputOrdersFromNK(DateTime fromDate) {
      var sut = Order.GetFullList<Order>().FindAll(x => x.ClosingTime == fromDate && x.OrderType.Id == 4011);

      string inventoryTypeUID = "0eb5a072-b857-4071-8b06-57a34822ec64";

      InventoryOrder inventoryOrder;
      for (int i = 0; i < 3; i++) {
        var order = sut[i];
        inventoryOrder = GenerateInventoryOrder(order, inventoryTypeUID);
        GenerateInventoryOrderItems(order, inventoryOrder);
      }
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


    public InventoryHolderDto UpdateInventoryOrderItemQuantity(string orderUID, string orderItemUID,
                                               InventoryOrderItemFields fields) {
      var order = InventoryOrder.Parse(orderUID);

      var item = order.GetItem<InventoryOrderItem>(orderItemUID);

      item.UpdateQuantity(fields.Quantity);

      item.Save();

      return GetInventoryOrder(order.UID);
    }

    #endregion Use cases

    #region Helpers

    private void AddInventoryEntry(InventoryOrder order, InventoryOrderItem orderItem) {
      var inventoryEntry = new InventoryEntry(order.UID, orderItem.UID);

      inventoryEntry.InputEntry(orderItem.UnitPrice, orderItem.Location);

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


    private InventoryOrder GenerateInventoryOrder(Order order, string inventoryTypeUID) {

      InventoryOrderFields fields = new InventoryOrderFields {
        WarehouseUID = "DA6017D5-ED38-449B-9659-ACE06C4565DE",
        InventoryTypeUID = inventoryTypeUID,
        Description = "Orden de inventario correspondiente " + order.OrderNo,
        RequestedByUID = order.RequestedBy.UID,
        ResponsibleUID = order.Responsible.UID,
        RelatedOrderId = order.Id,
      };

      var orderType = Orders.OrderType.Parse(4010);

      InventoryOrder inventoryOrder = new InventoryOrder(fields.WarehouseUID, orderType);

      inventoryOrder.Update(fields);

      inventoryOrder.Save();

      return inventoryOrder;
    }


    private void GenerateInventoryOrderItems(Order order, InventoryOrder inventoryOrder) {

      var items = order.GetItems<OrderItem>();

      var orderItemType = Orders.OrderItemType.Parse(4059);

      foreach (var item in items) {
        InventoryOrderItemFields fields = new InventoryOrderItemFields();

        fields.ProductUID = item.Product.UID;
        fields.Description = item.Product.Description;
        fields.ProductUnitUID = item.Product.BaseUnit.UID;
        fields.Quantity = item.Quantity;
        fields.Position = item.Position;
        fields.Location = "A-001-01-01";

        InventoryOrderItem orderItem = new InventoryOrderItem(orderItemType, inventoryOrder);

        orderItem.Update(fields);
        inventoryOrder.AddItem(orderItem);
        orderItem.Save();
      }
    }

    public void GenerateDifFicalInventory(string orderUID) {
      var order = InventoryOrder.Parse(orderUID);



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
