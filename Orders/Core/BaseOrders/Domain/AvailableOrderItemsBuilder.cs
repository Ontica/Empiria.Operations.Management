/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Service provider                        *
*  Type     : AvailableOrderItemsBuilder                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Builds a list of available order items for a given order.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Orders.Contracts;

using Empiria.Orders.Data;

namespace Empiria.Orders {

  public class AvailableOrderItem {

    internal AvailableOrderItem(OrderItem orderItem, decimal requestedTotal, decimal availableTotal) {
      Assertion.Require(orderItem, nameof(orderItem));

      OrderItem = orderItem;

      RequestedTotal = requestedTotal;
      AvailableTotal = availableTotal;
    }

    public OrderItem OrderItem {
      get;
    }

    public decimal RequestedTotal {
      get;
    }

    public decimal AvailableTotal {
      get;
    }

  }  // class AvailableOrderItem



  /// <summary>Builds a list of available order items for a given order.</summary>
  public class AvailableOrderItemsBuilder {

    private readonly Order _order;

    public AvailableOrderItemsBuilder(Order order) {
      Assertion.Require(order, nameof(order));

      _order = order;
    }


    public FixedList<AvailableOrderItem> BuildForAvailableBudget() {

      FixedList<OrderItem> requisitionItems = GetRequisitionItems();

      FixedList<OrderItem> relatedRequisitionItems = OrdersData.GetRelatedRequisitionItems(GetRequisition(), requisitionItems);

      relatedRequisitionItems = ApplyFilterToRelatedRequisitionItems(relatedRequisitionItems);

      FixedList<OrderItem> baseOrderItems = GetBaseOrderItemsWithBudgetEntry();

      return BuildAvailableBudgetItems(baseOrderItems, relatedRequisitionItems);
    }

    #region Helpers

    private FixedList<OrderItem> ApplyFilterToRelatedRequisitionItems(FixedList<OrderItem> relatedRequisitionItems) {

      if (_order is Contract contract) {
        return relatedRequisitionItems.FindAll(x => x.Order is Contract);
      }

      if (_order is ContractOrder contractOrder) {
        return relatedRequisitionItems.FindAll(x => x.Budget.Equals(contractOrder.BaseBudget) &&
                                                    !(x.Order is Contract));
      }

      if (_order is ExpensesReport expensesReport) {
        return relatedRequisitionItems.FindAll(x => x.Budget.Equals(expensesReport.BaseBudget));
      }

      return relatedRequisitionItems.FindAll(x => x.Budget.Equals(_order.BaseBudget));
    }


    private FixedList<AvailableOrderItem> BuildAvailableBudgetItems(FixedList<OrderItem> baseOrderItems,
                                                                    FixedList<OrderItem> relatedRequisitionItems) {

      var availableItems = new List<AvailableOrderItem>(baseOrderItems.Count);

      foreach (var item in baseOrderItems) {

        var requestedItems = relatedRequisitionItems.FindAll(x => x.RequisitionItem.Equals(item.RequisitionItem) ||
                                                                  x.RequisitionItem.Equals(item));

        decimal requestedTotal = requestedItems.Sum(x => x.Subtotal);

        var availableItem = new AvailableOrderItem(item, item.Subtotal, Math.Max(item.Subtotal - requestedTotal, 0));

        availableItems.Add(availableItem);
      }

      return availableItems.ToFixedList();
    }


    private FixedList<OrderItem> GetBaseOrderItemsWithBudgetEntry() {

      if (_order is ContractOrder contractOrder) {
        return contractOrder.Contract.GetItems<OrderItem>()
                                     .FindAll(x => x.BudgetEntry.NoRejected &&
                                                   x.Budget.Equals(contractOrder.BaseBudget));
      }

      if (_order is ExpensesReport expensesReport) {
        return expensesReport.PayableOrder.GetItems<OrderItem>()
                                          .FindAll(x => x.BudgetEntry.NoRejected &&
                                                        x.Budget.Equals(expensesReport.BaseBudget));
      }

      return _order.Requisition.GetItems<OrderItem>()
                                .FindAll(x => x.BudgetEntry.NoRejected &&
                                              x.Budget.Equals(_order.BaseBudget));
    }



    private Requisition GetRequisition() {
      if (_order is Requisition) {
        return _order as Requisition;
      }

      return _order.Requisition;
    }


    private FixedList<OrderItem> GetRequisitionItems() {

      if (_order is Requisition) {
        return _order.GetItems<OrderItem>()
                     .FindAll(x => x.BudgetEntry.NoRejected);
      }

      if (_order is ContractOrder contractOrder) {
        return _order.Contract.GetItems<OrderItem>()
                              .FindAll(x => x.BudgetEntry.NoRejected &&
                                            x.Budget.Equals(contractOrder.BaseBudget))
                              .Select(x => x.RequisitionItem)
                              .ToFixedList();
      }

      return _order.Requisition.GetItems<OrderItem>()
                  .ToFixedList()
                  .FindAll(x => x.BudgetEntry.NoRejected &&
                                x.Budget.Equals(_order.BaseBudget));
    }

    #endregion Helpers

  }  // class AvailableOrderItemsBuilder

}  // namespace Empiria.Orders
