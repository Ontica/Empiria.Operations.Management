/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data Transfer Object                    *
*  Type     : ExpensesReportItemDto                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return expenses report items.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders.Adapters {

  /// <summary>Data transfer object used to return expenses report items.</summary>
  public class ExpensesReportItemDto : PayableOrderItemDto {

    protected internal ExpensesReportItemDto(PayableOrderItem item) : base(item) {
      PayableOrderItem = PayableOrderMapper.Map(item);
    }

    public PayableOrderItemDto PayableOrderItem {
      get;
    }

  }  // class ExpensesReportItemDto

}  // namespace Empiria.Orders.Adapters
