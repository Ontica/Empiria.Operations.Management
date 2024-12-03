/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Procurement.Tests.dll              Pattern   : Unit Tests                              *
*  Type     : ContractTests                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for retrieving contracts.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using Empiria.Procurement.Contracts;

namespace Empiria.Tests.Procurement.Contracts {

  /// <summary>Test cases for retrieving contracts.</summary>
  public class ContractTests {

    #region Facts

    [Fact]
    public void Should_Read_The_Empty_Contract() {
      var sut = Contract.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class ContractTests

}  // namespace Empiria.Tests.Procurement.Contracts
