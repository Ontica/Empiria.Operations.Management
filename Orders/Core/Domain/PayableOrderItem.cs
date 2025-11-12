/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : PayableOrderItem                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payable order item.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

namespace Empiria.Orders {

  /// <summary>Represents a payable order item.</summary>
  public class PayableOrderItem : OrderItem, IPayableEntityItem {

    #region Constructors and parsers

    protected PayableOrderItem(OrderItemType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    protected internal PayableOrderItem(OrderItemType powertype, Order order) : base(powertype, order) {
      // no-op
    }

    static internal new PayableOrderItem Parse(int id) => ParseId<PayableOrderItem>(id);

    static internal new PayableOrderItem Parse(string uid) => ParseKey<PayableOrderItem>(uid);

    static internal new PayableOrderItem Empty => ParseEmpty<PayableOrderItem>();

    #endregion Constructors and parsers

    #region Properties

    public new PayableOrder Order {
      get {
        return (PayableOrder) base.Order;
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

    internal void Update(PayableOrderItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      base.Update(fields);
    }

    #endregion Methods

  }  // class PayableOrderItem

}  // namespace Empiria.Orders
