/*Empiria Operations *****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Test cases                              *
*  Assembly : Empiria.Inventory.Tests.dll                Pattern   : Unit tests                              *
*  Type     : AssetTypeTests                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for AssetType instances.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Inventory.Assets;

namespace Empiria.Tests.Inventory.Assets {

  /// <summary>Unit tests for AssetType instances.</summary>
  public class AssetTypeTests {

    #region Facts

    [Fact]
    public void Should_Read_All_Asset_Types() {
      var sut = BaseObject.GetList<AssetType>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_AssetType() {
      var sut = AssetType.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class AssetTypeTests

}  // namespace Empiria.Tests.Inventory.Assets
