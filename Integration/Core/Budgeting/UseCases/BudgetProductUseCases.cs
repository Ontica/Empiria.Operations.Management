﻿/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Use cases Layer                      *
*  Assembly : Empiria.Operations.Integration.UseCases.dll   Pattern   : Use case interactor class            *
*  Type     : BudgetProductUseCases                         License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to integrate budget accounts and budget account segments with products.         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Parties;
using Empiria.Products;

using Empiria.Budgeting;

using Empiria.Operations.Integration.Budgeting.Adapters;

namespace Empiria.Operations.Integration.Budgeting.UseCases {

  /// <summary>Use cases used to integrate budget accounts and budget account segments with products.</summary>
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

    public FixedList<NamedEntityDto> SearchBudgetAccountsForProduct(BudgetAccountsForProductQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureValid();

      var budgetType = BudgetType.Parse(query.BudgetTypeUID);
      var product = Product.Parse(query.ProductUID);
      var orgUnit = OrganizationalUnit.Parse(query.OrgUnitUID);

      var segments = BudgetAccountSegmentLink.GetBudgetAccountSegmentsForProduct(product);

      var searcher = new BudgetAccountSearcher(budgetType);

      FixedList<BudgetAccount> accounts = searcher.Search(orgUnit, segments);

      return accounts.MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class BudgetProductUseCases

}  // namespace Empiria.Operations.Integration.Budgeting.UseCases