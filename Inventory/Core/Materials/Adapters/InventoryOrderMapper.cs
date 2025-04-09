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


    private static FixedList<InventoryEntryDto> MapToItemsDto(FixedList<InventoryEntry> items) {
      throw new NotImplementedException();
    }


    private static InventoryOrderDto MapToOrderDto(InventoryOrder order) {
      throw new NotImplementedException();
    }

    #endregion Private methods

  } // class InventoryOrderMapper

} // namespace Empiria.Inventory.Adapters
