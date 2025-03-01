/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Products SKU Management                    Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Information Holder                      *
*  Type     : ProductSkuType                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a product stock keeping unit type.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory.SKUs {

  /// <summary>Represents a product SKU type.</summary>
  public class ProductSkuType : GeneralObject {

    #region Constructors and parsers

    static internal ProductSkuType Parse(int id) {
      return ParseId<ProductSkuType>(id);
    }

    static internal ProductSkuType Parse(string uid) {
      return ParseKey<ProductSkuType>(uid);
    }

    static internal FixedList<ProductSkuType> GetList() {
      return GetList<ProductSkuType>().ToFixedList();
    }

    static internal ProductSkuType Empty => ParseEmpty<ProductSkuType>();

    #endregion Constructors and parsers

  }  // class ProductSkuType

}  // namespace Empiria.Inventory.SKUs
