/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Data Transfer Object                    *
*  Type     : ContractItemDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return contracts item information.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders.Adapters;

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Data transfer object used to return contracts information.</summary>
  public class ContractItemDto : OrderItemDto {

    internal ContractItemDto(ContractItem item) : base(item) {
      Contract = item.Contract.MapToNamedEntity();
      MinQuantity = item.MinQuantity;
      MaxQuantity = item.MaxQuantity;
      Provider = item.Provider.MapToNamedEntity();
    }

    public NamedEntityDto Contract {
      get;
    }

    public decimal MinQuantity {
      get;
    }

    public decimal MaxQuantity {
      get;
    }

    public NamedEntityDto Provider {
      get;
    }

  }  // class ContractItemDto

}  // namespace Empiria.Procurement.Contracts.Adapters
