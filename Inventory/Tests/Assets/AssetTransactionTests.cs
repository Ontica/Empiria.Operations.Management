/*Empiria Operations *****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Test cases                              *
*  Assembly : Empiria.Inventory.Tests.dll                Pattern   : Unit tests                              *
*  Type     : AssetTransactionTests                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for AssetTransaction instances.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Inventory.Assets;
using Empiria.Inventory.Assets.Data;

namespace Empiria.Tests.Inventory.Assets {

  /// <summary>Unit tests for AssetTransaction instances.</summary>
  public class AssetTransactionTests {

    #region Facts

    [Fact]
    public void Should_Clone_Asset_Transaction() {
      var transactionType = AssetTransactionType.Parse("ObjectTypeInfo.AssetTransaction.FixedAssetsCustody");

      var sourceTransaction = AssetTransaction.Parse(2);

      var sut = sourceTransaction.Clone(transactionType);

      Assert.Equal(transactionType, sut.AssetTransactionType);
      Assert.Equal(transactionType.DisplayName, sut.Description);
      Assert.Equal(sourceTransaction.BaseLocation, sut.BaseLocation);
      Assert.Equal(sourceTransaction.Manager, sut.Manager);
      Assert.Equal(sourceTransaction.ManagerOrgUnit, sut.ManagerOrgUnit);
      Assert.Equal(sourceTransaction.Entries.Count, sut.Entries.Count);
    }


    [Fact]
    public void Should_Get_All_Assets_Transactions() {
      var sut = BaseObject.GetList<AssetTransaction>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Parse_All_Assets_Transactions() {
      var transactions = BaseObject.GetList<AssetTransaction>();

      foreach (AssetTransaction sut in transactions) {
        Assert.NotEmpty(sut.TransactionNo);
        Assert.NotNull(sut.AssetTransactionType);
        Assert.NotNull(sut.Description);
        Assert.NotNull(sut.Identificators);
        Assert.NotNull(sut.Tags);
        Assert.NotNull(sut.Manager);
        Assert.NotNull(sut.ManagerOrgUnit);
        Assert.NotNull(sut.AssignedTo);
        Assert.NotNull(sut.AssignedToOrgUnit);
        Assert.NotNull(sut.BaseLocation);
        Assert.NotNull(sut.Building);
        Assert.NotNull(sut.Floor);
        Assert.NotNull(sut.Place);
        Assert.NotNull(sut.OperationSource);
        Assert.NotNull(sut.RequestedBy);
        Assert.NotNull(sut.RecordedBy);
        Assert.NotNull(sut.AppliedBy);
      }
    }


    [Fact]
    public void Should_Parse_Empty_AssetTransaction() {
      var sut = AssetTransaction.Empty;

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Search_Asset_Transactions() {
      var sut = AssetsTransactionsData.SearchTransactions(string.Empty, string.Empty);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

  }  // class AssetTransactionTests

}  // namespace Empiria.Tests.Inventory.Assets
