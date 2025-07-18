/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                                 Component : Adapters Layer                   *
*  Assembly : Empiria.Inventory.Core.dll                        Pattern   : Data Transfer Object             *
*  Type     : AssetAssignmentDto, AssetAssignmentHolderDto      License   : Please read LICENSE.txt file     *
*                                                                                                            *
*  Summary  : Data transfer object used to return assets assignment data.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Data transfer object used to return assets assignment data.</summary>
  public class AssetAssignmentHolder {

    public AssetAssignmentDto Assignment {
      get; internal set;
    }

    public FixedList<AssetDescriptor> Assets {
      get; internal set;
    }

    public AssetTransactionActions Actions {
      get; internal set;
    }

  }  // class AssetAssignmentHolder



  /// <summary>Data transfer object used to return an asset assignment data.</summary>
  public class AssetAssignmentDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto AssignedTo {
      get; internal set;
    }

    public NamedEntityDto AssignedToOrgUnit {
      get; internal set;
    }

    public NamedEntityDto ReleasedBy {
      get; internal set;
    }

    public NamedEntityDto ReleasedByOrgUnit {
      get; internal set;
    }

    public NamedEntityDto Building {
      get; internal set;
    }

    public NamedEntityDto Floor {
      get; internal set;
    }

    public NamedEntityDto Place {
      get; internal set;
    }

    public string LocationName {
      get; internal set;
    }

    public AssetTransactionDto LastAssignmentTransaction {
      get; internal set;
    }

  }  // class AssetAssignmentDto



  /// <summary>Output Dto used to return minimal data for an asset assignment.</summary>
  public class AssetAssignmentDescriptor {

    public string UID {
      get; internal set;
    }

    public string AssignedToName {
      get; internal set;
    }

    public string AssignedToOrgUnitName {
      get; internal set;
    }

    public string ReleasedByName {
      get; internal set;
    }

    public string ReleasedByOrgUnitName {
      get; internal set;
    }

    public string LocationName {
      get; internal set;
    }

    public string LastAssignmentTransactionUID {
      get; internal set;
    }

    public string LastAssignmentTransactionNo {
      get; internal set;
    }

    public DateTime LastAssignmentApplicationDate {
      get; internal set;
    }

  } // class AssetAssignmentDescriptor

}  // namespace Empiria.Inventory.Assets.Adapters
