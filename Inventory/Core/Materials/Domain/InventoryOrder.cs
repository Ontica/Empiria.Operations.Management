/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrder                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an inventory order.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Locations;

using Empiria.Orders;

using Empiria.Inventory.Data;

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

      base.OrderNo = "INV-" + EmpiriaString.BuildRandomString(8)
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

    public InventoryType InventoryType {
     get {
        return (InventoryType) base.Category;
      }
    }


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


    internal InventoryOrderItem AddItem(Location location, InventoryOrderItemFields fields) {

      var orderItemType = Orders.OrderItemType.Parse(4059);

      InventoryOrderItem orderItem = new InventoryOrderItem(orderItemType, this, location);

      var position = GetItemPosition();
      fields.Position = position;

      orderItem.Update(fields);

      orderItem.Save();

      _items = InventoryOrderData.GetInventoryOrderItems(this);

      return orderItem;
    }



    internal void CloseItems() {
      foreach (var item in _items) {
        item.Close();
        item.Save();
      }
    }


    internal protected new void Delete() {
      base.Delete();

      this.DeleteItems();
    }


    internal void DeleteItem(string orderItemUID) {
       var orderItem = InventoryOrderItem.Parse(orderItemUID);

      orderItem.DelItem();

      orderItem.Save();

      _items = InventoryOrderData.GetInventoryOrderItems(this);
    }


    internal protected void Update(InventoryOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      fields.CategoryUID = fields.InventoryTypeUID;

      Warehouse = Patcher.Patch(fields.WarehouseUID, Warehouse);

      base.Update(fields);
    }

    #endregion Methods

    #region Helpers

    private void DeleteItems() {

      foreach (var item in this.Items) {
        item.DelItem();
        item.Save();
      }

    }


    private int GetItemPosition() {

      if (this.Items.Count == 0) {
        return 1;
      } else {
        return this.Items.Count + 1;
      }

    }

    #endregion Helpers

  } // class InventoryOrder

} // namespace Empiria.Inventory
