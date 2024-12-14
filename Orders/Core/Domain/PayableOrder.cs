/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : PayableOrder                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payable order.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.Financial;

namespace Empiria.Orders {

  /// <summary>Represents a payable order.</summary>
  public class PayableOrder : Order, IPayableEntity {

    #region Constructors and parsers

    protected PayableOrder(OrderType orderType) : base(orderType) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new PayableOrder Parse(int id) => ParseId<PayableOrder>(id);

    static public new PayableOrder Parse(string uid) => ParseKey<PayableOrder>(uid);

    static public new PayableOrder Empty => ParseEmpty<PayableOrder>();

    #endregion Constructors and parsers

    #region IPayableEntity interface

    INamedEntity IPayableEntity.Type {
      get {
        return base.GetEmpiriaType();
      }
    }

    string IPayableEntity.EntityNo {
      get {
        return base.OrderNo;
      }
    }

    INamedEntity IPayableEntity.PayTo {
      get {
        return base.Beneficiary;
      }
    }

    IEnumerable<IPayableEntityItem> IPayableEntity.Items {
      get {
        return base.GetItems<PayableOrderItem>()
                   .ToFixedList();
      }
    }

    #endregion IPayableEntity interface

    #region Methods

    internal protected virtual void AddItem(PayableOrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      base.AddItem(orderItem);
    }

    public decimal GetTotal() {
      return base.GetItems<PayableOrderItem>()
                  .Sum(x => x.Total);
    }

    internal protected virtual void RemoveItem(PayableOrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      base.RemoveItem(orderItem);
    }

    #endregion Methods

  }  // class PayableOrder

}  // namespace Empiria.Orders
