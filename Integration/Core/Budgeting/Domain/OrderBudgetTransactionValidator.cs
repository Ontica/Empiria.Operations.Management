/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Domain Layer                         *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Service provider                     *
*  Type     : OrderBudgetTransactionValidator               License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Provides services to validate budget availability for orders.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Budgeting;
using Empiria.Budgeting.Explorer;
using Empiria.Budgeting.Explorer.Adapters;
using Empiria.Budgeting.Explorer.UseCases;

using Empiria.Orders;

namespace Empiria.Operations.Integration.Budgeting {

  /// <summary>Provides services to validate budget availability for orders.</summary>
  internal class OrderBudgetTransactionValidator {

    private readonly Order _order;

    public OrderBudgetTransactionValidator(Order order) {

      Assertion.Require(order, nameof(order));
      Assertion.Require(order.GetItems<OrderItem>().Count > 0,
                        "Order has no items.");

      _order = order;
    }


    internal void EnsureOrderHasAvailableBudget() {

      var orderItems = _order.GetItems<OrderItem>()
                             .FindAll(x => x.Budget.Equals(_order.BaseBudget) &&
                                           x.BudgetEntry.IsEmptyInstance);

      Assertion.Require(orderItems.Count != 0,
                        "Todos los conceptos de la requisición ya cuentan con suficiencia presupuestal autorizada.");

      var budgetAccounts = orderItems.SelectDistinct(x => x.BudgetAccount);

      FixedList<BudgetDataInColumns> currentBudget = SearchAvailableBudget(budgetAccounts);

      EnsureOrderItemsAvailableBudget(orderItems, currentBudget);
    }

    #region Helpers

    private void EnsureOrderItemsAvailableBudget(FixedList<OrderItem> orderItems,
                                                 FixedList<BudgetDataInColumns> currentBudget) {

      foreach (var orderBudgetAccount in orderItems.GroupBy(x => x.BudgetAccount)) {

        var orderItemSubtotal = orderBudgetAccount.Sum(x => x.Subtotal);

        var budgetData = currentBudget.Find(x => x.BudgetAccount.Equals(orderBudgetAccount.Key));

        decimal available = (budgetData != null) ? budgetData.Available : 0m;

        if (orderItemSubtotal > available) {
          Assertion.RequireFail($"No hay presupuesto disponible " +
                                $"en la partida {orderBudgetAccount.Key.Name} para " +
                                $"el mes de {EmpiriaString.MonthName(_order.StartDate.Month)}. " +
                                $"Solicitado: {orderItemSubtotal.ToString("C2")}, " +
                                $"Disponible: {available.ToString("C2")}");
        }
      }
    }


    private FixedList<BudgetDataInColumns> SearchAvailableBudget(FixedList<BudgetAccount> budgetAccounts) {

      var query = new AvailableBudgetQuery {
        Budget = _order.BaseBudget,
        Year = _order.BaseBudget.Year,
        Month = _order.StartDate.Month,
        Accounts = budgetAccounts,
      };

      using (var usecases = BudgetExplorerUseCases.UseCaseInteractor()) {

        return usecases.GetAvailableBudget(query);
      }
    }

    #endregion Helpers

  }  // class OrderBudgetTransactionValidator

}  // namespace Empiria.Operations.Integration.Budgeting
