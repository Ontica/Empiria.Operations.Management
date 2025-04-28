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
using Empiria.Parties;
using Empiria.Products;
using Empiria.StateEnums;

namespace Empiria.Inventory {

  /// <summary>Represents an inventory order item.</summary>
  public class InventoryOrderItem {

    [DataField("Order_Item_Id")]
    internal int OrderItemId {
      get; set;
    }


    [DataField("Order_Item_UID")]
    internal string InventoryOrderItemUID {
      get; set;
    }


    [DataField("Order_Item_Type_Id")]
    internal int ItemTypeId {
      get; set;
    }


    [DataField("Order_Item_Order_Id")]
    internal int OrderId {
      get; set;
    }


    [DataField("Order_Item_Product_Id")]
    internal Product Product {
      get; set;
    }


    [DataField("Order_Item_Description")]
    internal string Description {
      get; set;
    }


    [DataField("Order_Item_Product_Unit_Id")]
    internal int ProductUnitId {
      get; set;
    }


    [DataField("Order_Item_Product_Qty")]
    internal decimal ProductQuantity {
      get; set;
    }


    [DataField("Order_Item_Posted_By_Id")]
    internal Party PostedBy {
      get; set;
    }


    [DataField("Order_Item_Posting_Time")]
    internal DateTime PostingTime {
      get; set;
    }


    [DataField("Order_Item_Status", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; set;
    }


    internal FixedList<InventoryEntry> Entries {
      get; set;
    }

  } // class InventoryOrderItem

} // namespace Empiria.Inventory
