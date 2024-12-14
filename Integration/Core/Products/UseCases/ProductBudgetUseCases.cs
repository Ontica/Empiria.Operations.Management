/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Products-Budgeting Integration                Component : Use cases Layer                      *
*  Assembly : Empiria.Operations.Integration.UseCases.dll   Pattern   : Use case interactor class            *
*  Type     : ProductBudgetUseCases                         License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to integrate products with budget segments and retrieve their budget accounts.  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria;
using Empiria.Services;

using Empiria.Parties;
using Empiria.Products;

using Empiria.Budgeting;

using Empiria.Operations.Integration.Budgeting.Adapters;
using Empiria.Operations.Integration.Products.Adapters;

namespace Empiria.Operations.Integration.Products.UseCases {

  /// <summary>Use cases used to integrate products with budget segments and
  /// retrieve their budget accounts.</summary>
  public class ProductBudgetUseCases : UseCase {

    #region Constructors and parsers

    protected ProductBudgetUseCases() {
      // no-op
    }

    static public ProductBudgetUseCases UseCaseInteractor() {
      return CreateInstance<ProductBudgetUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ProductBudgetSegmentDto AddProductBudgetSegment(string productUID,
                                                           ProductBudgetSegmentFields fields) {
      Assertion.Require(productUID, nameof(productUID));
      Assertion.Require(fields, nameof(fields));

      var product = Product.Parse(productUID);
      var segment = BudgetAccountSegment.Parse(fields.BudgetSegmentUID);

      var link = new BudgetAccountSegmentLink(segment, product);

      link.Save();

      return ProductBudgetSegmentMapper.Map(link);
    }


    public FixedList<ProductBudgetSegmentDto> GetProductBudgetSegments(string productUID) {
      Assertion.Require(productUID, nameof(productUID));

      var product = Product.Parse(productUID);

      FixedList<BudgetAccountSegmentLink> links = BudgetAccountSegmentLink.GetListForProduct(product);

      return ProductBudgetSegmentMapper.Map(links);
    }


    public ProductBudgetSegmentDto RemoveProductBudgetSegment(string productUID,
                                                              string budgetSegmentProductLinkUID) {
      Assertion.Require(productUID, nameof(productUID));
      Assertion.Require(budgetSegmentProductLinkUID, nameof(budgetSegmentProductLinkUID));

      var product = Product.Parse(productUID);
      var link = BudgetAccountSegmentLink.Parse(budgetSegmentProductLinkUID);

      Assertion.Require(link.GetLinkedObject<Product>().Equals(product),
                        "BudgetSegmentProductLink' product mismatch");

      link.Delete();

      link.Save();

      return ProductBudgetSegmentMapper.Map(link);
    }


    public FixedList<NamedEntityDto> SearchAvailableProductBudgetSegments(ProductBudgetSegmentsQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureValid();

      var product = Product.Parse(query.ProductUID);
      var budgetType = BudgetType.Parse(query.BudgetTypeUID);

      BudgetAccountSegmentType segmentType = budgetType.ProductProcurementSegmentType;

      FixedList<BudgetAccountSegment> segments = segmentType.SearchInstances(query.Keywords);

      var current = BudgetAccountSegmentLink.GetBudgetAccountSegmentsForProduct(product);

      return segments.Remove(current)
                     .MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> SearchProductBudgetAccounts(BudgetAccountsForProductQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureValid();

      var product = Product.Parse(query.ProductUID);
      var orgUnit = OrganizationalUnit.Parse(query.OrgUnitUID);
      var budgetType = BudgetType.Parse(query.BudgetTypeUID);

      var segments = BudgetAccountSegmentLink.GetBudgetAccountSegmentsForProduct(product);

      if (segments.Count == 0) {
        return new FixedList<NamedEntityDto>();
      }

      var searcher = new BudgetAccountSearcher(budgetType);

      FixedList<BudgetAccount> accounts = searcher.Search(orgUnit, segments);

      return accounts.MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class ProductBudgetUseCases

}  // namespace Empiria.Operations.Integration.Products.UseCases
