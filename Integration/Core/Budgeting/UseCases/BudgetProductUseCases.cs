/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Use cases Layer                      *
*  Assembly : Empiria.Operations.Integration.UseCases.dll   Pattern   : Use case interactor class            *
*  Type     : BudgetProductUseCases                         License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to integrate budget accounts and budget segments with products.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Parties;
using Empiria.Products;

using Empiria.Budgeting;

using Empiria.Operations.Integration.Budgeting.Adapters;
using System;
using Empiria.Collections;

namespace Empiria.Operations.Integration.Budgeting.UseCases {

  /// <summary>Use cases used to integrate budget accounts and budget segments with products.</summary>
  public class BudgetProductUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetProductUseCases() {
      // no-op
    }

    static public BudgetProductUseCases UseCaseInteractor() {
      return CreateInstance<BudgetProductUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ProductBudgetSegmentDto AddProductBudgetSegment(string productUID, NamedEntityFields fields) {
      Assertion.Require(productUID, nameof(productUID));
      Assertion.Require(fields, nameof(fields));

      var product = Product.Parse(productUID);
      var segment = BudgetAccountSegment.Parse(fields.UID);

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


    public FixedList<NamedEntityDto> SearchAvailableProductBudgetSegments(string productUID,
                                                                          string budgetTypeUID,
                                                                          string keywords) {
      Assertion.Require(productUID, nameof(productUID));
      Assertion.Require(budgetTypeUID, nameof(budgetTypeUID));
      keywords = keywords ?? string.Empty;

      var product = Product.Parse(productUID);
      var budgetType = BudgetType.Parse(budgetTypeUID);

      BudgetAccountSegmentType segmentType = budgetType.ProductProcurementSegmentType;

      FixedList<BudgetAccountSegment> segments = segmentType.SearchInstances(keywords);

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

      var searcher = new BudgetAccountSearcher(budgetType);

      FixedList<BudgetAccount> accounts = searcher.Search(orgUnit, segments);

      return accounts.MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class BudgetProductUseCases

}  // namespace Empiria.Operations.Integration.Budgeting.UseCases
