/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Use Cases Layer                         *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Use case interactor class               *
*  Type     : ExpensesReportUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to update and return expenses reports.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.History;
using Empiria.Orders.Adapters;
using Empiria.Orders.Data;
using Empiria.Parties;
using Empiria.Services;

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

      expensesReport.Authorize();

      expensesReport.Save();

      HistoryServices.CreateHistoryEntry(expensesReport, new HistoryFields("Autorizada"));

      return ExpensesReportMapper.Map(expensesReport);
    }

    public FixedList<OrderDescriptor> AvailableExpensesToReport(Party requestedBy) {
      Assertion.Require(requestedBy, nameof(requestedBy));

      var orders = PayableOrder.GetList()
                               .FindAll(x => x.RequestedBy.Equals(requestedBy) && x.OrderType.Equals(OrderType.Expenses) &&
                                             x.ExpenseType.IsExpenseToCheck && !x.ExpenseChecked && x.Status == StateEnums.EntityStatus.Closed);

      return PayableOrderMapper.Map(orders);
    }


    public ExpensesReportHolderDto CreateExpensesReport(ExpensesReportFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var payableOrder = PayableOrder.Parse(fields.PayableOrderUID);

      var expensesReport = new ExpensesReport(payableOrder);

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


    public FixedList<PayableOrderItemDto> GetAvailableExpensesReportItems(ExpensesReport expensesReport) {
      Assertion.Require(expensesReport, nameof(expensesReport));

      var items = expensesReport.PayableOrder.GetItems<PayableOrderItem>()
                                .FindAll(x => x.BudgetEntry.NoRejected);

      return PayableOrderMapper.Map(items);
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
