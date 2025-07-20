/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : AssetTransactionEntry                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an asset transaction entry.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Locations;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Inventory.Assets.Data;

namespace Empiria.Inventory.Assets {

  /// <summary>Represents an asset transaction entry.</summary>
  [PartitionedType(typeof(AssetTransactionEntryType))]
  public class AssetTransactionEntry : BaseObject, INamedEntity {

    #region Constructors and parsers

    protected AssetTransactionEntry(AssetTransactionEntryType entryType) : base(entryType) {
      // Required by Empiria Framework for all partitioned types.
    }

    internal AssetTransactionEntry(AssetTransactionEntryType entryType,
                                   AssetTransaction transaction,
                                   Asset asset) : base(entryType) {
      Assertion.Require(transaction, nameof(transaction));
      Assertion.Require(asset, nameof(asset));

      this.Transaction = transaction;
      this.Asset = asset;
    }

    static public AssetTransactionEntry Parse(int id) => ParseId<AssetTransactionEntry>(id);

    static public AssetTransactionEntry Parse(string uid) => ParseKey<AssetTransactionEntry>(uid);

    static public AssetTransactionEntry Empty => ParseEmpty<AssetTransactionEntry>();

    #endregion Constructors and parsers

    #region Properties

    public AssetTransactionEntryType AssetTransactionEntryType {
      get {
        return (AssetTransactionEntryType) base.GetEmpiriaType();
      }
    }


    [DataField("ASSET_ENTRY_TXN_ID")]
    public AssetTransaction Transaction {
      get; private set;
    }


    [DataField("ASSET_ENTRY_ASSET_ID")]
    public Asset Asset {
      get; private set;
    }


    [DataField("ASSET_ENTRY_DESCRIPTION")]
    public string Description {
      get; private set;
    }

    string INamedEntity.Name => Description;


    [DataField("ASSET_ENTRY_LOCATION_ID")]
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

    [DataField("ASSET_ENTRY_PREVIOUS_CONDITION")]
    public string PreviousCondition {
      get; private set;
    }


    [DataField("ASSET_ENTRY_CONDITION")]
    public string Condition {
      get; private set;
    }


    [DataField("ASSET_ENTRY_OPERATION_ID")]
    internal int OperationId {
      get; private set;
    }


    [DataField("ASSET_ENTRY_OPERATION_DATA")]
    protected JsonObject OperationData {
      get; private set;
    }


    [DataField("ASSET_ENTRY_EXT_DATA")]
    protected JsonObject ExtData {
      get; private set;
    }


    [DataField("ASSET_ENTRY_POSITION")]
    internal int Position {
      get; private set;
    }


    [DataField("ASSET_ENTRY_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("ASSET_ENTRY_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("ASSET_ENTRY_STATUS", Default = TransactionStatus.Pending)]
    public TransactionStatus Status {
      get; private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Description, Asset.Keywords, Transaction.Keywords);
      }
    }

    #endregion Properties

    #region Methods

    internal void Delete() {
      this.Status = TransactionStatus.Deleted;
      MarkAsDirty();
    }


    protected override void OnSave() {
      if (!IsDirty) {
        return;
      }
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      AssetsTransactionsData.WriteAssetTransactionEntry(this, this.OperationData.ToString(),
                                                        this.ExtData.ToString());
    }


    internal void Update(AssetTransactionEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Asset = Patcher.Patch(fields.AssetUID, Asset);
      Description = Patcher.PatchClean(fields.Description, Description);

      MarkAsDirty();
    }

    #endregion Methods

  }  // class AssetTransactionEntry

}  // namespace Empiria.Inventory.Assets
