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
using Empiria.StateEnums;

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

      Assertion.Require(_transaction.Entries.Count > 0,
                        "No es posible generar la transacción presupuestal debido " +
                        "a que la orden de compra o requisición no cuenta " +
                        "con conceptos pendientes de autorizar.");

      return _transaction;
    }

    #region Helpers

    private void BuildDoubleEntries(OrderItem entry,
                                    BalanceColumn withdrawalColumn,
                                    BalanceColumn depositColumn) {

      var fields = BuildEntryFields(entry, withdrawalColumn, false);

      _transaction.AddEntry(fields);

      fields = BuildEntryFields(entry, depositColumn, true);

      BudgetEntry budgetEntry = _transaction.AddEntry(fields);

      entry.SetBudgetEntry(budgetEntry);
    }


    private void BuildEntries() {

      FixedList<OrderItem> orderItems;

      if (_transaction.OperationType == BudgetOperationType.Request) {
        orderItems = _order.GetItems<OrderItem>()
                           .FindAll(x => x.BudgetEntry.IsEmptyInstance ||
                                         x.BudgetEntry.Status == TransactionStatus.Rejected);
      } else {
        orderItems = _order.GetItems<OrderItem>();
      }

      foreach (var item in orderItems) {

        if (_transaction.OperationType == BudgetOperationType.Request) {
          BuildDoubleEntries(item, BalanceColumn.Available, BalanceColumn.Requested);

        } else if (_transaction.OperationType == BudgetOperationType.Commit && !_order.HasCrossedBeneficiaries()) {
          BuildDoubleEntries(item, BalanceColumn.Requested, BalanceColumn.Commited);

        } else if (_transaction.OperationType == BudgetOperationType.Commit && _order.HasCrossedBeneficiaries()) {
          throw new NotImplementedException("Las transacciones de compromiso presupuestal " +
                                            "sobre requisiciones multiáreas no están disponibles.");

        } else if (_transaction.OperationType == BudgetOperationType.ApprovePayment) {
          BuildDoubleEntries(item, BalanceColumn.Commited, BalanceColumn.ToPay);

        } else if (_transaction.OperationType == BudgetOperationType.Exercise) {
          BuildDoubleEntries(item, BalanceColumn.ToPay, BalanceColumn.Exercised);

        } else {
          throw Assertion.EnsureNoReachThisCode($"Budget transaction entries rule is undefined: " +
                                                $"{_transaction.TransactionType.DisplayName}");
        }
      }
    }


    private BudgetEntryFields BuildEntryFields(OrderItem entry,
                                               BalanceColumn balanceColumn,
                                               bool isDeposit) {

      DateTime budgetingDate = DetermineEntryBudgetingDate(entry, balanceColumn, isDeposit);

      string relatedEntryUID = string.Empty;

      if (!entry.RequisitionItem.IsEmptyInstance) {
        relatedEntryUID = entry.RequisitionItem.BudgetEntry.UID;
      }

      return new BudgetEntryFields {
        BudgetUID = entry.Budget.UID,
        BudgetAccountUID = entry.BudgetAccount.UID,
        BalanceColumnUID = balanceColumn.UID,
        Description = entry.Description,
        Justification = entry.Justification,
        ProductUID = entry.Product.UID,
        ProductCode = entry.ProductCode,
        ProductName = entry.ProductName,
        ProductUnitUID = entry.ProductUnit.UID,
        OriginCountryUID = entry.OriginCountry.UID,
        ProductQty = entry.Quantity,
        ProjectUID = entry.Project.UID,
        PartyUID = entry.RequestedBy.UID,
        Year = budgetingDate.Year,
        Month = budgetingDate.Month,
        Day = budgetingDate.Day,
        EntityTypeId = entry.GetEmpiriaType().Id,
        EntityId = entry.Id,
        RelatedEntryUID = relatedEntryUID,
        ExchangeRate = _order.ExchangeRate,
        CurrencyUID = entry.Currency.UID,
        CurrencyAmount = entry.Subtotal,
        Amount = Math.Round((isDeposit ? entry.Subtotal : -1 * entry.Subtotal) * _order.ExchangeRate, 2)
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

      OperationSource operationSource;

      if (_transactionType.OperationType == BudgetOperationType.ApprovePayment ||
          _transactionType.OperationType == BudgetOperationType.Exercise) {
        operationSource = OperationSource.ParseNamedKey("SISTEMA_DE_PAGOS");
      } else {
        operationSource = OperationSource.ParseNamedKey("SISTEMA_DE_ADQUISICIONES");
      }

      return new BudgetTransactionFields {
        TransactionTypeUID = _transactionType.UID,
        BaseEntityTypeUID = _order.GetEmpiriaType().UID,
        BaseEntityUID = _order.UID,
        Justification = _order.Justification,
        Description = _order.Description,
        BaseBudgetUID = _order.BaseBudget.UID,
        BasePartyUID = _order.RequestedBy.UID,
        CurrencyUID = _order.Currency.UID,
        ExchangeRate = _order.ExchangeRate,
        OperationSourceUID = operationSource.UID,
        RequestedByUID = Party.ParseWithContact(ExecutionServer.CurrentContact).UID,
        ApplicationDate = DateTime.Today
      };
    }

    private DateTime DetermineEntryBudgetingDate(OrderItem entry, BalanceColumn balanceColumn, bool isDeposit) {

      switch (_transactionType.OperationType) {
        case BudgetOperationType.Request:

          return DetermineBudgetRequestDate(entry);

        case BudgetOperationType.Commit:

          return DetermineBudgetCommitRequestDate(entry, isDeposit);

        case BudgetOperationType.ApprovePayment:

          return DetermineApprovePaymentRequestDate(entry, isDeposit);

        case BudgetOperationType.Exercise:

          return DetermineExcerciseRequestDate(entry, isDeposit);

        default:
          throw Assertion.EnsureNoReachThisCode($"Budget entry budgeting date rule is undefined: " +
                                                $"{_transaction.TransactionType.DisplayName}");
      }
    }


    private DateTime DetermineApprovePaymentRequestDate(OrderItem entry, bool isDeposit) {
      if (isDeposit) {
        return DateTime.Today.Date;
      }

      BudgetEntry budgetEntry = entry.RequisitionItem.BudgetEntry;

      if (!budgetEntry.IsEmptyInstance) {
        return new DateTime(budgetEntry.Year, budgetEntry.Month, 1);
      }

      if (!entry.Order.Requisition.IsEmptyInstance) {
        return new DateTime(entry.Order.Requisition.StartDate.Year,
                            entry.Order.Requisition.StartDate.Month, 1);
      }

      return new DateTime(_transaction.BaseBudget.Year, entry.Order.PostingTime.Month, 1);
    }


    private DateTime DetermineBudgetCommitRequestDate(OrderItem entry, bool isDeposit) {
      if (isDeposit) {
        return new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
      }

      BudgetEntry budgetEntry = entry.RequisitionItem.BudgetEntry;

      if (!budgetEntry.IsEmptyInstance) {
        return new DateTime(budgetEntry.Year, budgetEntry.Month, 1);
      }

      if (!entry.Order.Requisition.IsEmptyInstance) {
        return new DateTime(entry.Order.Requisition.StartDate.Year,
                            entry.Order.Requisition.StartDate.Month, 1);
      }

      return new DateTime(_transaction.BaseBudget.Year, entry.Order.PostingTime.Month, 1);
    }


    private DateTime DetermineBudgetRequestDate(OrderItem entry) {
      DateTime budgetingDate = ExecutionServer.IsMinOrMaxDate(entry.StartDate) ?
                                                        entry.Order.StartDate : entry.StartDate;

      if (budgetingDate.Year < entry.Budget.Year) {
        budgetingDate = new DateTime(entry.Budget.Year, 1, 1);

      } else if (budgetingDate.Year == entry.Budget.Year) {
        budgetingDate = new DateTime(budgetingDate.Year, budgetingDate.Month, 1);

      } else if (budgetingDate.Year > entry.Budget.Year) {
        budgetingDate = new DateTime(entry.Budget.Year, 1, 1);
      }

      return budgetingDate;
    }


    private DateTime DetermineExcerciseRequestDate(OrderItem entry, bool isDeposit) {
      if (isDeposit) {
        return DateTime.Today.Date;
      }

      BudgetEntry budgetEntry = entry.RequisitionItem.BudgetEntry;

      if (!budgetEntry.IsEmptyInstance) {
        return new DateTime(budgetEntry.Year, budgetEntry.Month, 1);
      }

      if (!budgetEntry.IsEmptyInstance) {
        return new DateTime(budgetEntry.Year, budgetEntry.Month, 1);
      }

      if (!entry.Order.Requisition.IsEmptyInstance) {
        return new DateTime(entry.Order.Requisition.StartDate.Year,
                            entry.Order.Requisition.StartDate.Month, 1);
      }

      return new DateTime(_transaction.BaseBudget.Year, entry.Order.PostingTime.Month, 1);

    }


    #endregion Helpers

  }  // class OrderBudgetTransactionBuilder

}  // namespace Empiria.Operations.Integration.Budgeting
