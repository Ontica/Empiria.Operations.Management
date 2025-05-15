/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : InventoryHolderDto                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return inventory data.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory.Adapters {

  /// <summary>Output DTO used to return inventory data.</summary>
  public class InventoryHolderDto {

    public InventoryOrderDto Order {
      get; internal set;
    }


    public FixedList<InventoryOrderItemDto> Items {
      get; internal set;
    }


    public InventoryOrderActions Actions {
      get; internal set;
    }

  } // class InventoryHolderDto


  /// <summary>Output DTO used to return inventory entry data.</summary>
  public class InventoryEntryHolderDto {

    public FixedList<InventoryEntryDto> Items {
      get; internal set;
    }

  } // class InventoryEntryHolderDto


  public class InventoryOrderActions {


    public bool CanEdit {
      get; set;
    } = false;


    public bool CanEditItems {
      get; set;
    } = false;


    public bool CanEditEntries {
      get; set;
    } = false;


    public bool CanDelete {
      get; set;
    } = false;


    public bool CanClose {
      get; set;
    } = false;


    public bool CanOpen {
      get; set;
    } = false;


  } // class InventoryOrderActions

} // namespace Empiria.Inventory.Adapters
