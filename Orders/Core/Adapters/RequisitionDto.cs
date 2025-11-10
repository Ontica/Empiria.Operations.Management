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
using Empiria.Payments.Orders.Adapters;

namespace Empiria.Orders.Adapters {

  /// <summary>Data transfer object used to return requisitions.</summary>
  public class RequisitionHolderDto : OrderHolderDto {

    public new FixedList<PayableOrderItemDto> Items {
      get; set;
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

    public FixedList<BillDescriptorDto> Bills {
      get; internal set;
    }

  }  // class RequisitionHolderDto


  public class RequisitionActions : OrderActions {

    public bool CanRequestBudget {
      get; internal set;
    }

  } // class RequisitionActions


  /// <summary>Data transfer object used to return requisition information.</summary>
  public class RequisitionDto : OrderDto {

    protected internal RequisitionDto(Requisition requisition) : base(requisition) {
      Justification = requisition.Justification;
      BudgetType = requisition.BaseBudgetType.MapToNamedEntity();
      Budgets = requisition.Budgets.MapToNamedEntityList();
      Currency = requisition.Currency.MapToNamedEntity();
      Total = requisition.GetTotal();
    }

    public string Justification {
      get; private set;
    }

    public NamedEntityDto BudgetType {
      get; private set;
    }

    public FixedList<NamedEntityDto> Budgets {
      get; private set;
    }

    public NamedEntityDto Currency {
      get; private set;
    }

    public decimal Total {
      get; private set;
    }

  }  // class RequisitionDto



  /// <summary>Output Dto used to return minimal requisition data.</summary>
  public class RequisitionDescriptor : OrderDescriptor {

    protected internal RequisitionDescriptor(Requisition requisition) : base(requisition) {
      BudgetTypeName = requisition.BaseBudgetType.DisplayName;
      BudgetName = requisition.BaseBudget.Name;
      CurrencyName = requisition.Currency.Name;
      Justification = requisition.Justification;
      Total = requisition.GetTotal();
    }

    public string BudgetTypeName {
      get; private set;
    }

    public string BudgetName {
      get; private set;
    }

    public string CurrencyName {
      get; private set;
    }

    public string Justification {
      get; private set;
    }

    public decimal Total {
      get; private set;
    }

  } // class RequisitionDescriptor

}  // namespace Empiria.Orders.Adapters
