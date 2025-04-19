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
using System.Linq;
using Empiria.DynamicData;
using Empiria.Inventory.UseCases;
using Empiria.Orders;
using Empiria.Parties;
using Empiria.Products;
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
      return orders.Select(x => MapToDescriptor((InventoryOrder) x))
                   .ToFixedList();
    }


    static private InventoryOrderDescriptorDto MapToDescriptor(InventoryOrder x) {

      var descriptor = new InventoryOrderDescriptorDto();
      descriptor.UID = x.InventoryOrderUID;
      descriptor.OrderTypeName = GetOrderTypeName(x.InventoryOrderTypeId).Name;
      // OrderType.Parse(x.InventoryOrderTypeId).Name;
      descriptor.OrderNo = x.InventoryOrderNo;
      descriptor.ResponsibleName = x.ResponsibleId > 0 ? Party.Parse(x.ResponsibleId).Name : "Sin Asignar";
      descriptor.Description = x.Order_Description;
      descriptor.PostedByName = x.PostedById > 0 ? Party.Parse(x.PostedById).Name : "Sin asignar";
      descriptor.PostingTime = x.PostingTime;
      descriptor.Status = x.Status.GetName();

      return descriptor;
    }

    private static NamedEntityDto GetOrderTypeName(int inventoryOrderTypeId) {

      if (inventoryOrderTypeId == 4005) {

        return new NamedEntityDto("4005", "Orden de compra");
      }
      if (inventoryOrderTypeId == 4008) {

        return new NamedEntityDto("4008", "Orden de venta");
      }
      if (inventoryOrderTypeId == 4009) {

        return new NamedEntityDto("4009", "Orden de devolución de venta");
      } else {

        return NamedEntityDto.Empty;
      }
    }

    #endregion Public methods

    #region Private methods

    static internal InventoryEntryDto MapToInventoryEntryDto(InventoryEntry entry, string productName) {
      return new InventoryEntryDto {
        UID = entry.UID,
        Product = productName,
        Location = InventoryOrderUseCases.GetLocationEntryById(entry.LocationId).Name,
        Quantity = entry.InputQuantity,
        PostedBy = entry.PostedBy.MapToNamedEntity(),
        PostingTime = entry.PostingTime
      };
    }


    private static FixedList<InventoryOrderItemDto> MapToOrderItemsDto(FixedList<InventoryOrderItem> items) {

      return items.Select((x) => MapToOrderItemDto(x))
                         .ToFixedList();
    }


    static private InventoryOrderItemDto MapToOrderItemDto(InventoryOrderItem item) {

      var dto = new InventoryOrderItemDto();
      dto.UID = item.InventoryOrderItemUID;
      dto.ProductName = InventoryOrderUseCases.GetProductEntryById(item.ProductId).Name;
      dto.Quantity = item.ProductQuantity;
      dto.AssignedQuantity = item.Entries.Sum(x => x.InputQuantity);
      dto.PostedBy = new NamedEntityDto(item.PostedBy.UID, item.PostedBy.Name);
      dto.PostingTime = item.PostingTime;
      dto.Entries = MapToInventoryEntriesDto(item.Entries, dto.ProductName);
      dto.Status = item.Status;
      return dto;
    }


    private static FixedList<InventoryEntryDto> MapToInventoryEntriesDto(FixedList<InventoryEntry> items,
                                                                         string productName) {

      return items.Select((x) => MapToInventoryEntryDto(x, productName))
                         .ToFixedList();
    }


    private static InventoryOrderDto MapToOrderDto(InventoryOrder order) {

      return new InventoryOrderDto {
        UID = order.InventoryOrderUID,
        OrderType = GetOrderTypeName(order.InventoryOrderTypeId),
        Description = order.Order_Description,
        OrderNo = order.InventoryOrderNo,
        Responsible = new NamedEntityDto(Party.Parse(order.ResponsibleId).UID,
                                         Party.Parse(order.ResponsibleId).Name),
        PostedBy = new NamedEntityDto(Party.Parse(order.PostedById).UID, Party.Parse(order.PostedById).Name),
        PostingTime = order.PostingTime,
        ClosingTime = order.ClosingTime,
        Status = new NamedEntityDto(order.Status.ToString(), order.Status.GetName())
      };
    }

    #endregion Private methods

  } // class InventoryOrderMapper

} // namespace Empiria.Inventory.Adapters
