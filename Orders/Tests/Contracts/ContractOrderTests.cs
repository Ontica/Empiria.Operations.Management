/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Orders.Tests.dll                   Pattern   : Unit Tests                              *
*  Type     : ContractOrderTests                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for ContractOrder type.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using Empiria.Orders;
using Empiria.Orders.Contracts;

namespace Empiria.Tests.Orders.Contracts {

  /// <summary>Unit tests for ContractOrder type.</summary>
  public class ContractOrderTests {

    #region Facts

    [Fact]
    public void Should_Create_A_ContractOrder() {
      var contract = Contract.Parse(1);

      var sut = new ContractOrder(contract);

      Assert.NotNull(sut);
      Assert.Equal(OrderType.ContractOrder, sut.OrderType);
    }


    [Fact]
    public void Should_Read_All_Contracts_Orders() {
      var orders = BaseObject.GetFullList<ContractOrder>();

      foreach (var sut in orders) {
        Assert.NotNull(sut);
        Assert.NotNull(sut.OrderType);
        Assert.NotNull(sut.Contract);
        Assert.True(sut.GetTotal() >= 0);
      }
    }


    [Fact]
    public void Should_Read_Empty_ContractOrder() {
      var sut = ContractOrder.Empty;

      Assert.NotNull(sut);
      Assert.NotNull(sut.OrderType);
      Assert.NotNull(sut.Contract);
      Assert.Equal(0, sut.GetTotal());
    }

    #endregion Facts

  }  // class ContractOrderTests

}  // namespace Empiria.Tests.Orders.Contracts
