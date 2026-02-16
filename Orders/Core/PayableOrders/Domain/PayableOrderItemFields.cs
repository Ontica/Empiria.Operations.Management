/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Fields DTO                              *
*  Type     : PayableOrderItemFields                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO fields structure used for update payable order items.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders {

  /// <summary>DTO fields structure used for update payable order items.</summary>
  public class PayableOrderItemFields : OrderItemFields {

    public override void EnsureValid() {
      base.EnsureValid();
    }

  }  // class PayableOrderItemFields

}  // namespace Empiria.Orders
