/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Mapper                                  *
*  Type     : ContractMapper                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for Contract instances.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

using Empiria.Documents;
using Empiria.History;
using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Provides data mapping services for Contract instances.</summary>
  static internal class ContractMapper {

    static internal ContractHolderDto Map(FormerContract contract) {
      return new ContractHolderDto {
        Contract = MapContract(contract),
        Items = ContractItemMapper.Map(contract.GetItems()),
        BudgetTransactions = MapBudgetTransactions(contract),
        Orders = ContractOrderMapper.MapToDescriptor(ContractOrder.GetListFor(contract)),
        Documents = DocumentServices.GetAllEntityDocuments(contract),
        History = HistoryServices.GetEntityHistory(contract),
        Actions = MapActions(contract)
      };
    }


    static internal ContractDto MapContract(FormerContract contract) {
      return new ContractDto {
        UID = contract.UID,
        ContractCategory = contract.ContractCategory.MapToNamedEntity(),
        Requisition = contract.Requisition.MapToNamedEntity(),
        ContractNo = contract.ContractNo,
        Name = contract.Name,
        Description = contract.Description,
        Justification = contract.Justification,
        Notes = contract.Notes,

        FromDate = contract.FromDate,
        ToDate = contract.ToDate,
        SignDate = contract.SignDate,

        BudgetType = contract.Budgets[0].BudgetType.MapToNamedEntity(),
        Budgets = contract.Budgets.MapToNamedEntityList(),
        Currency = contract.Currency.MapToNamedEntity(),
        MinTotal = contract.MinTotal,
        MaxTotal = contract.MaxTotal,
        Total = contract.MaxTotal,

        RequestedBy = contract.RequestedBy.MapToNamedEntity(),
        Beneficiary = contract.Beneficiary.MapToNamedEntity(),
        IsForMultipleBeneficiaries = contract.IsForMultipleBeneficiaries,
        Responsible = contract.Responsible.MapToNamedEntity(),
        Provider = contract.Provider.MapToNamedEntity(),
        ProvidersGroup = contract.Provider is Parties.Group group ?
                                group.Members.MapToNamedEntityList() : new FixedList<NamedEntityDto>(),
        Project = contract.Project.MapToNamedEntity(),
        Status = contract.Status.MapToDto()
      };
    }


    static internal FixedList<ContractDto> MapContracts(FixedList<FormerContract> contracts) {
      return contracts.Select(x => MapContract(x))
                      .ToFixedList();
    }


    static internal FixedList<ContractDescriptor> MapToDescriptor(FixedList<FormerContract> contracts) {
      return contracts.Select(contract => MapToDescriptor(contract))
                       .ToFixedList();
    }



    static internal ContractDescriptor MapToDescriptor(FormerContract contract) {
      return new ContractDescriptor {
        UID = contract.UID,
        ContractCategory = contract.ContractCategory.Name,
        ContractNo = contract.ContractNo,
        RequisitionNo = contract.Requisition.OrderNo,
        Name = contract.Name,
        Description = contract.Description,
        RequestedBy = contract.RequestedBy.Name,
        Responsible = contract.Responsible.Name,
        Beneficiary = contract.Beneficiary.Name,
        Provider = contract.Provider.Name,
        FromDate = contract.FromDate,
        ToDate = contract.ToDate,
        SignDate = contract.SignDate,
        BudgetType = contract.Budgets[0].BudgetType.DisplayName,
        Currency = contract.Currency.Name,
        MinTotal = contract.MinTotal,
        MaxTotal = contract.MaxTotal,
        Total = contract.MaxTotal,
        StatusName = contract.Status.GetName()
      };
    }

    #region Helpers

    static private ContractActions MapActions(FormerContract contract) {
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


    static private FixedList<BudgetTransactionDescriptorDto> MapBudgetTransactions(FormerContract contract) {
      //FixedList<BudgetTransaction> transactions = BudgetTransaction.GetFor(contract);

      //return BudgetTransactionMapper.MapToDescriptor(transactions);

      return new FixedList<BudgetTransactionDescriptorDto>();
    }

    #endregion Helpers

  }  // class ContractMapper

}  // namespace Empiria.Procurement.Contracts.Adapters
