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

    [HttpPost]
    [Route("v2/budgeting/execute-operation/request")]
    public SingleObjectModel RequestBudget([FromBody] BudgetOperationFields fields) {

      using (var usecases = BudgetingProcurementUseCases.UseCaseInteractor()) {
        BudgetTransactionDescriptorDto transaction = usecases.RequestBudget(fields);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/execute-operation/validate")]
    public SingleObjectModel ValidateBudget([FromBody] BudgetOperationFields fields) {

      using (var usecases = BudgetingProcurementUseCases.UseCaseInteractor()) {
        BudgetValidationResultDto validationResult = usecases.ValidateBudget(fields);

        return new SingleObjectModel(base.Request, validationResult);
      }
    }

    #endregion Web Apis

  }  // class BudgetingIntegrationController

}  // namespace Empiria.Operations.Integration.Budgeting.WebApi
