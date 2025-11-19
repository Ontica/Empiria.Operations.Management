/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Use Cases Layer                         *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Use case interactor class               *
*  Type     : OrderTaxesUseCases                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to return and update order taxes information.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders.Adapters;

using Empiria.Services;

namespace Empiria.Orders.UseCases {

  /// <summary>Use cases used return and update order taxes information.</summary>
  public class OrderTaxesUseCases : UseCase {

    #region Constructors and parsers

    protected OrderTaxesUseCases() {
      // no-op
    }

    static public OrderTaxesUseCases UseCaseInteractor() {
      return CreateInstance<OrderTaxesUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public OrderTaxEntryDto AddTaxEntry(OrderTaxEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var order = Order.Parse(fields.OrderUID);

      OrderTaxEntry taxEntry = order.AddTaxEntry(fields);

      taxEntry.Save();

      return OrderTaxMapper.Map(taxEntry);
    }


    public OrderTaxEntryDto RemoveTaxEntry(string orderUID, string taxEntryUID) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(taxEntryUID, nameof(taxEntryUID));

      var order = Order.Parse(orderUID);

      OrderTaxEntry taxEntry = order.RemoveTaxEntry(taxEntryUID);

      taxEntry.Save();

      return OrderTaxMapper.Map(taxEntry);
    }


    public OrderTaxEntryDto UpdateTaxEntry(OrderTaxEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var order = Order.Parse(fields.OrderUID);

      OrderTaxEntry taxEntry = order.UpdateTaxEntry(fields);

      taxEntry.Save();

      return OrderTaxMapper.Map(taxEntry);
    }

    #endregion Use cases

  }  // class OrderTaxesUseCases

}  // namespace Empiria.Orders.UseCases
