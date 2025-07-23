/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Sales Orders Management                    Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Mapper                                  *
*  Type     : SalesOrderMapper                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps sales orders and their order items to their corresponding DTOs.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders.Adapters {

  /// <summary>Mapping methods for sales order.</summary>
  static public class SalesOrderMapper {

    static internal FixedList<SalesOrderDescriptor> Map(FixedList<Order> orders) {
      return orders.Select(x => MapToDescriptor((SalesOrder) x))
                   .ToFixedList();
    }

    static public SalesOrderHolderDto Map(SalesOrder order) {
      return new SalesOrderHolderDto {
        Order = new SalesOrderDto(order),
        Items = Map(order.GetItems<SalesOrderItem>()),
        Actions = MapActions(order),
      };
    }


    private static SalesOrderDescriptor MapToDescriptor(SalesOrder order) {
      return new SalesOrderDescriptor(order);
    }

    
    static internal FixedList<SalesOrderItemDto> Map(FixedList<SalesOrderItem> orderItems) {
        return orderItems.Select(x => Map(x))
                         .ToFixedList();
      }

      static internal SalesOrderItemDto Map(SalesOrderItem orderItem) {
        return new SalesOrderItemDto(orderItem);
      }

    #region Helpers

    static private SalesOrderActions MapActions(SalesOrder order) {
      return new SalesOrderActions {
        CanEdit = true,
        CanEditEntries = true,
        CanClose = true,
        CanDelete = true,
        CanEditItems = true,
        CanOpen = true,
      };
    }
    
        static internal SalesOrderDto MapToSalesOrderDto(SalesOrder order) {
           return new SalesOrderDto {
           //UID = order.UID,
           //Type = order.OrderType.MapToNamedEntity(),
           Category = order.Category.MapToNamedEntity(),
           OrderNo = order.OrderNo,
           Description = order.Description,
           Identificators = order.Identificators,
           Tags = order.Tags,
           Responsible = order.Responsible.MapToNamedEntity(),
           Beneficiary = order.Beneficiary.MapToNamedEntity(),
           IsForMultipleBeneficiaries = order.IsForMultipleBeneficiaries,
           Provider = order.Provider.MapToNamedEntity(),  
           RequestedBy = order.RequestedBy.MapToNamedEntity(),
           Project = order.Project.MapToNamedEntity(),
           //Priority = order.Priority.MapToDto(),
           AuthorizationTime = order.AuthorizationTime,
           AuthorizedBy = order.AuthorizedBy.MapToNamedEntity(),
           ClosingTime = order.ClosingTime,
           ClosedBy = order.ClosedBy.MapToNamedEntity(),
           //Status = order.Status.MapToDto()
         };
       }
    #endregion Helpers

  }// class SalesOrderMapper
}// namespace Empiria.Orders.Adapters

