/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : FinderInventoryMapper                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for inventory order finder.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Collections.Generic;

using Empiria.DynamicData;
using Empiria.Locations;

namespace Empiria.Inventory.Reporting.Adapters {

  /// <summary>Mapping methods for inventory order finder.</summary>
  static internal class FinderInventoryMapper {

    #region Public methods

    static public ReportingDataDto MapToInventoryEntryDataDto(FixedList<InventoryEntry> list, SearchInventoryQuery query) {

      return new ReportingDataDto {
        Query = query,
        Columns = GetColumns(),
        Entries = MapInventoryEntriesDescriptors(list)
      };
    }


    private static FixedList<DataTableColumn> GetColumns() {

      List<DataTableColumn> columns = new List<DataTableColumn>();
      columns.Add(new DataTableColumn("code", "Código", "text-link"));
      columns.Add(new DataTableColumn("name", "Nombre", "text"));
      columns.Add(new DataTableColumn("presentation", "Presentación", "text"));
      columns.Add(new DataTableColumn("tag", "Localización", "text-nowrap"));
      columns.Add(new DataTableColumn("stock", "Stock disponible", "decimal"));
      columns.Add(new DataTableColumn("realStock", "Stock real", "decimal"));
      columns.Add(new DataTableColumn("stockInProcess", "Stock entrante", "decimal"));
    
      return columns.ToFixedList();
    }


    

    static internal FixedList<InventoryStockDescriptorDto> MapInventoryEntriesDescriptors(
                                                            FixedList<InventoryEntry> entries) {
      return entries.Select(x => MapToEntryDescriptor(x))
                   .ToFixedList();
    }

    #endregion Public methods

    #region Private methods

    static private InventoryStockDescriptorDto MapToEntryDescriptor(InventoryEntry entry) {

      return new InventoryStockDescriptorDto() {
       UID = entry.Product.UID,
       Name = entry.Product.Name,
       Code = entry.Product.InternalCode,
       Presentation = "",
       WarehouseName = GetWarehoue(entry.Location).Name,
       Rack = GetRack(entry.Location).Name,
       Tag = entry.Location.FullName,
       Stock = entry.InputQuantity,
       };
    }


    static private Location GetWarehoue(Location location) {
      if (location.Level == 1) {
        return location;
      }
     
      var loc = location.GetParent<Location>();
      return GetWarehoue(loc);
    }


    static private Location GetRack(Location location) {
      if (location.Level == 2) {
        return location;
      }

      var loc = location.GetParent<Location>();
      return GetRack(loc);
    }

    #endregion Private methods

  } // class FinderInventoryMapper

} // namespace Empiria.Inventory.Reporting.Adapters
