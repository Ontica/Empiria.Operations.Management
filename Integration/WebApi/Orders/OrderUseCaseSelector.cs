/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Orders Management Integration                 Component : Web Api Layer                        *
*  Assembly : Empiria.Operations.Integration.WebApi.dll     Pattern   : Selector                             *
*  Type     : OrderUseCaseSelector                          License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Dispatches order actions to their corresponding use cases.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders;
using Empiria.Orders.Adapters;

using Empiria.Procurement.Contracts;
using Empiria.Procurement.Contracts.UseCases;

using Empiria.Operations.Integration.Orders.UseCases;
using System;

namespace Empiria.Operations.Integration.Orders {

  /// <summary>Dispatches order actions to their corresponding use cases.</summary>
  static internal class OrderUseCaseSelector {

    static internal OrderHolderDto CreateOrder(OrderFields fields) {
      Assertion.Require(fields, nameof(fields));
      Assertion.Require(fields.OrderTypeUID, nameof(fields.OrderTypeUID));

      var orderType = OrderType.Parse(fields.OrderTypeUID);

      if (orderType.Equals(OrderType.ContractOrder)) {

        using (var usecases = ContractOrderUseCases.UseCaseInteractor()) {
          return usecases.CreateContractOrder((ContractOrderFields) fields);
        }
      }

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        return usecases.CreateOrder((PayableOrderFields) fields);
      }
    }


    static internal OrderItemDto CreateOrderItem(string orderUID, OrderItemFields orderItemFields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemFields, nameof(orderItemFields));

      var order = Order.Parse(orderUID);

      if (order is ContractOrder) {
        using (var usecases = ContractOrderUseCases.UseCaseInteractor()) {
          return usecases.CreateContractOrderItem(order.UID, (ContractOrderItemFields) orderItemFields);
        }
      }

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        return usecases.CreateOrderItem(order.UID, (PayableOrderItemFields) orderItemFields);
      }
    }


    static internal OrderHolderDto DeleteOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = Order.Parse(orderUID);

      if (order is ContractOrder) {
        using (var usecases = ContractOrderUseCases.UseCaseInteractor()) {
          return usecases.DeleteContractOrder(order.UID);
        }
      }

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        return usecases.DeleteOrder(order.UID);
      }
    }


    static internal OrderItemDto DeleteOrderItem(string orderUID, string orderItemUID) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));

      var order = Order.Parse(orderUID);

      if (order is ContractOrder) {
        using (var usecases = ContractOrderUseCases.UseCaseInteractor()) {
          return usecases.DeleteContractOrderItem(order.UID, orderItemUID);
        }
      }

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        return usecases.DeleteOrderItem(order.UID, orderItemUID);
      }
    }


    static internal OrderHolderDto GetOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = Order.Parse(orderUID);

      if (order is ContractOrder) {
        using (var usecases = ContractOrderUseCases.UseCaseInteractor()) {
          return usecases.GetContractOrder(order.UID);
        }
      }

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        return usecases.GetOrder(order.UID);
      }
    }


    static internal OrderHolderDto UpdateOrder(OrderFields fields) {
      Assertion.Require(fields, nameof(fields));
      Assertion.Require(fields.OrderTypeUID, nameof(fields.OrderTypeUID));

      var orderType = OrderType.Parse(fields.OrderTypeUID);

      if (orderType.Equals(OrderType.ContractOrder)) {

        using (var usecases = ContractOrderUseCases.UseCaseInteractor()) {
          return usecases.UpdateContractOrder((ContractOrderFields) fields);
        }
      }

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        return usecases.UpdateOrder((PayableOrderFields) fields);
      }
    }


    static internal OrderItemDto UpdateOrderItem(string orderUID, string orderItemUID, OrderItemFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));
      Assertion.Require(fields, nameof(fields));

      var order = Order.Parse(orderUID);

      if (order is ContractOrder) {
        using (var usecases = ContractOrderUseCases.UseCaseInteractor()) {
          return usecases.UpdateContractOrderItem(order.UID, orderItemUID, (ContractOrderItemFields) fields);
        }
      }

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        return usecases.UpdateOrderItem(order.UID, orderItemUID, (PayableOrderItemFields) fields);
      }
    }

  }  // class OrderUseCaseSelector

} // namespace Empiria.Operations.Integration.Orders
