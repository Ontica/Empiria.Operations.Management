/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : OrderCategory                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an order category which holds orders of the same kind or order type.                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders {

  /// <summary>Represents an order category which holds orders of the same kind or order type.</summary>
  public class OrderCategory : CommonStorage {

    #region Constructors and parsers

    protected OrderCategory() {
      // Required by Empiria Framework
    }

    static public OrderCategory Parse(int id) => ParseId<OrderCategory>(id);

    static public OrderCategory Parse(string uid) => ParseKey<OrderCategory>(uid);

    static public FixedList<OrderCategory> GetList() {
      return GetList<OrderCategory>()
            .ToFixedList();
    }

    static internal FixedList<OrderCategory> GetListFor(OrderType orderType) {
      Assertion.Require(orderType, nameof(orderType));

      return GetFullList<OrderCategory>()
            .FindAll(x => x.OrderTypes.Contains(orderType));
    }

    static public OrderCategory Empty => ParseEmpty<OrderCategory>();

    #endregion Constructors and parsers

    #region Properties

    public FixedList<OrderType> OrderTypes {
      get {
        var orderTypes = ExtData.GetFixedList<OrderType>("orderTypes", false);

        if (orderTypes.Count != 0) {
          return orderTypes;
        }

        return OrderType.GetList();
      }
    }


    public OrderCategory Parent {
      get {
        return base.GetParent<OrderCategory>();
      }
    }


    public override string Keywords {
      get {
        if (IsEmptyInstance) {
          return string.Empty;
        }
        return EmpiriaString.BuildKeywords(base.Keywords, Parent.Keywords);
      }
    }

    #endregion Properties

  } // class OrderCategory

} // namespace Empiria.Orders
