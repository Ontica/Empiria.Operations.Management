﻿/* Empiria Operations ****************************************************************************************
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
using System.Linq;

using Empiria.DynamicData;
using Empiria.Orders;
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

      columns.Add(new DataTableColumn("inventoryTypeName", "Tipo", "text"));
      columns.Add(new DataTableColumn("orderNo", "Orden", "text-link"));
      columns.Add(new DataTableColumn("warehouseName", "Almacén", "text"));
      columns.Add(new DataTableColumn("responsibleName", "Responsable", "text"));
      columns.Add(new DataTableColumn("documentNo", "No. Documento", "text"));
      columns.Add(new DataTableColumn("stakeholderName", "Cliente/Proveedor", "text"));
      columns.Add(new DataTableColumn("postingTime", "Registro", "date"));
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

    #region Helpers

    static private InventoryOrderDescriptorDto MapToDescriptor(InventoryOrder order) {

      return new InventoryOrderDescriptorDto() {
        UID = order.UID,
        OrderTypeName = order.OrderType.DisplayName,
        OrderNo = order.OrderNo,
        InventoryTypeName = order.InventoryType.Name,
        WarehouseName = order.Warehouse.Name,
        ResponsibleName = order.Responsible.IsEmptyInstance ? "Sin Asignar" : order.Responsible.Name,
        RequestedByName = order.RequestedBy.Name,
        Description = order.Description,
        DocumentNo = GetDocumentNo(order),
        StakeholderName = GetStakeholderName(order),
        PostedByName = order.PostedBy.Name,
        PostingTime = order.PostingTime,
        Status = order.Status.GetName()
      };
    }

    
   static internal InventoryEntryDto MapToInventoryEntryDto(InventoryEntry entry) {
      decimal quantity = 0;

      if (entry.Order.Category.UID == "0eb5a072-b857-4071-8b06-57a34822ec64") {
        quantity = entry.OutputQuantity;
      } else {
        quantity = entry.InputQuantity;
      }

      return new InventoryEntryDto {
        UID = entry.UID,
        Product = entry.Product.Name,
        Location = entry.Location.Name,
        Quantity = quantity,
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
        Location = item.Location.Name,
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
        OrderNo = order.OrderNo,
        OrderType = order.OrderType.MapToNamedEntity(), // new NamedEntityDto("X","Orden de inventario"),
        InventoryType = MapToInventoryTypeDto(order.InventoryType),
        Warehouse = order.Warehouse.MapToNamedEntity(),
        Responsible = order.Responsible.MapToNamedEntity(),
        RequestedBy = order.RequestedBy.MapToNamedEntity(),
        Description = order.Description,
        PostedBy = order.PostedBy.MapToNamedEntity(),
        PostingTime = order.PostingTime,
        ClosingTime = order.ClosingTime,
        Status = order.Status.MapToDto()
      };
    }


    private static InventoryTypeDto MapToInventoryTypeDto(InventoryType inventoryType) {

      return new InventoryTypeDto {
        UID = inventoryType.UID,
        Name = inventoryType.Name,
        Rules = MapInventoryTypeRules(inventoryType),
      };
    }


    private static InventoryTypeRulesDto MapInventoryTypeRules(InventoryType inventoryType) {

      return new InventoryTypeRulesDto {
        EntriesRequired = inventoryType.EntriesRequired,
        ItemsRequired = inventoryType.ItemsRequired,
      };
    }


    static private string GetDocumentNo(InventoryOrder inventoryOrder) {

      if (inventoryOrder.RelatedOrderId == -1) {
        return string.Empty;
      }

      var order = Order.Parse(inventoryOrder.RelatedOrderId);
      return order.OrderNo;
    }


    private static string GetStakeholderName(InventoryOrder inventoryOrder) {
      if (inventoryOrder.RelatedOrderId == -1) {
        return string.Empty;
      }

      var order = Order.Parse(inventoryOrder.RelatedOrderId);

      if (order.Category.UID == "a40c65bd-9a56-48eb-a8bf-f9245ecd3004") {
        return order.Provider.Name;
      } else {
        return order.Beneficiary.Name;
      }
    }

    #endregion helpers

  } // class InventoryOrderMapper

} // namespace Empiria.Inventory.Adapters
