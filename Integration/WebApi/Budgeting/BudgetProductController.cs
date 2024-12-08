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

  /// <summary>Web api used to integrate budget accounts and budget segments with products.</summary>
  public class BudgetProductController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/budgeting/products/{productUID:guid}/budget-segments")]
    public SingleObjectModel AddProductBudgetSegment([FromUri] string productUID,
                                                     [FromBody] NamedEntityFields fields) {

      using (var usecases = BudgetProductUseCases.UseCaseInteractor()) {
        ProductBudgetSegmentDto segment = usecases.AddProductBudgetSegment(productUID, fields);

        return new SingleObjectModel(base.Request, segment);
      }
    }


    [HttpGet]
    [Route("v2/budgeting/products/{productUID:guid}/budget-segments")]
    public CollectionModel GetProductBudgetSegments([FromUri] string productUID) {

      using (var usecases = BudgetProductUseCases.UseCaseInteractor()) {
        FixedList<ProductBudgetSegmentDto> segments = usecases.GetProductBudgetSegments(productUID);

        return new CollectionModel(base.Request, segments);
      }
    }


    [HttpDelete]
    [Route("v2/budgeting/products/{productUID:guid}/budget-segments/{budgetSegmentProductLinkUID:guid}")]
    public NoDataModel RemoveProductBudgetSegment([FromUri] string productUID,
                                                  [FromUri] string budgetSegmentProductLinkUID) {

      using (var usecases = BudgetProductUseCases.UseCaseInteractor()) {
        _ = usecases.RemoveProductBudgetSegment(productUID, budgetSegmentProductLinkUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpGet]
    [Route("v2/budgeting/products/{productUID:guid}/budget-segments/available")]
    public CollectionModel SearchAvailableProductBudgetSegments([FromUri] string productUID,
                                                                [FromUri] string budgetTypeUID,
                                                                [FromUri] string keywords = "") {

      using (var usecases = BudgetProductUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> segments = usecases.SearchAvailableProductBudgetSegments(productUID, budgetTypeUID, keywords);

        return new CollectionModel(base.Request, segments);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/products/{productUID:guid}/budget-accounts")]
    public CollectionModel SearchBudgetAccountsForProduct([FromUri] string productUID,
                                                          [FromBody] BudgetAccountsForProductQuery query) {

      Assertion.Require(query.ProductUID.Length == 0 || query.ProductUID == productUID,
                        "ProductUID mismatch.");

      query.ProductUID = productUID;

      using (var usecases = BudgetProductUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> budgetAccounts = usecases.SearchProductBudgetAccounts(query);

        return new CollectionModel(base.Request, budgetAccounts);
      }
    }

    #endregion Web Apis

  }  // class BudgetProductController

}  // namespace Empiria.Operations.Integration.WebApi
