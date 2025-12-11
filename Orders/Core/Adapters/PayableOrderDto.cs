/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data Transfer Object                    *
*  Type     : PayableOrderDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return payable orders information.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Billing.Adapters;
using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Payments.Adapters;

namespace Empiria.Orders.Adapters {

  /// <summary>Data transfer object used to return complete payable orders information.</summary>
  public class PayableOrderHolderDto : OrderHolderDto {

    public new FixedList<PayableOrderItemDto> Items {
      get; internal set;
    }

    public FixedList<OrderTaxEntryDto> Taxes {
      get; internal set;
    }

    public FixedList<BudgetTransactionDescriptorDto> BudgetTransactions {
      get; internal set;
    }

    public FixedList<BillDto> Bills {
      get; internal set;
    }

    public FixedList<PaymentOrderDescriptor> PaymentOrders {
      get; internal set;
    }

  }  // class PayableOrderHolderDto


  /// <summary>Data transfer object used to return payable orders information.</summary>
  public class PayableOrderDto : OrderDto {

    protected internal PayableOrderDto(PayableOrder order) : base(order) {
      Budget = order.BaseBudget.MapToNamedEntity();
    }

    public NamedEntityDto Budget {
      get;
    }

  }  // class PayableOrderDto

}  // namespace Empiria.Orders.Adapters
