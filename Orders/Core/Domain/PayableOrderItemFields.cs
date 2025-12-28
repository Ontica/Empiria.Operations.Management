/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Fields DTO                              *
*  Type     : PayableOrderItemFields                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO fields structure used for update payable order items.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Products;

namespace Empiria.Orders {

  /// <summary>DTO fields structure used for update payable order items.</summary>
  public class PayableOrderItemFields : OrderItemFields {

    public override void EnsureValid() {
      base.EnsureValid();

      Assertion.Require(ProductUnitUID, "Se requiere proporcionar la unidad de medida.");

      var productUnit = ProductUnit.Parse(ProductUnitUID);

      if (productUnit.MoneyBased) {
        Assertion.Require(UnitPrice == 1m, "El precio unitario debe ser igual a uno.");
      } else {
        Assertion.Require(UnitPrice > 0, "El precio unitario debe ser mayor a cero.");
      }
    }

  }  // class PayableOrderItemFields

}  // namespace Empiria.Orders
