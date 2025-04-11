/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : InventoryOrderMapper                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for inventory order.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Inventory.Adapters {

  /// <summary>Mapping methods for inventory order.</summary>
  static internal class InventoryOrderMapper {

    #region Public methods

    static internal InventoryHolderDto Map(InventoryOrder order) {

      return new InventoryHolderDto {
        Order = MapToOrderDto(order),
        Items = MapToItemsDto(order.Items)
      };
    }

    #endregion Public methods

    #region Private methods

    static private InventoryEntryDto MapToInventoryEntryDto(InventoryEntry entry) {
      return new InventoryEntryDto {
        UID = entry.UID,
        InventoryEntryType = NamedEntityDto.Empty,
        Product = NamedEntityDto.Empty,
        Sku = NamedEntityDto.Empty,
        Location = NamedEntityDto.Empty,
        Unit = NamedEntityDto.Empty,
        Notes = entry.ObservationNotes,
        InputQuantity = entry.InputQuantity,
        InputCost = entry.InputCost,
        OutputQuantity = entry.OutputQuantity,
        OutputCost = entry.OutputCost,
        PostedBy = NamedEntityDto.Empty,
        PostingTime = entry.PostingTime
      };
    }


    private static FixedList<InventoryEntryDto> MapToItemsDto(FixedList<InventoryEntry> items) {

      return items.Select((x) => MapToInventoryEntryDto(x))
                         .ToFixedList();
    }


    private static InventoryOrderDto MapToOrderDto(InventoryOrder order) {

      return new InventoryOrderDto {
        UID = order.UID,
        InventoryOrderType = NamedEntityDto.Empty,
        Notes = order.Notes,
        InventoryOrderNo = order.InventoryOrderNo,
        Reference = NamedEntityDto.Empty,
        Responsible = NamedEntityDto.Empty,
        AssignedTo = NamedEntityDto.Empty,
        PostedBy = NamedEntityDto.Empty,
        PostingTime = order.PostingTime
      };
    }

    #endregion Private methods

  } // class InventoryOrderMapper

} // namespace Empiria.Inventory.Adapters
