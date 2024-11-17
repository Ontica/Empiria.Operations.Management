/*Empiria Operations *****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Test cases                              *
*  Assembly : Empiria.Inventory.Tests.dll                Pattern   : Unit tests                              *
*  Type     : FixedAssetTypeTests                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FixedAssetType instances.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Inventory.FixedAssets;

namespace Empiria.Tests.Inventory.FixedAssets {

  /// <summary>Unit tests for FixedAssetType instances.</summary>
  public class FixedAssetTypeTests {

    #region Facts

    [Fact]
    public void Should_Read_All_Fixed_Asset_Types() {
      var sut = BaseObject.GetList<FixedAssetType>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_FixedAssetType() {
      var sut = FixedAssetType.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class FixedAssetTypeTests

}  // namespace Empiria.Tests.Inventory.FixedAssets
