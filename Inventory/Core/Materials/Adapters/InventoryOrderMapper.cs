/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : InventoryOrderMapper                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for inventory order.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.DynamicData;
using Empiria.StateEnums;

namespace Empiria.Inventory.Adapters {

  /// <summary>Mapping methods for inventory order.</summary>
  static internal class InventoryOrderMapper {

    #region Public methods

    static public InventoryOrderDataDto InventoryOrderDataDto(FixedList<InventoryOrder> list, InventoryOrderQuery query) {

      return new InventoryOrderDataDto {
        Query = query,
        Columns = GetColumns(),
        Entries = MapInventoryOrderDescriptors(list)
      };
    }


    static private FixedList<DataTableColumn> GetColumns() {

      List<DataTableColumn> columns = new List<DataTableColumn>();

      columns.Add(new DataTableColumn("orderTypeName", "Tipo de orden", "text"));
      columns.Add(new DataTableColumn("orderNo", "Número de orden", "text-link"));
      columns.Add(new DataTableColumn("responsibleName", "Responsable", "text"));
      //columns.Add(new DataTableColumn("description", "Descripción", "text"));
      columns.Add(new DataTableColumn("postedByName", "Registrado por", "text"));
      columns.Add(new DataTableColumn("postingTime", "Fecha registro", "date"));
      columns.Add(new DataTableColumn("status", "Estatus", "text-tag"));

      return columns.ToFixedList();
    }


    static internal InventoryHolderDto MapToHolderDto(InventoryOrder order, InventoryOrderActions actions) {

      return new InventoryHolderDto {
        Order = MapToOrderDto(order),
        Items = MapToOrderItemsDto(order.Items),
        Actions = actions
      };
    }


    static internal FixedList<InventoryOrderDescriptorDto> MapInventoryOrderDescriptors(
                                                            FixedList<InventoryOrder> orders) {
      return orders.Select(x => MapToDescriptor(x))
                   .ToFixedList();
    }

    #endregion Public methods

    #region Private methods

    static private InventoryOrderDescriptorDto MapToDescriptor(InventoryOrder x) {

      return new InventoryOrderDescriptorDto() {
        UID = x.UID,
        OrderTypeName = x.OrderType.Name,
        OrderNo = x.OrderNo,
        Description = x.Description,
        ResponsibleName = x.Responsible.IsEmptyInstance ? "Sin Asignar" : x.Responsible.Name,
        PostedByName = x.PostedBy.Name,
        PostingTime = x.PostingTime,
        Status = x.Status.GetName()
      };
    }


    static private InventoryEntryDto MapToInventoryEntryDto(InventoryEntry entry) {

      return new InventoryEntryDto {
        UID = entry.UID,
        Product = entry.Product.Name,
        Location = entry.Location.Name,
        Quantity = entry.InputQuantity,
        PostedBy = entry.PostedBy.MapToNamedEntity(),
        PostingTime = entry.PostingTime
      };
    }


    static private FixedList<InventoryEntryDto> MapToInventoryEntriesDto(FixedList<InventoryEntry> items) {

      return items.Select((x) => MapToInventoryEntryDto(x))
                         .ToFixedList();
    }


    private static FixedList<InventoryOrderItemDto> MapToOrderItemsDto(FixedList<InventoryOrderItem> items) {

      return items.Select((x) => MapToOrderItemDto(x))
                         .ToFixedList();
    }


    static private InventoryOrderItemDto MapToOrderItemDto(InventoryOrderItem item) {

      return new InventoryOrderItemDto() {
        UID = item.UID,
        ProductName = item.Product.Name,
        Quantity = item.Quantity,
        AssignedQuantity = item.Entries.Sum(x => x.InputQuantity),
        PostedBy = item.PostedBy.MapToNamedEntity(),
        PostingTime = item.PostingTime,
        Entries = MapToInventoryEntriesDto(item.Entries),
        Status = item.Status
      };
    }


    private static InventoryOrderDto MapToOrderDto(InventoryOrder order) {

      return new InventoryOrderDto {
        UID = order.UID,
        OrderType = new NamedEntityDto("X","Orden de inventario"), //order.OrderType.MapToNamedEntity(),
        Description = order.Description,
        OrderNo = order.OrderNo,
        Responsible = order.Responsible.MapToNamedEntity(),
        PostedBy = order.PostedBy.MapToNamedEntity(),
        PostingTime = order.PostingTime,
        ClosingTime = order.ClosingTime,
        Status = order.Status.MapToDto()
      };
    }

    #endregion Private methods

  } // class InventoryOrderMapper

} // namespace Empiria.Inventory.Adapters
