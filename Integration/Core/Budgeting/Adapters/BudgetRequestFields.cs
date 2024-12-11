/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Adapters Layer                       *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Input Fields DTO                     *
*  Type     : BudgetRequestFields                           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Input fields DTO used to request budget.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Operations.Integration.Budgeting.Adapters {

  /// <summary>Input fields DTO used to request budget.</summary>
  public class BudgetRequestFields {

    public string BaseObjectTypeUID {
      get; set;
    } = string.Empty;


    public string BaseObjectUID {
      get; set;
    } = string.Empty;


  } // class BudgetRequestFields



  /// <summary>Extension methods for BudgetRequestFields type.</summary>
  static public class BudgetRequestFieldsExtensions {

    static internal void EnsureValid(this BudgetRequestFields fields) {
      Assertion.Require(fields.BaseObjectTypeUID, nameof(fields.BaseObjectTypeUID));
      Assertion.Require(fields.BaseObjectUID, nameof(fields.BaseObjectUID));
    }

  }  // class BudgetRequestFieldsExtensions

}  // namespace Empiria.Operations.Integration.Budgeting.Adapters
