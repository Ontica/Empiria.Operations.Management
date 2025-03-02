/*Empiria Operations *****************************************************************************************
*                                                                                                            *
*  Module   : Products SKU Management                    Component : Test cases                              *
*  Assembly : Empiria.Inventory.Tests.dll                Pattern   : Unit tests                              *
*  Type     : ProductSkuTests                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for ProductSku instances.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Inventory;

namespace Empiria.Tests.Inventory {

  /// <summary>Unit tests for ProductSku instances.</summary>
  public class ProductSkuTests {

    #region Facts

    [Fact]
    public void Should_Get_All_Products_Skus() {
      var sut = BaseObject.GetFullList<ProductSku>("SKU_ID < 1000");

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Parse_All_Products_Skus() {
      var productSkus = BaseObject.GetFullList<ProductSku>("SKU_ID < 1000");

      foreach (ProductSku sut in productSkus) {
        Assert.NotNull(sut.Brand);
        Assert.NotNull(sut.Description);
        Assert.NotNull(sut.Identificators);
        Assert.NotNull(sut.Model);
        Assert.NotEmpty(sut.Name);
        Assert.NotNull(sut.Product);
        Assert.True(sut.Quantity > 0);
        Assert.NotEmpty(sut.SkuNo);
        Assert.NotNull(sut.SkuType);
        Assert.NotNull(sut.Tags);
        Assert.NotNull(sut.Unit);
      }
    }


    [Fact]
    public void Should_Parse_Empty_ProductSku() {
      var sut = ProductSku.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class ProductSkuTests

}  // namespace Empiria.Tests.Inventory
