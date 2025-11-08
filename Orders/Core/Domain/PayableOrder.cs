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

using Empiria.Budgeting;
using Empiria.Financial;

namespace Empiria.Orders {

  /// <summary>Represents a payable order.</summary>
  public class PayableOrder : Order, IPayableEntity {

    #region Constructors and parsers

    internal protected PayableOrder(OrderType orderType) : base(orderType) {
      // Required by Empiria Framework for all partitioned types.

      base.OrderNo = EmpiriaString.BuildRandomString(8)
                                  .ToUpperInvariant();
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
        return base.Provider;
      }
    }

    INamedEntity IPayableEntity.OrganizationalUnit {
      get {
        return this.Responsible;
      }
    }


    INamedEntity IPayableEntity.Currency {
      get {
        return this.Currency;
      }
    }


    decimal IPayableEntity.Total {
      get {
        return this.GetTotal();
      }
    }


    INamedEntity IPayableEntity.Budget {
      get {
        return this.BaseBudget;
      }
    }


    INamedEntity IPayableEntity.Project {
      get {
        return this.Project;
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


    internal protected void Update(PayableOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      if (this.GetItems<PayableOrderItem>().Count != 0 &&
         (this.BaseBudget.Distinct(Budget.Parse(fields.Budgets[0])) ||
          this.Currency.Distinct(Currency.Parse(fields.CurrencyUID)))) {
        Assertion.RequireFail("No es posible cambiar el presupuesto o la moneda, " +
                              "debido a que la orden tiene registradas una o más partidas.");
      }
      base.BaseBudget = Budget.Parse(fields.Budgets[0]);

      if (fields.CurrencyUID.Length == 0) {
        base.Currency = Currency.Default;
      } else {
        base.Currency = Currency.Parse(fields.CurrencyUID);
      }

      base.Update(fields);
    }

    #endregion Methods

  }  // class PayableOrder

}  // namespace Empiria.Orders
