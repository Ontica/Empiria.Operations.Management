﻿/* Empiria Operations ****************************************************************************************
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


    static internal void WriteOrder(PayableOrder o, string extensionData) {
      var op = DataOperation.Parse("write_OMS_Order",
                     o.Id, o.UID, o.OrderType.Id, o.Category.Id, o.OrderNo, o.Description,
                     EmpiriaString.Tagging(o.Identificators), EmpiriaString.Tagging(o.Tags),
                     o.RequestedBy.Id, o.Responsible.Id, o.Beneficiary.Id, o.Provider.Id,
                     o.Budget.Id, o.RequisitionId, -1, o.Project.Id, o.Currency.Id,
                     o.Source.Id, (char) o.Priority, o.AuthorizationTime, o.AuthorizedBy.Id,
                     o.ClosingTime, o.ClosedBy.Id, extensionData, o.Keywords,
                     o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteOrderItem(PayableOrderItem o, string extensionData) {
      var op = DataOperation.Parse("write_OMS_Order_Item",
                     o.Id, o.UID, o.OrderItemType.Id, o.Order.Id, o.Product.Id,
                     o.Description, o.ProductUnit.Id, o.Quantity, o.UnitPrice, o.Discount,
                     o.Currency.Id, o.RelatedItemId, o.RequisitionItemId, o.RequestedBy.Id,
                     o.BudgetAccount.Id, o.Project.Id, o.Provider.Id, o.PerEachItemId, extensionData,
                     o.Keywords, o.Position, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class OrdersData

}  // namespace Empiria.Orders.Data
