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

      SearchInventoryQuery query = new SearchInventoryQuery {
        Keywords = "",
        Products = {}, 
        Locations = {},
        WarehouseUID = "", // TODO preguntar como hacerlo mas eficiente, 
        RackUID = "",
        LevelUID = "D4F5F352-05D2-464F-879E-B090AD915075",
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

      inventoryEntries.AddRange(GetInventoryEntries(query));

      var aux = inventoryEntries.ToFixedList();

      var sut = FinderInventoryMapper.MapToInventoryEntryDataDto(aux, query);
      
      Assert.NotNull(sut);
    }


    private IEnumerable<InventoryEntry> GetInventoryEntries(SearchInventoryQuery query) {
      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      return InventoryOrderData.SearchInventoryEntries(filter, sort);
    }

  } // class ReportingTests

} // namespace Empiria.Tests.Inventory
