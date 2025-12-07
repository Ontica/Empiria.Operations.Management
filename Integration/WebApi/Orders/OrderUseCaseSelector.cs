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
using Empiria.Orders.UseCases;

using Empiria.Procurement.Contracts;
using Empiria.Procurement.Contracts.UseCases;

namespace Empiria.Operations.Integration.Orders {

  /// <summary>Dispatches order actions to their corresponding use cases.</summary>
  static internal class OrderUseCaseSelector {

    static internal OrderHolderDto ActivateOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = Order.Parse(orderUID);

      if (order is Requisition) {
        using (var usecases = RequisitionUseCases.UseCaseInteractor()) {
          return usecases.Activate(order.UID);
        }
      }

      if (order is Contract) {
        using (var usecases = ContractUseCases.UseCaseInteractor()) {
          return usecases.Activate(contractUID: order.UID);
        }
      }

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        return usecases.Activate(order.UID);
      }

    }

    static internal OrderHolderDto CreateOrder(OrderFields fields) {
      Assertion.Require(fields, nameof(fields));
      Assertion.Require(fields.OrderTypeUID, nameof(fields.OrderTypeUID));

      var orderType = OrderType.Parse(fields.OrderTypeUID);


      if (orderType.Equals(OrderType.Requisition)) {
        using (var usecases = RequisitionUseCases.UseCaseInteractor()) {
          return usecases.Create((RequisitionFields) fields);
        }
      }

      if (orderType.Equals(OrderType.Contract)) {
        using (var usecases = ContractUseCases.UseCaseInteractor()) {
          return usecases.CreateContract((ContractFields) fields);
        }
      }

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

      if (order is Requisition) {
        using (var usecases = RequisitionUseCases.UseCaseInteractor()) {
          return usecases.CreateItem(order.UID, (PayableOrderItemFields) orderItemFields);
        }
      }

      if (order is Contract) {
        using (var usecases = ContractUseCases.UseCaseInteractor()) {
          return usecases.AddContractItem(order.UID, (ContractItemFields) orderItemFields);
        }
      }

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

      if (order is Requisition) {
        using (var usecases = RequisitionUseCases.UseCaseInteractor()) {
          return usecases.Delete(order.UID);
        }
      }

      if (order is Contract) {
        using (var usecases = ContractUseCases.UseCaseInteractor()) {
          return usecases.DeleteContract(order.UID);
        }
      }

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

      if (order is Requisition) {
        using (var usecases = RequisitionUseCases.UseCaseInteractor()) {
          return usecases.DeleteItem(order.UID, orderItemUID);
        }
      }

      if (order is Contract) {
        using (var usecases = ContractUseCases.UseCaseInteractor()) {
          return usecases.RemoveContractItem(order.UID, orderItemUID);
        }
      }

      if (order is ContractOrder) {
        using (var usecases = ContractOrderUseCases.UseCaseInteractor()) {
          return usecases.DeleteContractOrderItem(order.UID, orderItemUID);
        }
      }

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        return usecases.DeleteOrderItem(order.UID, orderItemUID);
      }
    }


    static internal FixedList<PayableOrderItemDto> GetAvailableOrderItems(string orderUID, string keywords) {
      Assertion.Require(orderUID, nameof(orderUID));

      keywords = keywords ?? string.Empty;

      var order = Order.Parse(orderUID);

      return PayableOrderMapper.Map(order.Requisition.GetItems<PayableOrderItem>());
    }


    static internal OrderHolderDto GetOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = Order.Parse(orderUID);

      if (order is Requisition) {
        using (var usecases = RequisitionUseCases.UseCaseInteractor()) {
          return usecases.Get(order.UID);
        }
      }

      if (order is Contract) {
        using (var usecases = ContractUseCases.UseCaseInteractor()) {
          return usecases.GetContract(order.UID);
        }
      }

      if (order is ContractOrder) {
        using (var usecases = ContractOrderUseCases.UseCaseInteractor()) {
          return usecases.GetContractOrder(order.UID);
        }
      }

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        return usecases.GetOrder(order.UID);
      }
    }


    static public FixedList<OrderDescriptor> Search(OrdersQuery query) {
      Assertion.Require(query, nameof(query));

      OrderType orderType = OrderType.Parse(query.OrderTypeUID);

      if (orderType.Equals(OrderType.Contract)) {

        using (var usecases = ContractUseCases.UseCaseInteractor()) {
          return usecases.SearchContracts(query)
                         .Select(x => (OrderDescriptor) x)
                         .ToFixedList();
        }

      } else if (orderType.Equals(OrderType.Requisition)) {
        using (var usecases = RequisitionUseCases.UseCaseInteractor()) {
          return usecases.Search(query)
                         .Select(x => (OrderDescriptor) x)
                         .ToFixedList();
        }

      } else {

        using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
          return usecases.SearchOrders(query)
                         .ToFixedList();
        }
      }
    }


    static internal OrderHolderDto SuspendOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = Order.Parse(orderUID);

      if (order is Requisition) {
        using (var usecases = RequisitionUseCases.UseCaseInteractor()) {
          return usecases.Suspend(order.UID);
        }
      }

      if (order is Contract) {
        using (var usecases = ContractUseCases.UseCaseInteractor()) {
          return usecases.Suspend(order.UID);
        }
      }

      using (var usecases = PayableOrderUseCases.UseCaseInteractor()) {
        return usecases.SuspendOrder(order.UID);
      }
    }


    static internal OrderHolderDto UpdateOrder(OrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      var order = Order.Parse(fields.UID);

      if (order is Requisition) {
        using (var usecases = RequisitionUseCases.UseCaseInteractor()) {
          return usecases.Update((RequisitionFields) fields);
        }
      }

      if (order is Contract) {
        using (var usecases = ContractUseCases.UseCaseInteractor()) {
          return usecases.UpdateContract(order.UID, (ContractFields) fields);
        }
      }

      if (order is ContractOrder) {

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

      if (order is Requisition) {
        using (var usecases = RequisitionUseCases.UseCaseInteractor()) {
          return usecases.UpdateItem(order.UID, orderItemUID, (PayableOrderItemFields) fields);
        }
      }

      if (order is Contract) {
        using (var usecases = ContractUseCases.UseCaseInteractor()) {
          return usecases.UpdateContractItem(order.UID, orderItemUID, (ContractItemFields) fields);
        }
      }

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
