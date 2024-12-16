/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Use cases Layer                      *
*  Assembly : Empiria.Operations.Integration.UseCases.dll   Pattern   : Use case interactor class            *
*  Type     : BudgetingProcurementUseCases                  License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to integrate organization's procurement operations with the budgeting system.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Services;

using Empiria.Financial;
using Empiria.Parties;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions.UseCases;

using Empiria.Operations.Integration.Budgeting.Adapters;

namespace Empiria.Operations.Integration.Budgeting.UseCases {

  /// <summary>Use cases used to integrate organization's procurement operations
  /// with the budgeting system.</summary>
  public class BudgetingProcurementUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetingProcurementUseCases() {
      // no-op
    }

    static public BudgetingProcurementUseCases UseCaseInteractor() {
      return CreateInstance<BudgetingProcurementUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<BudgetTransactionDescriptorDto> GetBudgetRequests(BudgetRequestFields fields) {
      return new FixedList<BudgetTransactionDescriptorDto>();
    }


    public BudgetTransactionDescriptorDto RequestBudget(BudgetRequestFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var payableEntity = (IPayableEntity) BaseObject.Parse(fields.BaseObjectTypeUID,
                                                            fields.BaseObjectUID);

      BudgetTransaction transaction = InvokeBudgetTransactionService(payableEntity,
                                                                     BudgetTransactionType.ApartarGastoCorriente);


      return BudgetTransactionMapper.MapToDescriptor(transaction);
    }


    public BudgetValidationResultDto ValidateBudget(BudgetRequestFields fields) {
      Assertion.Require(fields, nameof(fields));

      return new BudgetValidationResultDto();
    }

    #endregion Use cases

    #region Helpers

    static private BudgetTransaction InvokeBudgetTransactionService(IPayableEntity payableEntity,
                                                                    BudgetTransactionType budgetTransactionType) {

      BudgetTransactionFields fields = TransformToBudgetTransactionFields(payableEntity,
                                                                          budgetTransactionType);

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        return usecases.CreateTransaction(payableEntity, fields);
      }
    }


    static private BudgetTransactionFields TransformToBudgetTransactionFields(IPayableEntity payableEntity,
                                                                              BudgetTransactionType transactionType) {

      var SISTEMA_DE_ADQUISICIONES = OperationSource.Parse(10);

      return new BudgetTransactionFields {
        TransactionTypeUID = transactionType.UID,
        BaseBudgetUID = "f3163d06-10b7-4fe1-8c44-9f10604a6c6b",
        OperationSourceUID = SISTEMA_DE_ADQUISICIONES.UID,
        Description = payableEntity.Name,
        BasePartyUID = "f7ca6769-b771-4371-9ee2-a7710bf4291f",
        RequestedByUID = Party.ParseWithContact(ExecutionServer.CurrentContact).UID,
        ApplicationDate = DateTime.Today
      };
    }

    #endregion Helpers

  }  // class BudgetingProcurementUseCases

}  // namespace Empiria.Operations.Integration.Budgeting.UseCases
