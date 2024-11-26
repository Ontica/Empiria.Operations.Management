﻿/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Adapters Layer                       *
*  Assembly : Empiria.Operations.Integration.UseCases.dll   Pattern   : Output DTO                           *
*  Type     : BudgetValidationResultDto                     License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Output DTO that holds the result about a budget validation operation.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Operations.Integration.Adapters {

  /// <summary>Output DTO that holds the result about a budget validation operation.</summary>
  public class BudgetValidationResultDto {

    public string Result {
      get; internal set;
    }
  } // class BudgetValidationResultDto

}  // namespace Empiria.Operations.Integration.Adapters