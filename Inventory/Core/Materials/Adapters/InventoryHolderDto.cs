/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : InventoryHolderDto                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return inventory data.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Inventory.Adapters {

  /// <summary>Output DTO used to return inventory data.</summary>
  public class InventoryHolderDto {

    public InventoryOrderDto Order {
      get; internal set;
    }


    public FixedList<InventoryEntryDto> Items {
      get; internal set;
    }

  } // class InventoryHolderDto


  /// <summary>Output DTO used to return inventory entry data.</summary>
  public class InventoryEntryHolderDto {

    public FixedList<InventoryEntryDto> Items {
      get; internal set;
    }

  } // class InventoryEntryHolderDto

} // namespace Empiria.Inventory.Adapters
