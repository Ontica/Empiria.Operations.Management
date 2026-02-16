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
using Empiria.Orders.Adapters;

namespace Empiria.Tests.Orders {

  /// <summary>Unit tests for OrderItem type.</summary>
  public class OrderItemTests {

    #region Facts

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

    [Fact]
    public void Should_Parse_OrderItem() {
      var sut = Order.GetFullList<Order>();

      Assert.NotNull(sut);

      foreach (var order in sut) {
        Assert.NotNull(order.Requisition);
        Assert.NotNull(order.Requisition.GetItems<OrderItem>());

        foreach (var item in order.GetItems<OrderItem>()) {
          Assert.NotNull(item.RequisitionItem);


          if (item.RequisitionItem is PayableOrderItem rqp) {
            Assert.NotNull(PayableOrderMapper.Map(rqp));
          }

          if (item is PayableOrderItem payItem) {
            Assert.NotNull(PayableOrderMapper.Map(payItem));
          }


        }
      }
    }

    #endregion Facts

  }  // class OrderItemTests

}  // namespace Empiria.Tests.Orders
