/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Procurement.Tests.dll              Pattern   : Unit Tests                              *
*  Type     : ContractMilestoneTests                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for ContractMilestone type.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using Empiria.Procurement.Contracts;

namespace Empiria.Tests.Procurement.Contracts {

  /// <summary>Unit tests for ContractMilestone type.</summary>
  public class ContractMilestoneTests {

    #region Facts


    [Fact]
    public void Should_Read_All_Contract_Milestones() {
      var sut = BaseObject.GetFullList<ContractMilestone>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);

    }


    [Fact]
    public void Should_Read_The_Empty_ContractMilestone() {
      var sut = ContractMilestone.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class ContractMilestoneTests

}  // namespace Empiria.Tests.Procurement.Contracts
