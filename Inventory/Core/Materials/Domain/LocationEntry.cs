/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : LocationEntry                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a location entry.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Inventory {

  /// <summary>Represents a location entry.</summary>
  public class LocationEntry {

    [DataField("Object_Id")]
    internal int LocationId {
      get; set;
    }


    [DataField("Object_UID")]
    internal string LocationUID {
      get; set;
    }


    [DataField("Object_Type_Id")]
    internal int ObjectTypeId {
      get; set;
    }


    [DataField("Object_Name")]
    internal string Name {
      get; set;
    }


    [DataField("Object_Description")]
    internal string Description {
      get; set;
    }

  } // class LocationEntry

} // namespace Empiria.Inventory
