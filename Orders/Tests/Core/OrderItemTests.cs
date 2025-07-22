/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Test cases                              *
*  Assembly : Empiria.Orders.Tests.dll                   Pattern   : Unit Tests                              *
*  Type     : OrderItemTests                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for OrderItem type.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Orders;
using Empiria.Orders.UseCases;
using System;


namespace Empiria.Tests.Orders {

  /// <summary>Unit tests for OrderItem type.</summary>
  public class OrderItemTests {

    #region Facts

    [Fact]
    public void CreateOrderItem_WithValidFields_ShouldUpdateAndReturnDto() {

      TestsCommonMethods.Authenticate();
      string orderUID = "8d857a56-4bf6-48da-b208-0b4ab56c932e";//30
      //string itemUID = "abf84df2-fc38-479c-8791-802f23ff784c";//1902

      var fields = new SalesOrderItemFields {
        Quantity = 9876543210,//6
        UnitPrice = 100,//168.47
        Discount = 10,//70.75
        BudgetAccountUID = "Empty",//-1
        CurrencyUID = "MXN",//600
        ProductUID = "a4b3bd4b-6ba1-419b-9125-09126a0d2c0b",//49369dd9-90e8-4560-8972-ccc1b4f3ce53
        ProductUnitUID = "c3182e47-6175-4649-bddc-97eb37df09a8",//46926354-cc5a-4863-8790-e870ce33adcf
        Description = "Order Item Created by  HAGE 22 Julio 2025 16:00pm",//a0069243 01723 ov00145277 ag ic00187574
      };

      var useCase = SalesOrderUseCases.CreateInstance<SalesOrderUseCases>();

      var result = useCase.CreateOrderItem(orderUID, fields);

      Assert.NotNull(result);
    }


    [Fact]
    public void DeleteOrderItem_WithValidFields_ShouldUpdateAndReturnDto() {

      string orderUID = "8d857a56-4bf6-48da-b208-0b4ab56c932e";//30
      string itemUID = "bd7a9cff-1a96-4961-848a-70816fe5baa6";//1902
    
      var useCase = SalesOrderUseCases.CreateInstance<SalesOrderUseCases>();

      var result = useCase.DeleteOrderItem(orderUID, itemUID);

      Assert.NotNull(result);
    }


    [Fact]
    public void UpdateOrderItem_WithValidFields_ShouldUpdateAndReturnDto() {
      
      string orderUID = "8d857a56-4bf6-48da-b208-0b4ab56c932e";//30
      string itemUID  = "6788c05d-841b-4c29-8028-83b3febbb534";//1903

      var fields = new SalesOrderItemFields {
        Quantity = 1234567890,//6
        UnitPrice = 100,//168.47
        Discount = 10,//70.75
        BudgetAccountUID = "Empty",//-1
        CurrencyUID = "MXN",//600
        ProductUID = "81e44f98-4e90-4da1-a209-396bef8a0dae",//49369dd9-90e8-4560-8972-ccc1b4f3ce53
        ProductUnitUID = "c3182e47-6175-4649-bddc-97eb37df09a8",//46926354-cc5a-4863-8790-e870ce33adcf
        Description = $"Order Item Updated by  HAGE 22 Julio 2025 {DateTime.Now}",//a0069243 01723 ov00145277 ag ic00187574
      };

      var useCase = SalesOrderUseCases.CreateInstance<SalesOrderUseCases>();

      var result = useCase.UpdateOrderItem(orderUID, itemUID, fields);

      Assert.NotNull(result);
    }

    [Fact]
    public void GetSalesOrderItemTest() {

      SalesOrderItem sut = SalesOrderItem.Parse(1903);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Read_All_Sales_Orders_Items() {
 
      var salesOrder = SalesOrder.Parse(31);
      var salesOrderItems = salesOrder.GetItems<SalesOrderItem>();

      Assert.NotNull(salesOrderItems);
    }


    [Fact]
    public void Should_Read_All_Orders_Items() {
      var orderItems = BaseObject.GetFullList<SalesOrderItem>();

      foreach (var sut in orderItems) {
        Assert.NotNull(sut);
        Assert.NotNull(sut.Order);
        Assert.NotNull(sut.OrderItemType);
        Assert.NotNull(sut.Product);
        Assert.NotNull(sut.Description);
        Assert.NotNull(sut.ProductUnit);
        Assert.True(sut.IsEmptyInstance || sut.Quantity > 0);
        Assert.NotNull(sut.RequestedBy);
        Assert.NotNull(sut.Project);
        Assert.NotNull(sut.Provider);
      }
    }


    [Fact]
    public void Should_Read_Empty_OrderItem() {
      var sut = OrderItem.Empty;

      Assert.NotNull(sut);
      Assert.NotNull(sut.Order);
      Assert.NotNull(sut.OrderItemType);
      Assert.NotNull(sut.Product);
      Assert.NotNull(sut.Description);
      Assert.NotNull(sut.ProductUnit);
      Assert.NotNull(sut.RequestedBy);
      Assert.NotNull(sut.Project);
      Assert.NotNull(sut.Provider);
    }

    #endregion Facts

  }  // class OrderItemTests

}  // namespace Empiria.Tests.Orders
