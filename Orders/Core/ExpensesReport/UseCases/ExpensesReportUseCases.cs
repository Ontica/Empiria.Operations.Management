/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Use Cases Layer                         *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Use case interactor class               *
*  Type     : ExpensesReportUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to update and return expenses reports.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Parties;

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

    public PayableOrderHolderDto ActivateExpensesReport(string expensesReportUID) {
      Assertion.Require(expensesReportUID, nameof(expensesReportUID));

      var expensesReport = ExpensesReport.Parse(expensesReportUID);

      expensesReport.Activate();

      expensesReport.Save();

      return PayableOrderMapper.Map(expensesReport);
    }


    public FixedList<OrderDescriptor> AvailableExpensesReports(Party requestedBy) {
      Assertion.Require(requestedBy, nameof(requestedBy));

      var orders = PayableOrder.GetList()
                               .FindAll(x => x.RequestedBy.Equals(requestedBy) &&
                                             (x.Status == StateEnums.EntityStatus.Active ||
                                             x.Status == StateEnums.EntityStatus.Pending));

      return PayableOrderMapper.Map(orders);
    }


    public PayableOrderHolderDto CreateExpensesReport(OrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      var payableOrder = PayableOrder.Parse(fields.ParentOrderUID);

      var expensesReport = new ExpensesReport(payableOrder);

      expensesReport.Update(fields);

      expensesReport.Save();

      return PayableOrderMapper.Map(expensesReport);
    }


    public PayableOrderItemDto CreateExpensesReportItem(string expensesReportUID, PayableOrderItemFields fields) {
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

      var items = expensesReport.ParentOrder.GetItems<PayableOrderItem>()
                                            .FindAll(x => x.BudgetEntry.NoRejected);

      return PayableOrderMapper.Map(items);
    }


    public PayableOrderHolderDto GetExpensesReport(string expensesReportUID) {
      Assertion.Require(expensesReportUID, nameof(expensesReportUID));

      var expensesReport = ExpensesReport.Parse(expensesReportUID);

      return PayableOrderMapper.Map(expensesReport);
    }


    public FixedList<OrderDescriptor> SearchExpensesReports(OrdersQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      FixedList<PayableOrder> expensesReports = OrdersData.Search<PayableOrder>(filter, sort);

      return PayableOrderMapper.Map(expensesReports);
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
