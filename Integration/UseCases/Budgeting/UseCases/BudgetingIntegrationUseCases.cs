/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Use cases Layer                      *
*  Assembly : Empiria.Operations.Integration.UseCases.dll   Pattern   : Use case interactor class            *
*  Type     : BudgetingIntegrationUseCases                  License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to integrate organization's operations with the budgeting system.               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Budgeting.Transactions.Adapters;

using Empiria.Operations.Integration.Adapters;

namespace Empiria.Operations.Integration.UseCases {

  /// <summary>Use cases used to integrate organization's operations with the budgeting system.</summary>
  public class BudgetingIntegrationUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetingIntegrationUseCases() {
      // no-op
    }

    static public BudgetingIntegrationUseCases UseCaseInteractor() {
      return CreateInstance<BudgetingIntegrationUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public BudgetTransactionDescriptorDto RequestBudget(BudgetOperationFields fields) {
      Assertion.Require(fields, nameof(fields));

      return new BudgetTransactionDescriptorDto();
    }


    public BudgetValidationResultDto ValidateBudget(BudgetOperationFields fields) {
      Assertion.Require(fields, nameof(fields));

      return new BudgetValidationResultDto();
    }

    #endregion Use cases

  }  // class BudgetingIntegrationUseCases

}  // namespace Empiria.Operations.Integration.UseCases
