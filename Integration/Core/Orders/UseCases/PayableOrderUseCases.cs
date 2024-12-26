/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Orders Management Integration                 Component : Use cases Layer                      *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Use case interactor class            *
*  Type     : PayableOrderUseCases                          License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to update and return payable orders.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Orders;
using Empiria.Orders.Data;
using Empiria.Orders.Adapters;

using Empiria.Operations.Integration.Orders.Adapters;

namespace Empiria.Operations.Integration.Orders.UseCases {

  /// <summary>Use cases used to update and return payable orders.</summary>
  public class PayableOrderUseCases : UseCase {

    #region Constructors and parsers

    protected PayableOrderUseCases() {
      // no-op
    }

    static public PayableOrderUseCases UseCaseInteractor() {
      return CreateInstance<PayableOrderUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public OrderHolderDto ActivateOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = PayableOrder.Parse(orderUID);

      order.Activate();

      order.Save();

      return OrderHolderMapper.Map(order);
    }


    public OrderHolderDto CreateOrder(PayableOrderFields fields) {
      Assertion.Require(fields, nameof(fields));
      Assertion.Require(fields.OrderTypeUID, nameof(fields.OrderTypeUID));

      fields.EnsureValid();

      var orderType = OrderType.Parse(fields.OrderTypeUID);

      var order = new PayableOrder(orderType);

      order.Update(fields);

      order.Save();

      return OrderHolderMapper.Map(order);
    }


    public OrderHolderDto DeleteOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = PayableOrder.Parse(orderUID);

      order.Delete();

      order.Save();

      return OrderHolderMapper.Map(order);
    }


    public OrderHolderDto GetOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = PayableOrder.Parse(orderUID);

      return OrderHolderMapper.Map(order);
    }


    public FixedList<OrderDescriptor> SearchOrders(OrdersQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      FixedList<Order> orders = OrdersData.Search(filter, sort);

      return OrderHolderMapper.Map(orders);
    }


    public OrderHolderDto SuspendOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = PayableOrder.Parse(orderUID);

      order.Suspend();

      order.Save();

      return OrderHolderMapper.Map(order);
    }


    public OrderHolderDto UpdateOrder(PayableOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var order = PayableOrder.Parse(fields.UID);

      order.Update(fields);

      order.Save();

      return OrderHolderMapper.Map(order);
    }

    #endregion Use cases

  }  // class PayableOrderUseCases

}  // namespace Empiria.Operations.Integration.Orders.UseCases
