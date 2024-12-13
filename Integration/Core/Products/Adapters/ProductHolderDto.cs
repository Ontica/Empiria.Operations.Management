/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Products-Budgeting Integration                Component : Adapters Layer                       *
*  Assembly : Empiria.Operations.Integration.UseCases.dll   Pattern   : Output DTO                           *
*  Type     : ProductHolderDto                              License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Output DTO with integrated product information.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Products.Services.Adapters;

namespace Empiria.Operations.Integration.Products.Adapters {

  /// <summary>Output DTO with integrated product information.</summary>
  public class ProductHolderDto {

    public ProductDto Product {
      get; internal set;
    }

    public FixedList<ProductBudgetSegmentDto> BudgetSegments {
      get; internal set;
    }

    public ProductActions Actions {
      get; internal set;
    }

  }  // class ProductHolderDto



  /// <summary>Product actions control flags.</summary>
  public class ProductActions : BaseActions {

    public bool CanActivate {
      get; internal set;
    }

    public bool CanSuspend {
      get; internal set;
    }

  }  // class ProductActions

}  // namespace Empiria.Operations.Integration.Products.Adapters
