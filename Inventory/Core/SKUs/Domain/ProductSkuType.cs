/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Products SKU Management                    Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Power type                              *
*  Type     : ProductSkuType                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a product stock keeping unit type (SKU).                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;

namespace Empiria.Inventory {

  /// <summary>Power type that describes a product stock keeping unit type (SKU).</summary>
  [Powertype(typeof(ProductSku))]
  public class ProductSkuType : Powertype {

    #region Constructors and parsers

    private ProductSkuType() {
      // Empiria powertype types always have this constructor.
    }

    static public new ProductSkuType Parse(int typeId) => Parse<ProductSkuType>(typeId);

    static public new ProductSkuType Parse(string typeName) => Parse<ProductSkuType>(typeName);

    static public FixedList<ProductSkuType> GetList() {
      return Empty.GetAllSubclasses()
            .Select(x => (ProductSkuType) x)
            .ToFixedList();
    }

    static public ProductSkuType Empty => Parse("ObjectTypeInfo.ProductSku");

    #endregion Constructors and parsers

  }  // class ProductSkuType

}  // namespace Empiria.Inventory
