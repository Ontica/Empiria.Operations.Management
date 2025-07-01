/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : ReportingUseCases                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to manage inventory order.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.Services;

using Empiria.Inventory.Adapters;
using Empiria.Inventory.Data;

using Empiria.Inventory.Reporting.Adapters;


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

    public ReportingDataDto FinderInventory(FinderInventoryQuery query) {
      Assertion.Require(query, nameof(query));

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      var inventoryEntries = InventoryOrderData.FinderInventory(filter, sort);

      return FinderInventoryMapper.MapToInventoryEntryDataDto(inventoryEntries, query);
      ;
    }
          
    #endregion Use cases

    #region Helpers


    #endregion Helpers

  } // class ReportingUseCases

} // namespace Empiria.Inventory.Reporting.UseCases
