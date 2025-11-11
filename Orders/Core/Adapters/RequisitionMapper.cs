/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Mapper                                  *
*  Type     : PayableOrderMapper                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps requisitions to their corresponding DTOs.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;

using Empiria.Payments.Orders.Adapters;

namespace Empiria.Orders.Adapters {

  /// <summary>Maps requisitions to their corresponding DTOs.</summary>
  static public class RequisitionMapper {

    static internal FixedList<RequisitionDescriptor> Map(FixedList<Requisition> requisitions) {
      return requisitions.Select(x => MapToDescriptor(x))
                         .ToFixedList();
    }


    static public RequisitionHolderDto Map(Requisition requisition) {
      return new RequisitionHolderDto {
        Order = new RequisitionDto(requisition),
        Items = Map(requisition.GetItems<PayableOrderItem>()),
        BudgetTransactions = MapBudgetTransactions(requisition),
        Payables = new FixedList<OrderDescriptor>(),
        PaymentOrders = new FixedList<PaymentOrderDescriptor>(),
        Bills = new FixedList<Billing.Adapters.BillDescriptorDto>(),
        Documents = DocumentServices.GetAllEntityDocuments(requisition),
        History = HistoryServices.GetEntityHistory(requisition),
        Actions = MapActions(requisition),
      };
    }


    static private RequisitionDescriptor MapToDescriptor(Requisition requisition) {
      return new RequisitionDescriptor(requisition);
    }


    static private FixedList<PayableOrderItemDto> Map(FixedList<PayableOrderItem> requisitionItems) {
      return requisitionItems.Select(x => Map(x))
                             .ToFixedList();
    }


    static internal PayableOrderItemDto Map(PayableOrderItem requisitionItem) {
      return new PayableOrderItemDto(requisitionItem);
    }

    #region Helpers

    static private RequisitionActions MapActions(Requisition requisition) {
      return new RequisitionActions {
        CanEditDocuments = true,
        CanRequestBudget = true,
        CanActivate = true,
        CanDelete = true,
        CanEditItems = true,
        CanSuspend = true,
        CanUpdate = true,
      };
    }


    static private FixedList<BudgetTransactionDescriptorDto> MapBudgetTransactions(Requisition requisition) {
      FixedList<BudgetTransaction> transactions = BudgetTransaction.GetFor(requisition);

      return BudgetTransactionMapper.MapToDescriptor(transactions);
    }

    #endregion Helpers

  }  // class PayableOrderMapper

}  // namespace Empiria.Orders.Adapters
