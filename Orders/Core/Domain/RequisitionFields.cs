/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Input fields DTO                        *
*  Type     : RequisitionFields                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to update requisitions.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders {

  /// <summary>Input fields DTO used to update requisitions.</summary>
  public class RequisitionFields : OrderFields {

    public string[] Budgets {
      get; set;
    } = new string[0];


    public override void EnsureValid() {
      base.EnsureValid();
      Assertion.Require(Budgets.Length > 0, "At least one budget must be specified.");
    }

  }  // class RequisitionFields

}  // namespace Empiria.Orders


