/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Output DTO                              *
*  Type     : RequisitionHolderDto, RequisitionDto       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return requisitions.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Billing.Adapters;
using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Payments.Adapters;

namespace Empiria.Orders.Adapters {

  /// <summary>Data transfer object used to return requisitions.</summary>
  public class RequisitionHolderDto : OrderHolderDto {

    public new RequisitionDto Order {
      get; internal set;
    }

    public new FixedList<PayableOrderItemDto> Items {
      get; set;
    }

    public FixedList<OrderTaxEntryDto> Taxes {
      get; internal set;
    }

    public FixedList<BudgetTransactionDescriptorDto> BudgetTransactions {
      get; internal set;
    }

    public FixedList<OrderDescriptor> Orders {
      get; internal set;
    }

    public FixedList<PaymentOrderDescriptor> PaymentOrders {
      get; internal set;
    }

    public FixedList<BillDto> Bills {
      get; internal set;
    }

  }  // class RequisitionHolderDto


  /// <summary>Data transfer object used to return requisition information.</summary>
  public class RequisitionDto : OrderDto {

    protected internal RequisitionDto(Requisition requisition) : base(requisition) {

    }

  }  // class RequisitionDto



  /// <summary>Output Dto used to return minimal requisition data.</summary>
  public class RequisitionDescriptor : OrderDescriptor {

    protected internal RequisitionDescriptor(Requisition requisition) : base(requisition) {

    }

  } // class RequisitionDescriptor

}  // namespace Empiria.Orders.Adapters
