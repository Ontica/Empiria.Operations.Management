/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data Transfer Object                    *
*  Type     : PayableOrderItemDto                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return payable order items information.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders.Adapters {

  /// <summary>Data transfer object used to return payable order items information.</summary>
  public class PayableOrderItemDto : OrderItemDto {

    protected internal PayableOrderItemDto(PayableOrderItem item) : base(item) {
      BudgetAccount = item.BudgetAccount.MapToNamedEntity();
      UnitPrice = item.UnitPrice;
      Total = item.Total;
    }

    public NamedEntityDto BudgetAccount {
      get; private set;
    }

    public decimal UnitPrice {
      get; private set;
    }

    public decimal Total {
      get; private set;
    }

  }  // class PayableOrderItemDto

}  // namespace Empiria.Orders.Adapters
