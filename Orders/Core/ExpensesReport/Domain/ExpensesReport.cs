/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : ExpensesReport                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an expenses report that is a payable order.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Orders {

  /// <summary>Represents a payable order.</summary>
  public class ExpensesReport : PayableOrder {

    #region Constructors and parsers

    internal protected ExpensesReport(OrderType orderType) : base(orderType) {
      // Required by Empiria Framework for all partitioned types.

    }

    internal protected ExpensesReport(PayableOrder parentOrder) : base(OrderType.ExpensesReport) {
      Assertion.Require(parentOrder, nameof(parentOrder));
      Assertion.Require(!parentOrder.IsEmptyInstance, nameof(parentOrder));

      ClonePropertiesFrom(parentOrder);
      ParentOrder = parentOrder;
    }

    static public new ExpensesReport Parse(int id) => ParseId<ExpensesReport>(id);

    static public new ExpensesReport Parse(string uid) => ParseKey<ExpensesReport>(uid);

    static public new ExpensesReport Empty => ParseEmpty<ExpensesReport>();

    static public new FixedList<ExpensesReport> GetList() {
      return Order.GetList<ExpensesReport>();
    }

    #endregion Constructors and parsers

    #region Properties

    public PayableOrder PayableOrder {
      get {
        return (PayableOrder) base.ParentOrder;
      }
    }

    public bool BudgetControlAuthorized {
      get {
        return base.ExtData.Get("budgetControlAuthorized", false);
      }
      private set {
        base.ExtData.SetIf("budgetControlAuthorized", value, value != false);
      }
    }


    public bool EjecutorGastoAuthorized {
      get {
        return base.ExtData.Get("ejecutorGastoAuthorized", false);
      }
      private set {
        base.ExtData.SetIf("ejecutorGastoAuthorized", value, value != false);
      }
    }


    public bool PaymentControlAuthorized {
      get {
        return base.ExtData.Get("paymentControlAuthorized", false);
      }
      private set {
        base.ExtData.SetIf("paymentControlAuthorized", value, value != false);
      }
    }

    #endregion Properties

    #region Methods

    internal void Authorize() {
      if (Status == EntityStatus.Pending) {
        base.Activate();
        EjecutorGastoAuthorized = true;
        return;
      }

      if (Status == EntityStatus.Active && !PaymentControlAuthorized) {
        PaymentControlAuthorized = true;
        return;
      }

      if (Status == EntityStatus.Active && !BudgetControlAuthorized) {
        BudgetControlAuthorized = true;
        base.Close(Party.ParseWithContact(ExecutionServer.CurrentContact));
        return;
      }
    }

    #endregion Methods

  }  // class ExpensesReport

}  // namespace Empiria.Orders
