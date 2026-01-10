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

using Empiria.Budgeting.Transactions;

using Empiria.Payments;

namespace Empiria.Orders {

  /// <summary>Provides services to control order's rules.</summary>
  internal class OrderRules {

    private Order _order;

    internal OrderRules(Order order) {
      _order = order;
    }


    internal bool CanActivate() {
      return _order.Status == EntityStatus.Pending ||
             _order.Status == EntityStatus.Suspended;
    }


    internal bool CanCommitBudget() {
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


    internal bool CanDelete() {
      return _order.Status == EntityStatus.Pending &&
             GetBills().Count == 0 &&
             GetBudgetTransactions().Count == 0;
    }


    internal bool CanEditBills() {

      if (_order.Status == EntityStatus.Closed ||
          _order.Status == EntityStatus.Deleted ||
          _order.Status == EntityStatus.Suspended) {
        return false;
      }

      if (!IsPayable()) {
        return false;
      }

      var paymentOrders = GetActivePaymentOrders();

      if (paymentOrders.Count(x => x.InProgress) > 0) {
        return false;
      }

      // ToDo: Check if there are any unpaid concepts or if accepts advance payments

      return true;
    }


    internal bool CanEditDocuments() {
      return true;
    }


    internal bool CanEditItems() {

      if (_order.Status == EntityStatus.Closed ||
          _order.Status == EntityStatus.Deleted ||
          _order.Status == EntityStatus.Suspended) {
        return false;
      }

      if (_order.Status == EntityStatus.Pending && GetBills().Count == 0) {
        return true;
      }

      if (_order.Id < 0) {
        return false;
      }

      var budgetTxns = GetBudgetTransactions(BudgetOperationType.Request);

      if (budgetTxns.All(x => x.IsClosed)) {
        return true;
      }

      return false;
    }


    internal bool CanRequestBudget() {

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


    internal bool CanRequestPayment() {

      if (_order.Status == EntityStatus.Closed ||
          _order.Status == EntityStatus.Deleted ||
          _order.Status == EntityStatus.Suspended) {
        return false;
      }

      if (!IsPayable()) {
        return false;
      }

      if (GetActivePaymentOrders().Count > 0) {
        return false;
      }

      FixedList<Bill> bills = GetBills();

      if (bills.Count == 0) {   // ToDo: || Check if there are any payable amount left or accepts advance payments
        return false;
      }

      return true;
    }


    internal bool CanSuspend() {
      return _order.Status == EntityStatus.Pending;
    }


    internal bool CanUpdate() {
      return _order.Status == EntityStatus.Pending;
    }


    internal bool CanValidateBudget() {
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
      var budgetable = (IBudgetable) _order;

      var budgetTxns = BudgetTransaction.GetFor(budgetable);

      return budgetTxns.FindAll(x => x.InProcess || x.IsClosed);
    }


    private FixedList<BudgetTransaction> GetBudgetTransactions(BudgetOperationType operationType) {
      return GetBudgetTransactions()
            .FindAll(x => x.OperationType == operationType);
    }


    private bool IsPayable() {
      return _order is IPayableEntity;
    }

    #endregion Helpers

  }  // class OrderRules

}  // namespace Empiria.Orders
