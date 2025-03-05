/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Information Holder                      *
*  Type     : AssetTransactionEntry                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an asset transaction entry.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Inventory.Assets {

  /// <summary>Represents an asset transaction entry.</summary>
  public class AssetTransactionEntry : BaseObject {

    #region Constructors and parsers

    private AssetTransactionEntry() {
      // Required by Empiria Framework.
    }

    internal AssetTransactionEntry(AssetTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));

      this.Transaction = transaction;
    }

    static public AssetTransactionEntry Parse(int id) => ParseId<AssetTransactionEntry>(id);

    static public AssetTransactionEntry Parse(string uid) => ParseKey<AssetTransactionEntry>(uid);

    static public AssetTransactionEntry Empty => ParseEmpty<AssetTransactionEntry>();

    #endregion Constructors and parsers

    [DataField("ASSET_ENTRY_TXN_ID")]
    public AssetTransaction Transaction {
      get; private set;
    }


    [DataField("ASSET_ENTRY_TYPE_ID")]
    public int EntryTypeId {
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


    [DataField("ASSET_ENTRY_OPERATION_TYPE_ID")]
    public int OperationTypeId {
      get; private set;
    }


    [DataField("ASSET_ENTRY_OPERATION_ID")]
    public int OperationId {
      get; private set;
    }


    [DataField("ASSET_ENTRY_OPERATION_DATA")]
    internal protected JsonObject OperationData {
      get; private set;
    }


    [DataField("ASSET_ENTRY_OPERATION_VALUE")]
    public decimal OperationValue {
      get; private set;
    }


    [DataField("ASSET_ENTRY_EXT_DATA")]
    internal protected JsonObject ExtensionData {
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
        return EmpiriaString.BuildKeywords(Transaction.Keywords, Asset.Keywords, Description);
      }
    }

  }  // class AssetTransactionEntry

}  // namespace Empiria.Inventory.Assets
