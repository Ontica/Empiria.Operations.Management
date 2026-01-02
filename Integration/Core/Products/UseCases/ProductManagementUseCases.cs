/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Products-Budgeting Integration                Component : Use cases Layer                      *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Use case interactor class            *
*  Type     : ProductManagementUseCases                     License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to update and return integrated products information.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Services;

using Empiria.Products;

using Empiria.Products.Services;
using Empiria.Products.Services.Adapters;

using Empiria.Operations.Integration.Products.Adapters;

namespace Empiria.Operations.Integration.Products.UseCases {

  /// <summary>Use cases used to update and return integrated products information.</summary>
  public class ProductManagementUseCases : UseCase {

    #region Constructors and parsers

    protected ProductManagementUseCases() {
      // no-op
    }

    static public ProductManagementUseCases UseCaseInteractor() {
      return CreateInstance<ProductManagementUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ProductHolderDto ActivateProduct(string productUID) {
      Assertion.Require(productUID, nameof(productUID));

      Product product = ProductServices.ActivateProduct(productUID);

      return ProductHolderMapper.Map(product);
    }


    public ProductHolderDto CreateProduct(ProductFields fields) {
      Assertion.Require(fields, nameof(fields));

      Product product = ProductServices.CreateProduct(fields);

      return ProductHolderMapper.Map(product);
    }


    public ProductHolderDto DeleteProduct(string productUID) {
      Assertion.Require(productUID, nameof(productUID));

      Product product = ProductServices.DeleteProduct(productUID);

      return ProductHolderMapper.Map(product);
    }


    public ProductHolderDto GetProduct(string productUID) {
      Assertion.Require(productUID, nameof(productUID));

      Product product = Product.Parse(productUID);

      return ProductHolderMapper.Map(product);
    }


    public FixedList<ProductSearchDto> SearchProducts(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<Product> products = ProductServices.SearchProducts(keywords);

      products = products.FindAll(x => !(x is Documents.DocumentProduct));

      var list = new List<ProductSearchDto>(ProductMapper.MapToSearchDescriptor(products));

      //FixedList<SATProductoCucop> cucopProducts = ProductServices.SearchCucopProducts(keywords);

      //list.AddRange(ProductMapper.MapToSearchDescriptor(cucopProducts));

      return list.ToFixedList();
    }


    public FixedList<ProductDescriptorDto> SearchProducts(ProductsQuery query) {
      Assertion.Require(query, nameof(query));

      FixedList<Product> products = ProductServices.SearchProducts(query);

      products = products.FindAll(x => !(x is Documents.DocumentProduct));

      return ProductMapper.MapToDescriptor(products);
    }


    public ProductHolderDto SuspendProduct(string productUID) {
      Assertion.Require(productUID, nameof(productUID));

      Product product = ProductServices.SuspendProduct(productUID);

      return ProductHolderMapper.Map(product);
    }


    public ProductHolderDto UpdateProduct(ProductFields fields) {
      Assertion.Require(fields, nameof(fields));

      Product product = ProductServices.UpdateProduct(fields);

      return ProductHolderMapper.Map(product);
    }

    #endregion Use cases

  }  // class ProductManagementUseCases

}  // namespace Empiria.Operations.Integration.Products.UseCases
