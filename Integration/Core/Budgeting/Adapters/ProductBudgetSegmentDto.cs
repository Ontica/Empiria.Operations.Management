/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Adapters Layer                       *
*  Assembly : Empiria.Operations.Integration.UseCases.dll   Pattern   : Output DTO                           *
*  Type     : ProductBudgetSegmentDto                       License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Output DTO with a link between a product and a budget segment.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Operations.Integration.Budgeting.Adapters {

  /// <summary>Output DTO with a link between a product and a budget segment.</summary>
  public class ProductBudgetSegmentDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto Product {
      get; internal set;
    }

    public NamedEntityDto BudgetSegment {
      get; internal set;
    }

    public NamedEntityDto BudgetSegmentType {
      get; internal set;
    }

    public NamedEntityDto BudgetType {
      get; internal set;
    }

  }  // class ProductBudgetSegmentDto

}  // namespace Empiria.Operations.Integration.Budgeting.Adapters
