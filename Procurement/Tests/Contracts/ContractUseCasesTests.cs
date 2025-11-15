/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Procurement.Tests.dll              Pattern   : Use cases tests                         *
*  Type     : ContractUseCasesTests                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for contract use cases.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Xunit;

using Empiria.Procurement.Contracts;
using Empiria.Procurement.Contracts.Adapters;
using Empiria.Procurement.Contracts.UseCases;

namespace Empiria.Tests.Procurement.Contracts {

  /// <summary>Test cases for contract use cases.</summary>
  public class ContractUseCasesTests {

    #region Use cases initialization

    private readonly ContractUseCases _usecases;

    public ContractUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = ContractUseCases.UseCaseInteractor();
    }

    ~ContractUseCasesTests() {
      _usecases.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public void Should_Add_A_Contract() {
      var fields = new ContractFields {
        ContractCategoryUID = TestingConstants.CONTRACT_TYPE_UID,
        ContractNo = "DAGA/146/2023",
        Name = "BANOBRAS-2023-O-00ABCD",
        Description = "Fábrica de Software 2023-2025",
        CurrencyUID = TestingConstants.CONTRACT_CURRENCY_UID,
        RequestedByUID = TestingConstants.MANAGED_BY_ORG_UNIT_UID,
        BudgetTypeUID = TestingConstants.CONTRACT_BUDGET_TYPE_UID,
        BudgetsUIDs = new string[] { TestingConstants.CONTRACT_BUDGET_UID },
        ProviderUID = TestingConstants.SUPPLIER_UID,
      };

      ContractHolderDto sut = _usecases.CreateContract(fields);

      Assert.NotNull(sut);
      Assert.NotNull(sut.Contract.UID);
      Assert.Equal(fields.ContractNo, sut.Contract.ContractNo);
      Assert.Equal(fields.Name, sut.Contract.Name);
    }

    [Fact]
    public void Should_Update_A_Contract() {
      var fields = new ContractFields {
        ContractCategoryUID = TestingConstants.CONTRACT_TYPE_UID,
        ContractNo = "DAGA/031/2022",
        Name = "BANOBRAS-2024-O-XXXXXX",
        Description = "Servicios de soporte técnico y mantenimiento al Sistema Fiduciario que opera en Banobras YATLA",
        CurrencyUID = TestingConstants.CONTRACT_CURRENCY_UID,
        FromDate = new DateTime(2022, 09, 01),
        ToDate = new DateTime(2024, 08, 31),
        SignDate = new DateTime(2022, 09, 01),
        RequestedByUID = TestingConstants.MANAGED_BY_ORG_UNIT_UID,
        BudgetTypeUID = TestingConstants.CONTRACT_BUDGET_TYPE_UID,
        ProviderUID = TestingConstants.SUPPLIER_UID,
      };

      ContractHolderDto sut = _usecases.UpdateContract(TestingConstants.CONTRACT_UID, fields);

      Assert.NotNull(sut);
      Assert.NotNull(sut.Contract.UID);
      Assert.Equal(fields.ContractNo, sut.Contract.ContractNo);
      Assert.Equal(fields.Name, sut.Contract.Name);
    }

    [Fact]
    public void Should_Read_A_Contract() {

      ContractHolderDto sut = _usecases.GetContract(TestingConstants.CONTRACT_UID);

      Assert.NotNull(sut);

    }


    [Fact]
    public void Should_Search_Contracts() {

      var query = new ContractQuery {
        Keywords = "test",
      };

      var sut = _usecases.SearchContracts(query);

      Assert.NotNull(sut);

    }

    #endregion Facts

  }  // class ContractUseCasesTests

}  // namespace Empiria.Tests.Procurement.Contracts
