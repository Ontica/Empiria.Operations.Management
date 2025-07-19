/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Information Holder                      *
*  Type     : AssetAssignment                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an asset assignment.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Locations;
using Empiria.Parties;

using Empiria.Inventory.Assets.Data;

namespace Empiria.Inventory.Assets {

  /// <summary>Represents an asset assignment.</summary>
  public class AssetAssignment {

    #region Constructors and parsers

    internal static AssetAssignment Parse(string assignmentUID) {
      Assertion.Require(assignmentUID, nameof(assignmentUID));

      string[] parts = assignmentUID.Split('|');

      Assertion.Require(parts.Length == 2, "Unrecognized asset assignment UID.");

      return new AssetAssignment {
         Transaction = AssetTransaction.Parse(parts[0]),
         Location = Location.Parse(parts[1])
      };
    }

    #endregion Constructors and parsers

    #region Properties

    public string UID {
      get {
        return $"{Transaction.UID}|{Location.UID}";
      }
    }


    [DataField("LAST_ASGMT_TXN_ID")]
    public AssetTransaction Transaction {
      get; private set;
    }


    public Person AssignedTo {
      get {
        return Transaction.AssignedTo;
      }
    }


    public OrganizationalUnit AssignedToOrgUnit {
      get {
        return Transaction.AssignedToOrgUnit;
      }
    }

    public Person ReleasedBy {
      get {
        return Transaction.ReleasedBy;
      }
    }


    public OrganizationalUnit ReleasedByOrgUnit {
      get {
        return Transaction.ReleasedByOrgUnit;
      }
    }


    [DataField("LAST_ASGMT_LOCATION_ID")]
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
      return AssetsAssignmentsData.GetAssets(this);
    }

    #endregion Methods

  }  // class AssetAssignment

}  // namespace Empiria.Inventory.Assets
