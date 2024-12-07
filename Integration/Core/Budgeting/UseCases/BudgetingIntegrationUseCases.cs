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

using Empiria.Financial;
using Empiria.Procurement.Contracts;

using Empiria.Budgeting.Transactions;
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

      fields.EnsureValid();

      var bo = BaseObject.Parse(fields.BaseObjectTypeUID, fields.BaseObjectUID);

      BudgetTransaction transaction;

      if (bo is Contract contract) {
        transaction = RequestContractBudget(contract);
      } else if (bo is IPayableEntity payableEntity) {
        transaction = RequestPayableEntityBudget(payableEntity);
      } else {
        throw Assertion.EnsureNoReachThisCode($"Unhandled object type {fields.BaseObjectTypeUID}.");
      }

      return BudgetTransactionMapper.MapToDescriptor(transaction);
    }

    public BudgetValidationResultDto ValidateBudget(BudgetOperationFields fields) {
      Assertion.Require(fields, nameof(fields));

      return new BudgetValidationResultDto();
    }

    #endregion Use cases

    #region Helpers

    private BudgetTransaction RequestContractBudget(Contract contract) {
      throw Assertion.EnsureNoReachThisCode();
    }

    private BudgetTransaction RequestPayableEntityBudget(IPayableEntity payableEntity) {
      throw Assertion.EnsureNoReachThisCode();
    }

    #endregion Helpers

  }  // class BudgetingIntegrationUseCases

}  // namespace Empiria.Operations.Integration.UseCases
