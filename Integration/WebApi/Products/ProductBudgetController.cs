/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Products-Budgeting Integration                Component : Web Api Layer                        *
*  Assembly : Empiria.Operations.Integration.WebApi.dll     Pattern   : Web Api Controller                   *
*  Type     : ProductBudgetController                       License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web api for integrate products with their budget segments and retrieve their budget accounts.  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Operations.Integration.Budgeting.Adapters;
using Empiria.Operations.Integration.Products.Adapters;

using Empiria.Operations.Integration.Products.UseCases;

namespace Empiria.Operations.Integration.Products.WebApi {

  /// <summary>Web api for integrate products with their budget segments
  /// and retrieve their budget accounts.</summary>
  public class ProductBudgetController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v8/product-management/products/{productUID:guid}/budget-segments")]
    public SingleObjectModel AddProductBudgetSegment([FromUri] string productUID,
                                                     [FromBody] ProductBudgetSegmentFields fields) {

      base.RequireBody(fields);

      using (var usecases = ProductBudgetUseCases.UseCaseInteractor()) {
        ProductBudgetSegmentDto segment = usecases.AddProductBudgetSegment(productUID, fields);

        return new SingleObjectModel(Request, segment);
      }
    }


    [HttpGet]
    [Route("v8/product-management/products/{productUID:guid}/budget-segments")]
    public CollectionModel GetProductBudgetSegments([FromUri] string productUID) {

      using (var usecases = ProductBudgetUseCases.UseCaseInteractor()) {
        FixedList<ProductBudgetSegmentDto> segments = usecases.GetProductBudgetSegments(productUID);

        return new CollectionModel(Request, segments);
      }
    }


    [HttpDelete]
    [Route("v8/product-management/products/{productUID:guid}/budget-segments/{budgetSegmentProductLinkUID:guid}")]
    public NoDataModel RemoveProductBudgetSegment([FromUri] string productUID,
                                                  [FromUri] string budgetSegmentProductLinkUID) {

      using (var usecases = ProductBudgetUseCases.UseCaseInteractor()) {
        _ = usecases.RemoveProductBudgetSegment(productUID, budgetSegmentProductLinkUID);

        return new NoDataModel(Request);
      }
    }


    [HttpPost]
    //[Route("v8/product-management/products/{productUID:guid}/budget-segments/available")]
    [Route("v8/product-management/budget-segments/available")]
    public CollectionModel SearchAvailableProductBudgetSegments([FromBody] ProductBudgetSegmentsQuery query) {

      base.RequireBody(query);

      using (var usecases = ProductBudgetUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> segments = usecases.SearchAvailableProductBudgetSegments(query);

        return new CollectionModel(Request, segments);
      }
    }


    [HttpPost]
    [Route("v8/product-management/products/{productUID:guid}/budget-accounts")]
    public CollectionModel SearchBudgetAccountsForProduct([FromUri] string productUID,
                                                          [FromBody] BudgetAccountsForProductQuery query) {

      base.RequireBody(query);

      Assertion.Require(query.ProductUID.Length == 0 || query.ProductUID == productUID,
                        "ProductUID mismatch.");

      query.ProductUID = productUID;

      using (var usecases = ProductBudgetUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> budgetAccounts = usecases.SearchProductBudgetAccounts(query);

        return new CollectionModel(Request, budgetAccounts);
      }
    }

    #endregion Web Apis

  }  // class ProductBudgetController

}  // namespace Empiria.Operations.Integration.Products.WebApi
