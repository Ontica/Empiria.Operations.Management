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

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Orders.Adapters {

  /// <summary>Maps payable orders and their order items to their corresponding DTOs.</summary>
  static public class PayableOrderMapper {

    static internal FixedList<PayableOrderDescriptor> Map(FixedList<Order> orders) {
      return orders.Select(x => MapToDescriptor((PayableOrder) x))
                   .ToFixedList();
    }


    static public PayableOrderHolderDto Map(PayableOrder order) {
      return new PayableOrderHolderDto {
        Order = new PayableOrderDto(order),
        Items = Map(order.GetItems<PayableOrderItem>()),
        BudgetTransactions = MapBudgetTransactions(order),
        Documents = DocumentServices.GetAllEntityDocuments(order),
        History = HistoryServices.GetEntityHistory(order),
        Actions = MapActions(order),
      };
    }


    private static PayableOrderDescriptor MapToDescriptor(PayableOrder order) {
      return new PayableOrderDescriptor(order);
    }

    static internal FixedList<PayableOrderItemDto> Map(FixedList<PayableOrderItem> orderItems) {
      return orderItems.Select(x => Map(x))
                       .ToFixedList();
    }


    static internal PayableOrderItemDto Map(PayableOrderItem orderItem) {
      return new PayableOrderItemDto(orderItem);
    }

    #region Helpers

    static private PayableOrderActions MapActions(PayableOrder order) {
      return new PayableOrderActions {
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
