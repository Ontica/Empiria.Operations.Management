/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Adapters Layer                       *
*  Assembly : Empiria.Operations.Integration.UseCases.dll   Pattern   : Input Fields DTO                     *
*  Type     : BudgetOperationFields                         License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Input fields DTO used to invoke a budgeting operation.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Operations.Integration.Adapters {

  /// <summary>Input fields DTO used to invoke a budgeting operation.</summary>
  public class BudgetOperationFields {

    public string BaseObjectTypeUID {
      get; set;
    } = string.Empty;


    public string BaseObjectUID {
      get; set;
    } = string.Empty;


  } // class BudgetOperationFields



  /// <summary>Extension methods for BudgetOperationFields type.</summary>
  static public class BudgetOperationFieldsExtensions {

    static internal void EnsureValid(this BudgetOperationFields fields) {
      Assertion.Require(fields.BaseObjectTypeUID, nameof(fields.BaseObjectTypeUID));
      Assertion.Require(fields.BaseObjectUID, nameof(fields.BaseObjectUID));
    }

  }  // class BudgetOperationFieldsExtensions

}  // namespace Empiria.Operations.Integration.Adapters
