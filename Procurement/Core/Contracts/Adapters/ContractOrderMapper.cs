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

using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions;

using Empiria.Orders.Adapters;

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Provides data mapping services for contract supply orders.</summary>
  static internal class ContractOrderMapper {

    static internal ContractOrderHolderDto Map(ContractOrder order) {
      return new ContractOrderHolderDto {
        Order = new ContractOrderDto(order),
        Items = new FixedList<Orders.OrderItem>(),
        BudgetTransactions = MapBudgetTransactions(order),
        Documents = DocumentServices.GetEntityDocuments(order),
        History = HistoryServices.GetEntityHistory(order),
        Actions = MapActions(),
      };
    }


    static internal FixedList<ContractOrderDescriptor> MapToDescriptor(FixedList<ContractOrder> orders) {
      return orders.Select(x => MapToDescriptor(x))
                   .ToFixedList();
    }

    static internal ContractOrderDescriptor MapToDescriptor(ContractOrder order) {
      return new ContractOrderDescriptor(order);
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

    static private PayableOrderActions MapActions() {
      return new PayableOrderActions {
        CanEditDocuments = true,
        CanRequestBudget = true,
      };
    }


    static private FixedList<BudgetTransactionDescriptorDto> MapBudgetTransactions(ContractOrder order) {
      FixedList<BudgetTransaction> transactions = BudgetTransaction.GetFor(order);

      return BudgetTransactionMapper.MapToDescriptor(transactions);
    }

    #endregion Helpers

  }  // class ContractOrderMapper

}  // namespace Empiria.Procurement.Contracts.Adapters
