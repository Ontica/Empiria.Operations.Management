/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : InventoryOrderMapper                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for inventory order.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using Empiria.DynamicData;
using Empiria.Orders;
using Empiria.Parties;
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

      columns.Add(new DataTableColumn("orderTypeName", "Tipo", "text"));
      columns.Add(new DataTableColumn("inventoryOrderNo", "Número de orden", "text-link"));
      columns.Add(new DataTableColumn("responsibleName", "Responsable", "text"));
      columns.Add(new DataTableColumn("description", "Descripción", "text"));
      columns.Add(new DataTableColumn("postedByName", "Registrado por", "text"));
      columns.Add(new DataTableColumn("postingTime", "Fecha registro", "date"));
      columns.Add(new DataTableColumn("status", "Estatus", "text-tag"));

      return columns.ToFixedList();
    }


    static internal InventoryHolderDto MapToHolderDto(InventoryOrder order) {

      return new InventoryHolderDto {
        Order = MapToOrderDto(order),
        Items = MapToOrderItemsDto(order.Items)
      };
    }


    static internal InventoryEntryHolderDto Map(FixedList<InventoryEntry> items) {

      return new InventoryEntryHolderDto {
        Items = MapToInventoryEntryDto(items)
      };
    }


    static internal FixedList<InventoryOrderDescriptorDto> MapInventoryOrderDescriptors(
                                                            FixedList<InventoryOrder> orders) {
      return orders.Select(x => MapToDescriptor((InventoryOrder) x))
                   .ToFixedList();
    }


    static private InventoryOrderDescriptorDto MapToDescriptor(InventoryOrder x) {
      
        var descriptor = new InventoryOrderDescriptorDto();
        descriptor.UID = x.InventoryOrderUID;
        descriptor.OrderTypeName = GetOrderTypeName(x.InventoryOrderTypeId);
        // OrderType.Parse(x.InventoryOrderTypeId).Name;
        descriptor.OrderNo = x.InventoryOrderNo;
        descriptor.ResponsibleName = x.ResponsibleId > 0 ? Party.Parse(x.ResponsibleId).Name : "Sin Asignar";
        descriptor.Description = x.Order_Description;
        descriptor.PostedByName = x.PostedById > 0 ? Party.Parse(x.PostedById).Name : "Sin asignar";
        descriptor.PostingTime = x.PostingTime;
        descriptor.Status = x.Status.GetName();

        return descriptor;
    }

    private static string GetOrderTypeName(int inventoryOrderTypeId) {

      if (inventoryOrderTypeId == 4005) {
        return "Orden de compra";
      }
      if (inventoryOrderTypeId == 4008) {
        return "Orden de venta";
      }
      if (inventoryOrderTypeId == 4009) {
        return "Orden de devolución de venta";
      } else {
        return string.Empty;
      }
    }

    #endregion Public methods

    #region Private methods

    static internal InventoryEntryDto MapToInventoryEntryDto(InventoryEntry entry) {
      return new InventoryEntryDto {
        UID = entry.UID,
        InventoryEntryType = NamedEntityDto.Empty,
        Product = NamedEntityDto.Empty,
        Sku = NamedEntityDto.Empty,
        Location = NamedEntityDto.Empty,
        Unit = NamedEntityDto.Empty,
        Notes = entry.ObservationNotes,
        InputQuantity = entry.InputQuantity,
        InputCost = entry.InputCost,
        OutputQuantity = entry.OutputQuantity,
        OutputCost = entry.OutputCost,
        PostedBy = entry.PostedBy.MapToNamedEntity(),
        PostingTime = entry.PostingTime
      };
    }


    private static FixedList<InventoryOrderItemDto> MapToOrderItemsDto(FixedList<InventoryOrderItem> items) {

      return items.Select((x) => MapToOrderItemDto(x))
                         .ToFixedList();
    }


    static private InventoryOrderItemDto MapToOrderItemDto(InventoryOrderItem x) {
      return new InventoryOrderItemDto();
    }


    private static FixedList<InventoryEntryDto> MapToInventoryEntryDto(FixedList<InventoryEntry> items) {

      return items.Select((x) => MapToInventoryEntryDto(x))
                         .ToFixedList();
    }


    private static InventoryOrderDto MapToOrderDto(InventoryOrder order) {

      return new InventoryOrderDto {
        UID = order.InventoryOrderUID,
        InventoryOrderType = NamedEntityDto.Empty,
        Notes = order.Order_Description,
        InventoryOrderNo = order.InventoryOrderNo,
        Reference = NamedEntityDto.Empty,
        Responsible = NamedEntityDto.Empty,
        AssignedTo = NamedEntityDto.Empty,
        PostedBy = NamedEntityDto.Empty,
        PostingTime = order.PostingTime
      };
    }

    #endregion Private methods

  } // class InventoryOrderMapper

} // namespace Empiria.Inventory.Adapters
