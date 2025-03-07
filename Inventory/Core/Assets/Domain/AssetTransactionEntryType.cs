/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Power type                              *
*  Type     : AssetTransactionEntryType                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes an asset transaction entry.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;

namespace Empiria.Inventory.Assets {

  /// <summary>Power type that describes an asset transaction entry.</summary>
  [Powertype(typeof(AssetTransactionEntry))]
  public class AssetTransactionEntryType : Powertype {

    #region Constructors and parsers

    private AssetTransactionEntryType() {
      // Empiria power type types always have this constructor.
    }

    static public new AssetTransactionEntryType Parse(int typeId) => Parse<AssetTransactionEntryType>(typeId);

    static public new AssetTransactionEntryType Parse(string typeName) => Parse<AssetTransactionEntryType>(typeName);

    static public FixedList<AssetTransactionEntryType> GetList() {
      return Empty.GetAllSubclasses()
                  .Select(x => (AssetTransactionEntryType) x)
                  .ToFixedList();
    }

    static public AssetTransactionEntryType Empty => Parse("ObjectTypeInfo.AssetTransactionEntry");

    #endregion Constructors and parsers

  }  // class AssetTransactionEntryType

}  // namespace Empiria.Inventory.Assets
