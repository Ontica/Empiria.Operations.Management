/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Test cases                              *
*  Assembly : Empiria.Orders.Tests.dll                   Pattern   : Unit Tests                              *
*  Type     : OrderTests                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for Order type.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Orders;

namespace Empiria.Tests.Orders {

  /// <summary>Unit tests for Order type.</summary>
  public class OrderTests {

    #region Facts

    [Fact]
    public void Should_Read_All_Orders() {
      var orders = BaseObject.GetFullList<Order>();

      foreach (var sut in orders) {
        Assert.NotNull(sut.Responsible);
        Assert.NotNull(sut.Beneficiary);
        Assert.NotNull(sut.Provider);
      }
    }


    [Fact]
    public void Should_Read_Empty_Order() {
      var sut = Order.Empty;

      Assert.NotNull(sut);
      Assert.NotNull(sut.Responsible);
      Assert.NotNull(sut.Beneficiary);
      Assert.NotNull(sut.Provider);
    }

    #endregion Facts

  }  // class OrderTests

}  // namespace Empiria.Tests.Orders
