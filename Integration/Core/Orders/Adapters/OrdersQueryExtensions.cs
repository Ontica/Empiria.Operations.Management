/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Orders Management Integration                 Component : Adapters Layer                       *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Query Type Extensions                *
*  Type     : OrdersQueryExtensions                         License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Extension methods for OrdersQuery type.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Budgeting;
using Empiria.Orders;
using Empiria.Projects;

namespace Empiria.Operations.Integration.Orders.Adapters {

  /// <summary>Extension methods for OrdersQuery type.</summary>
  static internal class OrdersQueryExtensions {

    #region Extension Methods

    static internal void EnsureIsValid(this OrdersQuery query) {
      Assertion.Require(query.OrderTypeUID, nameof(query.OrderTypeUID));
    }

    static internal string MapToFilterString(this OrdersQuery query) {

      string orderTypeFilter = BuildOrderTypeFilter(query.OrderTypeUID);

      string responsibleFilter = BuildResponsibleFilter(query.ResponsibleUID);

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

      filter.AppendAnd(responsibleFilter);

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
      if (budgetUID == string.Empty) {
        return string.Empty;
      }

      var budget = Budget.Parse(budgetUID);

      return $"ORDER_BUDGET_ID = {budget.Id}";
    }


    static private string BuildBudgetTypeFilter(string budgetTypeUID) {
      if (budgetTypeUID == string.Empty) {
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
      if (categoryUID == string.Empty) {
        return string.Empty;
      }

      var category = OrderCategory.Parse(categoryUID);

      return $"ORDER_CATEGORY_ID = {category.Id}";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords == string.Empty) {
        return string.Empty;
      }

      return SearchExpression.ParseAndLike("ORDER_KEYWORDS", keywords);
    }


    static private string BuildOrderNoFilter(string orderNo) {
      if (orderNo == string.Empty) {
        return string.Empty;
      }

      return $"CONTRACT_NO LIKE '%{orderNo}%'";
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
      if (projectUID == string.Empty) {
        return string.Empty;
      }

      var project = Project.Parse(projectUID);

      return $"ORDER_PROJECT_ID = {project.Id}";
    }


    static private string BuildProviderFilter(string providerUID) {
      if (providerUID == string.Empty) {
        return string.Empty;
      }

      var provider = Party.Parse(providerUID);

      return $"ORDER_PROVIDER_ID = {provider.Id}";
    }


    static private string BuildResponsibleFilter(string responsibleUID) {
      if (responsibleUID == string.Empty) {
        return string.Empty;
      }

      var responsible = Party.Parse(responsibleUID);

      return $"ORDER_RESPONSIBLE_ID = {responsible.Id}";
    }


    static private string BuildStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return "ORDER_STATUS <> 'X' ";
      }

      return $"ORDER_STATUS = '{(char) status}'";
    }


    #endregion Helpers

  }  // class ContractQueryExtensions

} // namespace Empiria.Operations.Integration.Orders.Adapters
