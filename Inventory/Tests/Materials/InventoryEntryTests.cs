/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryEntryTests                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit Inventory Entry Tests for Inventory.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Inventory;
using Empiria.Inventory.Data;
using Empiria.Locations;
using Empiria.Orders;
using Empiria.Orders.Adapters;
using Empiria.Products;

using Xunit;

namespace Empiria.Tests.Inventory {

  /// <summary>Unit Inventory Entry Tests for Inventory.</summary>
  public class InventoryEntryTests {

    #region Initialization

    public InventoryEntryTests() {
    }

    #endregion Initialization
      
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
  
    #region Helpers

    #endregion Helpers

  } // class InventoryEntryTests

} // namespace Empiria.Tests.Inventory
