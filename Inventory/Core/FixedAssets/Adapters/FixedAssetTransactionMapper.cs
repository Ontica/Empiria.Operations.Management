/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Mapper                                  *
*  Type     : FixedAssetTransactionMapper                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for FixedAssetTransaction instances.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents.Services;
using Empiria.History.Services;

using Empiria.StateEnums;

namespace Empiria.Inventory.FixedAssets.Adapters {

  /// <summary>Provides data mapping services for FixedAssetTransaction instances.</summary>
  static internal class FixedAssetTransactionMapper {

    static internal FixedAssetTransactionHolderDto Map(FixedAssetTransaction transaction) {
      return new FixedAssetTransactionHolderDto {
        Transaction = MapFixedAssetTransaction(transaction),
        Entries = FixedAssetMapper.Map(transaction.GetFixedAssets()),
        Documents = DocumentServices.GetEntityDocuments(transaction),
        History = HistoryServices.GetEntityHistory(transaction),
        Actions = MapActions()
      };
    }


    static internal FixedAssetTransactionDto MapFixedAssetTransaction(FixedAssetTransaction transaction) {
      return new FixedAssetTransactionDto {
        UID = transaction.UID,
        TransactionType = transaction.FixedAssetTransactionType.MapToNamedEntity(),
        TransactionNo = transaction.TransactionNo,
        Description = transaction.Description,
        OperationSource = transaction.OperationSource.MapToNamedEntity(),
        BaseParty = transaction.BaseParty.MapToNamedEntity(),
        ApplicationDate = transaction.ApplicationDate,
        RequestedDate = transaction.RequestedTime,
        Status = transaction.Status.MapToNamedEntity()
      };
    }


    static internal FixedList<FixedAssetTransactionDescriptorDto> Map(FixedList<FixedAssetTransaction> transactions) {
      return transactions.Select(transaction => MapToDescriptor(transaction))
                         .ToFixedList();
    }

    #region Helpers

    static private FixedAssetTransactionActions MapActions() {
      return new FixedAssetTransactionActions {
        CanAuthorize = true,
        CanEditDocuments = true
      };
    }


    static private FixedAssetTransactionDescriptorDto MapToDescriptor(FixedAssetTransaction transaction) {
      return new FixedAssetTransactionDescriptorDto {
        UID = transaction.UID,
        TransactionTypeName = transaction.FixedAssetTransactionType.Name,
        TransactionNo = transaction.TransactionNo,
        Description = transaction.Description,
        OperationSourceName = transaction.OperationSource.Name,
        BasePartyName = transaction.BaseParty.Name,
        ApplicationDate = transaction.ApplicationDate,
        RequestedDate = transaction.RequestedTime,
        StatusName = transaction.Status.GetName()
      };
    }

    #endregion Helpers

  }  // class FixedAssetTransactionMapper

}  // namespace Empiria.Inventory.FixedAssets.Adapters
