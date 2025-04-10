/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Web Api Layer                        *
*  Assembly : Empiria.Operations.Integration.WebApi.dll     Pattern   : Web Api Controller                   *
*  Type     : BudgetingProcurementController                License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web API used to retrieve and generate budget procurement transactions.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Budgeting.Transactions.Adapters;

using Empiria.Operations.Integration.Budgeting.Adapters;
using Empiria.Operations.Integration.Budgeting.UseCases;

namespace Empiria.Operations.Integration.Budgeting.WebApi {

  /// <summary>Web API used to retrieve and generate budget procurement transactions.</summary>
  public class BudgetingProcurementController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/budgeting/procurement/requests")]
    public CollectionModel GetBudgetRequests([FromBody] BudgetRequestFields fields) {

      using (var usecases = BudgetingProcurementUseCases.UseCaseInteractor()) {
        FixedList<BudgetTransactionDescriptorDto> requests = usecases.GetBudgetRequests(fields);

        return new CollectionModel(base.Request, requests);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/procurement/requests")]
    public SingleObjectModel RequestBudget([FromBody] BudgetRequestFields fields) {

      using (var usecases = BudgetingProcurementUseCases.UseCaseInteractor()) {
        BudgetTransactionDescriptorDto transaction = usecases.RequestBudget(fields);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/related-documents/for-transaction-edition/search")]
    public CollectionModel SearchRelatedDocumentsForTransactionEdition([FromBody] RelatedDocumentsQuery query) {

      base.RequireBody(query);

      using (var usecases = BudgetingProcurementUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> list = usecases.SearchRelatedDocumentsForTransactionEdition(query);

        return new CollectionModel(base.Request, list);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/procurement/validate-budget")]
    public SingleObjectModel ValidateBudget([FromBody] BudgetRequestFields fields) {

      using (var usecases = BudgetingProcurementUseCases.UseCaseInteractor()) {
        BudgetValidationResultDto validationResult = usecases.ValidateBudget(fields);

        return new SingleObjectModel(base.Request, validationResult);
      }
    }

    #endregion Web Apis

  }  // class BudgetingProcurementController

}  // namespace Empiria.Operations.Integration.Budgeting.WebApi
