/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryTests                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for Inventory.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Xunit;

using Empiria.Locations;

using Empiria.Parties;
using Empiria.Products;
using Empiria.StateEnums;

using Empiria.Orders;

using Empiria.Inventory;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.Data;
using Empiria.Inventory.UseCases;

namespace Empiria.Tests.Inventory {

  /// <summary>Unit tests for Inventory.</summary>
  public class InventoryTests {

    #region Initialization

    public InventoryTests() {
    }

    #endregion Initialization

    [Fact]
    public void Should_Parse_All_Inventory_orders() {
      var orders = BaseObject.GetList<InventoryOrder>();

      foreach (var sut in orders) {
        Assert.NotEmpty(sut.OrderNo);
        Assert.NotEmpty(sut.Description);
        Assert.NotNull(sut.InventoryType);
      }
    }


    [Fact]
    public void Should_Read_All_Inventory_Orders() {
      var sut = BaseObject.GetList<InventoryOrder>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_InventoryOrder() {
      var sut = InventoryOrder.Empty;

      Assert.NotNull(sut);
      Assert.Equal(InventoryOrder.Parse("Empty"), sut);
      Assert.Equal(-1, sut.Id);
    }


    [Fact]
    public void Should_Create_InventoryOrder() {

      TestsCommonMethods.Authenticate();

      InventoryOrderFields fields = new InventoryOrderFields {
        WarehouseUID = "4D219B4A-3355-4BF3-8844-AA53211C8EFD",
        InventoryTypeUID = "9D5F118E-1A12-4774-A2E0-7CB2E180F559",
        Description = "Pruebas01",
        RequestedByUID = "72b902de-8840-4985-81aa-46700d915ea7",
        ResponsibleUID = "66c659c8-fee5-4487-baa5-84d056f123a1",
        //Priority = Empiria.StateEnums.Priority.Normal
      };

      var orderType = Orders.OrderType.Parse(4011);

      InventoryOrder order = new InventoryOrder(fields.WarehouseUID, orderType);

      order.Update(fields);

      order.Save();

      Assert.NotNull(order);
    }

    [Fact]
    public void Should_GetProductPriceFromHistoricCost() {

      TestsCommonMethods.Authenticate();

      var productKey = InventoryOrderData.GetProductPriceFromHistoricCost("MM10912X20-800");

      Assert.NotNull(productKey.ToString());
    }


    [Fact]
    public void Should_Create_Inventory_Order_Item() {

      TestsCommonMethods.Authenticate();

      InventoryOrderItemFields fields = new InventoryOrderItemFields {
        Location = "A-021-02-09",
        Product = "TG858X4-140",
        Quantity = 23801,
      };

      var order = InventoryOrder.Parse("d2413b35-8d79-4f89-9f7c-174c903b2510");

      var product = Product.TryParseWithCode(fields.Product);
      Assertion.Require(product, "El producto no existe");

      var location = CommonStorage.TryParseNamedKey<Location>(fields.Location);

      Assertion.Require(location, $"La ubicacion {fields.Location} no existe.");

      Assertion.Require(order.Warehouse == GetRootLocation(location),
                    $"La localización {fields.Location} no existe en el almacen {order.Warehouse.Name}");

      var isnotexistProductinLocation = VerifyProductAndLocationInOrder(order.Id, product.Id, location.Id);
      Assertion.Require(isnotexistProductinLocation, $"Ya existe ese producto en esa localización {fields.Location}.");

      fields.ProductUID = product.UID;
      fields.Description = product.Description;
      fields.ProductUnitUID = product.BaseUnit.UID;

      var orderItemType = Orders.OrderItemType.Parse(4059);

      InventoryOrderItem orderItem = new InventoryOrderItem(orderItemType, order, location);

      orderItem.Update(fields);
      order.AddItem(orderItem);
      orderItem.Save();

      AddInventoryEntry(order, orderItem);

      Assert.NotNull(order);
    }


    [Fact]
    public void Should_Close_Inventory_Order() {
      var orderUID = "cc68d35d-3bc2-470b-bf8a-7c5ba0fe5e1f";

      TestsCommonMethods.Authenticate();

      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);

      order.Close();
      order.Save();

      order.CloseItems();

      OutputInventoryEntriesVW(order);

      var inventoryEntryUseCase = InventoryEntryUseCases.UseCaseInteractor();

      inventoryEntryUseCase.CloseInventoryEntries(order.UID);

      Assert.NotNull(order);
    }

