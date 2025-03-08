/*Empiria Operations *****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Test cases                              *
*  Assembly : Empiria.Inventory.Tests.dll                Pattern   : Unit tests                              *
*  Type     : AssetTransactionEntryTests                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for AssetTransactionEntry instances.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Inventory.Assets;

namespace Empiria.Tests.Inventory.Assets {

  /// <summary>Unit tests for AssetTransactionEntry instances.</summary>
  public class AssetTransactionEntryTests {

    #region Facts

    [Fact]
    public void Should_Get_All_Assets_Transactions_Entries() {
      var sut = BaseObject.GetFullList<AssetTransactionEntry>("ASSET_ENTRY_ID < 100");

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Parse_All_Assets_Transactions_Entries() {
      var entries = BaseObject.GetFullList<AssetTransactionEntry>();

      foreach (AssetTransactionEntry sut in entries) {
        Assert.NotNull(sut.Transaction);
        Assert.NotNull(sut.Asset);
        Assert.NotNull(sut.Description);
        Assert.True(sut.Position >= 0);
      }
    }


    [Fact]
    public void Should_Parse_Empty_AssetTransactionEntry() {
      var sut = AssetTransactionEntry.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class AssetTransactionEntryTests

}  // namespace Empiria.Tests.Inventory.Assets
