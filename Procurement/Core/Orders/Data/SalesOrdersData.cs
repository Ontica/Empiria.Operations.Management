﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Data Layer                              *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data service                            *
*  Type     : SalesOrdersData                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for Salesorder and Sales order item instances.            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;
using Empiria.Procurement.Orders;

namespace Empiria.Orders.Data {

  /// <summary>Provides data read and write methods for order and order item instances.</summary>
  static internal class SalesOrdersData {

    #region Methods

    static internal void WriteSalesOrder(SalesOrder o, string extensionData) {
      var op = DataOperation.Parse("write_OMS_Order",
                     o.Id, o.UID, o.OrderType.Id, o.Category.Id, o.OrderNo, o.Description,
                     EmpiriaString.Tagging(o.Identificators), EmpiriaString.Tagging(o.Tags),
                     o.RequestedBy.Id, o.Responsible.Id, o.Beneficiary.Id, o.Provider.Id,
                     -1, -1, -1, o.Project.Id, o.Currency.Id,
                     o.Source.Id, (char) o.Priority, o.AuthorizationTime, o.AuthorizedBy.Id,
                     o.ClosingTime, o.ClosedBy.Id, extensionData, o.Keywords, -1,
                     o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteSalesOrderItem(SalesOrderItem o, string extensionData) {
      var op = DataOperation.Parse("write_OMS_Order_Item",
                     o.Id, o.UID, o.OrderItemType.Id, o.Order.Id, o.Product.Id,
                     o.Description, o.ProductUnit.Id, o.Quantity, o.UnitPrice, o.Discount,
                     o.Currency.Id, o.RelatedItemId, o.RequisitionItemId, o.RequestedBy.Id,
                     o.BudgetAccount.Id, o.Project.Id, o.Provider.Id, o.PerEachItemId, extensionData,
                     o.Keywords, -1, o.Position, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class SalesOrdersData

}  // namespace Empiria.Orders.Data
