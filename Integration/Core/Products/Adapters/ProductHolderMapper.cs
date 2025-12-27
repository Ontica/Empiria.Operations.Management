/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Products-Budgeting Integration                Component : Adapters Layer                       *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Mapper                               *
*  Type     : ProductHolderMapper                           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Maps integrated product information into ProductHolderDto instances.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Products;

using Empiria.Products.Services.Adapters;

namespace Empiria.Operations.Integration.Products.Adapters {

  /// <summary>Maps integrated product information into ProductHolderDto instances.</summary>
  static public class ProductHolderMapper {

    static public ProductHolderDto Map(Product product) {
      return new ProductHolderDto {
        Product = ProductMapper.Map(product),
        BudgetSegments = new FixedList<ProductBudgetSegmentDto>(),
        Actions = MapActions()
      };
    }


    static private ProductActions MapActions() {
      return new ProductActions {
        CanDelete = true,
        CanEditBudgetData = true,
        CanUpdate = true
      };
    }

  }  // class ProductHolderMapper

}  // namespace Empiria.Operations.Integration.Products.Adapters
