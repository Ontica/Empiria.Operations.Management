/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Products-Budgeting Integration                Component : Adapters Layer                       *
*  Assembly : Empiria.Operations.Integration.UseCases.dll   Pattern   : Mapper                               *
*  Type     : ProductHolderMapper                           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Maps integrated product information into ProductHolderDto instances.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Products;
using Empiria.Products.Services.Adapters;

using Empiria.Operations.Integration.Products.UseCases;

namespace Empiria.Operations.Integration.Products.Adapters {

  /// <summary>Maps integrated product information into ProductHolderDto instances.</summary>
  static public class ProductHolderMapper {

    static public ProductHolderDto Map(Product product) {
      return new ProductHolderDto {
        Product = ProductMapper.Map(product),
        BudgetSegments = GetProductBudgetSegments(product),
        Actions = MapActions()
      };
    }


    static private ProductActions MapActions() {
      return new ProductActions {
        CanDelete = true,
        CanUpdate = true
      };
    }


    #region Helpers

    static private FixedList<ProductBudgetSegmentDto> GetProductBudgetSegments(Product product) {

      using (var usecases = ProductBudgetUseCases.UseCaseInteractor()) {
        return usecases.GetProductBudgetSegments(product.UID);
      }
    }

    #endregion Helpers

  }  // class ProductHolderMapper

}  // namespace Empiria.Operations.Integration.Products.Adapters
