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
  public class FormerContractUseCases : UseCase {

    #region Constructors and parsers

    protected FormerContractUseCases() {
      // no-op
    }

    static public FormerContractUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FormerContractUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ContractHolderDto CreateContract(ContractFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var contract = new FormerContract(FormerContractType.Procurement);

      contract.Update(fields);

      contract.Save();

      return ContractMapper.Map(contract);
    }


    public ContractHolderDto DeleteContract(string contractUID) {
      Assertion.Require(contractUID, nameof(contractUID));

      var contract = FormerContract.Parse(contractUID);

      contract.Delete();

      contract.Save();

      return ContractMapper.Map(contract);
    }


    public ContractHolderDto GetContract(string contractUID) {
      Assertion.Require(contractUID, nameof(contractUID));

      var contract = FormerContract.Parse(contractUID);

      return ContractMapper.Map(contract);
    }


    public FixedList<NamedEntityDto> GetContractCategories() {
      var contractTypes = FormerContractCategory.GetList();

      return contractTypes.MapToNamedEntityList();
    }


    public FixedList<ContractDescriptor> SearchContracts(ContractQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();
      string sortBy = query.MapToSortString();

      FixedList<FormerContract> contracts = ContractData.GetContracts(filter, sortBy);

      return ContractMapper.MapToDescriptor(contracts);
    }


    public ContractHolderDto UpdateContract(string ContractUID,
                                            ContractFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var contract = FormerContract.Parse(ContractUID);

      contract.Update(fields);

      contract.Save();

      return ContractMapper.Map(contract);
    }

    #endregion Use cases

  }  // class ContractUseCases

}  // namespace Empiria.Procurement.Contracts.UseCases
