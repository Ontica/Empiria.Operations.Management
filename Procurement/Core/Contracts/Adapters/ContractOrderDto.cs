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

  /// <summary>Data transfer object used to return a contract supply order.</summary>
  public class ContractOrderDto : PayableOrderDto {

    internal ContractOrderDto(ContractOrder order) : base(order) {
      Contract = ContractMapper.MapToDescriptor(order.Contract);
    }

    public ContractDescriptor Contract {
      get; private set;
    }

  }  // class ContractOrderDto


  /// <summary>Output Dto used to return minimal contract supply order data.</summary>
  public class ContractOrderDescriptor : PayableOrderDescriptor {

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
