/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Mapper                                  *
*  Type     : ContractMapper                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for Contract instances.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents.Services;
using Empiria.History.Services;

using Empiria.StateEnums;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Provides data mapping services for Contract instances.</summary>
  static internal class ContractMapper {

    static internal ContractHolderDto Map(Contract contract) {
      return new ContractHolderDto {
        Contract = MapContract(contract),
        Items = ContractItemMapper.Map(contract.GetItems()),
        BudgetTransactions = MapBudgetTransactions(contract),
        Milestones = ContractMilestoneMapper.Map(contract.GetMilestones()),
        Documents = DocumentServices.GetEntityDocuments(contract),
        History = HistoryServices.GetEntityHistory(contract),
        Actions = MapActions(contract)
      };
    }


    static internal ContractDto MapContract(Contract contract) {
      return new ContractDto {
        UID = contract.UID,
        ContractCategory = contract.ContractCategory.MapToNamedEntity(),
        ContractNo = contract.ContractNo,
        Name = contract.Name,
        Description = contract.Description,
        Supplier = contract.Supplier.MapToNamedEntity(),
        SuppliersGroup = contract.Supplier is Parties.Group group ?
                                group.Members.MapToNamedEntityList() : new FixedList<NamedEntityDto>(),
        ManagedByOrgUnit = contract.ManagedByOrgUnit.MapToNamedEntity(),
        IsForMultipleOrgUnits = contract.IsForMultipleOrgUnits,
        BudgetType = contract.BudgetType.MapToNamedEntity(),
        FromDate = contract.FromDate,
        ToDate = contract.ToDate,
        SignDate = contract.SignDate,
        Currency = contract.Currency.MapToNamedEntity(),
        Total = contract.Total,
        Status = contract.Status.MapToDto()
      };
    }


    static internal FixedList<ContractDescriptor> MapToDescriptor(FixedList<Contract> contracts) {
      return contracts.Select(contract => MapToDescriptor(contract))
                       .ToFixedList();
    }

    #region Helpers

    static private ContractActions MapActions(Contract contract) {
      return new ContractActions {
        CanActivate = contract.CanActivate(),
        CanDelete = contract.CanDelete(),
        CanEditItems = contract.CanUpdate(),
        CanEditDocuments = contract.CanUpdate(),
        CanRequestBudget = contract.CanRequestBudget(),
        CanSuspend = contract.CanSuspend(),
        CanUpdate = contract.Status == EntityStatus.Pending
      };
    }


    static private FixedList<BudgetTransactionDescriptorDto> MapBudgetTransactions(Contract contract) {
      FixedList<BudgetTransaction> transactions = BudgetTransaction.GetFor(contract);

      return BudgetTransactionMapper.MapToDescriptor(transactions);
    }


    static private ContractDescriptor MapToDescriptor(Contract contract) {
      return new ContractDescriptor {
        UID = contract.UID,
        ContractCategory = contract.ContractCategory.Name,
        ContractNo = contract.ContractNo,
        Name = contract.Name,
        Description = contract.Description,
        ManagedByOrgUnit = contract.ManagedByOrgUnit.FullName,
        Supplier = contract.Supplier.Name,
        FromDate = contract.FromDate,
        ToDate = contract.ToDate,
        SignDate = contract.SignDate,
        BudgetType = contract.BudgetType.DisplayName,
        Currency = contract.Currency.Name,
        Total = contract.Total,
        StatusName = contract.Status.GetName()
      };
    }

    #endregion Helpers

  }  // class ContractMapper

}  // namespace Empiria.Procurement.Contracts.Adapters
