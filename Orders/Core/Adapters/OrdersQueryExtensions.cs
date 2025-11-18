/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Query Type Extensions                   *
*  Type     : OrdersQueryExtensions                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for OrdersQuery type.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Budgeting;
using Empiria.Projects;

namespace Empiria.Orders.Adapters {

  /// <summary>Extension methods for OrdersQuery type.</summary>
  static internal class OrdersQueryExtensions {

    #region Extension Methods

    static internal void EnsureIsValid(this OrdersQuery query) {
      Assertion.Require(query.OrderTypeUID, nameof(query.OrderTypeUID));
    }

    static internal string MapToFilterString(this OrdersQuery query) {

      string orderTypeFilter = BuildOrderTypeFilter(query.OrderTypeUID);

      string requestedByFilter = BuildRequestedByFilter(query.RequestedByUID);

      string providerFilter = BuildProviderFilter(query.ProviderUID);

      string categoryFilter = BuildCategoryFilter(query.CategoryUID);

      string budgetTypeFilter = BuildBudgetTypeFilter(query.BudgetTypeUID);

      string budgetFilter = BuildBudgetFilter(query.BudgetUID);

      string projectFilter = BuildProjectFilter(query.ProjectUID);

      string statusFilter = BuildStatusFilter(query.Status);

      string priorityFilter = BuildPriorityFilter(query.Priority);

      string orderNoFilter = BuildOrderNoFilter(query.OrderNo);

      string keywordFilter = BuildKeywordsFilter(query.Keywords);

      var filter = new Filter(orderTypeFilter);

      filter.AppendAnd(requestedByFilter);

      filter.AppendAnd(providerFilter);

      filter.AppendAnd(categoryFilter);

      filter.AppendAnd(budgetTypeFilter);

      filter.AppendAnd(budgetFilter);

      filter.AppendAnd(projectFilter);

      filter.AppendAnd(priorityFilter);

      filter.AppendAnd(statusFilter);

      filter.AppendAnd(orderNoFilter);

      filter.AppendAnd(keywordFilter);

      return filter.ToString();
    }


    static internal string MapToSortString(this OrdersQuery query) {

      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      }

      return "ORDER_NO";
    }

    #endregion Extension Methods

    #region Helpers

    static private string BuildBudgetFilter(string budgetUID) {
      if (budgetUID.Length == 0) {
        return string.Empty;
      }

      var budget = Budget.Parse(budgetUID);

      return $"ORDER_BUDGET_ID = {budget.Id}";
    }


    static private string BuildBudgetTypeFilter(string budgetTypeUID) {
      if (budgetTypeUID.Length == 0) {
        return string.Empty;
      }

      var budgetType = BudgetType.Parse(budgetTypeUID);

      FixedList<Budget> budgets = Budget.GetList(budgetType);

      if (budgets.Count == 0) {
        return string.Empty;
      }

      return SearchExpression.ParseInSet("ORDER_BUDGET_ID", budgets.Select(x => x.Id));
    }


    static private string BuildCategoryFilter(string categoryUID) {
      if (categoryUID.Length == 0) {
        return string.Empty;
      }

      var category = OrderCategory.Parse(categoryUID);

      return $"ORDER_CATEGORY_ID = {category.Id}";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }

      return SearchExpression.ParseAndLike("ORDER_KEYWORDS", keywords);
    }


    static private string BuildOrderNoFilter(string orderNo) {
      if (orderNo.Length == 0) {
        return string.Empty;
      }

      return $"ORDER_NO LIKE '%{orderNo}%'";
    }


    static private string BuildOrderTypeFilter(string orderTypeUID) {
      var orderType = OrderType.Parse(orderTypeUID);

      return $"ORDER_TYPE_ID IN ({orderType.GetAllSubclassesFilter()})";
    }


    static private string BuildPriorityFilter(Priority priority) {
      if (priority == Priority.All) {
        return string.Empty;
      }

      return $"ORDER_PRIORITY = '{(char) priority}'";
    }


    static private string BuildProjectFilter(string projectUID) {
      if (projectUID.Length == 0) {
        return string.Empty;
      }

      var project = Project.Parse(projectUID);

      return $"ORDER_PROJECT_ID = {project.Id}";
    }


    static private string BuildProviderFilter(string providerUID) {
      if (providerUID.Length == 0) {
        return string.Empty;
      }

      var provider = Party.Parse(providerUID);

      return $"ORDER_PROVIDER_ID = {provider.Id}";
    }


    static private string BuildRequestedByFilter(string requestedByUID) {
      if (requestedByUID.Length == 0) {
        return string.Empty;
      }

      var requestedBy = Party.Parse(requestedByUID);

      return $"ORDER_REQUESTED_BY_ID = {requestedBy.Id}";
    }


    static private string BuildStatusFilter(EntityStatus status) {

      if (status == EntityStatus.All) {
        return "ORDER_STATUS <> 'X' ";
      }

      if (status == EntityStatus.Deleted) {
        return "ORDER_STATUS = 'X' AND ORDER_ID <> -1";
      }

      return $"ORDER_STATUS = '{(char) status}'";
    }


    #endregion Helpers

  }  // class OrdersQueryExtensions

} // namespace Empiria.Orders.Adapters
