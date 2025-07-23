/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : SalesOrder                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a Sales order.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Financial;
using Empiria.Orders.Data;

namespace Empiria.Orders {

  /// <summary>Represents a Sales order.</summary>  
  public class SalesOrder : Order {

    #region Constructors and parsers

    internal protected SalesOrder(OrderType orderType) : base(orderType) {
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

    #region Properties

    [DataField("ORDER_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }

    #endregion Properties

    #region Methods

    internal protected virtual void AddItem(SalesOrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      base.AddItem(orderItem);
    }


    public FixedList<SalesOrderItem> GetItems(SalesOrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));
      return base.GetItems<SalesOrderItem>();
    }

    public decimal GetTotal() {
      return base.GetItems<SalesOrderItem>()
                  .Sum(x => x.Total);
    }


    protected override void OnSave() {
      SalesOrdersData.WriteSalesOrder(this, this.ExtData.ToString());
    }
 
    internal protected virtual void RemoveItem(SalesOrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      base.RemoveItem(orderItem);
    }


    internal protected void Update(SalesOrderFields fields) {
      Assertion.Require(fields, nameof(fields));
      base.Update(fields);
    }

    #endregion Methods

  } // class SalesOrder 
} // namespace Empiria.Orders.Domain  
