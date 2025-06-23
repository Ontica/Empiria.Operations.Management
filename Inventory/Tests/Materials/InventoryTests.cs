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
using Empiria.Products;
using Xunit;

namespace Empiria.Tests.Inventory {

  /// <summary>Unit tests for Inventory.</summary>
  public class InventoryTests {

    #region Initialization

    public InventoryTests() {
      //TestsCommonMethods.Authenticate();
    }

    #endregion Initialization

    [Fact]
    public void Should_Parse_All_Inventory_orders() {
      var orders = BaseObject.GetList<InventoryOrder>();

      foreach (var sut in orders) {
        Assert.NotEmpty(sut.OrderNo);
        Assert.NotEmpty(sut.Description);        
      }
    }


    [Fact]
    public void Should_Read_All_Inventory_orders() {
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
    public void CreateInventoryOrder() {
    
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

      var order = InventoryOrder.Parse("3e79e15e-c6a3-4be2-a1b1-5c68b6fc8664");
      var location = CommonStorage.ParseNamedKey<Location>(fields.Location);

      var product = Product.TryParseWithCode(fields.Product);     
      Assertion.Require(product, "El producto no existe");
     
      fields.ProductUID = product.UID;
      fields.Description  = product.Description;
      fields.ProductUnitUID = product.BaseUnit.UID;

      order.AddItem(location, fields);

      var sut = GetInventoryOrder(order.UID);

      Assert.NotNull(sut);
    }

    public InventoryHolderDto GetInventoryOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder inventoryOrder = InventoryUtility.GetInventoryOrder(orderUID);

      InventoryOrderActions actions = InventoryUtility.GetActions(inventoryOrder.Items);

      return InventoryOrderMapper.MapToHolderDto(inventoryOrder, actions);
    }

    [Fact]
    public void GetInventoryTypes() {

      var sut = InventoryType.GetList().MapToNamedEntityList();
      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void GetWarehouses() {

      var commonStorage = CommonStorage.GetList<Location>().FindAll(x => x.Level == 1).MapToNamedEntityList();
      Assert.NotNull(commonStorage);
    }

  } // class InventoryTests

} // namespace Empiria.Tests.Inventory
