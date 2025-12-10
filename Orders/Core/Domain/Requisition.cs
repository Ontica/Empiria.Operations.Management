/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder / Aggregate root     *
*  Type     : Requisition                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a requisition of good and services. A requisition is an aggregate of orders.        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

using Empiria.Orders.Data;

namespace Empiria.Orders {

  /// <summary>Represents a requisition of good and services. A requisition is an aggregate of orders.</summary>
  public class Requisition : Order {

    #region Constructors and parsers

    protected internal Requisition(OrderType orderType) : base(orderType) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new Requisition Parse(int id) => ParseId<Requisition>(id);

    static public new Requisition Parse(string uid) => ParseKey<Requisition>(uid);

    static public new Requisition Empty => ParseEmpty<Requisition>();

    #endregion Constructors and parsers

    #region Methods

    internal FixedList<Order> GetOrders() {
      return OrdersData.GetRequisitionOrders(this);
    }


    public override FixedList<IPayableEntity> GetPayableEntities() {
      return OrdersData.GetRequisitionOrders(this)
                       .FindAll(x => x is IPayableEntity)
                       .Select(x => (IPayableEntity) x)
                       .ToFixedList();
    }


    internal protected void Update(RequisitionFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      base.Update(fields);
    }

    #endregion Methods

  }  // class Requisition

}  // namespace Empiria.Orders
