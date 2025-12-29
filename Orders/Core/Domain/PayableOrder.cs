/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : PayableOrder                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payable order.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Parties;

using Empiria.Budgeting;

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

    static public FixedList<PayableOrder> GetList() {
      return Order.GetList<PayableOrder>();
    }

    #endregion Constructors and parsers

    #region Properties

    public ExpensesType ExpenseType {
      get {
        return base.ExtData.Get("expenseTypeId", ExpensesType.Empty);
      }
      private set {
        base.ExtData.SetIf("expenseTypeId", value.Id, !value.IsEmptyInstance);
      }
    }

    #endregion Properties

    #region IPayableEntity interface

    string IPayableEntity.EntityNo {
      get {
        return base.OrderNo;
      }
    }

    Party IPayableEntity.PayTo {
      get {
        return base.Provider;
      }
    }

    OrganizationalUnit IPayableEntity.OrganizationalUnit {
      get {
        return (OrganizationalUnit) this.RequestedBy;
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


    FixedList<IPayableEntityItem> IPayableEntity.Items {
      get {
        return base.GetItems<PayableOrderItem>()
                   .Select(x => (IPayableEntityItem) x)
                   .ToFixedList();
      }
    }

    #endregion IPayableEntity interface

    #region Methods

    internal protected virtual void AddItem(PayableOrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      base.Items.Add(orderItem);
    }


    public override FixedList<IPayableEntity> GetPayableEntities() {
      return new FixedList<IPayableEntity>(new[] { this });
    }


    internal protected void Update(PayableOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      if (this.GetItems<PayableOrderItem>().Count != 0 &&
         (this.BaseBudget.Distinct(Budget.Parse(fields.BudgetUID)) ||
          this.Currency.Distinct(Currency.Parse(fields.CurrencyUID)))) {
        Assertion.RequireFail("No es posible cambiar el presupuesto o la moneda, " +
                              "debido a que la orden tiene registradas una o más partidas.");
      }

      if (fields.Budgets.Length == 0 && fields.BudgetUID.Length != 0) {
        fields.Budgets = new string[] { fields.BudgetUID };

      } else if (fields.Budgets.Length == 0 && fields.BudgetUID.Length == 0) {
        fields.Budgets = new string[] { Requisition.BaseBudget.UID };
      }

      if (fields.ExpenseTypeUID.Length != 0) {
        ExpenseType = ExpensesType.Parse(fields.ExpenseTypeUID);
      }

      base.Update(fields);
    }

    #endregion Methods

  }  // class PayableOrder

}  // namespace Empiria.Orders
