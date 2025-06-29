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

     static internal FixedList<AssetAssignationDescriptor> Map(FixedList<AssetAssignation> assignations) {
      return assignations.Select(x => MapToDescriptor(x))
                         .ToFixedList();
    }

    #region Helpers

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
