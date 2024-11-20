/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Mapper                                  *
*  Type     : FixedAssetTransactionDto                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for FixedAssetTransaction instances.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Documents.Services.Adapters;
using Empiria.History.Services.Adapters;

namespace Empiria.Inventory.FixedAssets.Adapters {

  public class FixedAssetTransactionHolderDto {

    public FixedAssetTransactionDto Transaction {
      get; internal set;
    }

    public FixedList<FixedAssetDescriptor> Entries {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryDto> History {
      get; internal set;
    }

    public FixedAssetTransactionActions Actions {
      get; internal set;
    }


  } // class FixedAssetTransactionHolderDto



  public class FixedAssetTransactionActions : BaseActions {

    public bool CanAuthorize {
      get; internal set;
    }

  }  // class FixedAssetTransactionActions



  public class FixedAssetTransactionDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto TransactionType {
      get; internal set;
    }

    public string TransactionNo {
      get; internal set;
    }

    public NamedEntityDto BaseParty {
      get; internal set;
    }

    public NamedEntityDto OperationSource {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public DateTime RequestedDate {
      get; internal set;
    }

    public DateTime ApplicationDate {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // FixedAssetTransactionDto



  public class FixedAssetTransactionDescriptorDto {

    public string UID {
      get; internal set;
    }

    public string TransactionTypeName {
      get; internal set;
    }

    public string TransactionNo {
      get; internal set;
    }

    public string BasePartyName {
      get; internal set;
    }

    public string OperationSourceName {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public DateTime RequestedDate {
      get; internal set;
    }

    public DateTime ApplicationDate {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  }  // class FixedAssetTransactionDescriptorDto



  public class FixedAssetEntryDescriptorDto {

    public string UID {
      get; internal set;
    }

    public string FixedAsset {
      get; internal set;
    }

  }  // class FixedAssetEntryDescriptorDto

}  // namespace Empiria.Inventory.FixedAssets.Adapters
