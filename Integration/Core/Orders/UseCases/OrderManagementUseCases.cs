/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Orders Management Integration                 Component : Use cases Layer                      *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Use case interactor class            *
*  Type     : OrderManagementUseCases                       License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to update and return orders information.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Orders;
using Empiria.Orders.Adapters;
using Empiria.Orders.Data;

using Empiria.Procurement.Contracts;
using Empiria.Procurement.Contracts.Adapters;

using Empiria.Operations.Integration.Orders.Adapters;

namespace Empiria.Operations.Integration.Orders.UseCases {

  /// <summary>Use cases used to update and return orders information.</summary>
  public class OrderManagementUseCases : UseCase {

    #region Constructors and parsers

    protected OrderManagementUseCases() {
      // no-op
    }

    static public OrderManagementUseCases UseCaseInteractor() {
      return CreateInstance<OrderManagementUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public OrderHolderDto ActivateOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = Order.Parse(orderUID);

      order.Activate();

      order.Save();

      return OrderHolderMapper.Map(order);
    }


    public OrderHolderDto CreateOrder(ContractOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var orderType = OrderType.Parse(fields.OrderTypeUID);

      PayableOrder order;

      if (orderType.Equals(OrderType.ContractOrder)) {
        var contract = Contract.Parse(fields.ContractUID);
        order = new ContractOrder(contract);
        order.Update(fields);
      } else {
        order = new PayableOrder(orderType);
        order.Update(fields);
      }

      order.Save();

      return OrderHolderMapper.Map(order);
    }


    public OrderHolderDto DeleteOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = Order.Parse(orderUID);

      order.Delete();

      order.Save();

      return OrderHolderMapper.Map(order);
    }


    public OrderHolderDto GetOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = Order.Parse(orderUID);

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

      var order = Order.Parse(orderUID);

      order.Suspend();

      order.Save();

      return OrderHolderMapper.Map(order);
    }

    public OrderHolderDto UpdateOrder(ContractOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var order = PayableOrder.Parse(fields.UID);

      order.Update(fields);

      order.Save();

      return OrderHolderMapper.Map(order);
    }

    #endregion Use cases

  }  // class OrderManagementUseCases

}  // namespace Empiria.Operations.Integration.Orders.UseCases
