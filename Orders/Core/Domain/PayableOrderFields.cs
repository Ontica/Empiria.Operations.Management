/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Input fields DTO                        *
*  Type     : PayableOrderFields                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to update payable orders information.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders {

  /// <summary>Input fields DTO used to update payable orders information.</summary>
  public class PayableOrderFields : OrderFields {

    public string BudgetUID {
      get; set;
    } = string.Empty;


    public string ExpenseTypeUID {
      get; set;
    } = string.Empty;


    public override void EnsureValid() {
      base.EnsureValid();

      BudgetUID = Patcher.CleanUID(BudgetUID);
      ExpenseTypeUID = Patcher.CleanUID(ExpenseTypeUID);

      Assertion.Require(BudgetUID, nameof(BudgetUID));
    }

  }  // class PayableOrderFields

}  // namespace Empiria.Orders


