/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : AssetTransaction                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an assets transaction type.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Locations;

using Empiria.Inventory.Assets.Data;

namespace Empiria.Inventory.Assets {

  /// <summary>Represents an assets transaction type.</summary>
  [PartitionedType(typeof(AssetTransactionType))]
  public class AssetTransaction : BaseObject, INamedEntity {

    #region Fields

    private Lazy<List<AssetTransactionEntry>> _entries = new Lazy<List<AssetTransactionEntry>>();

    #endregion Fields

    #region Constructors and parsers

    internal protected AssetTransaction(AssetTransactionType transactionType) : base(transactionType) {
      // Required by Empiria Framework for all partitioned types.
    }

    static internal AssetTransaction Parse(int id) => ParseId<AssetTransaction>(id);

    static public AssetTransaction Parse(string uid) => ParseKey<AssetTransaction>(uid);

    static internal FixedList<AssetTransaction> GetList() {
      return GetList<AssetTransaction>().ToFixedList();
    }

    static internal AssetTransaction Empty => ParseEmpty<AssetTransaction>();

    protected override void OnLoad() {
      Reload();
    }

    #endregion Constructors and parsers

    #region Properties

    public AssetTransactionType AssetTransactionType {
      get {
        return (AssetTransactionType) base.GetEmpiriaType();
      }
    }


    [DataField("ASSET_TXN_NO")]
    public string TransactionNo {
      get; private set;
    }


    [DataField("ASSET_TXN_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    string INamedEntity.Name {
      get {
        return $"{TransactionNo}-{AssetTransactionType.DisplayName}";
      }
    }


    [DataField("ASSET_TXN_IDENTIFICATORS")]
    private string _identificators = string.Empty;

    public FixedList<string> Identificators {
      get {
        return EmpiriaString.Tagging(_identificators);
      }
    }


    [DataField("ASSET_TXN_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return EmpiriaString.Tagging(_tags);
      }
    }


    [DataField("ASSET_TXN_MGR_ID")]
    public Person Manager {
      get; private set;
    }


    [DataField("ASSET_TXN_MGR_ORG_UNIT_ID")]
    public OrganizationalUnit ManagerOrgUnit {
      get; private set;
    }


    [DataField("ASSET_TXN_RELEASED_BY_ID")]
    public Person ReleasedBy {
      get; private set;
    }


    [DataField("ASSET_TXN_RELEASED_BY_ORG_UNIT_ID")]
    public OrganizationalUnit ReleasedByOrgUnit {
      get; private set;
    }


    [DataField("ASSET_TXN_ASSIGNED_TO_ID")]
    public Person AssignedTo {
      get; private set;
    }


    [DataField("ASSET_TXN_ASSIGNED_TO_ORG_UNIT_ID")]
    public OrganizationalUnit AssignedToOrgUnit {
      get; private set;
    }


    [DataField("ASSET_TXN_BASE_LOCATION_ID")]
    public Location BaseLocation {
      get; private set;
    }


    public Location Building {
      get {
        return BaseLocation.SeekTree(LocationType.Building);
      }
    }


    public Location Floor {
      get {
        return BaseLocation.SeekTree(LocationType.Floor);
      }
    }


    public Location Place {
      get {
        return BaseLocation.SeekTree(LocationType.Place);
      }
    }


    [DataField("ASSET_TXN_SOURCE_ID")]
    public OperationSource OperationSource {
      get; private set;
    }


    [DataField("ASSET_TXN_EXT_DATA")]
    protected JsonObject ExtData {
      get; private set;
    }


    [DataField("ASSET_TXN_REQUESTED_TIME")]
    public DateTime RequestedTime {
      get; private set;
    }


    [DataField("ASSET_TXN_REQUESTED_BY_ID")]
    public Party RequestedBy {
      get; private set;
    }


    [DataField("ASSET_TXN_APPLICATION_DATE")]
    public DateTime ApplicationDate {
      get; private set;
    }


    [DataField("ASSET_TXN_APPLIED_BY_ID")]
    public Party AppliedBy {
      get; private set;
    }


    [DataField("ASSET_TXN_RECORDING_DATE")]
    public DateTime RecordingDate {
      get; private set;
    }


    [DataField("ASSET_TXN_RECORDED_BY_ID")]
    public Party RecordedBy {
      get; private set;
    }

    [DataField("ASSET_TXN_AUTHORIZATION_TIME")]
    public DateTime AuthorizationTime {
      get; private set;
    }


    [DataField("ASSET_TXN_AUTHORIZED_BY_ID")]
    public Party AuthorizedBy {
      get; private set;
    }


    [DataField("ASSET_TXN_POSTING_TIME")]
    public DateTime PostingTime {
      get;
      private set;
    }


    [DataField("ASSET_TXN_POSTED_BY_ID")]
    public Party PostedBy {
      get;
      private set;
    }


    [DataField("ASSET_TXN_STATUS", Default = TransactionStatus.Pending)]
    public TransactionStatus Status {
      get;
      private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(TransactionNo, Description, _identificators, _tags,
                                           AssetTransactionType.DisplayName,
                                           AssignedToOrgUnit.Keywords, AssignedTo.Keywords,
                                           BaseLocation.Keywords, RequestedBy.Keywords, ReleasedByOrgUnit.Keywords,
                                           Manager.Keywords, ManagerOrgUnit.Keywords, OperationSource.Keywords);
      }
    }


    public FixedList<AssetTransactionEntry> Entries {
      get {
        return _entries.Value.ToFixedList();
      }
    }

    #endregion Properties

    #region Methods

