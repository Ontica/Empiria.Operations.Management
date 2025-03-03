/*Empiria Operations *****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Test cases                              *
*  Assembly : Empiria.Inventory.Tests.dll                Pattern   : Unit tests                              *
*  Type     : AssetTransactionTypeTests                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for AssetTransactionType instances.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Inventory.Assets;

namespace Empiria.Tests.Inventory.Assets {

  /// <summary>Unit tests for AssetTransactionType instances.</summary>
  public class AssetTransactionTypeTests {

    #region Facts

    [Fact]
    public void Should_Read_All_Asset_Transaction_Types() {
      var sut = BaseObject.GetList<AssetTransactionType>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_AssetTransactionType() {
      var sut = AssetTransactionType.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class AssetTransactionTypeTests

}  // namespace Empiria.Tests.Inventory.Assets
