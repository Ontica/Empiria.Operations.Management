/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Adapters Layer                       *
*  Assembly : Empiria.Operations.Integration.UseCases.dll   Pattern   : Mapper                               *
*  Type     : ProductBudgetSegmentMapper                    License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Maps product budget segments to ProductBudgetSegmentDto instances.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Budgeting;
using Empiria.Products;

namespace Empiria.Operations.Integration.Budgeting.Adapters {

  /// <summary>Maps product budget segments to ProductBudgetSegmentDto instances.</summary>
  internal class ProductBudgetSegmentMapper {

    static internal FixedList<ProductBudgetSegmentDto> Map(FixedList<BudgetAccountSegmentLink> links) {
      return links.Select(x => Map(x))
                  .ToFixedList();
    }


    static internal ProductBudgetSegmentDto Map(BudgetAccountSegmentLink link) {
      return new ProductBudgetSegmentDto {
        UID = link.UID,
        Product = link.GetLinkedObject<Product>().MapToNamedEntity(),
        BudgetSegment = link.BudgetAccountSegment.MapToNamedEntity(),
        BudgetSegmentType = link.BudgetAccountSegment.BudgetSegmentType.MapToNamedEntity(),
        BudgetType = link.BudgetAccountSegment.BudgetSegmentType.BudgetType.MapToNamedEntity(),
      };
    }

  }  // class ProductBudgetSegmentMapper

}  // namespace Empiria.Operations.Integration.Budgeting.Adapters
