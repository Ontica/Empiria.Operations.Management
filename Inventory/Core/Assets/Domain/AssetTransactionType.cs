﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Power type                              *
*  Type     : AssetTransactionType                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes an assets transaction.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;

namespace Empiria.Inventory.Assets {

  /// <summary>Power type that describes an assets transaction.</summary>
  [Powertype(typeof(AssetTransaction))]
  public class AssetTransactionType : Powertype {

    #region Constructors and parsers

    private AssetTransactionType() {
      // Empiria power type types always have this constructor.
    }

    static public new AssetTransactionType Parse(int typeId) => Parse<AssetTransactionType>(typeId);

    static public new AssetTransactionType Parse(string typeName) => Parse<AssetTransactionType>(typeName);


    static public AssetTransactionType ParseWithOperation(string operationName) {
      AssetTransactionType txnType = GetList().Find(x => x.ExternalOperation == operationName);

      Assertion.Require(txnType, $"Unrecognized asset transaction operation '{operationName}'.");

      return txnType;
    }


    static public FixedList<AssetTransactionType> GetList() {
      return Empty.GetAllSubclasses(false)
                  .Select(x => (AssetTransactionType) x)
                  .ToFixedList()
                  .Sort((x, y) => x.SortOrder.CompareTo(y.SortOrder));
    }


    static public AssetTransactionType Empty => Parse("ObjectTypeInfo.AssetTransaction");

    #endregion Constructors and parsers

    #region Properties

    public AssetTransactionEntryType DefaultAssetTransactionEntryType {
      get {
        return base.ExtensionData.Get("defaultAssetTransactionEntryTypeId",
                                      AssetTransactionEntryType.Empty);
      }
    }


    public bool IsProtected {
      get {
        return base.ExtensionData.Get("isProtected", false);
      }
    }


    public string Prefix {
      get {
        return base.ExtensionData.Get("prefix", string.Empty);
      }
    }


    public string ExternalOperation {
      get {
        return base.ExtensionData.Get("externalOperation", string.Empty);
      }
    }


    public int SortOrder {
      get {
        return base.ExtensionData.Get("sortOrder", 0);
      }
    }

    #endregion Properties

  }  // class AssetTransactionType

}  // namespace Empiria.Inventory.Assets
