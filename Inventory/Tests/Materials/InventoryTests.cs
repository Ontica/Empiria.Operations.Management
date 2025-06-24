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
using Empiria.Locations;
using Empiria.Parties;
using Empiria.Products;
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
        Location = "A-025-03-27",
        Product= "ASF24",
        Quantity = 5,
      };

      var order = InventoryOrder.Parse("611d5a4e-9cb4-4006-86a0-85dc302fb5b0");
      var location = CommonStorage.ParseNamedKey<Location>(fields.Location);

      var product = Product.TryParseWithCode(fields.Product);     
      Assertion.Require(product, "El producto no existe");
     
      fields.ProductUID = product.UID;
      fields.Description  = product.Description;
      fields.ProductUnitUID = product.BaseUnit.UID;

      order.AddItem(location, fields);

      Assert.NotNull(order);
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
      var orderUID = "397d9db7-1367-47da-9f9f-9d11b535d508";

      TestsCommonMethods.Authenticate();

      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);

      order.Delete();
      order.Save();

      Assert.NotNull(order);
    }


    [Fact]
    public void Should_Get_InventoryOrder() {
      var orderUID = "397d9db7-1367-47da-9f9f-9d11b535d508";

      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder inventoryOrder = InventoryUtility.GetInventoryOrder(orderUID);

      InventoryOrderActions actions = InventoryUtility.GetActions(inventoryOrder);

      Assert.NotNull(inventoryOrder);
    }

    [Fact]
    public void Should_Get_InventoryTypes() {

      var sut = InventoryType.GetList().MapToNamedEntityList();
      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Warehouses() {

      var commonStorage = CommonStorage.GetList<Location>().FindAll(x => x.Level == 1).MapToNamedEntityList();
      Assert.NotNull(commonStorage);
    }


    [Fact]
    public void Should_Get_Parties() {

      var sut = Party.GetPartiesInRole("User").MapToNamedEntityList();
      Assert.NotNull(sut);
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
