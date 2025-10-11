/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : InventoryOrderDescriptorDto                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return inventory order descriptor data.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Inventory.Adapters {

  /// <summary>Output DTO used to return inventory order descriptor data.</summary>
  public class InventoryOrderDescriptorDto {


    public string UID {
      get; set;
    }


    public string OrderTypeName {
      get; set;
    }


    public string OrderNo {
      get; set;
    }


    public string InventoryTypeName {
      get; set;
    }


    public string WarehouseName {
      get; set;
    }


    public string ResponsibleName {
      get; set;
    }


    public string RequestedByName {
      get; set;
    }


    public string Description {
      get; set;
    }


    public string DocumentNo {
      get; set;
    }


    public string PostedByName {
      get; set;
    }


    public DateTime PostingTime {
      get; set;
    }


    public string Status {
      get; set;
    }


    public string StakeholderName {
      get;
      internal set;
    }

  } // class InventoryOrderDescriptorDto

} // namespace Empiria.Inventory.Adapters
