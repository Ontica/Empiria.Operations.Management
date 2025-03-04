﻿/* Empiria Operations ****************************************************************************************
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


    public string AssetNo {
      get {
        return Sku.SkuNo;
      }
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
        return ExtData.Get("year", 0);
      }
      private set {
        ExtData.SetIfValue("year", value);
      }
    }


    [DataField("ASSET_IDENTIFICATORS")]
    private string _identificators = string.Empty;


    public FixedList<string> Identificators {
      get {
        return _identificators.Split(' ').ToFixedList();
      }
    }


    [DataField("ASSET_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return _tags.Split(' ').ToFixedList();
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


    [DataField("ASSET_CONDITION")]
    public string Condition {
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


    [DataField("ASSET_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
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
                                           _identificators, _tags, this.Condition, this.Location.FullName,
                                           this.AssignedTo.Keywords, this.AssignedToOrgUnit.Keywords,
                                           this.Manager.Keywords, this.ManagerOrgUnit.Keywords);
      }
    }

    #endregion Properties

  }  // class Asset

}  // namespace Empiria.Inventory.Assets
