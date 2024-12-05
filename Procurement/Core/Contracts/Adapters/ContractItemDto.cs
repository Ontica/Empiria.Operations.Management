/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Data Transfer Object                    *
*  Type     : ContractItemDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return contracts item information.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Data transfer object used to return contracts information.</summary>
  public class ContractItemDto {

    internal ContractItemDto() {
      // no op
    }

    public string UID {
      get; internal set;
    }

    public NamedEntityDto ContractItemType {
      get; internal set;
    }

    public NamedEntityDto Contract {
      get; internal set;
    }

    public NamedEntityDto Supplier {
      get; internal set;
    }

    public NamedEntityDto Product {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public NamedEntityDto ProductUnit {
      get; internal set;
    }

    public decimal MinQuantity {
      get; internal set;
    }

    public decimal MaxQuantity {
      get; internal set;
    }

    public decimal UnitPrice {
      get; internal set;
    }

    public NamedEntityDto Project {
      get; internal set;
    }

    public NamedEntityDto BudgetAccount {
      get; internal set;
    }

    public NamedEntityDto PeriodicityType {
      get; internal set;
    }

  }  // class ContractItemDto

}  // namespace Empiria.Procurement.Contracts.Adapters
