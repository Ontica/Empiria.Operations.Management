/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Orders Management Integration                 Component : Adapters Layer                       *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Mapper                               *
*  Type     : OrderHolderMapper                             License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Maps integrated order information into OrderHolderDto instances.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders;
using Empiria.Orders.Adapters;

using Empiria.Procurement.Contracts;
using Empiria.Procurement.Contracts.Adapters;

namespace Empiria.Operations.Integration.Orders.Adapters {

  /// <summary>Maps integrated order information into OrderHolderDto instances.</summary>
  static public class OrderHolderMapper {

    static internal FixedList<OrderDescriptor> Map(FixedList<Order> orders) {
      return orders.Select(x => MapToDescriptor(x))
                   .ToFixedList();
    }


    static public OrderHolderDto Map(Order order) {
      if (order is ContractOrder contractOrder) {
        return ContractOrderMapper.Map(contractOrder);
      }
      throw Assertion.EnsureNoReachThisCode($"Unhandled order type {order.OrderType.Name}.");
    }


    private static OrderDescriptor MapToDescriptor(Order order) {
      if (order is ContractOrder contractOrder) {
        return ContractOrderMapper.MapToDescriptor(contractOrder);
      }
      throw Assertion.EnsureNoReachThisCode($"Unhandled order type {order.OrderType.Name}.");
    }


  }  // class OrderHolderMapper

}  // namespace Empiria.Operations.Integration.Orders.Adapters
