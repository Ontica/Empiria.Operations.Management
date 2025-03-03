/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Information Holder                      *
*  Type     : AssetTransaction                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an asset transaction.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Inventory.Assets.Data;

namespace Empiria.Inventory.Assets {

  /// <summary>Represents as asset transaction type.</summary>
  public class AssetTransaction : BaseObject {

    #region Fields

    private Lazy<List<AssetTransactionEntry>> _entries = new Lazy<List<AssetTransactionEntry>>();

    #endregion Fields

    #region Constructors and parsers

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

    [DataField("OMS_TXN_TYPE_ID")]
    public AssetTransactionType AssetTransactionType {
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
                                           AssetTransactionType.Name,
                                           BaseParty.Keywords);
      }
    }


    public FixedList<AssetTransactionEntry> Entries {
      get {
        return _entries.Value.ToFixedList();
      }
    }

    #endregion Properties

    #region Methods

    internal FixedList<Asset> GetAssets() {
      return Entries.Select(x => x.Asset)
                    .ToFixedList();
    }


    internal void Reload() {
      _entries = new Lazy<List<AssetTransactionEntry>>(() => AssetsData.GetTransactionEntries(this));
    }

    #endregion Methods

  }  // class AssetTransaction

}  // namespace Empiria.Inventory.Assets
