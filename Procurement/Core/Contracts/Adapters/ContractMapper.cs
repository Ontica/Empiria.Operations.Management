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

using Empiria.Billing;
using Empiria.Billing.Adapters;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;

using Empiria.Orders;
using Empiria.Orders.Adapters;

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Provides data mapping services for Contract instances.</summary>
  static internal class ContractMapper {

    static internal ContractHolderDto Map(Contract contract) {

      var contractOrders = ContractOrder.GetListFor(contract);

      FixedList<Order> orders = contractOrders.Select(x => (Order) x)
                                              .ToFixedList();

      FixedList<Bill> bills = contractOrders.SelectFlat(x => Bill.GetListFor(x));

      return new ContractHolderDto {
        Order = MapContract(contract),
        Items = ContractItemMapper.Map(contract.GetItems()),
        Taxes = OrderTaxMapper.Map(contract.Taxes.GetList()),
        BudgetTransactions = MapBudgetTransactions(contract),
        Orders = PayableOrderMapper.MapToDescriptor(orders),
        Payables = ContractOrderMapper.MapToDescriptor(contractOrders),
        Bills = BillMapper.MapToBillStructure(bills),
        Documents = DocumentServices.GetAllEntityDocuments(contract),
        History = HistoryServices.GetEntityHistory(contract),
        Actions = MapActions(contract.Rules)
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

    static private OrderActions MapActions(OrderRules rules) {

      return new OrderActions {
        CanActivate = rules.CanActivate(),
        CanDelete = rules.CanDelete(),
        CanEditDocuments = rules.CanEditDocuments(),
        CanEditItems = rules.CanEditItems(),
        CanSuspend = rules.CanSuspend(),
        CanUpdate = rules.CanUpdate(),

        CanCommitBudget = rules.CanCommitBudget(),
        CanEditBills = false,
        CanRequestBudget = false,
        CanRequestPayment = false,
        CanValidateBudget = false,
      };
    }


    static private FixedList<BudgetTransactionDescriptorDto> MapBudgetTransactions(Contract contract) {
      FixedList<BudgetTransaction> contractTxn = BudgetTransaction.GetFor(contract);
      FixedList<BudgetTransaction> ordersTxn = ContractOrder.GetListFor(contract)
                                                            .SelectFlat(x => BudgetTransaction.GetFor(x));

      FixedList<BudgetTransaction> transactions = FixedList<BudgetTransaction>.Merge(contractTxn, ordersTxn);

      return BudgetTransactionMapper.MapToDescriptor(transactions);
    }

    #endregion Helpers

  }  // class ContractMapper

}  // namespace Empiria.Procurement.Contracts.Adapters
