/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Mapper                                  *
*  Type     : AssetAssignationMapper                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for AssetAssignation instances.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Provides data mapping services for AssetAssignation instances.</summary>
  static internal class AssetAssignationMapper {

    static internal AssetAssignationHolder Map(AssetAssignation assignation) {
      return new AssetAssignationHolder {
        Assignation = MapAssignation(assignation),
        Assets = AssetMapper.Map(assignation.GetAssets())
      };
    }


    static internal FixedList<AssetAssignationDescriptor> Map(FixedList<AssetAssignation> assignations) {
      return assignations.Select(x => MapToDescriptor(x))
                         .ToFixedList();
    }

    #region Helpers

    static private AssetAssignationDto MapAssignation(AssetAssignation assignation) {
      return new AssetAssignationDto {
        UID = assignation.UID,
        AssignedTo = assignation.AssignedTo.MapToNamedEntity(),
        AssignedToOrgUnit = assignation.AssignedToOrgUnit.MapToNamedEntity(),
        Building = assignation.Building.MapToNamedEntity(),
        Floor = assignation.Floor.MapToNamedEntity(),
        Place = assignation.Place.MapToNamedEntity(),
        LocationName = assignation.Location.FullName
      };
    }


    static private AssetAssignationDescriptor MapToDescriptor(AssetAssignation assignation) {
      return new AssetAssignationDescriptor {
        UID = assignation.UID,
        LocationName = assignation.Location.FullName,
        AssignedToName = assignation.AssignedTo.FullName,
        AssignedToOrgUnitName = assignation.AssignedToOrgUnit.FullName
      };
    }

    #endregion Helpers

  }  // class AssetAssignationMapper

}  // namespace Empiria.Inventory.Assets.Adapters