    private void OutputInventoryEntriesVW(InventoryOrder order) {

      foreach (var item in order.GetItems<InventoryOrderItem>()) {

        var inventoryEntry = new InventoryEntry(order, item);

        var price = InventoryOrderData.GetProductPriceFromVirtualWarehouse(item.Product.Id);
        inventoryEntry.OutputEntry(price);

        inventoryEntry.Save();
      }
    }

    [Fact]
    public void Should_Delete_Inventory_Order() {
      var orderUID = "d2413b35-8d79-4f89-9f7c-174c903b2510";

      TestsCommonMethods.Authenticate();

      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);

      order.Delete();
      order.Save();

      Assert.NotNull(order);
    }


    [Fact]
    public void Should_Delete_Inventory_Order_Item() {
      var orderUID = "3652bd82-ea56-4d35-a0d9-75a165726063";
      var orderItemUID = "1001fb6b-dd7b-4270-92f9-2c73c35401a6";

      TestsCommonMethods.Authenticate();

      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);

      var item = order.GetItem<InventoryOrderItem>(orderItemUID);

      order.RemoveItem(item);

      item.Save();

      Assert.NotNull(order);
    }


    [Fact]
    public void Should_Get_InventoryOrder() {

      TestsCommonMethods.Authenticate();
      var orderUID = "a33c76c7-c266-43ff-bfb2-2b2b820b312a";

      Assertion.Require(orderUID, nameof(orderUID));
      //var x = ExecutionServer.CurrentPrincipal.Permissions;
      //var HasCountVariance = ExecutionServer.CurrentPrincipal.IsInRole("inventory-manager");

      InventoryOrder inventoryOrder = InventoryUtility.GetInventoryOrder(orderUID);

      InventoryOrderActions actions = InventoryUtility.GetActions(inventoryOrder);

      var sut = InventoryOrderMapper.MapToHolderDto(inventoryOrder, actions);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Get_InventoryOrderItem() {

      var item1 = InventoryOrderItem.Parse(33189);

      //var order = InventoryOrder.Parse(orderUID);

      //var item = order.GetItem<InventoryOrderItem>(itemUID);


      Assert.NotNull(item1);
    }

    [Fact]
    public void Should_Get_InventoryTypes() {

      var sut = InventoryType.GetList();
      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Warehouses() {

      var commonStorage = CommonStorage.GetList<Location>().FindAll(x => x.Level == 1 && x.GetStatus<EntityStatus>() != EntityStatus.Deleted).MapToNamedEntityList();
      Assert.NotNull(commonStorage);
    }


    [Fact]
    public void Should_Get_Parties() {

      var sut = Party.GetPartiesInRole("User").MapToNamedEntityList();
      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Load_Input_Orders_FromNK() {
      DateTime date = Convert.ToDateTime("01-10-2025");

      var sut = Order.GetFullList<Order>().FindAll(x => x.ClosingTime == date && x.OrderType.Id == 4005);

      string inventoryTypeUID = "a40c65bd-9a56-48eb-a8bf-f9245ecd3004";

      InventoryOrder inventoryOrder;
      for (int i = 0; i < 3; i++) {
        var order = sut[i];
        inventoryOrder = GenerateInventoryOrder(order, inventoryTypeUID);
        GenerateInventoryOrderItems(order, inventoryOrder);
      }

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Load_Output_Orders_FromNK() {
      DateTime date = Convert.ToDateTime("01-10-2025");

      var sut = Order.GetFullList<Order>().FindAll(x => x.ClosingTime == date && x.OrderType.Id == 4011);

      string inventoryTypeUID = "0eb5a072-b857-4071-8b06-57a34822ec64";

      InventoryOrder inventoryOrder;
      for (int i = 0; i < 3; i++) {
        var order = sut[i];
        inventoryOrder = GenerateInventoryOrder(order, inventoryTypeUID);
        GenerateInventoryOrderItems(order, inventoryOrder);
      }

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Load_Inventory_Output_Orders() {
      var order = Order.Parse(21209);

      string inventoryTypeUID = "0eb5a072-b857-4071-8b06-57a34822ec64";
      var sut = GenerateInventoryOrder(order, inventoryTypeUID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Load_Inventory_Input_Orders() {
      var order = Order.Parse(21243);

      string inventoryTypeUID = "a40c65bd-9a56-48eb-a8bf-f9245ecd3004";
      var sut = GenerateInventoryOrder(order, inventoryTypeUID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Load_Inventory_OrderItems() {
      var order = Order.Parse(21243);

      var inventoryOrder = InventoryOrder.Parse(21257);

      GenerateInventoryOrderItems(order, inventoryOrder);

      Assert.NotNull(inventoryOrder);
    }


    [Fact]
    public void Should_Update_Inventory_Order() {
      var orderUID = "d2413b35-8d79-4f89-9f7c-174c903b2510";

      InventoryOrderFields fields = new InventoryOrderFields {
        WarehouseUID = "DA6017D5-ED38-449B-9659-ACE06C4565DE",
        InventoryTypeUID = "020B1EF9-F30F-41D5-9E95-29E3B52D23B9",
        Description = "12 de nov prueba update",
        RequestedByUID = "0a384dc7-9c68-407c-afe1-d73b71d260cd",
        ResponsibleUID = "68188d1b-2b69-461a-86cb-f1e7386c4cb1",
        Priority = Empiria.StateEnums.Priority.Normal
      };

      TestsCommonMethods.Authenticate();

      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var order = InventoryOrder.Parse(orderUID);
      order.Update(fields);

      order.Save();

      Assert.NotNull(order);
    }


    [Fact]
    public void Should_Update_InventoryOrderItem() {

      string orderUID = "a33c76c7-c266-43ff-bfb2-2b2b820b312a";
      string itemUID = "aae0d7b5-1dd9-485a-ae29-0974c9a7951e";

      InventoryOrderItemFields fields = new InventoryOrderItemFields {
        Quantity = 555
      };

      var order = InventoryOrder.Parse(orderUID);

      var item = order.GetItem<InventoryOrderItem>(itemUID);

      item.UpdateQuantity(fields.Quantity);

      item.Save();

      Assert.NotNull(order);
    }


    [Fact]
    public void Should_UpdatePosition() {

      TestsCommonMethods.Authenticate();
      //TG5F38X34
      var order = InventoryOrder.Parse("75dadef4-0bc3-417b-a7e2-5b34f670f0a4");

      foreach (var item in order.GetItems<InventoryOrderItem>()) {
        var entry = InventoryOrderData.GetInventoryEntry(item.Id);
        entry.UpdatePosition(item.Position);
        entry.Save();
      }

      Assert.NotNull(order);
    }


    #region Helpers

    private void AddInventoryEntry(InventoryOrder order, InventoryOrderItem orderItem) {
      var inventoryEntry = new InventoryEntry(order.UID, orderItem.UID);

      inventoryEntry.InitialEntry(orderItem.UnitPrice, orderItem.Location);

      inventoryEntry.Save();
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

      TestsCommonMethods.Authenticate();

      InventoryOrderFields fields = new InventoryOrderFields {
        WarehouseUID = "DA6017D5-ED38-449B-9659-ACE06C4565DE",
        InventoryTypeUID = inventoryTypeUID,
        Description = "Orden de inventario correspondiente " + order.OrderNo,
        RequestedByUID = order.RequestedBy.UID,
        ResponsibleUID = order.Responsible.UID,
        ParentOrderUID = order.UID,
      };

      var orderType = OrderType.Parse(4011);

      InventoryOrder inventoryOrder = new InventoryOrder(fields.WarehouseUID, orderType);

      inventoryOrder.Update(fields);

      inventoryOrder.Save();

      return inventoryOrder;
    }


    private void GenerateInventoryOrderItems(Order order, InventoryOrder inventoryOrder) {

      TestsCommonMethods.Authenticate();

      var items = order.GetItems<OrderItem>();

      var orderItemType = OrderItemType.Parse(4059);

      foreach (var item in items) {
        InventoryOrderItemFields fields = new InventoryOrderItemFields();

        fields.ProductUID = item.Product.UID;
        fields.Description = item.Product.Description;
        fields.ProductUnitUID = item.Product.BaseUnit.UID;
        fields.Quantity = item.Quantity;
        fields.Location = "A-001-01-01";

        InventoryOrderItem orderItem = new InventoryOrderItem(orderItemType, inventoryOrder);

        orderItem.Update(fields);
        inventoryOrder.AddItem(orderItem);
        orderItem.Save();
      }
    }


    private bool VerifyProductAndLocationInOrder(int orderId, int productID, int locationID) {
      if (InventoryOrderData.VerifyProductAndLocationInOrder(orderId, productID, locationID) != 0) {
        return false;
      }
      return true;
    }

    #endregion Helpers

  } // class InventoryTests

} // namespace Empiria.Tests.Inventory
