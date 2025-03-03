/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Information Holder                      *
*  Type     : AssetTransactionType                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an asset transaction type.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory.Assets {

  /// <summary>Represents an asset transaction type.</summary>
  public class AssetTransactionType : GeneralObject {

    #region Constructors and parsers

    static internal AssetTransactionType Parse(int id) => ParseId<AssetTransactionType>(id);

    static internal AssetTransactionType Parse(string uid) => ParseKey<AssetTransactionType>(uid);

    static internal FixedList<AssetTransactionType> GetList() {
      return GetList<AssetTransactionType>().ToFixedList();
    }

    static internal AssetTransactionType Empty => ParseEmpty<AssetTransactionType>();

    #endregion Constructors and parsers

  }  // class AssetTransactionType

}  // namespace Empiria.Inventory.Assets
