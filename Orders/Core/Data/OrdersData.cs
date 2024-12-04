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


    static internal void WriteOrder(Order o, string extensionData) {
      var op = DataOperation.Parse("write_OMS_Order",
                     o.Id, o.UID, o.OrderType.Id, o.OrderNo, o.Description,
                     extensionData,
                     o.Keywords, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteOrderItem(OrderItem o, string extensionData) {
      var op = DataOperation.Parse("write_OMS_Order_Item",
                     o.Id, o.UID, o.OrderItemType.Id, o.Order.Id, o.Description,
                     extensionData,
                     o.Keywords, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class OrdersData

}  // namespace Empiria.Orders.Data
