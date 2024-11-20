/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Information Holder                      *
*  Type     : FixedAssetTransactionEntry                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a fixed asset transaction entry.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;
using System;

namespace Empiria.Inventory.FixedAssets {

  /// <summary>Represents a fixed asset transaction entry.</summary>
  public class FixedAssetTransactionEntry : BaseObject {

    #region Constructors and parsers

    private FixedAssetTransactionEntry() {
      // Required by Empiria Framework.
    }

    internal FixedAssetTransactionEntry(FixedAssetTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));

      this.Transaction = transaction;
    }

    static public FixedAssetTransactionEntry Parse(int id) => ParseId<FixedAssetTransactionEntry>(id);

    static public FixedAssetTransactionEntry Parse(string uid) => ParseKey<FixedAssetTransactionEntry>(uid);

    static public FixedAssetTransactionEntry Empty => ParseEmpty<FixedAssetTransactionEntry>();

    #endregion Constructors and parsers

    [DataField("OMS_TXN_ENTRY_TXN_ID")]
    public FixedAssetTransaction Transaction {
      get;
      private set;
    }


    [DataField("OMS_TXN_ENTRY_TYPE_ID")]
    public int EntryTypeId {
      get;
      private set;
    }


    [DataField("OMS_TXN_OBJECT_ID")]
    public FixedAsset FixedAsset {
      get;
      private set;
    }


    [DataField("OMS_TXN_ENTRY_OPERATION_TYPE_ID")]
    public int OperationTypeId {
      get;
      private set;
    }


    [DataField("OMS_TXN_ENTRY_OPERATION_ID")]
    public int OperationId {
      get;
      private set;
    }


    [DataField("OMS_TXN_ENTRY_DESCRIPTION")]
    public string Description {
      get;
      private set;
    }

    [DataField("OMS_TXN_ENTRY_EXT_DATA")]
    internal protected JsonObject ExtensionData {
      get;
      private set;
    }


    [DataField("OMS_TXN_ENTRY_POSTED_BY_ID")]
    public Party PostedBy {
      get;
      private set;
    }


    [DataField("OMS_TXN_ENTRY_POSTING_TIME")]
    public DateTime PostingTime {
      get;
      private set;
    }


    [DataField("OMS_TXN_ENTRY_STATUS", Default = TransactionStatus.Pending)]
    public TransactionStatus Status {
      get;
      private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Transaction.Keywords, FixedAsset.Keywords, Description);
      }
    }

  }  // class FixedAssetTransactionEntry

}  // namespace Empiria.Inventory.FixedAssets

