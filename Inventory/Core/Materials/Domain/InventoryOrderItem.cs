/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrderItem                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an inventory order item.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Inventory.Data;
using Empiria.Orders;
using Empiria.Parties;
using Empiria.Products;
using Empiria.StateEnums;

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

    static public new InventoryOrderItem Parse(int id) => ParseId<InventoryOrderItem>(id);

    static public new InventoryOrderItem Parse(string uid) => ParseKey<InventoryOrderItem>(uid);

    static public InventoryOrderItem Empty => ParseEmpty<InventoryOrderItem>();

    static internal FixedList<InventoryOrderItem> GetListFor(InventoryOrder order) {
      Assertion.Require(order, nameof(order));

      return InventoryOrderData.GetInventoryOrderItemsByOrder(order.Id);
    }

    #endregion Constructors and parsers

    #region Properties

    internal FixedList<InventoryEntry> Entries {
      get; set;
    }

    #endregion Properties

  } // class InventoryOrderItem

} // namespace Empiria.Inventory
