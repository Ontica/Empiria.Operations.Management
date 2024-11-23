/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Budgeting Integration                         Component : Use cases Layer                      *
*  Assembly : Empiria.OperationsManagement.UseCases.dll     Pattern   : Use case interactor class            *
*  Type     : BudgetValidationResultDto                     License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Output DTO that holds the result about a budget validation operation.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Integration.Adapters {

  /// <summary>Output DTO that holds the result about a budget validation operation.</summary>
  public class BudgetValidationResultDto {

    public string Result {
      get; internal set;
    }
  } // class BudgetValidationResultDto

}  // namespace Empiria.Budgeting.Integration.Adapters
