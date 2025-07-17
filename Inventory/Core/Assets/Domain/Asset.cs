/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Information Holder                      *
*  Type     : Asset                                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an asset.                                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Locations;

using Empiria.Inventory.Assets.Data;

namespace Empiria.Inventory.Assets {

  /// <summary>Represents an asset.</summary>
  public class Asset : BaseObject, INamedEntity {

    #region Constructors and parsers

    public Asset() {
      // Required by Empiria Framework.
    }

    static internal Asset Parse(int id) => ParseId<Asset>(id);

    static public Asset Parse(string uid) => ParseKey<Asset>(uid);

    static public Asset Empty => BaseObject.ParseEmpty<Asset>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("ASSET_TYPE_ID")]
    public AssetType AssetType {
      get; private set;
    }


    [DataField("ASSET_NO")]
    public string AssetNo {
      get; private set;
    }


    [DataField("ASSET_SKU_ID")]
    public ProductSku Sku {
      get; private set;
    }


    public string Name {
      get {
        return Sku.Name;
      }
    }


    [DataField("ASSET_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    public string Brand {
      get {
        return Sku.Brand;
      }
    }


    public string Model {
      get {
        return Sku.Model;
      }
    }


    public int Year {
      get {
        return Sku.Year;
      }
    }


    [DataField("ASSET_IDENTIFICATORS")]
    private string _identificators = string.Empty;


    public FixedList<string> Identificators {
      get {
        return EmpiriaString.Tagging(_identificators);
      }
    }


    [DataField("ASSET_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return EmpiriaString.Tagging(_tags);
      }
    }


    [DataField("ASSET_MGR_ID")]
    public Person Manager {
      get; private set;
    }


    [DataField("ASSET_MGR_ORG_UNIT_ID")]
    public OrganizationalUnit ManagerOrgUnit {
      get; private set;
    }


    public Person AssignedTo {
      get {
        return LastAssignment.AssignedTo;
      }
    }


    public OrganizationalUnit AssignedToOrgUnit {
      get {
        return LastAssignment.AssignedToOrgUnit;
      }
    }


    [DataField("ASSET_LAST_ASGMT_TXN_ID")]
    public AssetTransaction LastAssignment {
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


    [DataField("ASSET_CONDITION")]
    public string CurrentCondition {
      get; private set;
    }


    [DataField("ASSET_PREVIOUS_CONDITION")]
    public string PreviousCondition {
      get; private set;
    }


    [DataField("ASSET_START_DATE")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("ASSET_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    [DataField("ASSET_LAST_UPDATE")]
    public DateTime LastUpdate {
      get; private set;
    }


    [DataField("ASSET_ACCOUNTING_DATA")]
    protected JsonObject AccountingData {
      get; private set;
    }


    [DataField("ASSET_EXT_DATA")]
    protected JsonObject ExtData {
      get; private set;
    }


    [DataField("ASSET_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("ASSET_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("ASSET_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(AssetType.Name, Sku.Keywords, Description, Year.ToString(),
                                           _identificators, _tags, this.CurrentCondition, this.Location.Keywords,
                                           LastAssignment.Keywords);
      }
    }

    #endregion Properties

    #region Methods

    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      AssetsData.WriteAsset(this, this.AccountingData.ToString(), this.ExtData.ToString());
    }

    #endregion Methods

  }  // class Asset

}  // namespace Empiria.Inventory.Assets
