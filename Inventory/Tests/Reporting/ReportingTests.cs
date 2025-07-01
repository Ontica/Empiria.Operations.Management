/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : ReportingTests                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for Inventory reporting                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using Empiria.Inventory;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.Data;
using Empiria.Inventory.Reporting.Adapters;
using Xunit;

namespace Empiria.Tests.Inventory {

  /// <summary>Unit tests for Inventory reporting.</summary>
  public class ReportingTests {

    #region Initialization

    public ReportingTests() {
    }

    #endregion Initialization

    [Fact]
    public void SearchInventoryOrderTest() {
      List<InventoryEntry> inventoryEntries = new List<InventoryEntry>();

      FinderInventoryQuery query = new FinderInventoryQuery {
        Keywords = "",
        Products = { },
        Locations = {"A-001", "A-001-1" },
        WarehouseUID = "",
        RackUID = "",
        LevelUID = "",
        Position = "",
      };

      Assertion.Require(query, nameof(query));

      foreach (string location in query.Locations) {
        query.Location = location;

        inventoryEntries.AddRange(GetInventoryEntries(query));
      }


      foreach (string product in query.Products) {
        query.Product = product;
      
        inventoryEntries.AddRange(GetInventoryEntries(query));
      }

      var sut = FinderInventoryMapper.MapToInventoryEntryDataDto(inventoryEntries.ToFixedList(), query);
      
      Assert.NotNull(sut);
    }


    private IEnumerable<InventoryEntry> GetInventoryEntries(FinderInventoryQuery query) {
      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      return InventoryOrderData.FinderInventory(filter, sort);
    }

  } // class ReportingTests

} // namespace Empiria.Tests.Inventory
