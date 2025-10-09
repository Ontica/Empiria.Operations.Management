/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Use Cases Layer                         *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Use case interactor class               *
*  Type     : SalesOrderUseCases                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to update and return sales orders.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders.Adapters;
using Empiria.Procurement.Orders;
using Empiria.Services;


namespace Empiria.Orders.UseCases {

  /// <summary>Use cases used to update and return sales orders.</summary>
  public class SalesOrderUseCases : UseCase {

    //private const int INVENTORYORDERTYPEID = 4013;

    #region Constructors and parsers

    protected SalesOrderUseCases() {
      // no-op
    }

    static public SalesOrderUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<SalesOrderUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public SalesOrderHolderDto ActivateOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = SalesOrder.Parse(orderUID);

      order.Activate();

      order.Save();

      return SalesOrderMapper.Map(order);
    }

    public SalesOrderItemDto CreateOrderItem(string orderUID, SalesOrderItemFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(fields, nameof(fields));

      var order = SalesOrder.Parse(orderUID);

      var item = new SalesOrderItem(OrderItemType.SalesOrderItemType, order);

      item.Update(fields);

      order.AddItem(item);

      item.Save();

      return SalesOrderMapper.Map(item);
    }

    public SalesOrderHolderDto SuspendOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = SalesOrder.Parse(orderUID);

      order.Suspend();

      order.Save();

      return SalesOrderMapper.Map(order);
    }


    public SalesOrderItemDto DeleteOrderItem(string orderUID, string orderItemUID) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));

      var order = SalesOrder.Parse(orderUID);

      var item = order.GetItem<SalesOrderItem>(orderItemUID);

      order.RemoveItem(item);

      item.Save();

      return SalesOrderMapper.Map(item);
    }


    public SalesOrderHolderDto DeleteOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = SalesOrder.Parse(orderUID);

      order.Delete();

      order.Save();

      return SalesOrderMapper.Map(order);
    }


    public SalesOrderHolderDto UpdateOrder(SalesOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var order = SalesOrder.Parse(fields.UID);

      order.Update(fields);

      order.Save();//


      return SalesOrderMapper.Map(order);
    }

    public SalesOrderItemDto UpdateOrderItem(string orderUID, string orderItemUID,
                                               SalesOrderItemFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(fields, nameof(fields));

      var order = SalesOrder.Parse(orderUID);

      var item = order.GetItem<SalesOrderItem>(orderItemUID);

      item.Update(fields);

      item.Save();

      return SalesOrderMapper.Map(item);

    }


    public SalesOrderHolderDto GetSalesOrderByUID(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = SalesOrder.Parse(orderUID);

      return SalesOrderMapper.Map(order);
    }


    #endregion Use cases

    #region Helpers


    #endregion Helpers

  } // class SalesOrderUseCases

} // namespace Empiria.Orders.UseCases
