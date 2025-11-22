/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Mapper                                  *
*  Type     : ContractMapper                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for Contract instances.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.StateEnums;

using Empiria.Billing;
using Empiria.Billing.Adapters;

using Empiria.Orders;
using Empiria.Orders.Adapters;

using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Provides data mapping services for Contract instances.</summary>
  static internal class ContractMapper {

    static internal ContractHolderDto Map(Contract contract) {

      FixedList<Order> orders = contract.GetOrders()
                                        .Select(x => (Order) x)
                                        .ToFixedList();

      FixedList<Bill> bills = contract.GetOrders()
                                      .SelectFlat(x => Bill.GetListFor(x));

      return new ContractHolderDto {
        Order = MapContract(contract),
        Items = ContractItemMapper.Map(contract.GetItems()),
        Taxes = OrderTaxMapper.Map(contract.Taxes.GetList()),
        BudgetTransactions = MapBudgetTransactions(contract),
        Orders = PayableOrderMapper.MapToDescriptor(orders),
        Payables = ContractOrderMapper.MapToDescriptor(ContractOrder.GetListFor(contract)),
        Bills = BillMapper.MapToBillDto(bills),
        Documents = DocumentServices.GetAllEntityDocuments(contract),
        History = HistoryServices.GetEntityHistory(contract),
        Actions = MapActions(contract)
      };
    }


    static internal ContractDto MapContract(Contract contract) {
      return new ContractDto(contract);
    }


    static internal FixedList<ContractDto> MapContracts(FixedList<Contract> contracts) {
      return contracts.Select(x => MapContract(x))
                      .ToFixedList();
    }


    static internal FixedList<ContractDescriptor> MapToDescriptor(FixedList<Contract> contracts) {
      return contracts.Select(contract => MapToDescriptor(contract))
                       .ToFixedList();
    }


    static internal ContractDescriptor MapToDescriptor(Contract contract) {
      return new ContractDescriptor(contract);
    }

    #region Helpers

    static private OrderActions MapActions(Contract contract) {
      return new OrderActions {
        CanActivate = contract.CanActivate(),
        CanDelete = contract.CanDelete(),
        CanEditItems = contract.CanUpdate(),
        CanEditDocuments = contract.CanUpdate(),
        CanSuspend = contract.CanSuspend(),
        CanUpdate = contract.Status == EntityStatus.Pending
      };
    }


    static private FixedList<BudgetTransactionDescriptorDto> MapBudgetTransactions(Contract contract) {
      //FixedList<BudgetTransaction> transactions = BudgetTransaction.GetFor(contract);

      //return BudgetTransactionMapper.MapToDescriptor(transactions);

      return new FixedList<BudgetTransactionDescriptorDto>();
    }

    #endregion Helpers

  }  // class ContractMapper

}  // namespace Empiria.Procurement.Contracts.Adapters
