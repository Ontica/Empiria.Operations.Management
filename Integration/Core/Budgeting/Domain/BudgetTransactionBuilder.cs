/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Service provider                        *
*  Type     : BudgetTransactionBuilder                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Builds a budget transaction using a IPayableEntity instance.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Builds a budget transaction using a IPayableEntity instance.</summary>
  internal class BudgetTransactionBuilder {

    private readonly Order _budgetable;
    private readonly BudgetTransactionFields _fields;
    private BudgetTransaction _transaction;

    public BudgetTransactionBuilder(Order budgetable,
                                    BudgetTransactionFields fields) {
      Assertion.Require(budgetable, nameof(budgetable));
      Assertion.Require(budgetable.GetItems<OrderItem>().Count > 0,
                        "Budgetable entity has no items.");

      Assertion.Require(fields, nameof(fields));

      _budgetable = budgetable;
      _fields = fields;
    }


    internal BudgetTransaction Build() {
      _transaction = BuildTransaction();

      BuildEntries();

      return _transaction;
    }

    #region Helpers

    private void BuildDoubleEntries(OrderItem entry,
                                    BalanceColumn depositColumn,
                                    BalanceColumn withdrawalColumn) {

      var fields = BuildEntryFields(entry, depositColumn, true);

      _transaction.AddEntry(fields);

      fields = BuildEntryFields(entry, withdrawalColumn, false);

      _transaction.AddEntry(fields);
    }


    private void BuildEntries() {
      foreach (var item in _budgetable.GetItems<OrderItem>()) {

        if (_transaction.BudgetTransactionType.Equals(BudgetTransactionType.ApartarGastoCorriente)) {
          BuildDoubleEntries(item, BalanceColumn.Requested, BalanceColumn.Available);

        } else if (_transaction.BudgetTransactionType.Equals(BudgetTransactionType.ComprometerGastoCorriente)) {
          BuildDoubleEntries(item, BalanceColumn.Commited, BalanceColumn.Requested);

        } else if (_transaction.BudgetTransactionType.Equals(BudgetTransactionType.EjercerGastoCorriente)) {
          BuildDoubleEntries(item, BalanceColumn.Exercised, BalanceColumn.Commited);

        } else {
          throw Assertion.EnsureNoReachThisCode($"Budget transaction entries rule is undefined: " +
                                                $"{_transaction.BudgetTransactionType.DisplayName}");
        }
      }
    }


    static private BudgetEntryFields BuildEntryFields(OrderItem entry,
                                                      BalanceColumn balanceColumn,
                                                      bool isDeposit) {
      return new BudgetEntryFields {
        BudgetAccountUID = entry.BudgetAccount.UID,
        BalanceColumnUID = balanceColumn.UID,
        Description = entry.Description,
        ProductUID = entry.Product.UID,
        ProductUnitUID = entry.ProductUnit.UID,
        ProductQty = entry.Quantity,
        BaseEntityItemId = entry.Id,
        //ProjectUID = entry.Project.UID,
        CurrencyUID = entry.Currency.UID,
        OriginalAmount = entry.Subtotal,
        Amount = isDeposit ? entry.Subtotal : -1 * entry.Subtotal
      };
    }


    private BudgetTransaction BuildTransaction() {

      var transactionType = BudgetTransactionType.Parse(_fields.TransactionTypeUID);

      var budget = Budget.Parse(_fields.BaseBudgetUID);

      var transaction = new BudgetTransaction(transactionType, budget, _budgetable);

      transaction.Update(_fields);

      return transaction;
    }

    #endregion Helpers

  }  // class BudgetTransactionBuilder

}  // namespace Empiria.Budgeting.Transactions
