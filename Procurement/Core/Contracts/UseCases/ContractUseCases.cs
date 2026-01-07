/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Use cases Layer                         *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Use case interactor class               *
*  Type     : ContractUseCases                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for contract management.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Parties;
using Empiria.Services;

using Empiria.Orders;
using Empiria.Orders.Adapters;
using Empiria.Orders.Data;

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

    public ContractHolderDto Activate(string contractUID) {
      Assertion.Require(contractUID, nameof(contractUID));

      var contract = Contract.Parse(contractUID);

      contract.Activate();

      contract.Save();

      return ContractMapper.Map(contract);
    }


    public ContractItemDto AddContractItem(string contractUID, ContractItemFields fields) {
      Assertion.Require(contractUID, nameof(contractUID));
      Assertion.Require(fields, nameof(fields));

      fields.ContractUID = contractUID;

      fields.EnsureValid();

      var contract = Contract.Parse(contractUID);

      var contractItem = new ContractItem(OrderItemType.ContractItemPayable, contract);

      contractItem.Update(fields);

      contract.AddItem(contractItem);

      contractItem.Save();

      return ContractItemMapper.Map(contractItem);
    }


    public FixedList<ContractDescriptor> AvailableContracts(Party requestedBy) {
      Assertion.Require(requestedBy, nameof(requestedBy));

      var contracts = Contract.GetList()
                              .FindAll(x => x.RequestedBy.Equals(requestedBy));

      return ContractMapper.MapToDescriptor(contracts);
    }


    public ContractHolderDto CreateContract(ContractFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var contract = new Contract(OrderType.Contract);

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


    public FixedList<PayableOrderItemDto> GetAvailableOrderItems(Contract contract) {
      Assertion.Require(contract, nameof(contract));

      return PayableOrderMapper.Map(contract.Requisition.GetItems<PayableOrderItem>());
    }


    public ContractHolderDto GetContract(string contractUID) {
      Assertion.Require(contractUID, nameof(contractUID));

      var contract = Contract.Parse(contractUID);

      return ContractMapper.Map(contract);
    }


    public ContractItemDto RemoveContractItem(string contractUID, string contractItemUID) {
      Assertion.Require(contractUID, nameof(contractUID));
      Assertion.Require(contractItemUID, nameof(contractItemUID));

      var contract = Contract.Parse(contractUID);

      ContractItem contractItem = contract.RemoveItem(contractItemUID);

      contract.Save();
      contractItem.Save();

      return ContractItemMapper.Map(contractItem);
    }


    public FixedList<ContractDescriptor> SearchContracts(OrdersQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      FixedList<Contract> contracts = OrdersData.Search<Contract>(filter, sort);

      return ContractMapper.MapToDescriptor(contracts);
    }


    public OrderHolderDto Suspend(string uID) {
      throw new NotImplementedException();
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


    public ContractItemDto UpdateContractItem(string contractUID,
                                              string contractItemUID,
                                              ContractItemFields fields) {
      Assertion.Require(contractUID, nameof(contractUID));
      Assertion.Require(contractItemUID, nameof(contractItemUID));
      Assertion.Require(fields, nameof(fields));

      fields.ContractUID = contractUID;

      fields.EnsureValid();

      var contract = Contract.Parse(contractUID);

      ContractItem contractItem = contract.GetItem(contractItemUID);

      contract.UpdateItem(contractItem, fields);

      contract.Save();

      return ContractItemMapper.Map(contractItem);
    }

    #endregion Use cases

  }  // class ContractUseCases

}  // namespace Empiria.Procurement.Contracts.UseCases
