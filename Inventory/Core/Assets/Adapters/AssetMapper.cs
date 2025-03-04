﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Mapper                                  *
*  Type     : AssetMapper                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for Asset instances.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents.Services;
using Empiria.History.Services;

using Empiria.StateEnums;

using Empiria.Inventory.Assets.Data;

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Provides data mapping services for Asset instances.</summary>
  static internal class AssetMapper {

    static internal AssetHolderDto Map(Asset asset) {
      return new AssetHolderDto {
        Asset = MapAsset(asset),
        Transactions = AssetTransactionMapper.Map(AssetsData.GetTransactions(asset)),
        Documents = DocumentServices.GetEntityDocuments(asset),
        History = HistoryServices.GetEntityHistory(asset),
        Actions = MapActions()
      };
    }


    static internal AssetDto MapAsset(Asset asset) {
      return new AssetDto {
        UID = asset.UID,
        AssetNo = asset.AssetNo,
        AssetType = asset.AssetType.MapToNamedEntity(),
        Name = asset.Name,
        Description = asset.Description,
        Brand = asset.Brand,
        Model = asset.Model,
        Year = asset.Year,
        AssignedTo = asset.AssignedTo.MapToNamedEntity(),
        AssignedToOrgUnit = asset.AssignedToOrgUnit.MapToNamedEntity(),
        Manager = asset.Manager.MapToNamedEntity(),
        ManagerOrgUnit = asset.ManagerOrgUnit.MapToNamedEntity(),
        Building = asset.Building.MapToNamedEntity(),
        Floor = asset.Floor.MapToNamedEntity(),
        Place = asset.Place.MapToNamedEntity(),
        LocationName = asset.Location.FullName,
        StartDate = asset.StartDate,
        EndDate = asset.EndDate,
        Status = asset.Status.MapToDto()
      };
    }


    static internal FixedList<AssetDescriptor> Map(FixedList<Asset> assets) {
      return assets.Select(asset => MapToDescriptor(asset))
                   .ToFixedList();
    }

    #region Helpers

    static private BaseActions MapActions() {
      return new BaseActions {
        CanEditDocuments = true
      };
    }


    static private AssetDescriptor MapToDescriptor(Asset asset) {
      return new AssetDescriptor {
        UID = asset.UID,
        AssetNo = asset.AssetNo,
        AssetTypeName = asset.AssetType.Name,
        Name = asset.Name,
        Description = asset.Description,
        LocationName = asset.Location.FullName,
        Condition = asset.Condition,
        AssignedToName = asset.AssignedTo.FullName,
        AssignedToOrgUnitName = asset.AssignedToOrgUnit.FullName,
        StartDate = asset.StartDate,
        EndDate = asset.EndDate,
        StatusName = asset.Status.GetName()
      };
    }

    #endregion Helpers

  }  // class AssetMapper

}  // namespace Empiria.Inventory.Assets.Adapters
