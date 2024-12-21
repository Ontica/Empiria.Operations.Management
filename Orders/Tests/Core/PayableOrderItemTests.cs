﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Test cases                              *
*  Assembly : Empiria.Orders.Tests.dll                   Pattern   : Unit Tests                              *
*  Type     : PayableOrderItemTests                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for PayableOrderItem type.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Orders;

namespace Empiria.Tests.Orders {

  /// <summary>Unit tests for PayableOrderItem type.</summary>
  public class PayableOrderItemTests {

    #region Facts

    [Fact]
    public void Should_Read_All_Payable_Orders_Items() {
      var orderItems = BaseObject.GetFullList<PayableOrderItem>();

      foreach (var sut in orderItems) {
        Assert.NotNull(sut);
        Assert.NotNull(sut.Order);
        Assert.NotNull(sut.OrderItemType);
        Assert.NotNull(sut.Product);
        Assert.NotNull(sut.Description);
        Assert.NotNull(sut.ProductUnit);
        Assert.NotNull(sut.Currency);
        Assert.NotNull(sut.BudgetAccount);
        Assert.True(sut.IsEmptyInstance || sut.UnitPrice > 0);
        Assert.True(sut.IsEmptyInstance || sut.Quantity > 0);
        Assert.NotNull(sut.RequestedBy);
        Assert.NotNull(sut.Project);
        Assert.NotNull(sut.Provider);
      }
    }


    [Fact]
    public void Should_Read_Empty_PayableOrderItem() {
      var sut = PayableOrderItem.Empty;

      Assert.NotNull(sut);
      Assert.NotNull(sut.Order);
      Assert.NotNull(sut.OrderItemType);
      Assert.NotNull(sut.Product);
      Assert.NotNull(sut.Description);
      Assert.NotNull(sut.ProductUnit);
      Assert.NotNull(sut.Currency);
      Assert.NotNull(sut.BudgetAccount);
      Assert.NotNull(sut.RequestedBy);
      Assert.NotNull(sut.Project);
      Assert.NotNull(sut.Provider);
    }

    #endregion Facts

  }  // class PayableOrderItemTests

}  // namespace Empiria.Tests.Orders
