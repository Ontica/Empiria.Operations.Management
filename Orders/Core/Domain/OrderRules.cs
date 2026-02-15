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

namespace Empiria.Orders {

  /// <summary>Provides services to control order's rules.</summary>
  public class OrderRules {

    private Order _order;

    internal OrderRules(Order order) {
      _order = order;
    }


    public bool CanActivate() {
      return _order.Status == EntityStatus.Pending ||
             _order.Status == EntityStatus.Suspended;
    }


    public bool CanCommitBudget() {

      if (!IsBudgetable()) {
        return false;
      }

      if (_order.Status == EntityStatus.Closed ||
          _order.Status == EntityStatus.Deleted ||
          _order.Status == EntityStatus.Suspended) {
        return false;
      }

      if (_order.Items.Count == 0) {
        return false;
      }

      if (_order.OrderType.Equals(OrderType.ContractOrder) && !_order.HasCrossedBeneficiaries()) {
        return false;
      }

      FixedList<BudgetTransaction> budgetTxns = GetBudgetTransactions(BudgetOperationType.Commit);

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


    public bool CanDelete() {
      return _order.Status == EntityStatus.Pending &&
             GetBills().Count == 0 &&
             GetBudgetTransactions().Count == 0;
    }


    public bool CanEditBills() {

      if (_order.Status == EntityStatus.Closed ||
          _order.Status == EntityStatus.Deleted ||
          _order.Status == EntityStatus.Suspended) {
        return false;
      }

      if (!IsPayable()) {
        return false;
      }

      if (GetActivePaymentOrders().Count() > 0) {
        return false;
      }

      // ToDo: Check if there are any unpaid concepts or if accepts advance payments

      return true;
    }


    public bool CanEditDocuments() {
      return true;
    }


    public bool CanEditItems() {

      if (_order.Status == EntityStatus.Pending) {
        return true;
      }

      if (_order.Status == EntityStatus.Closed ||
          _order.Status == EntityStatus.Deleted ||
          _order.Status == EntityStatus.Suspended) {
        return false;
      }

      var budgetTxns = GetBudgetTransactions();

      if (budgetTxns.Any(x => !x.IsClosed)) {
        return false;
      }

      if (GetActivePaymentOrders().Count > 0) {
        return false;
      }

      return true;
    }


    public bool CanRequestBudget() {

      if (!IsBudgetable()) {
        return false;
      }

      if (_order.Status == EntityStatus.Closed ||
          _order.Status == EntityStatus.Deleted ||
          _order.Status == EntityStatus.Suspended) {
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

      if (!IsPayable()) {
        return false;
      }


      if (_order.Status == EntityStatus.Closed ||
          _order.Status == EntityStatus.Deleted ||
          _order.Status == EntityStatus.Suspended) {
        return false;
      }


      FixedList<Bill> bills = GetBills();

      if (bills.Count == 0) {
        return false;
      }

      if (IsBudgetable() && !_order.HasCrossedBeneficiaries()) {

        var budgetTxns = GetBudgetTransactions();

        if (budgetTxns.Count == 0 || budgetTxns.Any(x => !x.IsClosed)) {
          return false;
        }
      }

      if (GetActivePaymentOrders().Count > 0) {
        return false;
      }

      return true;
    }


    public bool CanSuspend() {
      return _order.Status == EntityStatus.Pending;
    }


    public bool CanUpdate() {
      return _order.Status == EntityStatus.Pending;
    }


    public bool CanValidateBudget() {
      return CanRequestBudget();
    }

    #region Helpers

    private FixedList<PaymentOrder> GetActivePaymentOrders() {
      if (!IsPayable()) {
        return FixedList<PaymentOrder>.Empty;
      }

      var paymentOrders = PaymentOrder.GetListFor((IPayableEntity) _order);

      return paymentOrders.FindAll(x => x.InProgress || x.Payed);
    }


    private FixedList<Bill> GetBills() {
      if (!IsPayable()) {
        return FixedList<Bill>.Empty;
      }

      return Bill.GetListFor((IPayableEntity) _order);
    }


    private FixedList<BudgetTransaction> GetBudgetTransactions() {

      if (!IsBudgetable()) {
        return FixedList<BudgetTransaction>.Empty;
      }

      var budgetable = _order.OrderType.Equals(OrderType.ContractOrder) ? _order.Contract : _order;

      var budgetTxns = BudgetTransaction.GetFor(budgetable);

      return budgetTxns.FindAll(x => x.InProcess || x.IsClosed);
    }


    private FixedList<BudgetTransaction> GetBudgetTransactions(BudgetOperationType operationType) {
      return GetBudgetTransactions()
            .FindAll(x => x.OperationType == operationType);
    }


    private bool IsBudgetable() {
      return _order.BudgetType != BudgetType.None;
    }


    private bool IsPayable() {
      return _order is IPayableEntity;
    }

    #endregion Helpers

  }  // class OrderRules

}  // namespace Empiria.Orders
