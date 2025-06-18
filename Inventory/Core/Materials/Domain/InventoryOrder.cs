/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrder                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an inventory order.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Financial;
using Empiria.Inventory.Data;
using Empiria.Orders;
using Empiria.StateEnums;

namespace Empiria.Inventory {

  /// <summary>Represents an inventory order.</summary>
  public class InventoryOrder : Order {

    #region Constructors and parsers

    internal protected InventoryOrder(OrderType orderType) : base(orderType) {
      // Required by Empiria Framework for all partitioned types.

      base.OrderNo = EmpiriaString.BuildRandomString(8)
                                  .ToUpperInvariant();
    }

    internal InventoryOrder(OrderType orderType, int WarehouseId) : base(orderType) {
      this.WarehouseId = WarehouseId;
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
      get; 
      internal set; 
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
