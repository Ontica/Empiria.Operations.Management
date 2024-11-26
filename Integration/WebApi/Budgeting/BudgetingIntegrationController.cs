/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Web Api Layer                        *
*  Assembly : Empiria.Operations.Integration.WebApi.dll     Pattern   : Web Api Controller                   *
*  Type     : BudgetingIntegrationController                License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web api used to integrate organization's operations with the budgeting system.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Budgeting.Transactions.Adapters;

using Empiria.Operations.Integration.Adapters;
using Empiria.Operations.Integration.UseCases;

namespace Empiria.Operations.Integration.WebApi {

  /// <summary>Web API used to retrieve and edit budget transactions.</summary>
  public class BudgetingIntegrationController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/budgeting/execute-operation/request")]
    public SingleObjectModel RequestBudget([FromBody] BudgetOperationFields fields) {

      using (var usecases = BudgetingIntegrationUseCases.UseCaseInteractor()) {
        BudgetTransactionDescriptorDto transaction = usecases.RequestBudget(fields);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/execute-operation/validate")]
    public SingleObjectModel ValidateBudget([FromBody] BudgetOperationFields fields) {

      using (var usecases = BudgetingIntegrationUseCases.UseCaseInteractor()) {
        BudgetValidationResultDto validationResult = usecases.ValidateBudget(fields);

        return new SingleObjectModel(base.Request, validationResult);
      }
    }

    #endregion Web Apis

  }  // class BudgetingIntegrationController

}  // namespace Empiria.Operations.Integration.WebApi
