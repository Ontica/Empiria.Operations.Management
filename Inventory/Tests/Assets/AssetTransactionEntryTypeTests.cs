/*Empiria Operations *****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Test cases                              *
*  Assembly : Empiria.Inventory.Tests.dll                Pattern   : Unit tests                              *
*  Type     : AssetTransactionEntryTypeTests             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for AssetTransactionEntryType instances.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Inventory.Assets;

namespace Empiria.Tests.Inventory.Assets {

  /// <summary>Unit tests for AssetTransactionEntryType instances.</summary>
  public class AssetTransactionEntryTypeTests {

    #region Facts

    [Fact]
    public void Should_Read_All_Asset_Transaction_Entry_Types() {
      var sut = AssetTransactionEntryType.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_AssetTransactionEntryType() {
      var sut = AssetTransactionEntryType.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class AssetTransactionEntryTypeTests

}  // namespace Empiria.Tests.Inventory.Assets
