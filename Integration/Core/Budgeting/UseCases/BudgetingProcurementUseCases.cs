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
using Empiria.Parties;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions.UseCases;

using Empiria.Procurement.Contracts;

using Empiria.Operations.Integration.Budgeting.Adapters;
using Empiria.Orders;

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

      var budgetable = (Order) BaseObject.Parse(fields.BaseObjectTypeUID,
                                                fields.BaseObjectUID);

      BudgetTransaction transaction = InvokeBudgetTransactionService(budgetable,
                                                                     BudgetTransactionType.ApartarGastoCorriente);


      return BudgetTransactionMapper.MapToDescriptor(transaction);
    }


    public FixedList<NamedEntityDto> SearchRelatedDocumentsForTransactionEdition(RelatedDocumentsQuery query) {
      Assertion.Require(query, nameof(query));

      var filter = new Filter();

      var orgUnit = OrganizationalUnit.Parse(query.OrganizationalUnitUID);

      filter.AppendAnd($"ORDER_REQUESTED_BY_ID = {orgUnit.Id}");
      filter.AppendAnd($"ORDER_STATUS <> 'X'");

      if (query.Keywords.Length != 0) {
        filter.AppendAnd(SearchExpression.ParseAndLikeKeywords("ORDER_KEYWORDS", query.Keywords));
      }

      return Orders.Data.OrdersData.Search(filter.ToString(), "ORDER_NO")
                                   .MapToNamedEntityList();
    }


    public BudgetValidationResultDto ValidateBudget(BudgetRequestFields fields) {
      Assertion.Require(fields, nameof(fields));

      return new BudgetValidationResultDto();
    }

    #endregion Use cases

    #region Helpers

    static private BudgetTransaction InvokeBudgetTransactionService(Order order,
                                                                    BudgetTransactionType budgetTransactionType) {

      BudgetTransactionFields fields = TransformToBudgetTransactionFields(order,
                                                                          budgetTransactionType);

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {

        var txn = usecases.CreateTransaction(fields);

        return BudgetTransaction.Parse(txn.Transaction.UID);
      }
    }


    static private BudgetTransactionFields TransformToBudgetTransactionFields(Order budgetable,
                                                                              BudgetTransactionType transactionType) {

      int contractId = -1;

      if (budgetable is ContractOrder contractOrder) {
        contractId = contractOrder.Contract.Id;
      }
      return new BudgetTransactionFields {
        TransactionTypeUID = transactionType.UID,
        ContractId = contractId,
        BaseBudgetUID = budgetable.BaseBudget.UID,
        OperationSourceUID = OperationSource.ParseNamedKey("SISTEMA_DE_ADQUISICIONES").UID,
        Description = budgetable.Description,
        BasePartyUID = budgetable.RequestedBy.UID,
        RequestedByUID = Party.ParseWithContact(ExecutionServer.CurrentContact).UID,
        ApplicationDate = DateTime.Today
      };
    }

    #endregion Helpers

  }  // class BudgetingProcurementUseCases

}  // namespace Empiria.Operations.Integration.Budgeting.UseCases
