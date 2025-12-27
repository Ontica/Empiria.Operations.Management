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

using Empiria.Operations.Integration.Products.Adapters;

namespace Empiria.Operations.Integration.Products.WebApi {

  /// <summary>Web api for integrate products with their budget segments
  /// and retrieve their budget accounts.</summary>
  public class ProductBudgetController : WebApiController {

    #region Web Apis

    [HttpPost]  //   // ToDo: To be deprecated
    [Route("v8/product-management/products/{productUID:guid}/budget-segments")]
    public SingleObjectModel AddProductBudgetSegment([FromUri] string productUID,
                                                     [FromBody] ProductBudgetSegmentFields fields) {

      return new SingleObjectModel(base.Request, new ProductBudgetSegmentDto());
    }


    [HttpGet]  // ToDo: To be deprecated
    [Route("v8/product-management/products/{productUID:guid}/budget-segments")]
    public CollectionModel GetProductBudgetSegments([FromUri] string productUID) {

      return new CollectionModel(Request, new FixedList<ProductBudgetSegmentDto>());
    }


    [HttpDelete]  // ToDo: To be deprecated
    [Route("v8/product-management/products/{productUID:guid}/budget-segments/{budgetSegmentProductLinkUID:guid}")]
    public NoDataModel RemoveProductBudgetSegment([FromUri] string productUID,
                                                  [FromUri] string budgetSegmentProductLinkUID) {

      return new NoDataModel(Request);
    }


    [HttpPost]  // ToDo: To be deprecated
    [Route("v8/product-management/budget-segments/available")]
    public CollectionModel SearchAvailableProductBudgetSegments([FromBody] ProductBudgetSegmentsQuery query) {

      return new CollectionModel(base.Request, new FixedList<NamedEntityDto>());
    }

    #endregion Web Apis

  }  // class ProductBudgetController

}  // namespace Empiria.Operations.Integration.Products.WebApi
