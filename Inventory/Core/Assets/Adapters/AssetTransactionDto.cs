/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Mapper                                  *
*  Type     : AssetTransactionDto                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for AssetTransaction instances.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Documents;
using Empiria.History.Services.Adapters;

namespace Empiria.Inventory.Assets.Adapters {

  public class AssetTransactionHolderDto {

    public AssetTransactionDto Transaction {
      get; internal set;
    }

    public FixedList<AssetTransactionEntryDto> Entries {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }

    public AssetTransactionActions Actions {
      get; internal set;
    }


  } // class AssetTransactionHolderDto



  public class AssetTransactionActions : BaseActions {

    public bool CanAuthorize {
      get; internal set;
    }

    public bool CanClone {
      get; internal set;
    }

    public bool CanClose {
      get; internal set;
    }

    public bool CanOpen {
      get; internal set;
    }

  }  // class AssetTransactionActions



  public class AssetTransactionDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto TransactionType {
      get; internal set;
    }

    public string TransactionNo {
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

    public NamedEntityDto Manager {
      get; internal set;
    }

    public NamedEntityDto ManagerOrgUnit {
      get; internal set;
    }

    public NamedEntityDto AssignedTo {
      get; internal set;
    }

    public NamedEntityDto AssignedToOrgUnit {
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

    public NamedEntityDto OperationSource {
      get; internal set;
    }

    public DateTime RequestedTime {
      get; internal set;
    }

    public DateTime ApplicationTime {
      get; internal set;
    }

    public DateTime RecordingTime {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // class AssetTransactionDto



  public class AssetTransactionDescriptorDto {

    public string UID {
      get; internal set;
    }

    public string TransactionTypeName {
      get; internal set;
    }

    public string TransactionNo {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string ManagerName {
      get; internal set;
    }

    public string ManagerOrgUnitName {
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

    public string OperationSourceName {
      get; internal set;
    }

    public DateTime RequestedTime {
      get; internal set;
    }

    public DateTime ApplicationTime {
      get; internal set;
    }

    public DateTime RecordingTime {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  }  // class AssetTransactionDescriptorDto



  public class AssetEntryDescriptorDto {

    public string UID {
      get; internal set;
    }

    public string Asset {
      get; internal set;
    }

  }  // class AssetEntryDescriptorDto

}  // namespace Empiria.Inventory.Assets.Adapters
