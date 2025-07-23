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
    internal int SkuId {
      get; private set;
    }


    [DataField("ASSET_DESCRIPTION")]
    public string Description {
      get; private set;
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


    [DataField("ASSET_CURRENT_CONDITION")]
    public string CurrentCondition {
      get; private set;
    }


    [DataField("ASSET_CURRENT_LOCATION_ID")]
    public Location CurrentLocation {
      get; private set;
    }


    [DataField("ASSET_IN_USE", Default = ThreeStateValue.False)]
    public ThreeStateValue InUse {
      get; private set;
    }


    public Location Building {
      get {
        return CurrentLocation.SeekTree(LocationType.Building);
      }
    }


    public Location Floor {
      get {
        return CurrentLocation.SeekTree(LocationType.Floor);
      }
    }


    public Location Place {
      get {
        return CurrentLocation.SeekTree(LocationType.Place);
      }
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
    private JsonObject AccountingData {
      get; set;
    }


    public string InvoiceNo {
      get {
        return AccountingData.Get("invoiceNo", string.Empty);
      }
      private set {
        AccountingData.SetIfValue("invoiceNo", value);
      }
    }


    public string SupplierName {
      get {
        return AccountingData.Get("supplierName", string.Empty);
      }
      private set {
        AccountingData.SetIfValue("supplierName", value);
      }
    }


    public string AccountingTag {
      get {
        return AccountingData.Get("accountingTag", string.Empty);
      }
      private set {
        AccountingData.SetIfValue("accountingTag", value);
      }
    }


    public decimal HistoricalValue {
      get {
        return AccountingData.Get("historicalValue", 0m);
      }
      private set {
        AccountingData.SetIf("historicalValue", value, value > 0m);
      }
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


    [DataField("ASSET_LAST_ASGMT_TXN_ENTRY_ID")]
    internal int LastAssignmentEntryId {
      get; private set;
    }


    [DataField("LAST_ASGMT_ASSIGNED_TO_ID")]
    public Person AssignedTo {
      get; private set;
    }


    [DataField("LAST_ASGMT_ASSIGNED_TO_ORG_UNIT_ID")]
    public OrganizationalUnit AssignedToOrgUnit {
      get; private set;
    }



    [DataField("LAST_ASGMT_TXN_ID")]
    private int LastAssignmentTransactionId {
      get; set;
    }

    [DataField("LAST_ASGMT_TXN_NO")]
    public string LastAssignmentTransactionNo {
      get; private set;
    }


    [DataField("LAST_ASGMT_TXN_DATE")]
    public DateTime LastAssignmentDate {
      get; private set;
    }


    [DataField("LAST_ASGMT_LOCATION_ID")]
    public Location LastAssignmentLocation {
      get; private set;
    }


    [DataField("LAST_ASGMT_CONDITION")]
    public string LastAssignmentCondition {
      get; private set;
    }


    [DataField("LAST_ASGMT_PREVIOUS_CONDITION")]
    public string LastAssignmentPreviousCondition {
      get; private set;
    }


    [DataField("SKU_NAME")]
    public string Name {
      get; private set;
    }


    [DataField("SKU_BRAND")]
    public string Brand {
      get; private set;
    }


    [DataField("SKU_MODEL")]
    public string Model {
      get; private set;
    }


    [DataField("SKU_SERIAL_NO")]
    public string SerialNo {
      get; private set;
    }


    [DataField("SKU_ACQUISITION_DATE")]
    public DateTime AcquisitionDate {
      get; private set;
    }

    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(AssetNo, AssetType.Name, Description,
                                           _identificators, _tags, GetSku().Keywords,
                                           CurrentCondition, CurrentLocation.Keywords);
      }
    }

    #endregion Properties

    #region Methods

    public AssetTransaction GetLastAssignment() {
      return AssetTransaction.Parse(LastAssignmentTransactionId);
    }

    public AssetTransactionEntry GetLastAssignmentEntry() {
      return AssetTransactionEntry.Parse(LastAssignmentEntryId);
    }

    public ProductSku GetSku() {
      return ProductSku.Parse(SkuId);
    }

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
