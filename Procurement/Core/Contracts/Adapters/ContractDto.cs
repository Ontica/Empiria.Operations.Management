/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Data Transfer Object                    *
*  Type     : ContractDto                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return contracts information.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Billing.Adapters;
using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Orders.Adapters;

namespace Empiria.Procurement.Contracts.Adapters {

  public class ContractHolderDto : OrderHolderDto {

    public new ContractDto Order {
      get; internal set;
    }

    public new FixedList<ContractItemDto> Items {
      get; internal set;
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


    public FixedList<ContractOrderDescriptor> Payables {
      get; internal set;
    }


    public BillsStructureDto Bills {
      get; internal set;
    }

  }  // class ContractHolderDto



  /// <summary>Data transfer object used to return contracts information.</summary>
  public class ContractDto : OrderDto {

    internal ContractDto(Contract contract) : base(contract) {
      ContractNo = contract.ContractNo;
      SignDate = contract.SignDate;
      MinTotal = contract.MinTotal;
      MaxTotal = contract.MaxTotal;
    }


    public string ContractNo {
      get;
    }


    public DateTime SignDate {
      get;
    }

    public decimal MinTotal {
      get;
    }

    public decimal MaxTotal {
      get;
    }

  }  // class ContractDto


  /// Output Dto used to return minimal contract data
  public class ContractDescriptor : OrderDescriptor {

    internal ContractDescriptor(Contract contract) : base(contract) {
      ContractNo = contract.ContractNo;
      SignDate = contract.SignDate;
      MinTotal = contract.MinTotal;
      MaxTotal = contract.MaxTotal;
    }


    public string ContractNo {
      get;
    }

    public DateTime SignDate {
      get;
    }

    public decimal MinTotal {
      get;
    }

    public decimal MaxTotal {
      get;
    }

  } // class ContractDescriptor

}  // namespace Empiria.Procurement.Contracts.Adapters
