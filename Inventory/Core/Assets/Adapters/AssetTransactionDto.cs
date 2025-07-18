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
using Empiria.History;

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

    public bool CanReject {
      get; internal set;
    }

    public bool CanSendToAuthorization {
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

    public string BaseLocationName {
      get; internal set;
    }

    public NamedEntityDto OperationSource {
      get; internal set;
    }

    public DateTime ApplicationDate {
      get; internal set;
    }

    public NamedEntityDto AppliedBy {
      get; internal set;
    }

    public DateTime RecordingTime {
      get; internal set;
    }

    public NamedEntityDto RecordedBy {
      get; internal set;
    }

    public DateTime AuthorizationTime {
      get; internal set;
    }

    public NamedEntityDto AuthorizedBy {
      get; internal set;
    }

    public DateTime RequestedTime {
      get; internal set;
    }

    public NamedEntityDto RequestedBy {
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

    public string BaseLocationName {
      get; internal set;
    }

    public string OperationSourceName {
      get; internal set;
    }

    public DateTime ApplicationDate {
      get; internal set;
    }

    public string AppliedByName {
      get; internal set;
    }

    public DateTime RecordingTime {
      get; internal set;
    }

    public string RecordedByName {
      get; internal set;
    }

    public DateTime AuthorizationTime {
      get; internal set;
    }

    public string AuthorizedByName {
      get; internal set;
    }

    public DateTime RequestedTime {
      get; internal set;
    }

    public string RequestedBy {
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
