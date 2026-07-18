/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : ExpensesReport                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an expenses report that is a payable order.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

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

      ParentOrder = parentOrder;
    }

    static public new ExpensesReport Parse(int id) => ParseId<ExpensesReport>(id);

    static public new ExpensesReport Parse(string uid) => ParseKey<ExpensesReport>(uid);

    static public new ExpensesReport Empty => ParseEmpty<ExpensesReport>();

    static public new FixedList<ExpensesReport> GetList() {
      return Order.GetList<ExpensesReport>();
    }

    #endregion Constructors and parsers

  }  // class ExpensesReport

}  // namespace Empiria.Orders
