/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Information Holder                      *
*  Type     : FixedAssetTransaction                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a fixed asset transaction.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Inventory.FixedAssets.Data;
using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Inventory.FixedAssets {

  /// <summary>Represents a fixed asset transaction type.</summary>
  public class FixedAssetTransaction : BaseObject {

    #region Fields

    private Lazy<List<FixedAssetTransactionEntry>> _entries = new Lazy<List<FixedAssetTransactionEntry>>();

    #endregion Fields

    #region Constructors and parsers

    static internal FixedAssetTransaction Parse(int id) => ParseId<FixedAssetTransaction>(id);

    static public FixedAssetTransaction Parse(string uid) => ParseKey<FixedAssetTransaction>(uid);

    static internal FixedList<FixedAssetTransaction> GetList() {
      return GetList<FixedAssetTransaction>().ToFixedList();
    }

    static internal FixedAssetTransaction Empty => ParseEmpty<FixedAssetTransaction>();

    protected override void OnLoad() {
      Reload();
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("OMS_TXN_TYPE_ID")]
    public FixedAssetTransactionType FixedAssetTransactionType {
      get;
      private set;
    }


    [DataField("OMS_TXN_SOURCE_ID")]
    public OperationSource OperationSource {
      get;
      private set;
    }


    [DataField("OMS_TXN_BASE_PARTY_ID")]
    public Party BaseParty {
      get;
      private set;
    }


    [DataField("OMS_TXN_NUMBER")]
    public string TransactionNo {
      get;
      private set;
    }


    [DataField("OMS_TXN_DESCRIPTION")]
    public string Description {
      get;
      private set;
    }


    [DataField("OMS_TXN_IDENTIFICATORS")]
    public string Identificators {
      get;
      private set;
    }


    [DataField("OMS_TXN_TAGS")]
    public string Tags {
      get;
      private set;
    }


    [DataField("OMS_TXN_BASE_ENTITY_TYPE_ID")]
    public int BaseEntityTypeId {
      get;
      private set;
    }


    [DataField("OMS_TXN_BASE_ENTITY_ID")]
    public int BaseEntityId {
      get;
      private set;
    }


    [DataField("OMS_TXN_APPLICATION_DATE")]
    public DateTime ApplicationDate {
      get;
      private set;
    }


    [DataField("OMS_TXN_APPLIED_BY_ID")]
    public Party AppliedBy {
      get;
      private set;
    }


    [DataField("OMS_TXN_RECORDING_DATE")]
    public DateTime RecordingDate {
      get;
      private set;
    }


    [DataField("OMS_TXN_RECORDED_BY_ID")]
    public Party RecordedBy {
      get;
      private set;
    }


    [DataField("OMS_TXN_AUTHORIZATION_TIME")]
    public DateTime AuthorizationTime {
      get;
      private set;
    }


    [DataField("OMS_TXN_AUTHORIZED_BY_ID")]
    public Party AuthorizedBy {
      get;
      private set;
    }


    [DataField("OMS_TXN_REQUESTED_TIME")]
    public DateTime RequestedTime {
      get;
      private set;
    }


    [DataField("OMS_TXN_REQUESTED_BY_ID")]
    public Party RequestedBy {
      get;
      private set;
    }


    [DataField("OMS_TXN_EXT_DATA")]
    internal protected JsonObject ExtensionData {
      get;
      private set;
    }


    [DataField("OMS_TXN_POSTING_TIME")]
    public DateTime PostingTime {
      get;
      private set;
    }


    [DataField("OMS_TXN_POSTED_BY_ID")]
    public Party PostedBy {
      get;
      private set;
    }


    [DataField("OMS_TXN_STATUS", Default = TransactionStatus.Pending)]
    public TransactionStatus Status {
      get;
      private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(TransactionNo, Description, Identificators, Tags,
                                           FixedAssetTransactionType.Name,
                                           BaseParty.Keywords);
      }
    }


    public FixedList<FixedAssetTransactionEntry> Entries {
      get {
        return _entries.Value.ToFixedList();
      }
    }

    #endregion Properties

    #region Methods

    internal FixedList<FixedAsset> GetFixedAssets() {
      return Entries.Select(x => x.FixedAsset)
                    .ToFixedList();
    }


    internal void Reload() {
      _entries = new Lazy<List<FixedAssetTransactionEntry>>(() => FixedAssetsData.GetTransactionEntries(this));
    }

    #endregion Methods

  }  // class FixedAssetTransaction

}  // namespace Empiria.Inventory.FixedAssets
