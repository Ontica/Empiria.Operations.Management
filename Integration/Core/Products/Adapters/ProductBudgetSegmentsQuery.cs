/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Products-Budgeting Integration                Component : Adapters Layer                       *
*  Assembly : Empiria.Operations.Integration.UseCases.dll   Pattern   : Input Query DTO                      *
*  Type     : ProductBudgetSegmentsQuery                    License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Input query DTO used to search product budget segments.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Products;

namespace Empiria.Operations.Integration.Products.Adapters {

  /// <summary>Input query DTO used to search product budget segments.</summary>
  public class ProductBudgetSegmentsQuery {

    public string ProductUID {
      get; set;
    } = string.Empty;


    public string BudgetTypeUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;

  }  // class ProductBudgetSegmentsQuery



  /// <summary>Extension methods for ProductBudgetSegmentsQuery type.</summary>
  static public class ProductBudgetSegmentsQueryExtensions {

    static public void EnsureValid(this ProductBudgetSegmentsQuery query) {
      Assertion.Require(query.ProductUID, nameof(query.ProductUID));
      Assertion.Require(query.BudgetTypeUID, nameof(query.BudgetTypeUID));
      query.Keywords = query.Keywords ?? string.Empty;
    }

  }  // class ProductBudgetSegmentsQueryExtensions

}  // namespace Empiria.Operations.Integration.Products.Adapters
