/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : SalesOrderItem                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an abstract sales order item.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders;

namespace Empiria.Procurement.Orders {

  /// <summary>Represents an abstract sales order item.</summary>
  public class SalesOrderItem : OrderItem {

    #region Constructors and parsers

    protected SalesOrderItem(OrderItemType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    protected internal SalesOrderItem(OrderItemType powertype,
                                      SalesOrder order) : base(powertype, order) {
      // no-op
    }

    static internal new SalesOrderItem Parse(int id) => ParseId<SalesOrderItem>(id);

    static internal new SalesOrderItem Parse(string uid) => ParseKey<SalesOrderItem>(uid);

    static internal new SalesOrderItem Empty => ParseEmpty<SalesOrderItem>();

    #endregion Constructors and parsers

    #region Properties

    public new SalesOrder Order {
      get {
        return (SalesOrder) base.Order;
      }
    }

    #endregion Properties

    #region Methods

    internal void Update(SalesOrderItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      base.Update(fields);
    }

    #endregion Methods

  }// class SalesOrderItem
}// namespace Empiria.Orders.Domain 
