/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : SalesOrderItem                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an abstract sales order item.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Budgeting;
using Empiria.Financial;
using Empiria.Orders;
using Empiria.Orders.Data;

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

    internal FixedList<Order> Orders {
      get; set;
    }


    public new SalesOrder Order {
      get {
        return (SalesOrder) base.Order;
      }
    }


    [DataField("ORDER_ITEM_UNIT_PRICE")]
    public decimal UnitPrice {
      get; private set;
    }


    [DataField("ORDER_ITEM_DISCOUNT")]
    public decimal Discount {
      get; private set;
    }


    [DataField("ORDER_ITEM_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("ORDER_ITEM_BUDGET_ACCOUNT_ID")]
    public FormerBudgetAccount BudgetAccount {
      get; private set;
    }


    public decimal Total {
      get {
        return (Quantity * UnitPrice) - Discount;
      }
    }

    #endregion Properties


    #region Methods

    protected override void OnSave() {
      SalesOrdersData.WriteSalesOrderItem(this, this.ExtData.ToString());
    }


    internal void Update(SalesOrderItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      UnitPrice = fields.UnitPrice;
      Discount = fields.Discount;
      Currency = Patcher.Patch(fields.CurrencyUID, Order.Currency);

      BudgetAccount = FormerBudgetAccount.Parse(fields.BudgetAccountUID);

      base.Update(fields);
    }

    #endregion Methods

  }// class SalesOrderItem
}// namespace Empiria.Orders.Domain 
