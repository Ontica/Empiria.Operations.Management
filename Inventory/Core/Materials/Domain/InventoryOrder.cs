/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrder                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an inventory order.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Inventory.Data;
using Empiria.Locations;
using Empiria.Orders;


namespace Empiria.Inventory {

  /// <summary>Represents an inventory order.</summary>
  public class InventoryOrder : Order {

    #region Constructors and parsers

    protected InventoryOrder(OrderType orderType) : base(orderType) {
     //Requeired by Empiria Framework
    }


    internal InventoryOrder(string warehouseUID ,OrderType orderType) : base(orderType) {
      Assertion.Require(warehouseUID, nameof(warehouseUID));

      this.Warehouse = Location.Parse(warehouseUID);

      base.OrderNo = EmpiriaString.BuildRandomString(8)
                                  .ToUpperInvariant();
    }


    internal InventoryOrder() {
      //no-op
    }

    static public new InventoryOrder Parse(int id) => ParseId<InventoryOrder>(id);

    static public new InventoryOrder Parse(string uid) => ParseKey<InventoryOrder>(uid);

    static public new InventoryOrder Empty => ParseEmpty<InventoryOrder>();

    #endregion Constructors and parsers

    #region Properties

    public Currency Currency {
      get;
      private set;
    } = Currency.Default;


    [DataField("ORDER_LOCATION_ID")]
    public Location Warehouse {
      get;
      private set;
    }


    private FixedList<InventoryOrderItem> _items;

    public FixedList<InventoryOrderItem> Items {
      get {
        return _items ?? (_items = InventoryOrderData.GetInventoryOrderItems(this));
      }
      set {
        _items = value;
      }
    }


    #endregion Properties

    #region Methods

    protected override void OnSave() {
     InventoryOrderData.WriteOrder(this, this.ExtData.ToString());
    }


    internal void AddItem(Location location, InventoryOrderItemFields fields) {

      var orderItemType = Orders.OrderItemType.Parse(4059);

      InventoryOrderItem orderItem = new InventoryOrderItem(orderItemType, this, location);

      orderItem.Update(fields);

      orderItem.Save();

      _items = InventoryOrderData.GetInventoryOrderItems(this);
    }


    internal protected void Update(InventoryOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      base.Update(fields);
    }


    #endregion Methods

  } // class InventoryOrder

} // namespace Empiria.Inventory
