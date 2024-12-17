/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data Transfer Object                    *
*  Type     : PayableOrderDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return payable orders information.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Orders.Adapters {

  /// <summary>Data transfer object used to return complete payable orders information.</summary>
  public class PayableOrderHolderDto : OrderHolderDto {

    public FixedList<BudgetTransactionDescriptorDto> BudgetTransactions {
      get; internal set;
    }

  }  // class PayableOrderHolderDto


  public class PayableOrderActions : OrderActions {

    public bool CanRequestBudget {
      get; internal set;
    }

  } // class PayableOrderActions


  /// <summary>Data transfer object used to return payable orders information.</summary>
  public class PayableOrderDto : OrderDto {

    public NamedEntityDto Budget {
      get; set;
    }

    public NamedEntityDto Currency {
      get; set;
    }

    public decimal Total {
      get; set;
    }

  }  // class PayableOrderDto



  /// <summary>Output Dto used to return minimal payable order data.</summary>
  public class PayableOrderDescriptor : OrderDescriptor {

    public string BudgetTypeName {
      get; set;
    }

    public string BudgetName {
      get; set;
    }

    public string CurrencyName {
      get; set;
    }

    public decimal Total {
      get; set;
    }

  } // class PayableOrderDescriptor

}  // namespace Empiria.Orders.Adapters
