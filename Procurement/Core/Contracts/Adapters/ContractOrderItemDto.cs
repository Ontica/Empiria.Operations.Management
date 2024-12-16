/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Data Transfer Object                    *
*  Type     : ContractOrderItemDto                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return contract supply order items.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Data transfer object used to return contract supply order items.</summary>
  public class ContractOrderItemDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto Order {
      get; internal set;
    }

    public NamedEntityDto OrderType {
      get; internal set;
    }

    public NamedEntityDto ContractItem {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public NamedEntityDto Product {
      get; internal set;
    }

    public NamedEntityDto ProductUnit {
      get; internal set;
    }

    public decimal Quantity {
      get; internal set;
    }

    public decimal UnitPrice {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public NamedEntityDto BudgetAccount {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // class ContractOrderItemDto

}  // namespace Empiria.Procurement.Contracts.Adapters
