/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Mapper                                  *
*  Type     : PayableOrderMapper                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps payable orders and their order items to their corresponding DTOs.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.Billing;
using Empiria.Billing.Adapters;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;

using Empiria.Payments;
using Empiria.Payments.Adapters;

namespace Empiria.Orders.Adapters {

  /// <summary>Maps payable orders and their order items to their corresponding DTOs.</summary>
  static public class PayableOrderMapper {

    static internal FixedList<OrderDescriptor> Map(FixedList<PayableOrder> orders) {
      return orders.Select(x => MapToDescriptor(x))
                   .ToFixedList();
    }


    static public PayableOrderHolderDto Map(PayableOrder order) {
      FixedList<Bill> bills = Bill.GetListFor(order);

      return new PayableOrderHolderDto {
        Order = new PayableOrderDto(order),
        Items = Map(order.GetItems<PayableOrderItem>()),
        BudgetTransactions = MapBudgetTransactions(order),
        Bills = BillMapper.MapToBillStructure(bills),
        PaymentOrders = PaymentOrderMapper.MapToDescriptor(PaymentOrder.GetListFor(order)),
        Documents = DocumentServices.GetAllEntityDocuments(order),
        History = HistoryServices.GetEntityHistory(order),
        Actions = MapActions(order.Rules),
      };
    }


    static public FixedList<PayableOrderItemDto> Map(FixedList<PayableOrderItem> orderItems) {
      return orderItems.Select(x => Map(x))
                       .ToFixedList();
    }

    static internal FixedList<OrderDescriptor> MapToDescriptor(FixedList<Order> orders) {
      return orders.Select(x => MapToDescriptor(x))
                   .ToFixedList();
    }


    static public OrderDescriptor MapToDescriptor(Order order) {
      return new OrderDescriptor(order);
    }


    static internal PayableOrderItemDto Map(PayableOrderItem orderItem) {
      return new PayableOrderItemDto(orderItem);
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
        CanEditBills = rules.CanEditBills(),
        CanRequestBudget = false,
        CanRequestPayment = rules.CanRequestPayment(),
        CanValidateBudget = false
      };
    }


    static private FixedList<BudgetTransactionDescriptorDto> MapBudgetTransactions(PayableOrder order) {
      FixedList<BudgetTransaction> transactions = BudgetTransaction.GetFor(order);

      return BudgetTransactionMapper.MapToDescriptor(transactions);
    }

    #endregion Helpers

  }  // class PayableOrderMapper

}  // namespace Empiria.Orders.Adapters
