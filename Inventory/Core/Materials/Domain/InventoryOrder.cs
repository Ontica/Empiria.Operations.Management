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
using Empiria.Orders;


namespace Empiria.Inventory {

  /// <summary>Represents an inventory order.</summary>
  public class InventoryOrder : Order {

    #region Constructors and parsers

    protected InventoryOrder(OrderType orderType) : base(orderType) {
     //Requeired by Empiria Framework
    }

    internal InventoryOrder(int warehouseId ,OrderType orderType) : base(orderType) {
      Assertion.Require(warehouseId, nameof(warehouseId));

      this.WarehouseId = warehouseId;

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

    public Currency currency {
      get;
      set;
    } = Currency.Default;


    public int WarehouseId {
      get {
        return base.ExtData.Get("warehouseId", 0);
      }
      private set {
        base.ExtData.SetIfValue("warehouseId", value);
      }
    }


    public FixedList<InventoryOrderItem> Items {
      get; internal set;
    }

    #endregion Properties

    #region Methods

    protected override void OnSave() {
    
     InventoryOrderData.WriteOrder(this, this.ExtData.ToString());
    }


    internal protected void Update(InventoryOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      base.Update(fields);
    }

    #endregion Methods

  } // class InventoryOrder

} // namespace Empiria.Inventory
