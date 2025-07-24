/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Mapper                                  *
*  Type     : AssetAssignmentMapper                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for AssetAssignment instances.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Inventory.Assets.Data;

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Provides data mapping services for AssetAssignment instances.</summary>
  static internal class AssetAssignmentMapper {

    static internal AssetAssignmentHolder Map(AssetAssignment assignment) {
      FixedList<AssetTransaction> txns = AssetsAssignmentsData.GetTransactionsFor(assignment.AssignedTo);

      return new AssetAssignmentHolder {
        Assignment = MapAssignment(assignment),
        Entries = AssetTransactionMapper.Map(assignment.Transaction.Entries),
        Transactions = AssetTransactionMapper.Map(txns),
        Actions = AssetTransactionMapper.MapActions(assignment.Transaction.Rules)
      };
    }


    static internal FixedList<AssetAssignmentDescriptor> Map(FixedList<AssetAssignment> assignments) {
      return assignments.Select(x => MapToDescriptor(x))
                         .ToFixedList();
    }

    #region Helpers

    static private AssetAssignmentDto MapAssignment(AssetAssignment assignment) {
      return new AssetAssignmentDto {
        UID = assignment.UID,
        AssignedTo = assignment.AssignedTo.MapToNamedEntity(),
        AssignedToOrgUnit = assignment.AssignedToOrgUnit.MapToNamedEntity(),
        ReleasedBy = assignment.ReleasedBy.MapToNamedEntity(),
        ReleasedByOrgUnit = assignment.ReleasedByOrgUnit.MapToNamedEntity(),
        Building = assignment.Building.MapToNamedEntity(),
        Floor = assignment.Floor.MapToNamedEntity(),
        Place = assignment.Place.MapToNamedEntity(),
        LocationName = assignment.Location.FullName,
        LastAssignmentTransaction = AssetTransactionMapper.MapAssetTransaction(assignment.Transaction),
      };
    }


    static private AssetAssignmentDescriptor MapToDescriptor(AssetAssignment assignment) {
      return new AssetAssignmentDescriptor {
        UID = assignment.UID,
        AssignedToName = assignment.AssignedTo.FullName,
        AssignedToOrgUnitName = assignment.AssignedToOrgUnit.FullName,
        ReleasedByName = assignment.ReleasedBy.Name,
        ReleasedByOrgUnitName = assignment.ReleasedByOrgUnit.Name,
        LocationName = assignment.Location.FullName,
        LastAssignmentTransactionUID = assignment.Transaction.UID,
        LastAssignmentTransactionNo = assignment.Transaction.TransactionNo,
        LastAssignmentApplicationDate = assignment.Transaction.ApplicationDate
      };
    }

    #endregion Helpers

  }  // class AssetAssignmentMapper

}  // namespace Empiria.Inventory.Assets.Adapters
