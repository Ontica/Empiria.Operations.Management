/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Products SKU Management                    Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Information Holder                      *
*  Type     : ProductSkuCategory                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a product SKU Category.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory {

  /// <summary>Represents a product SKU Category.</summary>
  public class ProductSkuCategory : GeneralObject {

    #region Constructors and parsers

    static internal ProductSkuCategory Parse(int id) {
      return ParseId<ProductSkuCategory>(id);
    }

    static internal ProductSkuCategory Parse(string uid) {
      return ParseKey<ProductSkuCategory>(uid);
    }

    static internal FixedList<ProductSkuCategory> GetList() {
      return GetList<ProductSkuCategory>().ToFixedList();
    }

    static internal ProductSkuCategory Empty => ParseEmpty<ProductSkuCategory>();

    #endregion Constructors and parsers

  }  // class ProductSkuCategory

}  // namespace Empiria.Inventory
