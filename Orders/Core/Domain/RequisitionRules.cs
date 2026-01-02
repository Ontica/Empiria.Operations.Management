/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Service provider                        *
*  Type     : RequisitionRules                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to control requisition rules.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Financial;
using Empiria.StateEnums;

using Empiria.Budgeting.Transactions;

namespace Empiria.Orders {

  /// <summary>Provides services to control requisition rules.</summary>
  internal class RequisitionRules {

    private Requisition _requisition;

    internal RequisitionRules(Requisition requisition) {
      _requisition = requisition;
    }


    internal bool CanActivate() {
      return _requisition.Status == EntityStatus.Suspended;
    }


    internal bool CanDelete() {
      return _requisition.Status == EntityStatus.Pending;
    }


    internal bool CanEditDocuments() {
      return true;
    }


    internal bool CanEditItems() {
      if (_requisition.Status == EntityStatus.Pending) {
        return true;
      }

      if (_requisition.Status == EntityStatus.Closed) {
        return false;
      }

      if (_requisition.Id < 0) {
        return false;
      }

      var budgetTxns = GetRequestedBudgetTransactions();

      if (budgetTxns.All(x => x.IsClosed)) {
        return true;
      }

      return false;
    }


    internal bool CanRequestBudget() {

      if (_requisition.Status == EntityStatus.Closed) {
        return false;
      }

      if (_requisition.Items.Count == 0) {
        return false;
      }

      FixedList<BudgetTransaction> budgetTxns = GetRequestedBudgetTransactions();

      if (budgetTxns.Count == 0) {
        return true;
      }

      if (budgetTxns.Any(x => x.InProcess)) {
        return false;
      }

      if (_requisition.Items.GetItems().Sum(x => x.Subtotal) != budgetTxns.Sum(x => x.GetTotal())) {
        return true;
      }

      return false;
    }

    internal bool CanSuspend() {
      return _requisition.Status == EntityStatus.Pending;
    }

    internal bool CanUpdate() {
      return _requisition.Status == EntityStatus.Pending;
    }


    internal bool CanValidateBudget() {
      return CanRequestBudget();
    }

    #region Helpers

    private FixedList<BudgetTransaction> GetRequestedBudgetTransactions() {
      var budgetable = (IBudgetable) _requisition;

      var budgetTxns = BudgetTransaction.GetFor(budgetable)
                                        .FindAll(x => x.OperationType == BudgetOperationType.Request);

      return budgetTxns.FindAll(x => x.InProcess || x.IsClosed);
    }


    #endregion Helpers

  }  // class PaymentInstructionRules

}  // namespace Empiria.Orders
