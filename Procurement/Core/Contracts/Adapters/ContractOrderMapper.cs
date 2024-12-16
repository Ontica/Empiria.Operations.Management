/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Mapper                                  *
*  Type     : ContractOrderMapper                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for contract supply orders.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

using Empiria.Documents.Services;
using Empiria.History.Services;

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Provides data mapping services for contract supply orders.</summary>
  static internal class ContractOrderMapper {

    static internal FixedList<ContractOrderDto> Map(FixedList<ContractOrder> orders) {
      return orders.Select(x => Map(x))
                   .ToFixedList();
    }


    static internal ContractOrderDto Map(ContractOrder order) {
      return new ContractOrderDto {
        UID = order.UID,
        Contract = order.Contract.MapToNamedEntity(),
        OrderNo = order.OrderNo,
        Description = order.Description,
        ManagedByOrgUnit = order.Responsible.MapToNamedEntity(),
        Supplier = order.Provider.MapToNamedEntity(),
        Total = order.GetTotal(),
        Status = order.Status.MapToDto(),
        Items = Map(order.GetItems<ContractOrderItem>()),
        Documents = DocumentServices.GetEntityDocuments(order),
        History = HistoryServices.GetEntityHistory(order),
        Actions = MapActions()
      };
    }


    static internal FixedList<ContractOrderDescriptor> MapToDescriptor(FixedList<ContractOrder> orders) {
      return orders.Select(x => MapToDescriptor(x))
                   .ToFixedList();

    }


    static internal FixedList<ContractOrderItemDto> Map(FixedList<ContractOrderItem> orderItems) {
      return orderItems.Select(x => Map(x))
                       .ToFixedList();
    }


    static internal ContractOrderItemDto Map(ContractOrderItem orderItem) {
      return new ContractOrderItemDto {
        UID = orderItem.UID,
        Order = orderItem.Order.MapToNamedEntity(),
        OrderType = orderItem.Order.OrderType.MapToNamedEntity(),
        ContractItem = orderItem.ContractItem.MapToNamedEntity(),
        Description = orderItem.Description,
        Quantity = orderItem.Quantity,
        ProductUnit = orderItem.ProductUnit.MapToNamedEntity(),
        Product = orderItem.Product.MapToNamedEntity(),
        UnitPrice = orderItem.UnitPrice,
        BudgetAccount = orderItem.BudgetAccount.MapToNamedEntity(),
        Status = orderItem.Status.MapToDto()
      };

    }

    #region Helpers

    static private BaseActions MapActions() {
      return new BaseActions {
        CanEditDocuments = true
      };
    }


    static private ContractOrderDescriptor MapToDescriptor(ContractOrder order) {
      return new ContractOrderDescriptor {
        UID = order.UID,
        ContractUID = order.Contract.UID,
        OrderNo = order.OrderNo,
        Description = order.Description,
        Provider = order.Provider.Name,
        Beneficiary = order.Beneficiary.Name,
        Responsible = order.Responsible.Name,
        Total = order.GetTotal(),
        StatusName = order.Status.GetName()
      };
    }

    #endregion Helpers

  }  // class ContractOrderMapper

}  // namespace Empiria.Procurement.Contracts.Adapters
