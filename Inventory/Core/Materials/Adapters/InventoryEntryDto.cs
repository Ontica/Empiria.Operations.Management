﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : InventoryEntryDto                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return inventory entry data.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Inventory.Adapters {

  /// <summary>Output DTO used to return inventory entry data.</summary>
  public class InventoryEntryDto {

    public string UID {
      get; internal set;
    }


    public string Product {
      get; internal set;
    }


    public string Location {
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

  } // class InventoryEntryDto

} // namespace Empiria.Inventory.Adapters
