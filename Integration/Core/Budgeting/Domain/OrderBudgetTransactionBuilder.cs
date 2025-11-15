/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Domain Layer                         *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Service provider                     *
*  Type     : OrderBudgetTransactionBuilder                 License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a budget transaction from a products order.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Parties;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;

using Empiria.Orders;

namespace Empiria.Operations.Integration.Budgeting {

  /// <summary>Builds a budget transaction from a products order.</summary>
  internal class OrderBudgetTransactionBuilder {

    private readonly BudgetTransactionType _transactionType;
    private readonly Order _order;

    private BudgetTransaction _transaction;

    public OrderBudgetTransactionBuilder(BudgetTransactionType transactionType, Order order) {

      Assertion.Require(transactionType, nameof(transactionType));
      Assertion.Require(order, nameof(order));
      Assertion.Require(order.GetItems<OrderItem>().Count > 0,
                        "Order has no items.");

      _transactionType = transactionType;
      _order = order;
    }


    internal BudgetTransaction Build() {
      _transaction = BuildTransaction();

      BuildEntries();

      return _transaction;
    }

    #region Helpers

    private void BuildDoubleEntries(OrderItem entry,
                                    BalanceColumn withdrawalColumn,
                                    BalanceColumn depositColumn) {

      var fields = BuildEntryFields(entry, withdrawalColumn, false);

      _transaction.AddEntry(fields);

      fields = BuildEntryFields(entry, depositColumn, true);

      _transaction.AddEntry(fields);
    }


    private void BuildEntries() {

      foreach (var item in _order.GetItems<OrderItem>()) {

        if (_transaction.BudgetTransactionType.Equals(BudgetTransactionType.ApartarGastoCorriente)) {
          BuildDoubleEntries(item, BalanceColumn.Available, BalanceColumn.Requested);

        } else if (_transaction.BudgetTransactionType.Equals(BudgetTransactionType.ComprometerGastoCorriente)) {
          BuildDoubleEntries(item, BalanceColumn.Requested, BalanceColumn.Commited);

        } else if (_transaction.BudgetTransactionType.Equals(BudgetTransactionType.EjercerGastoCorriente)) {
          BuildDoubleEntries(item, BalanceColumn.Commited, BalanceColumn.Exercised);

        } else {
          throw Assertion.EnsureNoReachThisCode($"Budget transaction entries rule is undefined: " +
                                                $"{_transaction.BudgetTransactionType.DisplayName}");
        }
      }
    }


    private BudgetEntryFields BuildEntryFields(OrderItem entry,
                                               BalanceColumn balanceColumn,
                                               bool isDeposit) {
      return new BudgetEntryFields {
        BudgetAccountUID = entry.BudgetAccount.UID,
        BalanceColumnUID = balanceColumn.UID,
        Description = entry.Description,
        Justification = entry.Justification,
        ProductUID = entry.Product.UID,
        ProductUnitUID = entry.ProductUnit.UID,
        ProductQty = entry.Quantity,
        ProjectUID = entry.Project.UID,
        PartyUID = entry.RequestedBy.UID,
        Year = _order.BaseBudget.Year,
        BaseEntityItemId = entry.Id,
        CurrencyUID = entry.Currency.UID,
        OriginalAmount = entry.Subtotal,
        Amount = isDeposit ? entry.Subtotal : -1 * entry.Subtotal
      };
    }


    private BudgetTransaction BuildTransaction() {

      BudgetTransactionFields fields = BuildTransactionFields();

      var budget = Budget.Parse(fields.BaseBudgetUID);

      var transaction = new BudgetTransaction(_transactionType, budget, _order);

      transaction.Update(fields);

      return transaction;
    }


    private BudgetTransactionFields BuildTransactionFields() {

      return new BudgetTransactionFields {
        TransactionTypeUID = _transactionType.UID,
        BaseEntityTypeUID = _order.GetEmpiriaType().UID,
        BaseEntityUID = _order.UID,
        Justification = _order.Justification,
        Description = _order.Description,
        BaseBudgetUID = _order.BaseBudget.UID,
        OperationSourceUID = OperationSource.ParseNamedKey("SISTEMA_DE_ADQUISICIONES").UID,
        BasePartyUID = _order.RequestedBy.UID,
        RequestedByUID = Party.ParseWithContact(ExecutionServer.CurrentContact).UID,
        ApplicationDate = DateTime.Today
      };
    }

    #endregion Helpers

  }  // class OrderBudgetTransactionBuilder

}  // namespace Empiria.Operations.Integration.Budgeting
