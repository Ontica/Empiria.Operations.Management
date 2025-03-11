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
using Empiria.Inventory.Assets.Adapters;

namespace Empiria.Inventory.Assets {

  /// <summary>Represents an assets transaction type.</summary>
  [PartitionedType(typeof(AssetTransactionType))]
  public class AssetTransaction : BaseObject {

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


    [DataField("ASSET_TXN_IDENTIFICATORS")]
    private string _identificators = string.Empty;


    public FixedList<string> Identificators {
      get {
        return _identificators.Split(' ').ToFixedList();
      }
    }


    [DataField("ASSET_TXN_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return _tags.Split(' ').ToFixedList();
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


    [DataField("ASSET_TXN_ASSIGNED_TO_ID")]
    public Person AssignedTo {
      get; private set;
    }


    [DataField("ASSET_TXN_ASSIGNED_TO_ORG_UNIT_ID")]
    public OrganizationalUnit AssignedToOrgUnit {
      get; private set;
    }


    [DataField("ASSET_TXN_LOCATION_ID")]
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


    [DataField("ASSET_TXN_SOURCE_ID")]
    public OperationSource OperationSource {
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


    [DataField("ASSET_TXN_APPLICATION_TIME")]
    public DateTime ApplicationTime {
      get; private set;
    }


    [DataField("ASSET_TXN_APPLIED_BY_ID")]
    public Party AppliedBy {
      get; private set;
    }


    [DataField("ASSET_TXN_RECORDING_TIME")]
    public DateTime RecordingTime {
      get; private set;
    }


    [DataField("ASSET_TXN_RECORDED_BY_ID")]
    public Party RecordedBy {
      get; private set;
    }


    [DataField("ASSET_TXN_EXT_DATA")]
    protected JsonObject ExtData {
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
                                           AssetTransactionType.DisplayName, Manager.Keywords,
                                           ManagerOrgUnit.Keywords, AssignedTo.Keywords,
                                           AssignedToOrgUnit.Keywords, Location.Keywords,
                                           OperationSource.Keywords);
      }
    }


    public FixedList<AssetTransactionEntry> Entries {
      get {
        return _entries.Value.ToFixedList();
      }
    }

    #endregion Properties

    #region Methods

    internal void Delete() {

    }

    internal FixedList<Asset> GetAssets() {
      return Entries.Select(x => x.Asset)
                    .ToFixedList();
    }


    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      AssetsTransactionsData.WriteAssetTransaction(this, this.ExtData.ToString());
    }


    internal void Reload() {
      _entries = new Lazy<List<AssetTransactionEntry>>(() => AssetsTransactionsData.GetTransactionEntries(this));
    }


    internal void Update(AssetTransactionFields fields) {

    }

    #endregion Methods

  }  // class AssetTransaction

}  // namespace Empiria.Inventory.Assets
