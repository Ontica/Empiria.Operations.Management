/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Power type                              *
*  Type     : OrderType                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes an order.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;

namespace Empiria.Orders {

  /// <summary>Power type that describes an order.</summary>
  [Powertype(typeof(Order))]
  public sealed class OrderType : Powertype {

    #region Constructors and parsers

    private OrderType() {
      // Empiria powertype types always have this constructor.
    }

    static public new OrderType Parse(int typeId) => Parse<OrderType>(typeId);

    static public new OrderType Parse(string typeName) => Parse<OrderType>(typeName);

    static public FixedList<OrderType> GetList() {
      return Empty.GetAllSubclasses()
            .Select(x => (OrderType) x)
            .ToFixedList();
    }

    static public OrderType Empty => Parse("ObjectTypeInfo.Order");

    static public OrderType Contract => Parse("ObjectTypeInfo.Order.Contract");

    static public OrderType ContractOrder => Parse("ObjectTypeInfo.Order.PayableOrder.ContractOrder");

    public static OrderType Requisition => Parse("ObjectTypeInfo.Order.Requisition");

    #endregion Constructors and parsers

  } // class OrderType

}  // namespace Empiria.Orders
