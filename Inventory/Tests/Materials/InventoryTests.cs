/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryTests                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for Inventory.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Inventory;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.Data;
using Empiria.Locations;
using Empiria.Orders;
using Empiria.Parties;
using Empiria.Products;
using Empiria.StateEnums;
using Xunit;

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
    public void Should_Add_Inventory_EntriesTest() {

      TestsCommonMethods.Authenticate();

      string orderUID = "75dadef4-0bc3-417b-a7e2-5b34f670f0a4";
      string orderItemUID = "ea4b9d7c-f9dd-4987-aa87-572cda50a526";

      InventoryEntryFields fields = new InventoryEntryFields {

        Location = "A-005-01-23",
        Product = "TG5F916X112-320",
        Cost = 3.7628m ,
        Quantity = 200
      };

      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));
      Assertion.Require(fields, nameof(fields));

      ProductEntry productEntry = InventoryOrderData.GetProductEntryByName(fields.Product.Trim());
      LocationEntry locationEntry = InventoryOrderData.GetLocationEntryByName(fields.Location.Trim());

      var orderItem = InventoryOrderItem.Parse(orderItemUID);
      Assertion.Require(productEntry.ProductId == orderItem.Product.Id, "El producto no coincide con el seleccionado.");

      fields.ProductUID = Product.Parse(productEntry.ProductId).UID;
      fields.LocationUID = Location.Parse(locationEntry.LocationId).UID;

      var inventoryEntry = new InventoryEntry(orderUID, orderItemUID);

      inventoryEntry.AddEntry(fields);

      inventoryEntry.Save();

      Assert.NotNull(inventoryEntry);
    }


    [Fact]
    public void Should_Create_InventoryOrder() {
    
      TestsCommonMethods.Authenticate();

      InventoryOrderFields fields = new InventoryOrderFields {
        WarehouseUID = "35EA9626-332F-4234-B62C-053A8E81350C",
        InventoryTypeUID = "68AC65E2-4122-42B2-BEC6-48E9417086AD", 
        Description = "Prueba 20 de Junio antes de ir por las memelas",
        RequestedByUID = "0a384dc7-9c68-407c-afe1-d73b71d260cd",
        ResponsibleUID = "68188d1b-2b69-461a-86cb-f1e7386c4cb1",        
      };

      var orderType = Orders.OrderType.Parse(4010);

      InventoryOrder order = new InventoryOrder(fields.WarehouseUID, orderType);

      order.Update(fields);

      order.Save();

      Assert.NotNull(order);
    }


    [Fact]
    public void Should_Create_Inventory_Order_Item() {

      TestsCommonMethods.Authenticate();
      //TG5F38X34
      InventoryOrderItemFields fields = new InventoryOrderItemFields {
        Location = "A-001-01-23",
        Product= "ASF24",
        Quantity = 13,
      };

      var order = InventoryOrder.Parse("a7a99924-0efc-41b9-9b65-5b78d31bf329");

      var location = CommonStorage.TryParseNamedKey<Location>(fields.Location);
      Assertion.Require(location, $"La ubiacacion {fields.Location} no existe.");

      var product = Product.TryParseWithCode(fields.Product);     
      Assertion.Require(product, "El producto no existe");
     
      fields.ProductUID = product.UID;
      fields.Description  = product.Description;
      fields.ProductUnitUID = product.BaseUnit.UID;

      var orderItem = order.AddItem(location, fields);

      var inventoryEntry = new InventoryEntry(order.UID, orderItem.UID);

      InventoryEntryFields entryFields = new InventoryEntryFields();
      
      entryFields.Product = fields.Product;
      entryFields.Quantity = fields.Quantity;
      entryFields.Location = fields.Location;

      inventoryEntry.Update(entryFields, orderItem.UID);

      inventoryEntry.Save();

      Assert.NotNull(order);
    }


    [Fact]
    public void Should_Create_Inventory_EntriesTest() {

      TestsCommonMethods.Authenticate();

      string orderUID = "a7a99924-0efc-41b9-9b65-5b78d31bf329";
      string orderItemUID = "f4874ac2-52f4-4061-87f4-28a554a5a0d7";

      InventoryEntryFields fields = new InventoryEntryFields {

        Location = "A-001-01-23",
        Product = "ASF24",
        Quantity = 5
      };

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
     
      Assert.NotNull(inventoryEntry);
    }


    [Fact]
    public void Should_Close_Inventory_Order() {
      var orderUID = "ee38424a-be8f-41bb-aca8-e8599cb9a1c6";

      TestsCommonMethods.Authenticate();

      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);

      order.Close();
      order.Save();

      Assert.NotNull(order);
    }


    [Fact]
    public void Should_Delete_Inventory_Order() {
      var orderUID = "611d5a4e-9cb4-4006-86a0-85dc302fb5b0";

      TestsCommonMethods.Authenticate();

      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);

      order.Delete();
      order.Save();

      Assert.NotNull(order);
    }


    [Fact]
    public void Should_Delete_Inventory_Order_Item() {
      var orderUID = "bedf1014-f49d-47a8-a1da-3e56f712ec47";
      var orderItemUID = "504e14a9-6158-42a6-8054-17cd1f5cacf3"; 

      TestsCommonMethods.Authenticate();

      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);

      order.DeleteItem(orderItemUID);
     
      Assert.NotNull(order);
    }


    [Fact]
    public void Should_Get_InventoryOrder() {
      var orderUID = "17f8f807-1dc1-414c-aaa9-bda2f65711b1";

      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder inventoryOrder = InventoryUtility.GetInventoryOrder(orderUID);

      InventoryOrderActions actions = InventoryUtility.GetActions(inventoryOrder);

      var sut = InventoryOrderMapper.MapToHolderDto(inventoryOrder, actions);

       Assert.NotNull(sut);
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
    public void Should_Output_Inventory_EntriesTest() {

      TestsCommonMethods.Authenticate();

      string orderUID = "75dadef4-0bc3-417b-a7e2-5b34f670f0a4";
      string orderItemUID = "ea4b9d7c-f9dd-4987-aa87-572cda50a526";

      InventoryEntryFields fields = new InventoryEntryFields {

        Location = "A-005-01-23",
        Product = "TG5F916X112-320",
        Cost = 3.7628m,
        Quantity = 500
      };

      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));
      Assertion.Require(fields, nameof(fields));

      ProductEntry productEntry = InventoryOrderData.GetProductEntryByName(fields.Product.Trim());
      LocationEntry locationEntry = InventoryOrderData.GetLocationEntryByName(fields.Location.Trim());

      var orderItem = InventoryOrderItem.Parse(orderItemUID);
      Assertion.Require(productEntry.ProductId == orderItem.Product.Id, "El producto no coincide con el seleccionado.");

      fields.ProductUID = Product.Parse(productEntry.ProductId).UID;
      fields.LocationUID = Location.Parse(locationEntry.LocationId).UID;

      var inventoryEntry = new InventoryEntry(orderUID, orderItemUID);

      inventoryEntry.OutputEntry(fields);

      inventoryEntry.Save();

      Assert.NotNull(inventoryEntry);
    }


    [Fact]
    public void Should_Output_Inventory_Entries_VWTest() {

      TestsCommonMethods.Authenticate();

      string orderUID = "1bb0543e-f733-4fa8-8f02-56844de8f95a";
      var items = InventoryOrder.Parse("3652bd82-ea56-4d35-a0d9-75a165726063").Items;

      var order = Order.Parse(orderUID);

      foreach (var item in items) {
        
        var inventoryEntry = new InventoryEntry(order, item);

        var price = InventoryOrderData.GetProductPriceFromVirtualWarehouse(item.Product.Id);
        inventoryEntry.OutputEntry(price);

        inventoryEntry.Save();
      }      

      Assert.NotNull(orderUID);
    }


    [Fact]
    public void Should_Update_Inventory_Order() {
      var orderUID = "e4c7b65f-fd7c-4e43-b5e6-505015dbb22b";

      InventoryOrderFields fields = new InventoryOrderFields {
        WarehouseUID = "6E18F039-291D-43D9-9660-9C206326F01E",
        InventoryTypeUID = "0691ACCC-7787-444B-930C-A07035A6DE09",
        Description = "24 de junio prueba update cambiado por Chris Y Hugo v2",
        RequestedByUID = "68188d1b-2b69-461a-86cb-f1e7386c4cb1",
        ResponsibleUID = "0a384dc7-9c68-407c-afe1-d73b71d260cd",
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


  } // class InventoryTests

} // namespace Empiria.Tests.Inventory
