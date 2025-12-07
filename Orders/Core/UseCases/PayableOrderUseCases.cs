/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Use Cases Layer                         *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Use case interactor class               *
*  Type     : PayableOrderUseCases                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to update and return payable orders.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Orders.Data;

using Empiria.Orders.Adapters;

namespace Empiria.Orders.UseCases {

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

    public PayableOrderHolderDto Activate(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = PayableOrder.Parse(orderUID);

      order.Activate();

      order.Save();

      return PayableOrderMapper.Map(order);
    }


    public PayableOrderHolderDto CreateOrder(PayableOrderFields fields) {
      Assertion.Require(fields, nameof(fields));
      Assertion.Require(fields.OrderTypeUID, nameof(fields.OrderTypeUID));

      fields.EnsureValid();

      var orderType = OrderType.Parse(fields.OrderTypeUID);

      var order = new PayableOrder(orderType);

      order.Update(fields);

      order.Save();

      return PayableOrderMapper.Map(order);
    }


    public PayableOrderItemDto CreateOrderItem(string orderUID, PayableOrderItemFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(fields, nameof(fields));

      var order = PayableOrder.Parse(orderUID);

      var item = new PayableOrderItem(OrderItemType.PurchaseOrderItemType, order);

      item.Update(fields);

      order.AddItem(item);

      item.Save();

      return PayableOrderMapper.Map(item);
    }


    public PayableOrderHolderDto DeleteOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = PayableOrder.Parse(orderUID);

      order.Delete();

      order.Save();

      return PayableOrderMapper.Map(order);
    }


    public PayableOrderItemDto DeleteOrderItem(string orderUID, string orderItemUID) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));

      var order = PayableOrder.Parse(orderUID);

      var item = order.GetItem<PayableOrderItem>(orderItemUID);

      order.Items.Remove(item);

      item.Save();

      return PayableOrderMapper.Map(item);
    }


    public PayableOrderHolderDto GetOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = PayableOrder.Parse(orderUID);

      return PayableOrderMapper.Map(order);
    }


    public FixedList<OrderDescriptor> SearchOrders(OrdersQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      FixedList<PayableOrder> orders = OrdersData.Search<PayableOrder>(filter, sort);

      return PayableOrderMapper.Map(orders);
    }


    public PayableOrderHolderDto SuspendOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = PayableOrder.Parse(orderUID);

      order.Suspend();

      order.Save();

      return PayableOrderMapper.Map(order);
    }


    public PayableOrderHolderDto UpdateOrder(PayableOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var order = PayableOrder.Parse(fields.UID);

      order.Update(fields);

      order.Save();

      return PayableOrderMapper.Map(order);
    }


    public PayableOrderItemDto UpdateOrderItem(string orderUID, string orderItemUID,
                                               PayableOrderItemFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(fields, nameof(fields));

      var order = PayableOrder.Parse(orderUID);

      var item = order.GetItem<PayableOrderItem>(orderItemUID);

      item.Update(fields);

      item.Save();

      order.Items.Update(item);

      return PayableOrderMapper.Map(item);
    }

    #endregion Use cases

  }  // class PayableOrderUseCases

}  // namespace Empiria.Operations.Integration.Orders.UseCases
