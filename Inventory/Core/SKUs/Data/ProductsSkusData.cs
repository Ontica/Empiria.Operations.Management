/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Products SKU Management                    Component : Data Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data service                            *
*  Type     : ProductsSkusData                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for products SKUs instances.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Data;

namespace Empiria.Inventory.Data {

  /// <summary>Provides data read and write methods for products SKUs instances.</summary>
  static internal class ProductsSkusData {

    static internal void Clean(ProductSku sku) {
      if (sku.IsEmptyInstance) {
        return;
      }
      var sql = "UPDATE OMS_PRODUCTS_SKUS " +
                $"SET SKU_UID = '{Guid.NewGuid().ToString()}', " +
                $"SKU_KEYWORDS = '{sku.Keywords}', " +
                $"SKU_START_DATE = {DataCommonMethods.FormatSqlDbDate(new DateTime(2025, 06, 25))}, " +
                $"SKU_POSTING_TIME = {DataCommonMethods.FormatSqlDbDate(new DateTime(2025, 06, 25))}, " +
                $"SKU_END_DATE = {DataCommonMethods.FormatSqlDbDate(new DateTime(2078, 12, 31))}, " +
                $"SKU_LAST_UPDATE = {DataCommonMethods.FormatSqlDbDate(new DateTime(2025, 06, 25))}, " +
                $"SKU_POSTED_BY_ID = 152 " +
                $"WHERE SKU_ID = {sku.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }

    #region Methods

    static internal void WriteProductSku(ProductSku o, string variantAttrs, string extensionData) {

      var op = DataOperation.Parse("WRITE_OMS_PRODUCT_SKU", o.Id, o.UID, o.SkuType.Id,
        o.Product.Id, o.SkuNo, o.Name, o.Description, o.Unit.Id, o.Quantity, o.ExpirationDate,
        o.Brand, o.Model, o.SerialNo, variantAttrs, EmpiriaString.Tagging(o.Identificators),
        EmpiriaString.Tagging(o.Tags), extensionData, o.Keywords, o.StartDate, o.EndDate, o.LastUpdate,
        o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class ProductsSkusData

}  // namespace Empiria.Inventory.Data
