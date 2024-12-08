/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Web Api Layer                        *
*  Assembly : Empiria.Operations.Integration.WebApi.dll     Pattern   : Web Api Controller                   *
*  Type     : BudgetProductController                       License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web api used to integrate budget accounts and budget segments with products.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Operations.Integration.Budgeting.Adapters;
using Empiria.Operations.Integration.Budgeting.UseCases;

namespace Empiria.Operations.Integration.Budgeting.WebApi {

  /// <summary>Web API used to retrieve and edit budget transactions.</summary>
  public class BudgetProductController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/budgeting/budget-accounts-for-product")]
    public CollectionModel SearchBudgetAccountsForProduct([FromBody] BudgetAccountsForProductQuery query) {

      using (var usecases = BudgetProductUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> budgetAccounts = usecases.SearchBudgetAccountsForProduct(query);

        return new CollectionModel(base.Request, budgetAccounts);
      }
    }

    #endregion Web Apis

  }  // class BudgetProductController

}  // namespace Empiria.Operations.Integration.WebApi
