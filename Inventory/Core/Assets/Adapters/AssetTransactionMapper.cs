/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Mapper                                  *
*  Type     : AssetTransactionMapper                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for AssetTransaction instances.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents.Services;
using Empiria.History.Services;

using Empiria.StateEnums;

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Provides data mapping services for AssetTransaction instances.</summary>
  static internal class AssetTransactionMapper {

    static internal AssetTransactionHolderDto Map(AssetTransaction transaction) {
      return new AssetTransactionHolderDto {
        Transaction = MapAssetTransaction(transaction),
        Entries = Map(transaction.Entries),
        Documents = DocumentServices.GetEntityDocuments(transaction),
        History = HistoryServices.GetEntityHistory(transaction),
        Actions = MapActions(transaction)
      };
    }


    static internal AssetTransactionEntryDto Map(AssetTransactionEntry entry) {
      return new AssetTransactionEntryDto {
        UID = entry.UID,
        EntryType = entry.AssetTransactionEntryType.MapToNamedEntity(),
        Transaction = entry.Transaction.MapToNamedEntity(),
        Asset = AssetMapper.MapAsset(entry.Asset),
        Description = entry.Description
      };
    }


    static internal FixedList<AssetTransactionDescriptorDto> Map(FixedList<AssetTransaction> transactions) {
      return transactions.Select(transaction => MapToDescriptor(transaction))
                         .ToFixedList();
    }

    #region Helpers

    static private FixedList<AssetTransactionEntryDto> Map(FixedList<AssetTransactionEntry> entries) {
      return entries.Select(entry => Map(entry))
                    .ToFixedList();
    }


    static private AssetTransactionActions MapActions(AssetTransaction transaction) {
      return new AssetTransactionActions {
        CanAuthorize = false,
        CanEditDocuments = true,
        CanClone = transaction.CanOpen(),
        CanClose = transaction.CanClose(),
        CanDelete = transaction.CanEdit(),
        CanOpen = transaction.CanOpen(),
        CanUpdate = transaction.CanEdit(),
      };
    }


    static private AssetTransactionDto MapAssetTransaction(AssetTransaction transaction) {
      return new AssetTransactionDto {
        UID = transaction.UID,
        TransactionType = transaction.AssetTransactionType.MapToNamedEntity(),
        TransactionNo = transaction.TransactionNo,
        Description = transaction.Description,
        Identificators = transaction.Identificators,
        Tags = transaction.Tags,
        OperationSource = transaction.OperationSource.MapToNamedEntity(),
        Manager = transaction.Manager.MapToNamedEntity(),
        ManagerOrgUnit = transaction.ManagerOrgUnit.MapToNamedEntity(),
        AssignedTo = transaction.AssignedTo.MapToNamedEntity(),
        AssignedToOrgUnit = transaction.AssignedToOrgUnit.MapToNamedEntity(),
        Building = transaction.Building.MapToNamedEntity(),
        Floor = transaction.Floor.MapToNamedEntity(),
        Place = transaction.Place.MapToNamedEntity(),
        LocationName = transaction.Location.Name,
        ApplicationTime = transaction.ApplicationTime,
        RecordingTime = transaction.RecordingTime,
        RequestedTime = transaction.RequestedTime,
        Status = transaction.Status.MapToNamedEntity()
      };
    }


    static private AssetTransactionDescriptorDto MapToDescriptor(AssetTransaction transaction) {
      return new AssetTransactionDescriptorDto {
        UID = transaction.UID,
        TransactionTypeName = transaction.AssetTransactionType.DisplayName,
        TransactionNo = transaction.TransactionNo,
        Description = transaction.Description,
        OperationSourceName = transaction.OperationSource.Name,
        ManagerName = transaction.Manager.Name,
        ManagerOrgUnitName = transaction.ManagerOrgUnit.Name,
        AssignedToName = transaction.AssignedTo.Name,
        AssignedToOrgUnitName = transaction.AssignedToOrgUnit.Name,
        LocationName = transaction.Location.Name,
        ApplicationTime = transaction.ApplicationTime,
        RecordingTime = transaction.RecordingTime,
        RequestedTime = transaction.RequestedTime,
        StatusName = transaction.Status.GetName()
      };
    }

    #endregion Helpers

  }  // class AssetTransactionMapper

}  // namespace Empiria.Inventory.Assets.Adapters
