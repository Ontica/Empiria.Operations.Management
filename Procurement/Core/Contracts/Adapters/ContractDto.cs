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

using Empiria.Documents;
using Empiria.History;

using Empiria.Orders.Adapters;

using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Procurement.Contracts.Adapters {

  public class ContractHolderDto {

    public ContractDto Contract {
      get; internal set;
    }

    public FixedList<ContractItemDto> Items {
      get; internal set;
    }

    public FixedList<BudgetTransactionDescriptorDto> BudgetTransactions {
      get; internal set;
    }

    public FixedList<ContractOrderDescriptor> Orders {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }

    public ContractActions Actions {
      get; internal set;
    }

  }  // class ContractHolderDto



  public class ContractActions : BaseActions {

    public bool CanActivate {
      get; internal set;
    }

    public bool CanEditItems {
      get; internal set;
    }

    public bool CanSuspend {
      get; internal set;
    }

  } // class ContractActions



  /// <summary>Data transfer object used to return contracts information.</summary>
  public class ContractDto : OrderDto {

    internal ContractDto(Contract contract) : base(contract) {
      ContractNo = contract.ContractNo;


      SignDate = contract.SignDate;
      BudgetType = contract.BudgetType.MapToNamedEntity();
      Budgets = contract.Budgets.MapToNamedEntityList();
      MinTotal = contract.MinTotal;
      MaxTotal = contract.MaxTotal;
    }


    public string ContractNo {
      get;
    }


    public DateTime SignDate {
      get;
    }

    public NamedEntityDto BudgetType {
      get;
    }

    public FixedList<NamedEntityDto> Budgets {
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