    internal bool CanClose() {
      return Status == TransactionStatus.Pending &&
             Entries.Count > 0 &&
             !AssignedTo.IsEmptyInstance &&
             !AssignedToOrgUnit.IsEmptyInstance &&
             !Manager.IsEmptyInstance &&
             !ManagerOrgUnit.IsEmptyInstance &&
             ApplicationDate != ExecutionServer.DateMaxValue;
    }


    internal bool CanEdit() {
      return this.Status == TransactionStatus.Pending;
    }


    internal bool CanOpen() {
      return this.Status == TransactionStatus.Closed;
    }


    internal AssetTransaction Clone(AssetTransactionType transactionType) {
      Assertion.Require(transactionType, nameof(transactionType));
      Assertion.Require(!this.IsEmptyInstance, "Can not clone the empty instance.");
      Assertion.Require(this.CanOpen(),
                        $"Can not clone transaction {this.UID}. Its status is {this.Status.GetName()}.");

      var transaction = new AssetTransaction(transactionType);

      transaction.Description = transactionType.DisplayName;
      transaction.Manager = Manager;
      transaction.ManagerOrgUnit = ManagerOrgUnit;
      transaction.BaseLocation = BaseLocation;

      foreach (var entry in Entries) {
        var fields = new AssetTransactionEntryFields {
          EntryTypeUID = transactionType.DefaultAssetTransactionEntryType.UID,
          AssetUID = entry.Asset.UID,
        };

        transaction.AppendEntry(fields);
      }

      return transaction;
    }


    internal void Close() {
      Assertion.Require(this.CanClose(),
                        $"Transaction can not be closed. Its status is {Status.GetName()}, " +
                        $"Entries = {Entries.Count}.");

      AppliedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      Status = TransactionStatus.Closed;
      TransactionNo = AssetsTransactionsData.GenerateNextTransactionNo(this);
      if (OperationSource.IsEmptyInstance) {
        OperationSource = OperationSource.Default;
      }
    }


    internal void Delete() {
      Assertion.Require(this.Status == TransactionStatus.Pending,
                        $"Transaction can not be deleted. Its status is {Status.GetName()}.");

      this.Status = TransactionStatus.Deleted;
    }


    internal FixedList<Asset> GetAssets() {
      return Entries.Select(x => x.Asset)
                    .ToFixedList();
    }


    protected override void OnSave() {
      if (IsNew) {
        TransactionNo = "No determinada";
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      RecordingDate = DateTime.Now;
      RecordedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);

      AssetsTransactionsData.WriteAssetTransaction(this, this.ExtData.ToString());

      foreach (AssetTransactionEntry entry in Entries) {
        entry.Save();
      }
    }


    private void Reload() {
      _entries = new Lazy<List<AssetTransactionEntry>>(() => AssetsTransactionsData.GetTransactionEntries(this));
    }


    internal void Update(AssetTransactionFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Description = Patcher.PatchClean(fields.Description, Description);
      Manager = Patcher.Patch(fields.ManagerUID, Manager);
      ManagerOrgUnit = Patcher.Patch(fields.ManagerOrgUnitUID, ManagerOrgUnit);
      AssignedTo = Patcher.Patch(fields.AssignedToUID, AssignedTo);
      AssignedToOrgUnit = Patcher.Patch(fields.AssignedToOrgUnitUID, AssignedToOrgUnit);
      BaseLocation = Patcher.Patch(fields.LocationUID, BaseLocation);
      RequestedTime = Patcher.Patch(fields.RequestedTime, RequestedTime);
      ApplicationDate = Patcher.Patch(fields.ApplicationDate, ApplicationDate);

      _identificators = EmpiriaString.Tagging(fields.Identificators);
      _tags = EmpiriaString.Tagging(fields.Tags);
    }

    #endregion Methods

    #region Entries Methods

    internal AssetTransactionEntry AppendEntry(AssetTransactionEntryFields fields) {
      Assertion.Require(fields, nameof(fields));
      Assertion.Require(CanEdit(), $"Asset transaction {this.UID} can not be edited.");

      fields.EnsureValid();

      var entryType = AssetTransactionEntryType.Parse(fields.EntryTypeUID);
      var asset = Asset.Parse(fields.AssetUID);

      AssetTransactionEntry entry = new AssetTransactionEntry(entryType, this, asset);

      entry.Update(fields);

      _entries.Value.Add(entry);

      return entry;
    }


    internal AssetTransactionEntry GetEntry(string transactionEntryUID) {
      Assertion.Require(transactionEntryUID, nameof(transactionEntryUID));

      var entry = Entries.Find(x => x.UID == transactionEntryUID);

      Assertion.Require(entry, $"Transaction entry {transactionEntryUID} not found.");

      return entry;
    }


    internal void RemoveEntry(AssetTransactionEntry entry) {
      Assertion.Require(entry, nameof(entry));
      Assertion.Require(CanEdit(), $"Asset transaction {this.UID} can not be edited.");

      _ = GetEntry(entry.UID);

      entry.Delete();

      _entries.Value.Remove(entry);
    }


    internal void UpdateEntry(AssetTransactionEntry entry,
                              AssetTransactionEntryFields fields) {
      Assertion.Require(entry, nameof(entry));
      Assertion.Require(fields, nameof(fields));
      Assertion.Require(CanEdit(), $"Asset transaction {this.UID} can not be edited.");

      fields.EnsureValid();

      _ = GetEntry(entry.UID);

      entry.Update(fields);
    }


    #endregion Entries Methods

  }  // class AssetTransaction

}  // namespace Empiria.Inventory.Assets
