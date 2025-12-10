/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : SalesOrder                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a Sales order.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Orders;

namespace Empiria.Procurement.Orders {

  /// <summary>Represents a Sales order.</summary>  
  public class SalesOrder : Order {

    #region Constructors and parsers

    internal protected SalesOrder(OrderType orderType) : base(orderType) {
      // Required by Empiria Framework for all partitioned types.

      base.OrderNo = EmpiriaString.BuildRandomString(8)
                                  .ToUpperInvariant();
    }

    static public new SalesOrder Parse(int id) => ParseId<SalesOrder>(id);

    static public new SalesOrder Parse(string uid) => ParseKey<SalesOrder>(uid);

    static public new SalesOrder Empty => ParseEmpty<SalesOrder>();

    #endregion Constructors and parsers

    #region Methods

    internal protected virtual void AddItem(SalesOrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      base.Items.Add(orderItem);
    }


    public FixedList<SalesOrderItem> GetItems() {
      return base.GetItems<SalesOrderItem>();
    }


    public override FixedList<IPayableEntity> GetPayableEntities() {
      return new FixedList<IPayableEntity>();
    }

    internal protected virtual void RemoveItem(SalesOrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      base.Items.Remove(orderItem);
    }


    internal protected void Update(SalesOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      base.Update(fields);
    }

    #endregion Methods

  } // class SalesOrder 
} // namespace Empiria.Orders.Domain  
