/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : SalesOrder                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a Sales order.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders {

  /// <summary> Represents a Sales order.</summary>  
  internal class SalesOrder : Order {


    #region Constructors and parsers

    protected SalesOrder(OrderType orderType) : base(orderType) {
      // Required by Empiria Framework for all partitioned types.

      base.OrderNo = EmpiriaString.BuildRandomString(8)
                                  .ToUpperInvariant();
    }

    internal SalesOrder() {
      //no-op
    }

    static public new SalesOrder Parse(int id) => ParseId<SalesOrder>(id);

    static public new SalesOrder Parse(string uid) => ParseKey<SalesOrder>(uid);

    static public new SalesOrder Empty => ParseEmpty<SalesOrder>();

    #endregion Constructors and parsers


  } // class SalesOrder 
} // namespace Empiria.Orders.Domain  
