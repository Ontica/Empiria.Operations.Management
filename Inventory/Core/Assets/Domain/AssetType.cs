/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Information Holder                      *
*  Type     : AssetType                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an asset type.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory.Assets {

  /// <summary>Represents an asset type.</summary>
  public class AssetType : GeneralObject {

    #region Constructors and parsers

    static internal AssetType Parse(int id) {
      return ParseId<AssetType>(id);
    }

    static internal AssetType Parse(string uid) {
      return ParseKey<AssetType>(uid);
    }

    static internal FixedList<AssetType> GetList() {
      return GetList<AssetType>().ToFixedList();
    }

    static internal AssetType Empty => ParseEmpty<AssetType>();

    #endregion Constructors and parsers

  }  // class AssetType

}  // namespace Empiria.Inventory.Assets
