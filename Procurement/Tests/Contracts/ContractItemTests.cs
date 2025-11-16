/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Procurement.Tests.dll              Pattern   : Unit Tests                              *
*  Type     : ContractItemTests                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for ContractItem type.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using Empiria.Procurement.Contracts;

namespace Empiria.Tests.Procurement.Contracts {

  /// <summary>Test cases for ContractItem type.</summary>
  public class ContractItemTests {

    #region Facts

    [Fact]
    public void Should_Read_All_Contract_Items() {
      var items = BaseObject.GetFullList<FormerContractItem>();

      foreach (var sut in items) {
        Assert.NotNull(sut);
        Assert.NotNull(sut.Contract);
      }
    }


    [Fact]
    public void Should_Read_Empty_ContractItem() {
      var sut = FormerContractItem.Empty;

      Assert.NotNull(sut);
      Assert.NotNull(sut.Contract);
    }

    #endregion Facts

  }  // class ContractItemTests

}  // namespace Empiria.Tests.Procurement.Contracts
