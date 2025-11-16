/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrderItem                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an inventory order item.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

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

    protected internal InventoryOrderItem(OrderItemType powertype,
                                          InventoryOrder order) : base(powertype, order) {
    }

    protected internal InventoryOrderItem(OrderItemType powertype,
                                          InventoryOrder order, Location location) : base(powertype, order) {

      Assertion.Require(location, nameof(location));
      Assertion.Require(!location.IsEmptyInstance, nameof(location));

      base.Location = location;
    }


    static public new InventoryOrderItem Parse(int id) => ParseId<InventoryOrderItem>(id);

    static public new InventoryOrderItem Parse(string uid) => ParseKey<InventoryOrderItem>(uid);

    static public InventoryOrderItem Empty => ParseEmpty<InventoryOrderItem>();

    #endregion Constructors and parsers

    #region Properties

    internal FixedList<InventoryEntry> Entries {
      get; set;
    }

    #endregion Properties

    #region Methods


    internal void DelItem() {
      base.Delete();
    }


    internal void Update(InventoryOrderItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      fields.UnitPrice = GetProductPrice();

      base.Update(fields);
    }


    internal new void UpdateQuantity(decimal quantity) {

      base.UpdateQuantity(quantity);
    }


    internal new void Close() {
      base.Close();
    }

    #endregion Methods

    #region Helpers

    private decimal GetProductPrice() {

      var unitPrice = InventoryOrderData.GetProductPriceFromVirtualWarehouse(this.Product.Id);

      if (unitPrice == 0) {
        return InventoryOrderData.GetProductPriceFromHistoricCost(this.Product.Name);
      } else {
        return unitPrice;
      }
    }

    #endregion Helpers

  } // class InventoryOrderItem

} // namespace Empiria.Inventory
