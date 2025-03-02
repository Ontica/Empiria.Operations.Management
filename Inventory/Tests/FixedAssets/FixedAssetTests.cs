/*Empiria Operations *****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Test cases                              *
*  Assembly : Empiria.Inventory.Tests.dll                Pattern   : Unit tests                              *
*  Type     : FixedAssetTests                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FixedAsset instances.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Inventory.FixedAssets;
using Empiria.Inventory.FixedAssets.Data;

namespace Empiria.Tests.Inventory.FixedAssets {

  /// <summary>Unit tests for FixedAsset instances.</summary>
  public class FixedAssetTests {

    #region Facts

    [Fact]
    public void Should_Get_All_Fixed_Assets() {
      var sut = BaseObject.GetList<FixedAsset>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Parse_All_Fixed_Assets() {
      var fixedAssets = BaseObject.GetList<FixedAsset>();

      foreach (FixedAsset sut in fixedAssets) {
        Assert.NotNull(sut.Building);
        Assert.NotNull(sut.Floor);
        Assert.NotNull(sut.Place);
      }
    }


    [Fact]
    public void Should_Parse_Empty_FixedAsset() {
      var sut = FixedAsset.Empty;

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Search_Fixed_Assets() {
      var sut = FixedAssetsData.GetFixedAssets(string.Empty, string.Empty);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

  }  // class FixedAssetTests

}  // namespace Empiria.Tests.Inventory.FixedAssets
