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

using Empiria.Budgeting.Transactions;

namespace Empiria.Orders {

  /// <summary>Provides services to control order's rules.</summary>
  internal class OrderRules {

    private Order _order;

    internal OrderRules(Order order) {
      _order = order;
    }


    internal bool CanActivate() {
      return _order.Status == EntityStatus.Suspended;
    }


    internal bool CanDelete() {
      return _order.Status == EntityStatus.Pending;
    }


    internal bool CanEditDocuments() {
      return true;
    }


    internal bool CanEditItems() {
      if (_order.Status == EntityStatus.Pending) {
        return true;
      }

      if (_order.Status == EntityStatus.Closed) {
        return false;
      }

      if (_order.Id < 0) {
        return false;
      }

      var budgetTxns = GetRequestedBudgetTransactions();

      if (budgetTxns.All(x => x.IsClosed)) {
        return true;
      }

      return false;
    }


    internal bool CanRequestBudget() {

      if (_order.Status == EntityStatus.Closed) {
        return false;
      }

      if (_order.Items.Count == 0) {
        return false;
      }

      FixedList<BudgetTransaction> budgetTxns = GetRequestedBudgetTransactions();

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

    private FixedList<BudgetTransaction> GetRequestedBudgetTransactions() {
      var budgetable = (IBudgetable) _order;

      var budgetTxns = BudgetTransaction.GetFor(budgetable)
                                        .FindAll(x => x.OperationType == BudgetOperationType.Request);

      return budgetTxns.FindAll(x => x.InProcess || x.IsClosed);
    }

    #endregion Helpers

  }  // class OrderRules

}  // namespace Empiria.Orders
