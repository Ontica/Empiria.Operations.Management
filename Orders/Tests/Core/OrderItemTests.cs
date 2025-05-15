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

namespace Empiria.Tests.Orders {

  /// <summary>Unit tests for OrderItem type.</summary>
  public class OrderItemTests {

    #region Facts

    [Fact]
    public void GetSalesOrderItemTest() {

      SalesOrderItem sut = SalesOrderItem.Parse(1903);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Read_All_Sales_Orders_Items() {
 
      var salesOrderItems = SalesOrderItem.Parse(1902);

      Assert.NotNull(salesOrderItems);
    }

    [Fact]
    public void Should_Read_All_Orders_Items() {
      var orderItems = BaseObject.GetFullList<OrderItem>();

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
