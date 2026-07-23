/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Mapper                                  *
*  Type     : ExpensesReportMapper                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps expenses reports and their items to their corresponding DTOs.                             *
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

  /// <summary>Maps expenses reports and their items to their corresponding DTOs.</summary>
  static public class ExpensesReportMapper {

    static public ExpensesReportHolderDto Map(ExpensesReport expensesReport) {
      FixedList<Bill> bills = Bill.GetListFor(expensesReport);

      return new ExpensesReportHolderDto {
        Order = new ExpensesReportDto(expensesReport),
        Items = Map(expensesReport.GetItems<PayableOrderItem>()),
        Taxes = OrderTaxMapper.Map(expensesReport.Taxes.GetList()),
        BudgetTransactions = MapBudgetTransactions(expensesReport),
        Bills = BillMapper.MapToBillStructure(bills),
        PaymentOrders = PaymentOrderMapper.MapToDescriptor(PaymentOrder.GetListFor(expensesReport)),
        Documents = DocumentServices.GetAllEntityDocuments(expensesReport),
        History = HistoryServices.GetEntityHistory(expensesReport),
        Actions = MapActions(expensesReport.Rules),
      };
    }


    static public FixedList<ExpensesReportItemDto> Map(FixedList<PayableOrderItem> orderItems) {
      return orderItems.Select(x => Map(x))
                       .ToFixedList();
    }


    static internal ExpensesReportItemDto Map(PayableOrderItem orderItem) {
      return new ExpensesReportItemDto(orderItem);
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


    static private FixedList<BudgetTransactionDescriptorDto> MapBudgetTransactions(ExpensesReport expensesReport) {
      FixedList<BudgetTransaction> transactions = BudgetTransaction.GetFor(expensesReport);

      return BudgetTransactionMapper.MapToDescriptor(transactions);
    }

    #endregion Helpers

  }  // class ExpensesReportMapper

}  // namespace Empiria.Orders.Adapters
