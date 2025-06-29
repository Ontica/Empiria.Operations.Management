/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                                 Component : Adapters Layer                   *
*  Assembly : Empiria.Inventory.Core.dll                        Pattern   : Data Transfer Object             *
*  Type     : AssetAssignationDto, AssetAssignationHolderDto    License   : Please read LICENSE.txt file     *
*                                                                                                            *
*  Summary  : Data transfer object used to return assets assignations data.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Output Dto used to return minimal data for an asset assignation.</summary>
  public class AssetAssignationDescriptor {

    public string UID {
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

  } // class AssetAssignationDescriptor

}  // namespace Empiria.Inventory.Assets.Adapters
