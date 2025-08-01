﻿/* Empiria Operations ****************************************************************************************
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

  /// <summary>Output DTO used to return inventory type.</summary>
  public class InventoryTypeDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public InventoryTypeRulesDto Rules {
      get; internal set;
    }

  } // class InventoryType

  /// <summary>Output DTO used to return inventory display rules.</summary>
  public class InventoryTypeRulesDto {

    public Boolean EntriesRequired {
      get; internal set;
    }


    public Boolean ItemsRequired {
      get; internal set;
    }

  } //  class InventoryTypeRules


  /// <summary>Output DTO used to return inventory order data.</summary>
  public class InventoryOrderDto {


    public string UID {
      get; internal set;
    }


    public NamedEntityDto OrderType {
      get; internal set;
    }


    public string OrderNo {
      get; internal set;
    }


    public InventoryTypeDto InventoryType {
      get; internal set;
    }


    public NamedEntityDto Warehouse {
      get; internal set;
    }


    public NamedEntityDto Responsible {
      get; internal set;
    }


    public NamedEntityDto RequestedBy {
      get; internal set;
    }


    public string Description {
      get; internal set;
    }


    public DateTime ClosingTime {
      get; internal set;
    }


    public DateTime PostingTime {
      get; internal set;
    }


    public NamedEntityDto PostedBy {
      get; internal set;
    }


    public NamedEntityDto Status {
      get;
      internal set;
    }

  } // class InventoryOrderDto


  /// <summary>Output DTO used to return inventory order item data.</summary>
  public class InventoryOrderItemDto {

    public string UID {
      get; internal set;
    } = string.Empty;


    public string ProductName {
      get; internal set;
    } = string.Empty;


    public string Description {
      get; internal set;
    } = string.Empty;


    public NamedEntityDto ProductUnit {
      get; internal set;
    }


    public decimal Quantity {
      get; internal set;
    }


    public string Location {
      get; internal set;
    }


    public decimal AssignedQuantity {
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


    public FixedList<InventoryEntryDto> Entries {
      get; internal set;
    }

  } // class InventoryOrderItemDto

} // namespace Empiria.Inventory.Adapters
