/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data Transfer Object                    *
*  Type     : PayableOrderItemDto                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return payable order items.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders.Adapters {

  /// <summary>Data transfer object used to return payable order items.</summary>
  public class PayableOrderItemDto : OrderItemDto {

    protected internal PayableOrderItemDto(PayableOrderItem item) : base(item) {
      Total = item.Subtotal;
    }

    public decimal Total {
      get; private set;
    }

  }  // class PayableOrderItemDto

}  // namespace Empiria.Orders.Adapters
