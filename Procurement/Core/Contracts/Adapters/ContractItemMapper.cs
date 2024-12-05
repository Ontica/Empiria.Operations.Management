/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Mapper                                  *
*  Type     : ContractItemMapper                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for ContractItem instances.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Provides data mapping services for ContractItem instances.</summary>
  static internal class ContractItemMapper {

    static internal FixedList<ContractItemDto> Map(FixedList<ContractItem> contractsItem) {
      return contractsItem.Select(x => Map(x))
                      .ToFixedList();
    }


    static internal ContractItemDto Map(ContractItem contractItem) {
      return new ContractItemDto {
        UID = contractItem.UID,
        Contract = contractItem.Contract.MapToNamedEntity(),
        Supplier = contractItem.Supplier.MapToNamedEntity(),
        Product = contractItem.Product.MapToNamedEntity(),
        Description = contractItem.Description,
        ProductUnit = contractItem.ProductUnit.MapToNamedEntity(),
        MinQuantity = contractItem.MinQuantity,
        MaxQuantity = contractItem.MaxQuantity,
        UnitPrice = contractItem.UnitPrice,
        Project = contractItem.Project.MapToNamedEntity(),
        BudgetAccount = contractItem.BudgetAccount.MapToNamedEntity(),
        PeriodicityType = contractItem.PeriodicityType.MapToNamedEntity(),
      };
    }

  }  // class ContractItemMapper

}  // namespace Empiria.Procurement.Contracts.Adapters
