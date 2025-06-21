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
using Empiria.Locations;
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
        LocationUID = "7BDE8202-AA48-4883-9624-BA596893F9E8",
        ProductUID = "85fa3371-929b-43bf-86e4-85644db76473",
        Description = "Producto X",
        ProductUnitUID = "46926354-cc5a-4863-8790-e870ce33adcf",
        Quantity = 5,
        RequestedByUID = "55278637-91d5-4ea1-997c-668e32d73080",
        ProviderUID = "237878be-8df0-4da1-9289-88ea47d6c875",
        UnitPrice =  20.25m,
        Discount = 0
      };

     var orderItemType = Orders.OrderItemType.Parse(4059);

      var order = InventoryOrder.Parse("47893b38-c6e2-4317-af99-2c5baef49e2f");
      var location = Location.Parse(fields.LocationUID);


      InventoryOrderItem orderItem = new InventoryOrderItem(order, location, orderItemType);

      orderItem.Update(fields);

      orderItem.Save();

      Assert.NotNull(order);
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
