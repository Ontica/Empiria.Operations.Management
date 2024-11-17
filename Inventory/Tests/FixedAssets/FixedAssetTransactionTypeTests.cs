/*Empiria Operations *****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Test cases                              *
*  Assembly : Empiria.Inventory.Tests.dll                Pattern   : Unit tests                              *
*  Type     : FixedAssetTransactionTypeTests             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FixedAssetTransactionType instances.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Inventory.FixedAssets;

namespace Empiria.Tests.Inventory.FixedAssets {

  /// <summary>Unit tests for FixedAssetTransactionType instances.</summary>
  public class FixedAssetTransactionTypeTests {

    #region Facts

    [Fact]
    public void Should_Read_All_Fixed_Asset_Transaction_Types() {
      var sut = BaseObject.GetList<FixedAssetTransactionType>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_FixedAssetTransactionType() {
      var sut = FixedAssetTransactionType.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class FixedAssetTransactionTypeTests

}  // namespace Empiria.Tests.Inventory.FixedAssets
