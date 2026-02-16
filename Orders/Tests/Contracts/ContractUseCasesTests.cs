/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Orders.Tests.dll                   Pattern   : Use cases tests                         *
*  Type     : ContractUseCasesTests                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for contract use cases.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Xunit;

using Empiria.Orders.Contracts;
using Empiria.Orders.Contracts.Adapters;
using Empiria.Orders.Contracts.UseCases;

namespace Empiria.Tests.Orders.Contracts {

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
        ContractNo = "DAGA/146/2023",
        Name = "BANOBRAS-2023-O-00ABCD",
        Description = "Fábrica de Software 2023-2025",
        CurrencyUID = TestingConstants.CONTRACT_CURRENCY_UID,
        RequestedByUID = TestingConstants.MANAGED_BY_ORG_UNIT_UID,
        Budgets = new string[] { TestingConstants.CONTRACT_BUDGET_UID },
        ProviderUID = TestingConstants.SUPPLIER_UID,
      };

      ContractHolderDto sut = _usecases.CreateContract(fields);

      Assert.NotNull(sut);
      Assert.NotNull(sut.Order.UID);
      Assert.Equal(fields.ContractNo, sut.Order.ContractNo);
      Assert.Equal(fields.Name, sut.Order.Name);
    }


    [Fact]
    public void Should_Update_A_Contract() {
      var fields = new ContractFields {
        ContractNo = "DAGA/031/2022",
        Name = "BANOBRAS-2024-O-XXXXXX",
        Description = "Servicios de soporte técnico y mantenimiento al Sistema Fiduciario que opera en Banobras YATLA",
        CurrencyUID = TestingConstants.CONTRACT_CURRENCY_UID,
        StartDate = new DateTime(2022, 09, 01),
        EndDate = new DateTime(2024, 08, 31),
        SignDate = new DateTime(2022, 09, 01),
        RequestedByUID = TestingConstants.MANAGED_BY_ORG_UNIT_UID,
        Budgets = new string[] { TestingConstants.CONTRACT_BUDGET_UID },
        ProviderUID = TestingConstants.SUPPLIER_UID,
      };

      ContractHolderDto sut = _usecases.UpdateContract(TestingConstants.CONTRACT_UID, fields);

      Assert.NotNull(sut);
      Assert.NotNull(sut.Order.UID);
      Assert.Equal(fields.ContractNo, sut.Order.ContractNo);
      Assert.Equal(fields.Name, sut.Order.Name);
    }

    [Fact]
    public void Should_Read_A_Contract() {

      ContractHolderDto sut = _usecases.GetContract(TestingConstants.CONTRACT_UID);

      Assert.NotNull(sut);

    }

    #endregion Facts

  }  // class ContractUseCasesTests

}  // namespace Empiria.Tests.Orders.Contracts
