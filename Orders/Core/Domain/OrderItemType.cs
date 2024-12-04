/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Power type                              *
*  Type     : OrderItemType                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes an order item.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;

namespace Empiria.Orders {

  /// <summary>Power type that describes an order item.</summary>
  [Powertype(typeof(OrderItem))]
  public sealed class OrderItemType : Powertype {

    #region Constructors and parsers

    private OrderItemType() {
      // Empiria powertype types always have this constructor.
    }

    static public new OrderItemType Parse(int typeId) => Parse<OrderItemType>(typeId);

    static public new OrderItemType Parse(string typeName) => Parse<OrderItemType>(typeName);

    static public FixedList<OrderItemType> GetList() {
      return Empty.GetAllSubclasses()
                  .Select(x => (OrderItemType) x)
                  .ToFixedList();
    }

    static public OrderItemType Empty => Parse("ObjectTypeInfo.OrderItem");

    #endregion Constructors and parsers

  } // class OrderItemType

}  // namespace Empiria.Orders
