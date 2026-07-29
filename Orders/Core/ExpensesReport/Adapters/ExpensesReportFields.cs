/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Input fields DTO                        *
*  Type     : ExpensesReportFields                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to update expenses report information.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders {

  /// <summary>Input fields DTO used to update expenses report information.</summary>
  public class ExpensesReportFields : OrderFields {

    public string ExpensesReportTypeUID {
      get; set;
    } = string.Empty;


    public string PayableOrderUID {
      get; set;
    } = string.Empty;


    public override void EnsureValid() {

      ExpensesReportTypeUID = Patcher.CleanUID(ExpensesReportTypeUID);
      PayableOrderUID = Patcher.CleanUID(PayableOrderUID);

    }

  }  // class ExpensesReportFields

}  // namespace Empiria.Orders
