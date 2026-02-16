/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : ExpensesType                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about an expense category including its operating rules.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders {

  /// <summary>Holds information about an expense category including its operating rules.</summary>
  public class ExpensesType : CommonStorage {

    #region Constructors and parsers

    protected ExpensesType() {
      // Required by Empiria Framework
    }

    static public ExpensesType Parse(int id) => ParseId<ExpensesType>(id);

    static public ExpensesType Parse(string uid) => ParseKey<ExpensesType>(uid);

    static public FixedList<ExpensesType> GetList() {
      return GetStorageObjects<ExpensesType>();
    }

    static public ExpensesType Empty => ParseEmpty<ExpensesType>();

    #endregion Constructors and parsers

  } // class ExpensesType

} // namespace Empiria.Orders
