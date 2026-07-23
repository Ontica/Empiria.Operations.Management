/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data Transfer Object                    *
*  Type     : PayableOrderDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return payable orders information.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders.Adapters {

  /// <summary>Data transfer object used to return complete expenses reports information.</summary>
  public class ExpensesReportHolderDto : PayableOrderHolderDto {

    public new FixedList<ExpensesReportItemDto> Items {
      get; internal set;
    }

  }  // class ExpensesReportHolderDto



  /// <summary>Data transfer object used to return payable orders information.</summary>
  public class ExpensesReportDto : PayableOrderDto {

    protected internal ExpensesReportDto(PayableOrder order) : base(order) {
      PayableOrder = PayableOrderMapper.MapToDescriptor(order);
    }

    public OrderDescriptor PayableOrder {
      get; internal set;
    }

  }  // class ExpensesReportDto

}  // namespace Empiria.Orders.Adapters
