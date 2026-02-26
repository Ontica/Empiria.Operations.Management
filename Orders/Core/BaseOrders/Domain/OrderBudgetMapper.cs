/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Mapper                                  *
*  Type     : OrderBudgetMapper                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides mapping methods from order and order items to budgetable interfaces.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;

using Empiria.Orders.Contracts;

namespace Empiria.Orders {

  /// <summary>Provides mapping methods from order and order items to budgetable interfaces.</summary>
  static internal class OrderBudgetMapper {

    static internal BudgetableData Map(Order order) {
      return new BudgetableData {
        BudgetableType = order.GetEmpiriaType(),
        BudgetableNo = order.OrderNo,
        BaseBudget = order.BaseBudget,
        Currency = order.Currency,
        ExchangeRate = order.ExchangeRate,
        RequestedBy = order.RequestedBy,
        Justification = order.Justification,
        Description = order.Description,
        Keywords = order.Keywords
      };
    }


    static internal BudgetableItemData Map(OrderItem orderItem) {

      return new BudgetableItemData {
        BudgetableItem = orderItem,
        BudgetEntry = orderItem.BudgetEntry,
        Budget = orderItem.Budget,
        BudgetAccount = orderItem.BudgetAccount,
        RelatedBudgetableItem = GetRelatedBudgetableItem(orderItem),
        BudgetingDate = CalculateBudgetingDate(orderItem),
        PreviousBudgetEntry = null,
        Product = orderItem.Product,
        ProductCode = orderItem.ProductCode,
        ProductName = orderItem.ProductName,
        Description = orderItem.Description,
        Justification = orderItem.Justification,
        ProductUnit = orderItem.ProductUnit,
        OriginCountry = orderItem.OriginCountry,
        ProductQty = orderItem.Quantity,
        Project = orderItem.Project,
        Party = orderItem.RequestedBy,
        Currency = orderItem.Currency,
        ExchangeRate = orderItem.Order.ExchangeRate,
        CurrencyAmount = orderItem.Subtotal
      };
    }

    static private DateTime CalculateBudgetingDate(OrderItem orderItem) {

      int budgetYear = orderItem.Budget.Year;

      if (orderItem.Order is Requisition requisition) {

        if (requisition.StartDate.Year == budgetYear) {

          return new DateTime(budgetYear, requisition.StartDate.Month, 1);

        } else {

          return new DateTime(budgetYear, 1, 1);
        }
      }

      if (orderItem.Order is Contract contract) {

        if (contract.StartDate.Year == budgetYear &&
            contract.StartDate >= orderItem.Requisition.StartDate) {

          return new DateTime(budgetYear, contract.StartDate.Month, 1);

        } else if (contract.StartDate.Year == budgetYear &&
                   contract.StartDate < orderItem.Requisition.StartDate) {

          return new DateTime(budgetYear, DateTime.Today.Month, DateTime.Today.Day);

        }
      }

      if (budgetYear == DateTime.Today.Year) {

        return DateTime.Today;

      } else if (budgetYear > DateTime.Today.Year) {

        return new DateTime(budgetYear, 1, 1);

      } else {
        Assertion.RequireFail("Budget date can not be determined");

        return DateTime.Today;
      }

    }


    static private OrderItem GetRelatedBudgetableItem(OrderItem orderItem) {

      if (orderItem.Order is Requisition requisition) {
        return null;
      }

      if (orderItem is ContractItem) {
        return orderItem.RequisitionItem;
      }

      if (orderItem is ContractOrderItem) {
        return orderItem.ContractItem;
      }

      if (orderItem is PayableOrderItem) {
        return orderItem.RequisitionItem;
      }

      return null;
    }

  }  // class OrderBudgetMapper

} // namespace Empiria.Orders
