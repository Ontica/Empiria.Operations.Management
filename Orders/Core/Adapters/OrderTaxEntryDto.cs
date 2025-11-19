/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Output DTO                              *
*  Type     : OrderTaxEntryDto                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object representing an order tax entry.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders.Adapters {

  /// <summary>Data transfer object representing an order tax entry.</summary>
  public class OrderTaxEntryDto {

    public string UID {
      get; internal set;
    }

    public string OrderUID {
      get; internal set;
    }

    public NamedEntityDto TaxType {
      get; internal set;
    }

    public decimal BaseAmount {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

  }  // class OrderTaxEntryDto

}  // namespace Empiria.Orders.Adapters
