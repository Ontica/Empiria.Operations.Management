/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Mapper                                  *
*  Type     : OrderTaxMapper                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps order tax entries between domain and dto objects.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders.Adapters {

  /// <summary>Maps order tax entries between domain and dto objects.</summary>
  static internal class OrderTaxMapper {

    static public OrderTaxEntryDto Map(OrderTaxEntry taxEntry) {
      return new OrderTaxEntryDto {
        UID = taxEntry.UID,
        OrderUID = taxEntry.Order.UID,
        TaxTypeName = taxEntry.TaxType.Name,
        BaseAmount = taxEntry.BaseAmount,
        Total = taxEntry.Total
      };
    }

  }  // class OrderTaxMapper

}  // namespace Empiria.Orders.Adapters
