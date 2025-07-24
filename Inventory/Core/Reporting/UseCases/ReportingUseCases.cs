/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : ReportingUseCases                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to manage inventory order.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Collections.Generic;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.Data;
using Empiria.Inventory.Reporting.Adapters;
using Empiria.Services;


namespace Empiria.Inventory.Reporting.UseCases {

  /// <summary>Use cases to manage inventory order.</summary>
  public class ReportingUseCases : UseCase {
        
    #region Constructors and parsers

    protected ReportingUseCases() {
      // no-op
    }

    static public ReportingUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<ReportingUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ReportingDataDto FinderInventory(SearchInventoryQuery query) {
      Assertion.Require(query, nameof(query));

      List<InventoryEntry> inventoryEntries = new List<InventoryEntry>();
        
      foreach (string location in query.Locations) {
        query.Location = location;

        inventoryEntries.AddRange(GetInventoryEntries(query));
      }


      foreach (string product in query.Products) {
        query.Product = product;

        inventoryEntries.AddRange(GetInventoryEntries(query));
      }

      return FinderInventoryMapper.MapToInventoryEntryDataDto(inventoryEntries.ToFixedList(), query);   
    }

    #endregion Use cases

    #region Helpers

    private IEnumerable<InventoryEntry> GetInventoryEntries(SearchInventoryQuery query) {
      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      return InventoryOrderData.SearchInventoryEntries(filter, sort);
    }

    #endregion Helpers

  } // class ReportingUseCases

} // namespace Empiria.Inventory.Reporting.UseCases
