/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Mapper                                  *
*  Type     : AssetAssignmentMapper                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for AssetAssignment instances.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Provides data mapping services for AssetAssignment instances.</summary>
  static internal class AssetAssignmentMapper {

    static internal AssetAssignmentHolder Map(AssetAssignment assignment) {
      return new AssetAssignmentHolder {
        Assignment = MapAssignment(assignment),
        Assets = AssetMapper.Map(assignment.GetAssets())
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
        Building = assignment.Building.MapToNamedEntity(),
        Floor = assignment.Floor.MapToNamedEntity(),
        Place = assignment.Place.MapToNamedEntity(),
        LocationName = assignment.Location.FullName
      };
    }


    static private AssetAssignmentDescriptor MapToDescriptor(AssetAssignment assignment) {
      return new AssetAssignmentDescriptor {
        UID = assignment.UID,
        LocationName = assignment.Location.FullName,
        AssignedToName = assignment.AssignedTo.FullName,
        AssignedToOrgUnitName = assignment.AssignedToOrgUnit.FullName
      };
    }

    #endregion Helpers

  }  // class AssetAssignmentMapper

}  // namespace Empiria.Inventory.Assets.Adapters
