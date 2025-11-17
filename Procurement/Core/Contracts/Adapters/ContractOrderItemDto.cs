/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Data Transfer Object                    *
*  Type     : ContractOrderItemDto                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return contract supply order items.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders.Adapters;

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Data transfer object used to return contract supply order items.</summary>
  public class ContractOrderItemDto : PayableOrderItemDto {

    internal ContractOrderItemDto(ContractOrderItem item) : base(item) {
      ContractItem = ContractItemMapper.Map((ContractItem) item.ContractItem);
    }

    public ContractItemDto ContractItem {
      get; private set;
    }

  }  // class ContractOrderItemDto

}  // namespace Empiria.Procurement.Contracts.Adapters
