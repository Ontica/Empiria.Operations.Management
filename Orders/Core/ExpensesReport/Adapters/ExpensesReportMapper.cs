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

    static public ExpensesReportHolderDto Map(ExpensesReport expensesReport, string queryType = "") {
      FixedList<Bill> bills = Bill.GetListFor(expensesReport);

      return new ExpensesReportHolderDto {
        Order = new ExpensesReportDto(expensesReport),
        Items = Map(expensesReport.GetItems<PayableOrderItem>()),
        Taxes = OrderTaxMapper.Map(expensesReport.Taxes.GetList()),
        BudgetTransactions = MapBudgetTransactions(expensesReport),
        Bills = BillMapper.MapToBillStructure(bills),
        PaymentOrders = MapPaymentOrders(expensesReport),
        Documents = DocumentServices.GetAllEntityDocuments(expensesReport),
        History = HistoryServices.GetEntityHistory(expensesReport),
        Actions = MapActions(expensesReport, queryType, bills),
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

    static private OrderActions MapActions(ExpensesReport expensesReport,
                                           string queryType,
                                           FixedList<Bill> bills) {

      OrderRules rules = expensesReport.Rules;

      bool updatable = !expensesReport.EjecutorGastoAuthorized;

      bool canAuthorize = false;

      if (bills.Count == 0) {
        canAuthorize = false;

      } else if (queryType == "Procurement" && !expensesReport.EjecutorGastoAuthorized) {
        canAuthorize = true;
      } else if (queryType == "Payments" && !expensesReport.PaymentControlAuthorized) {
        canAuthorize = true;
      } else if (queryType == "Budget" && !expensesReport.BudgetControlAuthorized) {
        canAuthorize = true;
      }

      bool canReject = false;

      if (queryType == "Payments" && !expensesReport.PaymentControlAuthorized) {
        canReject = true;
      } else if (queryType == "Budget" &&
                !expensesReport.BudgetControlAuthorized &&
                 expensesReport.PaymentControlAuthorized) {
        canReject = true;
      }

      updatable = (queryType == string.Empty || queryType == "Procurement") &&
                  !expensesReport.EjecutorGastoAuthorized &&
                  expensesReport.Status == StateEnums.EntityStatus.Pending;

      return new OrderActions {
        CanDelete = updatable,
        CanEditDocuments = updatable,
        CanEditItems = updatable,
        CanSuspend = updatable,
        CanUpdate = updatable,
        CanAuthorize = canAuthorize,
        CanReject = canReject,
        CanEditBills = updatable,
      };
    }


    static private FixedList<BudgetTransactionDescriptorDto> MapBudgetTransactions(ExpensesReport expensesReport) {

      FixedList<BudgetTransaction> transactions = BudgetTransaction.GetFor(expensesReport.PayableOrder);

      transactions = FixedList<BudgetTransaction>.MergeDistinct(transactions,
                                                                BudgetTransaction.GetFor(expensesReport));

      return BudgetTransactionMapper.MapToDescriptor(transactions);
    }


    static private FixedList<PaymentOrder> GetPaymentOrders(ExpensesReport expensesReport) {
      var paymentOrders = PaymentOrder.GetListFor(expensesReport.PayableOrder);

      return FixedList<PaymentOrder>.MergeDistinct(paymentOrders,
                                                   PaymentOrder.GetListFor(expensesReport));
    }


    static private FixedList<PaymentOrderDescriptor> MapPaymentOrders(ExpensesReport expensesReport) {
      FixedList<PaymentOrder> paymentOrders = GetPaymentOrders(expensesReport);

      return PaymentOrderMapper.MapToDescriptor(paymentOrders);
    }

    #endregion Helpers

  }  // class ExpensesReportMapper

}  // namespace Empiria.Orders.Adapters
