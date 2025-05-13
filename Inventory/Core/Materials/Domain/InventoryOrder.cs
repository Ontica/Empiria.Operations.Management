/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrder                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an inventory order.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders;

namespace Empiria.Inventory {

  /// <summary>Represents an inventory order.</summary>
  public class InventoryOrder : Order {

    #region Constructors and parsers

    protected InventoryOrder(OrderType orderType) : base(orderType) {
      // Required by Empiria Framework for all partitioned types.

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

    public FixedList<InventoryOrderItem> Items {
      get; internal set;
    }

    #endregion Properties
  } // class InventoryOrder

} // namespace Empiria.Inventory
