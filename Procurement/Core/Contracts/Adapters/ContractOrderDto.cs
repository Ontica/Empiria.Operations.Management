/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Data Transfer Object                    *
*  Type     : ContractOrderDto                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return a contract supply order.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders.Adapters;

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Data transfer object used to return complete contract supply orders information.</summary>
  public class ContractOrderHolderDto : PayableOrderHolderDto {

    public new FixedList<ContractOrderItemDto> Items {
      get; set;
    }

  }  // class ContractOrderHolderDto


  /// <summary>Data transfer object used to return a contract supply order.</summary>
  public class ContractOrderDto : PayableOrderDto {

    internal ContractOrderDto(ContractOrder order) : base(order) {
      Contract = ContractMapper.MapContract(order.Contract);
    }

    public ContractDto Contract {
      get; private set;
    }

  }  // class ContractOrderDto



  /// <summary>Output Dto used to return minimal contract supply order data.</summary>
  public class ContractOrderDescriptor : OrderDescriptor {

    internal ContractOrderDescriptor(ContractOrder order) : base(order) {
      ContractNo = order.Contract.ContractNo;
      ContractName = order.Contract.Name;
    }

    public string ContractNo {
      get; private set;
    }

    public string ContractName {
      get; private set;
    }

  } // class ContractOrderDescriptor

}  // namespace Empiria.Procurement.Contracts.Adapters
