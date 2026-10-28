/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Use Cases Layer                         *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Use case interactor class               *
*  Type     : ExpensesReportUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to update and return expenses reports.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.History;
using Empiria.Parties;
using Empiria.Services;

using Empiria.Billing;

using Empiria.Budgeting.Transactions;

using Empiria.Payments;

using Empiria.Orders.Adapters;
using Empiria.Orders.Data;

namespace Empiria.Orders.UseCases {

  /// <summary>Use cases used to update and return expenses reports.</summary>
  public class ExpensesReportUseCases : UseCase {

    #region Constructors and parsers

    protected ExpensesReportUseCases() {
      // no-op
    }

    static public ExpensesReportUseCases UseCaseInteractor() {
      return CreateInstance<ExpensesReportUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ExpensesReportHolderDto ActivateExpensesReport(string expensesReportUID) {
      Assertion.Require(expensesReportUID, nameof(expensesReportUID));

      var expensesReport = ExpensesReport.Parse(expensesReportUID);

      expensesReport.Activate();

      expensesReport.Save();

      return ExpensesReportMapper.Map(expensesReport);
    }


    public ExpensesReportHolderDto Authorize(ExpensesReport expensesReport) {
      Assertion.Require(expensesReport, nameof(expensesReport));

      var bills = Bill.GetListFor(expensesReport);


      var paymentOrders = PaymentOrder.GetListFor(expensesReport.PayableOrder);

      BillsTotals billTotals = new BillsTotals(bills);

      if (billTotals.Total != paymentOrders.Sum(x => x.Total)) {
        Assertion.RequireFail($"La suma de los comprobantes ({billTotals.Total:C2}) " +
                              $"no coincide con la suma de los pagos efectuados ({paymentOrders.Sum(x => x.Total):C2}). " +
                              "De ser el caso, los recibos de devolución de anticipos " +
                              "también deben anexarse como parte de la comprobación. " +
                              $"Diferencia: {paymentOrders.Sum(x => x.Total) - billTotals.Total:C2}");
      }


      if (!expensesReport.PaymentControlAuthorized) {
        expensesReport.Authorize();

        expensesReport.Save();

        HistoryServices.CreateHistoryEntry(expensesReport, new HistoryFields("Autorizada"));

        return ExpensesReportMapper.Map(expensesReport);
      }


      var builder = new BudgetTransactionBuilder(expensesReport,
                                                 OperationSource.ParseNamedKey("SISTEMA_DE_CONTROL_PRESUPUESTAL"),
                                                 System.DateTime.Today);

      var exerciseTxn = BudgetTransaction.GetFor(expensesReport.PayableOrder)
                                         .Find(x => x.OperationType == BudgetOperationType.Exercise);

      var adjustmentTxn = builder.BuildForAdjustment(exerciseTxn);

      if (adjustmentTxn.Entries.Count == 0) {
        expensesReport.Authorize();

        expensesReport.Save();

        HistoryServices.CreateHistoryEntry(expensesReport, new HistoryFields("Autorizada"));

        return ExpensesReportMapper.Map(expensesReport);
      }

      adjustmentTxn.SendToAuthorization();

      adjustmentTxn.Authorize();

      adjustmentTxn.Close();

      adjustmentTxn.Save();

      HistoryServices.CreateHistoryEntry(expensesReport, new HistoryFields("Se ajustó el presupuesto",
                                          $"Se generó automáticamente la transacción " +
                                          $"presupuestal de ajuste {adjustmentTxn.TransactionNo}."));

      expensesReport.PayableOrder.ExpenseChecked = true;
      expensesReport.PayableOrder.Save();

      expensesReport.Authorize();

      expensesReport.Save();

      HistoryServices.CreateHistoryEntry(expensesReport, new HistoryFields("Autorizada"));

      return ExpensesReportMapper.Map(expensesReport);
    }


    public ExpensesReportHolderDto Reject(ExpensesReport expensesReport, string reason) {
      Assertion.Require(expensesReport, nameof(expensesReport));
      Assertion.Require(reason, nameof(reason));

      expensesReport.Reject();

      expensesReport.Save();

      HistoryServices.CreateHistoryEntry(expensesReport, new HistoryFields("Rechazada", reason));

      return ExpensesReportMapper.Map(expensesReport);
    }


    public FixedList<OrderDescriptor> AvailableExpensesToReport(Party requestedBy) {
      Assertion.Require(requestedBy, nameof(requestedBy));

      var orders = PayableOrder.GetList()
                               .FindAll(x => x.RequestedBy.Equals(requestedBy) &&
                                             x.OrderType.Equals(OrderType.Expenses) &&
                                             x.ExpenseType.IsExpenseToCheck &&
                                             !x.ExpenseChecked && x.Status == StateEnums.EntityStatus.Closed);

      return PayableOrderMapper.Map(orders);
    }


    public ExpensesReportHolderDto CreateExpensesReport(ExpensesReportFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var payableOrder = PayableOrder.Parse(fields.PayableOrderUID);

      var expensesReport = new ExpensesReport(payableOrder);

      fields.Name = $"{payableOrder.OrderNo}: {payableOrder.Name}";

      expensesReport.Update(fields);

      expensesReport.Save();

      return ExpensesReportMapper.Map(expensesReport);
    }


    public PayableOrderItemDto CreateExpensesReportItem(string expensesReportUID,
                                                        PayableOrderItemFields fields) {

      Assertion.Require(expensesReportUID, nameof(expensesReportUID));
      Assertion.Require(fields, nameof(fields));

      var expensesReport = ExpensesReport.Parse(expensesReportUID);

      var item = new PayableOrderItem(OrderItemType.PurchaseOrderItemType, expensesReport);

      item.Update(fields);

      expensesReport.AddItem(item);

      item.Save();

      return PayableOrderMapper.Map(item);
    }


    public PayableOrderHolderDto DeleteExpensesReport(string expensesReportUID) {
      Assertion.Require(expensesReportUID, nameof(expensesReportUID));

      var expensesReport = ExpensesReport.Parse(expensesReportUID);

      expensesReport.Delete();

      expensesReport.Save();

      return PayableOrderMapper.Map(expensesReport);
    }


    public PayableOrderItemDto DeleteExpensesReportItem(string expensesReportUID, string orderItemUID) {
      Assertion.Require(expensesReportUID, nameof(expensesReportUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));

      var expensesReport = ExpensesReport.Parse(expensesReportUID);

      var item = expensesReport.GetItem<PayableOrderItem>(orderItemUID);

      expensesReport.Items.Remove(item);

      item.Save();

      return PayableOrderMapper.Map(item);
    }


    public ExpensesReportHolderDto GetExpensesReport(string expensesReportUID, string queryType) {
      Assertion.Require(expensesReportUID, nameof(expensesReportUID));

      var expensesReport = ExpensesReport.Parse(expensesReportUID);

      return ExpensesReportMapper.Map(expensesReport, queryType);
    }


    public FixedList<OrderDescriptor> SearchExpensesReports(OrdersQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      FixedList<ExpensesReport> expensesReports = OrdersData.Search<ExpensesReport>(filter, sort);

      if (query.QueryType == "Payments") {
        expensesReports = expensesReports.FindAll(x => x.EjecutorGastoAuthorized);

      } else if (query.QueryType == "Budget") {
        expensesReports = expensesReports.FindAll(x => x.PaymentControlAuthorized);
      }

      return PayableOrderMapper.Map(expensesReports.Select(x => (PayableOrder) x).ToFixedList());
    }


    public PayableOrderHolderDto SuspendExpensesReport(string expensesReportUID) {
      Assertion.Require(expensesReportUID, nameof(expensesReportUID));

      var expensesReport = ExpensesReport.Parse(expensesReportUID);

      expensesReport.Suspend();

      expensesReport.Save();

      return PayableOrderMapper.Map(expensesReport);
    }


    public PayableOrderHolderDto UpdateExpensesReport(PayableOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var expensesReport = ExpensesReport.Parse(fields.UID);

      expensesReport.Update(fields);

      expensesReport.Save();

      return PayableOrderMapper.Map(expensesReport);
    }


    public PayableOrderItemDto UpdateExpensesReportItem(string expensesReportUID,
                                                        string orderItemUID,
                                                        PayableOrderItemFields fields) {
      Assertion.Require(expensesReportUID, nameof(expensesReportUID));
      Assertion.Require(fields, nameof(fields));

      var expensesReport = ExpensesReport.Parse(expensesReportUID);

      var item = expensesReport.GetItem<PayableOrderItem>(orderItemUID);

      item.Update(fields);

      item.Save();

      expensesReport.Items.Update(item);

      return PayableOrderMapper.Map(item);
    }

    #endregion Use cases

  }  // class ExpensesReportUseCases

}  // namespace Empiria.Orders.UseCases
