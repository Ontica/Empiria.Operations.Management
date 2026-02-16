/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Service provider                        *
*  Type     : OrderRules                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to control order's rules.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Financial;
using Empiria.StateEnums;

using Empiria.Billing;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;

using Empiria.Payments;

using Empiria.Orders.Contracts;

namespace Empiria.Orders {

  /// <summary>Provides services to control order's rules.</summary>
  public class OrderRules {

    private readonly Order _order;
    private readonly bool _isBudgetable;
    private readonly bool _isPayable;
    private readonly FixedList<BudgetTransaction> _budgetTransations;
    private readonly FixedList<PaymentOrder> _activePaymentOrders;
    private readonly FixedList<Bill> _bills;

    internal OrderRules(Order order) {
      _order = order;
      _isPayable = order is IPayableEntity;
      _isBudgetable = _order.HasBudgetableItems || _order.BudgetType != BudgetType.None;
      _budgetTransations = GetBudgetTransactions();
      _activePaymentOrders = GetActivePaymentOrders();
      _bills = GetBills();
    }


    public bool CanActivate() {
      return _order.Status == EntityStatus.Suspended;
    }


    public bool CanCommitBudget() {

      if (!_isBudgetable) {
        return false;
      }


      if (!CanEditItems()) {
        return false;
      }

      if (_order is Requisition) {
        return false;
      }

      if (_order.Items.Count == 0) {
        return false;
      }

      if (_order is ContractOrder && !_order.HasCrossedBeneficiaries()) {
        return false;
      }

      if (_budgetTransations.Any(x => x.InProcess)) {
        return false;
      }

      if (GetBudgetTransactions(BudgetOperationType.Commit).Any(x => x.IsClosed)) {
        return false;
      }

      if (_order is Contract) {
        return true;
      }

      if (_bills.Count == 0) {
        return false;
      }

      if (!BillsTotalsEqualsOrderTotals()) {
        return false;
      }

      return true;
    }


    public bool CanDelete() {
      return (_order.Status == EntityStatus.Pending || _order.Status == EntityStatus.Active) &&
             _bills.Count == 0 &&
             _budgetTransations.Count == 0 &&
             _activePaymentOrders.Count == 0;
    }


    public bool CanEditBills() {

      if (!_isPayable) {
        return false;
      }

      if (!CanEditItems()) {
        return false;
      }

      return true;
    }


    public bool CanEditDocuments() {
      return (CanEditItems() || CanRequestPayment()) && _order.Status != EntityStatus.Suspended;
    }


    public bool CanEditItems() {

      if (_activePaymentOrders.Count > 0) {
        return false;
      }

      if (_order.Status == EntityStatus.Closed ||
          _order.Status == EntityStatus.Deleted ||
          _order.Status == EntityStatus.Suspended) {
        return false;
      }

      if (_budgetTransations.Any(x => !x.IsClosed)) {
        return false;
      }

      if (_isPayable && GetBudgetTransactions(BudgetOperationType.Commit).Any(x => x.IsClosed)) {
        return false;
      }

      if (_order is Contract contract) {
        var contractOrders = _order.GetPayableEntities().Cast<PayableOrder>();

        if (contractOrders.Any(x => x.Rules._budgetTransations.Count > 0)) {
          return false;
        }
      }

      return true;
    }


    public bool CanRequestBudget() {

      if (!_isBudgetable) {
        return false;
      }

      if (_order.Status == EntityStatus.Closed ||
          _order.Status == EntityStatus.Deleted ||
          _order.Status == EntityStatus.Suspended) {
        return false;
      }

      if (!(_order is Requisition)) {
        return false;
      }

      if (_order.Items.Count == 0) {
        return false;
      }

      FixedList<BudgetTransaction> budgetTxns = GetBudgetTransactions(BudgetOperationType.Request);

      if (budgetTxns.Count == 0) {
        return true;
      }

      if (budgetTxns.Any(x => x.InProcess)) {
        return false;
      }

      if (_order.Items.GetItems().Sum(x => x.Subtotal) != budgetTxns.Sum(x => x.GetTotal())) {
        return true;
      }

      return false;
    }


    public bool CanRequestPayment() {

      if (!_isPayable) {
        return false;
      }

      if (_activePaymentOrders.Count > 0) {
        return false;
      }

      if (!CanEditItems()) {
        return false;
      }

      if (_bills.Count == 0) {
        return false;
      }

      if (!BillsTotalsEqualsOrderTotals()) {
        return false;
      }


      if (!_isBudgetable || _order.HasCrossedBeneficiaries()) {
        return true;
      }

      if (_budgetTransations.Count == 0) {
        return false;
      }

      if (_budgetTransations.Any(x => x.InProcess)) {
        return false;
      }

      if (!GetBudgetTransactions(BudgetOperationType.Commit).Any(x => x.IsClosed)) {
        return false;
      }

      return true;
    }


    public bool CanSuspend() {
      return CanEditItems() && _order.Status != EntityStatus.Suspended;
    }


    public bool CanUpdate() {
      return CanEditItems();
    }


    public bool CanValidateBudget() {
      return CanRequestBudget();
    }

    #region Helpers

    private bool BillsTotalsEqualsOrderTotals() {
      var billsTotals = new BillsTotals(_bills);

      decimal orderTotals = _order.Subtotal + _order.Taxes.ControlConceptsTotal;
      decimal billed = billsTotals.Subtotal - billsTotals.Discounts + billsTotals.BudgetableTaxesTotal;

      return orderTotals == billed;
    }


    private FixedList<PaymentOrder> GetActivePaymentOrders() {

      if (!_isPayable) {
        return FixedList<PaymentOrder>.Empty;
      }

      var paymentOrders = PaymentOrder.GetListFor((IPayableEntity) _order);

      return paymentOrders.FindAll(x => x.InProgress || x.Payed);
    }


    private FixedList<Bill> GetBills() {
      if (!_isPayable) {
        return FixedList<Bill>.Empty;
      }

      return Bill.GetListFor((IPayableEntity) _order);
    }


    private FixedList<BudgetTransaction> GetBudgetTransactions() {

      if (!_isBudgetable) {
        return FixedList<BudgetTransaction>.Empty;
      }

      var budgetable = _order.OrderType.Equals(OrderType.ContractOrder) ? _order.Contract : _order;

      var budgetTxns = BudgetTransaction.GetFor(budgetable);

      return budgetTxns.FindAll(x => x.InProcess || x.IsClosed);
    }


    private FixedList<BudgetTransaction> GetBudgetTransactions(BudgetOperationType operationType) {
      return _budgetTransations.FindAll(x => x.OperationType == operationType);
    }

    #endregion Helpers

  }  // class OrderRules

}  // namespace Empiria.Orders
