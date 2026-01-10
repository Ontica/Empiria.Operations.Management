/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : OrderTaxEntry                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a tax applied to an order.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;
using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Orders.Data;

namespace Empiria.Orders {

  /// <summary>Represents a tax applied to an order.</summary>
  public class OrderTaxEntry : BaseObject, ITaxEntry {

    #region Constructors and parsers

    protected OrderTaxEntry() {
      // Required by Empiria Framework
    }

    internal protected OrderTaxEntry(Order order, TaxType taxType, decimal total) {
      Assertion.Require(order, nameof(order));
      Assertion.Require(taxType, nameof(taxType));
      Assertion.Require(total != 0.00m, nameof(total));

      Order = order;
      TaxType = taxType;
      Total = total;
    }


    internal protected OrderTaxEntry(OrderItem orderItem, TaxType taxType, decimal total) {
      Assertion.Require(orderItem, nameof(orderItem));
      Assertion.Require(taxType, nameof(taxType));
      Assertion.Require(total != 0.00m, nameof(total));

      Order = orderItem.Order;
      OrderItem = orderItem;
      TaxType = taxType;
      Total = total;
    }


    static public OrderTaxEntry Parse(int id) => ParseId<OrderTaxEntry>(id);

    static public OrderTaxEntry Parse(string uid) => ParseKey<OrderTaxEntry>(uid);

    #endregion Constructors and parsers

    #region Properties

    [DataField("ORDER_TAX_TYPE_ID")]
    public TaxType TaxType {
      get; private set;
    }


    [DataField("ORDER_TAX_ORDER_ID")]
    public Order Order {
      get; private set;
    }


    [DataField("ORDER_TAX_ORDER_ITEM_ID")]
    public OrderItem OrderItem {
      get; private set;
    }


    [DataField("ORDER_TAX_BASE_AMOUNT")]
    public decimal BaseAmount {
      get; private set;
    }

    [DataField("ORDER_TAX_TOTAL")]
    public decimal Total {
      get; private set;
    }


    [DataField("ORDER_TAX_EXT_DATA")]
    protected internal JsonObject ExtData {
      get; private set;
    }


    [DataField("ORDER_TAX_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("ORDER_TAX_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("ORDER_TAX_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    }

    #endregion Properties

    #region Methods

    internal void Delete() {
      Assertion.Require(Order.Status != EntityStatus.Closed,
                       "Can not delete tax entry because the order is closed.");

      Status = EntityStatus.Deleted;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }
      OrdersData.WriteOrderTax(this);
    }


    internal void Sum(decimal amount) {
      BaseAmount = 0;
      Total += amount;
    }


    internal void Update(decimal total) {
      Assertion.Require(total != 0.00m, nameof(total));

      Assertion.Require(Order.Status != EntityStatus.Closed,
                        "Can not update tax entry because the order is closed.");
      BaseAmount = 0;
      Total = total;
    }


    internal void Update(decimal baseAmount, decimal total) {
      Assertion.Require(baseAmount != 0.00m, nameof(baseAmount));
      Assertion.Require(total != 0.00m, nameof(total));

      Assertion.Require(Order.Status != EntityStatus.Closed,
                        "Can not update tax entry because the order is closed.");

      BaseAmount = baseAmount;
      Total = total;
    }

    #endregion Methods

  }  // class OrderTaxEntry

}  // namespace Empiria.Orders
