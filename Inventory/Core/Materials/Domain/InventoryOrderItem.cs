/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrderItem                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an inventory order item.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Locations;
using Empiria.Orders;
using Empiria.Inventory.Data;

namespace Empiria.Inventory {

  /// <summary>Represents an inventory order item.</summary>
  public class InventoryOrderItem : OrderItem {

    #region Constructors and parsers

    protected InventoryOrderItem(OrderItemType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    internal InventoryOrderItem() {
      //no-op
    }

    protected internal InventoryOrderItem(OrderItemType powertype,
                                        InventoryOrder order, Location location) : base(powertype, order) {

      Assertion.Require(location, nameof(location));
      Assertion.Require(!location.IsEmptyInstance, nameof(location));

      this.Location = location;
    }


    static public new InventoryOrderItem Parse(int id) => ParseId<InventoryOrderItem>(id);

    static public new InventoryOrderItem Parse(string uid) => ParseKey<InventoryOrderItem>(uid);

    static public InventoryOrderItem Empty => ParseEmpty<InventoryOrderItem>();

    #endregion Constructors and parsers

    #region Properties
    
    [DataField("ORDER_ITEM_LOCATION_ID")]
    public Location Location {
      get; private set; 
    }


    [DataField("ORDER_ITEM_UNIT_PRICE")]
    public decimal UnitPrice {
      get; private set;
    }


    [DataField("ORDER_ITEM_DISCOUNT")]
    public decimal Discount {
      get; private set;
    }


    [DataField("ORDER_ITEM_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    } 


    internal FixedList<InventoryEntry> Entries {
      get; set;
    }

    #endregion Properties

    #region Methods

    protected override void OnSave() {
      InventoryOrderData.WriteOrderItem(this, this.ExtData.ToString());
    }


    internal void DelItem() {
      base.Delete();
    }
     

    internal void Update(InventoryOrderItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      base.Update(fields);

      this.UnitPrice = InventoryOrderData.GetProductPriceFromVirtualWarehouse(this.Product.Id);
    }


    internal new void Close() {
      base.Close();
    }

    #endregion Methods

  } // class InventoryOrderItem

} // namespace Empiria.Inventory
