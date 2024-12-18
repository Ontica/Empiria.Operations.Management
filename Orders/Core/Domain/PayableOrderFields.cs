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


    public string CurrencyUID {
      get; set;
    } = string.Empty;


    internal override void EnsureIsValid() {
      base.EnsureIsValid();

      Assertion.Require(BudgetUID, nameof(BudgetUID));
      Assertion.Require(CurrencyUID, nameof(CurrencyUID));
    }

  }  // class PayableOrderFields

}  // namespace Empiria.Orders


