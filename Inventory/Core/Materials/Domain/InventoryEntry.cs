/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryEntry                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an inventory entry.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.Data;
using Empiria.Locations;
using Empiria.Orders;
using Empiria.Parties;
using Empiria.Products;
using Empiria.Projects;

namespace Empiria.Inventory {

  /// <summary>Represents an inventory entry.</summary>
  internal class InventoryEntry : BaseObject {

    #region Constructors and parsers

    public InventoryEntry() {
      //no-op
    }


    static public InventoryEntry Parse(int id) => ParseId<InventoryEntry>(id);

    static public InventoryEntry Parse(string uid) => ParseKey<InventoryEntry>(uid);

    static public InventoryEntry Empty => ParseEmpty<InventoryEntry>();

    public InventoryEntry(InventoryOrder order, InventoryOrderItem orderItem) {
      Assertion.Require(order, nameof(order));
      Assertion.Require(orderItem, nameof(orderItem));

      this.OrderId = order.OrderId;
      this.OrderItemId = orderItem.OrderItemId;
      this.InventoryEntryTypeId = orderItem.ItemTypeId;
      this.UnitId = orderItem.ProductUnitId;
      this.OrderItemProductId = orderItem.ProductId;
    }


    #endregion Constructors and parsers

    #region Properties


    [DataField("Inv_Entry_Id")]
    internal int InventoryEntryId {
      get; set;
    }


    [DataField("Inv_Entry_Uid")]
    internal string InventoryEntryUID {
      get; set;
    }


    [DataField("Inv_Entry_Type_Id")]
    internal int InventoryEntryTypeId {
      get; set;
    }


    [DataField("Inv_Entry_Order_Id")]
    internal int OrderId {
      get; set;
    }


    [DataField("Inv_Entry_Order_Item_Id")]
    internal int OrderItemId {
      get; set;
    }


    [DataField("Inv_Entry_Product_Id")]
    internal int ProductId {
      get; set;
    }


    [DataField("Inv_Entry_Sku_Id")]
    internal int SkuId {
      get; set;
    }


    [DataField("Inv_Entry_Location_Id")]
    internal int LocationId {
      get; set;
    }


    [DataField("Inv_Entry_Observations")]
    public string Observations {
      get; set;
    } = string.Empty;


    [DataField("Inv_Entry_Unit_Id")]
    internal int UnitId {
      get; set;
    }


    [DataField("Inv_Entry_Input_Qty")]
    public decimal InputQuantity {
      get; set;
    }


    [DataField("Inv_Entry_Input_Cost")]
    public decimal InputCost {
      get; set;
    }


    [DataField("Inv_Entry_Output_Qty")]
    public decimal OutputQuantity {
      get; set;
    }


    [DataField("Inv_Entry_Output_Cost")]
    public decimal OutputCost {
      get; set;
    }


    [DataField("Inv_Entry_Time")]
    public DateTime EntryTime {
      get; set;
    }


    [DataField("Inv_Entry_Tags")]
    public string Tags {
      get; set;
    }


    [DataField("Inv_Entry_Ext_Data")]
    public string ExtData {
      get; set;
    } = string.Empty;


    [DataField("Inv_Entry_Keywords")]
    public string InventoryEntryKeywords {
      get; set;
    } = string.Empty;


    [DataField("Inv_Entry_Posted_By_Id")]
    internal Party PostedBy {
      get; set;
    }


    [DataField("Inv_Entry_Posting_Time")]
    public DateTime PostingTime {
      get; set;
    }


    [DataField("Inv_Entry_Status", Default = InventoryStatus.Abierto)]
    internal InventoryStatus Status {
      get; set;
    } = InventoryStatus.Abierto;


    internal int OrderItemProductId {
      get; private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Observations);
      }
    }


    #endregion Properties


    #region Private methods

    protected override void OnSave() {

      if (IsNew) {
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
        this.EntryTime = DateTime.Now;
      }
      InventoryOrderData.WriteInventoryEntry(this);
    }


    internal void Update(InventoryEntryFields fields, string orderItemUID) {

      ProductEntry product = InventoryOrderData.GetProductEntryByName(fields.Product.Trim());
      LocationEntry location = InventoryOrderData.GetLocationEntryByName(fields.Location.Trim());
      
      fields.EnsureIsValid(product.ProductId, orderItemUID);

      this.InputQuantity = fields.Quantity;
      this.ProductId = product.ProductId;
      this.LocationId = location.LocationId;
      this.SkuId = -1;
      this.InputCost = 0;
      this.OutputQuantity = 0;
      this.OutputCost = 0;
      this.Tags = string.Empty;
      this.ExtData = string.Empty;
      this.Status = InventoryStatus.Abierto;
    }


    #endregion Private methods

  } // class InventoryEntry

} // namespace Empiria.Inventory
