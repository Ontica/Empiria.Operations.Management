/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Information Holder                      *
*  Type     : AssetAssignation                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an asset assignation.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Inventory.Assets.Data;
using Empiria.Locations;
using Empiria.Parties;

namespace Empiria.Inventory.Assets {

  /// <summary>Represents an asset assignation.</summary>
  public class AssetAssignation {

    #region Constructors and parsers

    internal static AssetAssignation Parse(string assignationUID) {
      Assertion.Require(assignationUID, nameof(assignationUID));

      string[] parts = assignationUID.Split('|');

      Assertion.Require(parts.Length == 3, "Unrecognized asset assignation UID.");

      return new AssetAssignation {
         AssignedTo = Person.Parse(int.Parse(parts[0])),
         AssignedToOrgUnit = OrganizationalUnit.Parse(int.Parse(parts[1])),
         Location = Location.Parse(int.Parse(parts[2]))
      };
    }

    #endregion Constructors and parsers

    #region Properties

    public string UID {
      get {
        return $"{AssignedTo.Id}|{AssignedToOrgUnit.Id}|{Location.Id}";
      }
    }


    [DataField("ASSET_ASSIGNED_TO_ID")]
    public Person AssignedTo {
      get; private set;
    }


    [DataField("ASSET_ASSIGNED_TO_ORG_UNIT_ID")]
    public OrganizationalUnit AssignedToOrgUnit {
      get; private set;
    }


    [DataField("ASSET_LOCATION_ID")]
    public Location Location {
      get; private set;
    }


    public Location Building {
      get {
        return Location.SeekTree(LocationType.Building);
      }
    }


    public Location Floor {
      get {
        return Location.SeekTree(LocationType.Floor);
      }
    }


    public Location Place {
      get {
        return Location.SeekTree(LocationType.Place);
      }
    }

    #endregion Properties

    #region Methods

    internal FixedList<Asset> GetAssets() {
      return AssetsAssignationsData.GetAssets(this);
    }

    #endregion Methods

  }  // class AssetAssignation

}  // namespace Empiria.Inventory.Assets
