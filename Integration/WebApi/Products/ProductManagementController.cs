/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Web Api Layer                        *
*  Assembly : Empiria.Operations.Integration.WebApi.dll     Pattern   : Web Api Controller                   *
*  Type     : ProductManagementController                   License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web api used to handle integrated product information.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Products;
using Empiria.Products.Services.Adapters;

using Empiria.Operations.Integration.Products.Adapters;
using Empiria.Operations.Integration.Products.UseCases;

namespace Empiria.Operations.Integration.Products.WebApi {

  /// <summary>Web api used to handle integrated product information.</summary>
  public class ProductManagementController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v8/product-management/products/{productUID:guid}/activate")]
    public SingleObjectModel ActivateProduct([FromUri] string productUID) {

      using (var services = ProductManagementUseCases.UseCaseInteractor()) {
        ProductHolderDto product = services.ActivateProduct(productUID);

        return new SingleObjectModel(base.Request, product);
      }
    }


    [HttpPost]
    [Route("v8/product-management/products")]
    public SingleObjectModel CreateProduct([FromBody] ProductFields fields) {

      base.RequireBody(fields);

      using (var services = ProductManagementUseCases.UseCaseInteractor()) {
        ProductHolderDto product = services.CreateProduct(fields);

        return new SingleObjectModel(base.Request, product);
      }
    }


    [HttpDelete]
    [Route("v8/product-management/products/{productUID:guid}")]
    public NoDataModel DeleteProduct([FromUri] string productUID) {

      using (var services = ProductManagementUseCases.UseCaseInteractor()) {
        _ = services.DeleteProduct(productUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpGet]
    [Route("v8/product-management/products/{productUID:guid}")]
    public SingleObjectModel GetProduct([FromUri] string productUID) {

      using (var services = ProductManagementUseCases.UseCaseInteractor()) {
        ProductHolderDto product = services.GetProduct(productUID);

        return new SingleObjectModel(base.Request, product);
      }
    }


    [HttpGet]
    [Route("v8/product-management/products/search")]
    public CollectionModel SearchProducts([FromUri] string keywords = "") {

      using (var services = ProductManagementUseCases.UseCaseInteractor()) {
        FixedList<ProductSearchDto> products = services.SearchProducts(keywords);

        return new CollectionModel(base.Request, products);
      }
    }


    [HttpPost]
    [Route("v8/product-management/products/search")]
    public CollectionModel SearchProducts([FromBody] ProductsQuery query) {

      base.RequireBody(query);

      using (var services = ProductManagementUseCases.UseCaseInteractor()) {
        FixedList<ProductDescriptorDto> products = services.SearchProducts(query);

        return new CollectionModel(base.Request, products);
      }
    }


    [HttpPost]
    [Route("v8/product-management/products/{productUID:guid}/suspend")]
    public SingleObjectModel SuspendProduct([FromUri] string productUID) {

      using (var services = ProductManagementUseCases.UseCaseInteractor()) {
        ProductHolderDto product = services.SuspendProduct(productUID);

        return new SingleObjectModel(base.Request, product);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v8/product-management/products/{productUID:guid}")]
    public SingleObjectModel UpdateProduct([FromUri] string productUID,
                                           [FromBody] ProductFields fields) {

      base.RequireBody(fields);

      Assertion.Require(fields.UID.Length == 0 || fields.UID == productUID,
                        "ProductUID mismatch.");

      fields.UID = productUID;

      using (var services = ProductManagementUseCases.UseCaseInteractor()) {
        ProductHolderDto product = services.UpdateProduct(fields);

        return new SingleObjectModel(base.Request, product);
      }
    }

    #endregion Web Apis

  }  // class ProductManagementController

}  // namespace Empiria.Operations.Integration.WebApi
