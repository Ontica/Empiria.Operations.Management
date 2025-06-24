/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : OrderCategory                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an order category which holds orders of the same kind or order type.                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

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
      return BaseObject.GetList<OrderCategory>()
                       .ToFixedList();
    }

    internal static FixedList<OrderCategory> GetListFor(OrderType orderType) {
      Assertion.Require(orderType, nameof(orderType));

      return BaseObject.GetFullList<OrderCategory>()
                       .FindAll(x => x.OrderType.Equals(orderType));
    }

    static public OrderCategory Empty => ParseEmpty<OrderCategory>();

    #endregion Constructors and parsers

    #region Properties

    public OrderType OrderType {
      get {
        return base.ExtData.Get("orderTypeId", OrderType.Empty);
      }
    }

    
    public bool IsAssignable {
      get {
        return base.ExtData.Get("isAssignable", true);
      }
    }


    public OrderCategory Parent {
      get {
        return base.GetParent<OrderCategory>();
      }
    }


    public override string Keywords {
      get {
        if (this.IsEmptyInstance) {
          return string.Empty;
        }
        return EmpiriaString.BuildKeywords(Name, OrderType.DisplayName, Parent.Keywords);
      }
    }

    #endregion Properties

  } // class OrderCategory

} // namespace Empiria.Orders
