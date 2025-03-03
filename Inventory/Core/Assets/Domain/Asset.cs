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
using Empiria.Locations;
using Empiria.Parties;
using Empiria.Products;
using Empiria.StateEnums;

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

    [DataField("FXD_ASST_TYPE_ID")]
    public AssetType AssetType {
      get; private set;
    }


    [DataField("FXD_ASST_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }


    [DataField("FXD_ASST_NAME")]
    private string _name;

    public string Name {
      get {
        return _name.Length == 0 ? Product.Name : _name;
      }
      private set {
        _name = EmpiriaString.Clean(value);
      }
    }


    [DataField("FXD_ASST_INVENTORY_NO")]
    public string InventoryNo {
      get; private set;
    }


    [DataField("FXD_ASST_LABEL")]
    public string Label {
      get; private set;
    }


    [DataField("FXD_ASST_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    public string Model  {
      get {
        return ExtData.Get("model", "N/D");
      }
      private set {
        ExtData.SetIfValue("model", EmpiriaString.Clean(value));
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


    [DataField("FXD_ASST_BRAND")]
    public string Brand {
      get; private set;
    }


    [DataField("FXD_ASST_IDENTIFICATORS")]
    private string _identificators = string.Empty;


    public FixedList<string> Identificators {
      get {
        return _identificators.Split(' ').ToFixedList();
      }
    }


    [DataField("FXD_ASST_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return _tags.Split(' ').ToFixedList();
      }
    }


    [DataField("FXD_ASST_CUSTODIAN_ORG_UNIT_ID")]
    public OrganizationalUnit CustodianOrgUnit {
      get; private set;
    }


    [DataField("FXD_ASST_CUSTODIAN_PERSON_ID")]
    public Person CustodianPerson {
      get; private set;
    }


    [DataField("FXD_ASST_LOCATION_ID")]
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


    [DataField("FXD_ASST_CONDITION")]
    public string Condition {
      get; private set;
    }


    [DataField("FXD_ASST_START_DATE")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("FXD_ASST_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    [DataField("FXD_ASST_LAST_UPDATE")]
    public DateTime LastUpdate {
      get; private set;
    }


    [DataField("FXD_ASST_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    [DataField("FXD_ASST_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("FXD_ASST_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("FXD_ASST_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(InventoryNo, Name, Description, Label,
                                           Brand, Model, Year.ToString(), AssetType.Name,
                                           _identificators, _tags, this.Condition, this.Location.FullName,
                                           this.CustodianOrgUnit.Keywords, this.CustodianPerson.Keywords);
      }
    }

    #endregion Properties

  }  // class Asset

}  // namespace Empiria.Inventory.Assets
