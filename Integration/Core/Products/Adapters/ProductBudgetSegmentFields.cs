/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Products-Budgeting Integration                Component : Adapters Layer                       *
*  Assembly : Empiria.Operations.Integration.UseCases.dll   Pattern   : Input fields DTO                     *
*  Type     : ProductBudgetSegmentFields                    License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Input fields used to update product budget segments.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Operations.Integration.Products.Adapters {

  /// <summary>Input fields used to update product budget segments.</summary>
  public class ProductBudgetSegmentFields {

    public string BudgetSegmentUID {
      get; set;
    } = string.Empty;


    public string Observations {
      get; set;
    } = string.Empty;

  }  // class ProductBudgetSegmentFields

}  // namespace Empiria.Operations.Integration.Products.Adapters
