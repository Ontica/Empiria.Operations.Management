﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Products SKU Management                    Component : Data Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data service                            *
*  Type     : ProductsSkusData                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for products SKUs instances.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Inventory.Data {

  /// <summary>Provides data read and write methods for products SKUs instances.</summary>
  static internal class ProductsSkusData {

    #region Methods

    static internal void WriteProductSku(ProductSku o, string variantAttrs, string extensionData) {

      var op = DataOperation.Parse("WRITE_OMS_PRODUCT_SKU", o.Id, o.UID, o.SkuType.Id,
        o.Product.Id, o.SkuNo, o.Name, o.Description, o.Unit.Id, o.Quantity, o.ExpirationDate,
        o.Brand, o.Model, o.SerialNo, variantAttrs, string.Join(" ", o.Identificators),
        string.Join(" ", o.Tags), extensionData, o.Keywords, o.StartDate, o.EndDate, o.LastUpdate,
        o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class ProductsSkusData

}  // namespace Empiria.Inventory.Data
