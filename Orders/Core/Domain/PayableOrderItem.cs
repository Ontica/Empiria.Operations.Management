/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : PayableOrderItem                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payable order item.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Budgeting;
using Empiria.Financial;

using Empiria.Orders.Data;

namespace Empiria.Orders {

  /// <summary>Represents a payable order item.</summary>
  public class PayableOrderItem : OrderItem, IPayableEntityItem {

    #region Constructors and parsers

    protected PayableOrderItem(OrderItemType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static private PayableOrderItem Parse(int id) => ParseId<PayableOrderItem>(id);

    static private PayableOrderItem Parse(string uid) => ParseKey<PayableOrderItem>(uid);

    static internal new PayableOrderItem Empty => ParseEmpty<PayableOrderItem>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("ORDER_ITEM_UNIT_PRICE")]
    public decimal UnitPrice {
      get; private set;
    }


    [DataField("ORDER_ITEM_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("ORDER_ITEM_BUDGET_ACCOUNT_ID")]
    public BudgetAccount BudgetAccount {
      get; private set;
    }


    public decimal Total {
      get {
        return Quantity * UnitPrice;
      }
    }

    #endregion Properties

    #region IPayableEntityItem implementation

    INamedEntity IPayableEntityItem.BudgetAccount {
      get {
        return BudgetAccount;
      }
    }


    INamedEntity IPayableEntityItem.Currency {
      get {
        return Currency;
      }
    }


    INamedEntity IPayableEntityItem.Product {
      get {
        return base.Product;
      }
    }


    INamedEntity IPayableEntityItem.Unit {
      get {
        return base.ProductUnit;
      }
    }

    #endregion IPayableEntityItem implementation

    #region Methods

    protected override void OnSave() {
      OrdersData.WriteOrderItem(this, this.ExtData.ToString());
    }

    #endregion Methods

  }  // class PayableOrderItem

}  // namespace Empiria.Orders
