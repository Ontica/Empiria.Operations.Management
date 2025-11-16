/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Data Layer                              *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data service                            *
*  Type     : OrdersData                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for order and order item instances.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Data;

namespace Empiria.Orders.Data {

  /// <summary>Provides data read and write methods for order and order item instances.</summary>
  static internal class OrdersData {

    #region Methods

    static internal List<OrderItem> GetOrderItems(Order order) {
      var sql = "SELECT * FROM OMS_ORDER_ITEMS " +
                $"WHERE ORDER_ITEM_ORDER_ID = {order.Id} AND " +
                       "ORDER_ITEM_STATUS <> 'X' " +
                "ORDER BY ORDER_ITEM_POSITION";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<OrderItem>(op);
    }


    static internal FixedList<Order> Search(string filter, string sort) {
      var sql = "SELECT * FROM OMS_ORDERS";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }
      if (!string.IsNullOrWhiteSpace(sort)) {
        sql += $" ORDER BY {sort}";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Order>(op);
    }


    static internal void WriteOrder(Order o) {
      var op = DataOperation.Parse("write_OMS_Order",
        o.Id, o.UID, o.OrderType.Id, o.Category.Id, o.OrderNo, o.Description, o.Justification,
        EmpiriaString.Tagging(o.Identificators), EmpiriaString.Tagging(o.Tags),
        o.Requisition.Id, o.ParentOrder.Id, o.ContractId, o.Project.Id,
        o.BaseBudgetType.Id, o.BaseBudget.Id, o.Currency.Id, o.RequestedBy.Id, o.Responsible.Id,
        o.Beneficiary.Id, o.Provider.Id, o.Warehouse.Id, o.Source.Id, (char) o.Priority,
        o.ConditionsData.ToString(), o.SpecificationsData.ToString(), o.DeliveryData.ToString(), o.ExtData.ToString(),
        o.Keywords, o.RecordingTime, o.RecordedBy.Id, o.ApplicationDate, o.AppliedBy.Id,
        o.AuthorizationTime, o.AuthorizedBy.Id, o.ClosingTime, o.ClosedBy.Id,
        o.PostingTime, o.PostedBy.Id, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteOrderItem(OrderItem o) {
      var op = DataOperation.Parse("write_OMS_Order_Item",
        o.Id, o.UID, o.OrderItemType.Id, o.Order.Id, o.Product.Id, o.SkuId, o.ProductCode, o.Description,
        o.Justification, o.ProductUnit.Id, o.RequestedQty, o.Quantity, o.UnitPrice, o.Discount, o.PriceId,
        o.Currency.Id, o.Budget.Id, o.BudgetAccount.Id, o.BudgetEntry.Id, o.Project.Id, o.Provider.Id,
        o.Requisition.Id, o.RequisitionItem.Id, o.ContractItemId, o.RelatedItem.Id, o.OriginCountry.Id,
        o.StartDate, o.EndDate, o.Location.Id, o.DeliveryPlace.Id, o.ConfigData.ToString(),
        o.ConditionsData.ToString(), o.SpecificationData.ToString(), o.ExtData.ToString(), o.Keywords,
        o.RequestedTime, o.RequestedBy.Id, o.RequiredTime, o.ReceivedBy.Id, o.ClosingTime, o.ClosedBy.Id,
        o.Position, o.PostingTime, o.PostedBy.Id, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class OrdersData

}  // namespace Empiria.Orders.Data
