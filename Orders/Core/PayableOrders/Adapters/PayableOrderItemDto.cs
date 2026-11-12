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


    protected internal PayableOrderItemDto(PayableOrderItem item,
                                           decimal requestedTotal,
                                           decimal availableTotal) : this(item) {
      Total = item.Subtotal;
      RequestedTotal = requestedTotal;
      AvailableTotal = availableTotal;
    }

    public decimal Total {
      get;
    }

    public decimal RequestedTotal {
      get;
    }

    public decimal AvailableTotal {
      get;
    }

  }  // class PayableOrderItemDto

}  // namespace Empiria.Orders.Adapters
