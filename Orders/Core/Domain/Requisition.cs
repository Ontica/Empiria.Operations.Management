/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder / Aggregate root     *
*  Type     : Requisition                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a requisition of good and services. A requisition is an aggregate of orders.        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Budgeting;

namespace Empiria.Orders {

  /// <summary>Represents a requisition of good and services. A requisition is an aggregate of orders.</summary>
  public class Requisition : Order, IBudgetingEntity {

    #region Constructors and parsers

    protected internal Requisition(OrderType orderType) : base(orderType) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new Requisition Parse(int id) => ParseId<Requisition>(id);

    static public new Requisition Parse(string uid) => ParseKey<Requisition>(uid);

    static public new Requisition Empty => ParseEmpty<Requisition>();

    #endregion Constructors and parsers

    #region Properties

    public FixedList<Budget> Budgets {
      get {
        return ExtData.GetFixedList<Budget>("budgets", false);
      }
      private set {
        base.ExtData.SetIf("budgets", value.Select(x => (object) x.Id).ToList(), true);
      }
    }

    #endregion Properties

    #region Methods

    internal protected virtual void AddItem(PayableOrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      base.AddItem(orderItem);
    }


    public decimal GetTotal() {
      return base.GetItems<PayableOrderItem>()
                 .Sum(x => x.Subtotal);
    }


    internal protected virtual void RemoveItem(PayableOrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      base.RemoveItem(orderItem);
    }


    internal protected void Update(RequisitionFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      FixedList<Budget> budgets = fields.Budgets.Select(x => Budget.Parse(x))
                                                .ToFixedList()
                                                .Sort((x, y) => x.Year.CompareTo(y.Year));

      BaseBudget = budgets.First();

      Budgets = budgets;

      base.Update(fields);
    }

    #endregion Methods

  }  // class Requisition

}  // namespace Empiria.Orders
