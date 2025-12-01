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
using Empiria.Payments.Orders;
using Empiria.Budgeting.Transactions;

using Empiria.Billing.Adapters;
using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Payments.Orders.Adapters;

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
        Taxes = OrderTaxMapper.Map(order.Taxes.GetList()),
        BudgetTransactions = MapBudgetTransactions(order),
        Bills = BillMapper.MapToBillDto(bills),
        Payments = PaymentOrderMapper.MapToDescriptor(PaymentOrder.GetListFor(order)),
        Documents = DocumentServices.GetAllEntityDocuments(order),
        History = HistoryServices.GetEntityHistory(order),
        Actions = MapActions(order),
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

    static private PayableOrderActions MapActions(PayableOrder order) {
      return new PayableOrderActions {
        CanEditBills = true,
        CanEditDocuments = true,
        CanRequestBudget = true,
        CanActivate = true,
        CanDelete = true,
        CanEditItems = true,
        CanSuspend = true,
        CanUpdate = true,
      };
    }


    static private FixedList<BudgetTransactionDescriptorDto> MapBudgetTransactions(PayableOrder order) {
      FixedList<BudgetTransaction> transactions = BudgetTransaction.GetFor(order);

      return BudgetTransactionMapper.MapToDescriptor(transactions);
    }

    #endregion Helpers

  }  // class PayableOrderMapper

}  // namespace Empiria.Orders.Adapters
