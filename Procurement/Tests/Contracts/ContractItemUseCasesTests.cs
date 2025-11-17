/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Procurement.Tests.dll              Pattern   : Use cases tests                         *
*  Type     : ContractItemUseCasesTests                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for contract items use cases.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Procurement.Contracts;
using Empiria.Procurement.Contracts.Adapters;
using Empiria.Procurement.Contracts.UseCases;

namespace Empiria.Tests.Procurement.Contracts {

  /// <summary>Test cases for contract items use cases.</summary>
  public class ContractItemUseCasesTests {

    #region Use cases initialization

    private readonly ContractItemUseCases _itemusecases;

    public ContractItemUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _itemusecases = ContractItemUseCases.UseCaseInteractor();
    }

    ~ContractItemUseCasesTests() {
      _itemusecases.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public void Should_Create_A_Contract_Item() {

      var fields = new ContractItemFields {
        ProductUID = TestingConstants.CONTRACT_ITEM_PRODUCT_UID,
        Description = "Prueba contract items  2000",
        ProductUnitUID = TestingConstants.CONTRACT_ITEM_UNIT_UID,
        MaxQuantity = 5,
        MinQuantity = 2,
        UnitPrice = 20,
        BudgetAccountUID = TestingConstants.CONTRACT_BUDGET_ACCOUNT_UID,
        ProjectUID = TestingConstants.CONTRACT_ITEM_PROJECT_UID
      };

      ContractItemDto sut = _itemusecases.AddContractItem(TestingConstants.CONTRACT_UID, fields);

      Assert.NotNull(sut);
      Assert.NotNull(sut.UID);
    }


    [Fact]
    public void Should_Read_A_Contract_Item() {

      ContractItemDto sut = _itemusecases.GetContractItem(TestingConstants.CONTRACT_ITEM_UID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Remove_A_Contract_Item() {

      _itemusecases.RemoveContractItem(TestingConstants.CONTRACT_UID, TestingConstants.CONTRACT_ITEM_UID);

    }


    [Fact]
    public void Should_Update_A_Contract_Item() {

      var fields = new ContractItemFields {
        ProductUID = TestingConstants.CONTRACT_ITEM_PRODUCT_UID,
        Description = "Prueba contract item modificar en test",
        ProductUnitUID = TestingConstants.CONTRACT_ITEM_UNIT_UID,
        MaxQuantity = 10,
        MinQuantity = 5,
        UnitPrice = 20,
        BudgetAccountUID = TestingConstants.CONTRACT_BUDGET_ACCOUNT_UID,
        ProjectUID = TestingConstants.CONTRACT_ITEM_PROJECT_UID
      };

      ContractItemDto sut = _itemusecases.UpdateContractItem(TestingConstants.CONTRACT_UID,
                                                             TestingConstants.CONTRACT_ITEM_UID,
                                                             fields);

      Assert.NotNull(sut);
      Assert.NotNull(sut.UID);
    }

    #endregion Facts

  }  // class ContractItemUseCasesTests

}  // namespace Empiria.Tests.Procurement.Contracts
