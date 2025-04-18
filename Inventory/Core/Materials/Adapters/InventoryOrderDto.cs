/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : InventoryOrderDto                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return inventory order data.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.StateEnums;

namespace Empiria.Inventory.Adapters {

  /// <summary>Output DTO used to return inventory order data.</summary>
  public class InventoryOrderDto {


    public string UID {
      get; internal set;
    }


    public NamedEntityDto InventoryOrderType {
      get; internal set;
    }


    public string Notes {
      get; internal set;
    }


    public string InventoryOrderNo {
      get; internal set;
    }


    public NamedEntityDto Reference {
      get; internal set;
    }


    public NamedEntityDto Responsible {
      get; internal set;
    }


    public NamedEntityDto AssignedTo {
      get; internal set;
    }


    public NamedEntityDto PostedBy {
      get; internal set;
    }


    public DateTime PostingTime {
      get; internal set;
    }

  } // class InventoryOrderDto


  /// <summary>Output DTO used to return inventory order item data.</summary>
  public class InventoryOrderItemDto {

    public string UID {
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


    public decimal Quantity {
      get; internal set;
    }


    public NamedEntityDto PostedBy {
      get; internal set;
    }


    public DateTime PostingTime {
      get; internal set;
    }


    public EntityStatus Status {
      get; internal set;
    }


    public FixedList<InventoryEntryDto> EntryItemsDto {
      get; internal set;
    }

  } // class InventoryOrderItemDto

} // namespace Empiria.Inventory.Adapters
