/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Use cases Layer                         *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Use case interactor class               *
*  Type     : ContractUseCases                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for contract management.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Procurement.Contracts.Data;
using Empiria.Procurement.Contracts.Adapters;

namespace Empiria.Procurement.Contracts.UseCases {

  /// <summary>Use cases for contract management.</summary>
  public class ContractUseCases : UseCase {

    #region Constructors and parsers

    protected ContractUseCases() {
      // no-op
    }

    static public ContractUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<ContractUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ContractHolderDto CreateContract(ContractFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var contract = new Contract();

      contract.Update(fields);

      contract.Save();

      return ContractMapper.Map(contract);
    }


    public ContractHolderDto DeleteContract(string contractUID) {
      Assertion.Require(contractUID, nameof(contractUID));

      var contract = Contract.Parse(contractUID);

      contract.Delete();

      contract.Save();

      return ContractMapper.Map(contract);
    }


    public ContractHolderDto GetContract(string contractUID) {
      Assertion.Require(contractUID, nameof(contractUID));

      var contract = Contract.Parse(contractUID);

      return ContractMapper.Map(contract);
    }


    public FixedList<NamedEntityDto> GetContractTypes() {
      var contractTypes = ContractType.GetList();

      return contractTypes.MapToNamedEntityList();
    }


    public FixedList<ContractDescriptor> SearchContracts(ContractQuery query) {
      Assertion.Require(query, nameof(query));

      string condition = query.MapToFilterString();
      string orderBy = query.MapToSortString();

      FixedList<Contract> contracts = ContractData.GetContracts(condition, orderBy);

      return ContractMapper.MapToDescriptor(contracts);
    }


    public ContractHolderDto UpdateContract(string ContractUID,
                                            ContractFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var contract = Contract.Parse(ContractUID);

      contract.Update(fields);

      contract.Save();

      return ContractMapper.Map(contract);
    }

    #endregion Use cases

  }  // class ContractUseCases

}  // namespace Empiria.Procurement.Contracts.UseCases
