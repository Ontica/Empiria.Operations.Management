/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Use cases Layer                      *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Use case interactor class            *
*  Type     : BudgetingProcurementUseCases                  License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to integrate organization's procurement operations with the budgeting system.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Services;

using Empiria.Financial;
using Empiria.Parties;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions.UseCases;

using Empiria.Procurement.Contracts;

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


    public FixedList<NamedEntityDto> SearchRelatedDocumentsForTransactionEdition(RelatedDocumentsQuery query) {
      Assertion.Require(query, nameof(query));

      var filter = new Filter();

      var orgUnit = OrganizationalUnit.Parse(query.OrganizationalUnitUID);

      filter.AppendAnd($"CONTRACT_MGMT_ORG_UNIT_ID = {orgUnit.Id}");
      filter.AppendAnd($"CONTRACT_STATUS <> 'X'");

      if (query.Keywords.Length != 0) {
        filter.AppendAnd(SearchExpression.ParseAndLikeKeywords("CONTRACT_KEYWORDS", query.Keywords));
      }

      return Contract.GetFullList<Contract>(filter.ToString(), "CONTRACT_NO")
                     .MapToNamedEntityList();
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

      int contractId = -1;

      if (payableEntity is Contract contract) {
        contractId = contract.Id;
      }
      if (payableEntity is ContractOrder contractOrder) {
        contractId = contractOrder.Contract.Id;
      }
      return new BudgetTransactionFields {
        TransactionTypeUID = transactionType.UID,
        ContractId = contractId,
        BaseBudgetUID = payableEntity.Budget.UID,
        OperationSourceUID = OperationSource.ParseNamedKey("SISTEMA_DE_ADQUISICIONES").UID,
        Description = payableEntity.Name,
        BasePartyUID = payableEntity.OrganizationalUnit.UID,
        RequestedByUID = Party.ParseWithContact(ExecutionServer.CurrentContact).UID,
        ApplicationDate = DateTime.Today
      }
    }

    #endregion Helpers

  }  // class BudgetingProcurementUseCases

}  // namespace Empiria.Operations.Integration.Budgeting.UseCases
