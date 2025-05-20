/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : AssetDto, AssetHolderDto                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return assets data.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Documents;
using Empiria.History;

namespace Empiria.Inventory.Assets.Adapters {

  public class AssetHolderDto {

    public AssetDto Asset {
      get; internal set;
    }

    public FixedList<AssetTransactionDescriptorDto> Transactions {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }

    public BaseActions Actions {
      get; internal set;
    }

  }  // class AssetHolderDto



  /// <summary>Data transfer object used to return an asset data.</summary>
  public class AssetDto {

    public string UID {
      get; internal set;
    }

    public string AssetNo {
      get; internal set;
    }

    public NamedEntityDto AssetType {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public FixedList<string> Identificators {
      get; internal set;
    }

    public FixedList<string> Tags {
      get; internal set;
    }

    public string Brand {
      get; internal set;
    }

    public string Model {
      get; internal set;
    }

    public int Year {
      get; internal set;
    }

    public NamedEntityDto AssignedTo {
      get; internal set;
    }

    public NamedEntityDto AssignedToOrgUnit {
      get; internal set;
    }

    public NamedEntityDto Manager {
      get; internal set;
    }

    public NamedEntityDto ManagerOrgUnit {
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

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // class AssetDto



  /// <summary>Output Dto used to return minimal data for an asset.</summary>
  public class AssetDescriptor {

    public string UID {
      get; internal set;
    }

    public string AssetNo {
      get; internal set;
    }

    public string AssetTypeName {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string AssignedToName {
      get; internal set;
    }

    public string AssignedToOrgUnitName {
      get; internal set;
    }

    public string LocationName {
      get; internal set;
    }

    public string Condition {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  } // class AssetDescriptor

}  // namespace Empiria.Inventory.Assets.Adapters
