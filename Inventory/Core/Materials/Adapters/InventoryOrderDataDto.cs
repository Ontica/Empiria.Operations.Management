/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : InventoryOrderDataDto                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return inventory descriptor data.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.DynamicData;

namespace Empiria.Inventory.Adapters {

  /// <summary>Output DTO used to return inventory descriptor data.</summary>
  public class InventoryOrderDataDto {

    public InventoryOrderQuery Query {
      get; set;
    }


    public FixedList<DataTableColumn> Columns {
      get; set;
    }


    public FixedList<InventoryOrderDescriptorDto> Entries {
      get; set;
    }

  } // class InventoryOrderDataDto

} // namespace Empiria.Inventory.Adapters
