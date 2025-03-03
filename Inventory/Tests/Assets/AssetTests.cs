/*Empiria Operations *****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Test cases                              *
*  Assembly : Empiria.Inventory.Tests.dll                Pattern   : Unit tests                              *
*  Type     : AssetTests                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for Asset instances.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Inventory.Assets;
using Empiria.Inventory.Assets.Data;

namespace Empiria.Tests.Inventory.Assets {

  /// <summary>Unit tests for Asset instances.</summary>
  public class AssetTests {

    #region Facts

    [Fact]
    public void Should_Get_All_Assets() {
      var sut = BaseObject.GetList<Asset>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Parse_All_Assets() {
      var assets = BaseObject.GetList<Asset>();

      foreach (Asset sut in assets) {
        Assert.NotNull(sut.Building);
        Assert.NotNull(sut.Floor);
        Assert.NotNull(sut.Place);
      }
    }


    [Fact]
    public void Should_Parse_Empty_Asset() {
      var sut = Asset.Empty;

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Search_Assets() {
      var sut = AssetsData.GetAssets(string.Empty, string.Empty);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

  }  // class AssetTests

}  // namespace Empiria.Tests.Inventory.Assets
