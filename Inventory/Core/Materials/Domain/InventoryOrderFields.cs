﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrderFields                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to create and update inventory order.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Inventory {

  public enum InventoryStatus {

    Todos = 'T',

    Abierto = 'A',

    EnProceso = 'E',

    Cerrado = 'C'

  }


  /// <summary>Input fields DTO used to create and update inventory order.</summary>
  internal class InventoryOrderFields {

    public string ReferenceUID {
      get; internal set;
    }


    public string ResponsibleUID {
      get; internal set;
    }


    public string AssignedToUID {
      get; internal set;
    }


    public string Notes {
      get; internal set;
    }


    public FixedList<InventoryEntryFields> Items {
      get; internal set;
    } = new FixedList<InventoryEntryFields>();


    public InventoryStatus Status {
      get; internal set;
    } = InventoryStatus.Abierto;

  } // class InventoryOrderFields


  public class InventoryEntryFields {

    public string UID {
      get; internal set;
    }


    public string Location {
      get; internal set;
    }


    public string Product {
      get; internal set;
    }


    public decimal InputQuantity {
      get; internal set;
    }
  }

} // namespace Empiria.Inventory
