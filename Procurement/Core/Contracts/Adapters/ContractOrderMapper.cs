/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Mapper                                  *
*  Type     : ContractOrderMapper                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for procurement contract orders.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.Billing;
using Empiria.Billing.Adapters;

using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions;

using Empiria.Payments.Adapters;
using Empiria.Payments;

using Empiria.Orders.Adapters;

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Provides data mapping services for procurement contract orders.</summary>
  static internal class ContractOrderMapper {

    static internal ContractOrderHolderDto Map(ContractOrder order) {
      return new ContractOrderHolderDto {
        Order = new ContractOrderDto(order),
        Items = Map(order.GetItems<ContractOrderItem>()),
        Bills = BillMapper.MapToBillStructure(Bill.GetListFor(order)),
        PaymentOrders = PaymentOrderMapper.MapToDescriptor(PaymentOrder.GetListFor(order)),
        BudgetTransactions = MapBudgetTransactions(order),
        Taxes = OrderTaxMapper.Map(order.Taxes.GetList()),
        Documents = DocumentServices.GetAllEntityDocuments(order),
        History = HistoryServices.GetEntityHistory(order),
        Actions = MapActions(order),
      };
    }


    static internal FixedList<ContractOrderDescriptor> MapToDescriptor(FixedList<ContractOrder> orders) {
      return orders.Select(x => MapToDescriptor(x))
                   .ToFixedList();
    }

    static internal ContractOrderDescriptor MapToDescriptor(ContractOrder order) {
      return new ContractOrderDescriptor(order);
    }


    static internal FixedList<ContractOrderItemDto> Map(FixedList<ContractOrderItem> orderItems) {
      return orderItems.Select(x => Map(x))
                       .ToFixedList();
    }


    static internal ContractOrderItemDto Map(ContractOrderItem orderItem) {
      return new ContractOrderItemDto(orderItem);
    }

    #region Helpers

    static private OrderActions MapActions(ContractOrder order) {
      return new OrderActions {
        CanEditDocuments = true,
        CanEditBills = true,
        CanRequestPayment = true,
        CanActivate = true,
        CanDelete = true,
        CanEditItems = true,
        CanSuspend = true,
        CanUpdate = true,
      };
    }


    static private FixedList<BudgetTransactionDescriptorDto> MapBudgetTransactions(ContractOrder order) {
      FixedList<BudgetTransaction> transactions = BudgetTransaction.GetFor(order);

      return BudgetTransactionMapper.MapToDescriptor(transactions);
    }

    #endregion Helpers

  }  // class ContractOrderMapper

}  // namespace Empiria.Procurement.Contracts.Adapters
