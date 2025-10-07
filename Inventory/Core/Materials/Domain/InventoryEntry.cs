/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Information Holder                      *
*  Type     : InventoryEntry                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an inventory entry.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Locations;
using Empiria.Orders;
using Empiria.Parties;
using Empiria.Products;

using Empiria.Inventory.Data;

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

    public InventoryEntry(string orderUID, string orderItemUID) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));

      this.Order = Order.Parse(orderUID);
      this.OrderItem = OrderItem.Parse(orderItemUID);
      this.InventoryEntryTypeId = 4311; // TODO PREGUNTAR A JM COMO JALAR EL TIPO
      this.Unit = ProductUnit.Parse(OrderItem.ProductUnit.Id);
      this.Position = OrderItem.Position;
      this.Sku = ProductSku.Empty;
    }


    public InventoryEntry(Order order, InventoryOrderItem orderItem) {
      Assertion.Require(order, nameof(order));
      Assertion.Require(orderItem, nameof(orderItem));

      this.Order = order;
      this.OrderItem = orderItem;
      this.InventoryEntryTypeId = 4311; // TODO PREGUNTAR A JM COMO JALAR EL TIPO
      this.Unit = orderItem.ProductUnit;
      this.Position = orderItem.Position;
      this.Sku = ProductSku.Empty;
    }


    static internal FixedList<InventoryEntry> GetListFor(InventoryOrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      return InventoryOrderData.GetInventoryEntriesByOrderItem(orderItem);
    }


    public static InventoryEntry TryParseWithOrderItemId(int orderId) {
      Assertion.Require(orderId, nameof(orderId));

      return TryParse<InventoryEntry>($"Inv_Entry_Order_Item_Id = {orderId}");
    }

    #endregion Constructors and parsers

    #region Properties


    [DataField("Inv_Entry_Type_Id")]
    internal int InventoryEntryTypeId {
      get; set;
    }


    [DataField("Inv_Entry_Order_Id")]
    internal Order Order {
      get; set;
    }


    [DataField("Inv_Entry_Order_Item_Id")]
    internal OrderItem OrderItem {
      get; set;
    }


    [DataField("Inv_Entry_Product_Id")]
    internal Product Product {
      get; set;
    }


    [DataField("Inv_Entry_Sku_Id")]
    internal ProductSku Sku {
      get; set;
    }


    [DataField("Inv_Entry_Location_Id")]
    internal Location Location {
      get; set;
    }


    [DataField("Inv_Entry_Observations")]
    public string Observations {
      get; set;
    } = string.Empty;


    [DataField("Inv_Entry_Unit_Id")]
    internal ProductUnit Unit {
      get; set;
    }


    [DataField("Inv_Entry_Input_Qty")]
    public decimal InputQuantity {
      get; set;
    } = 0;


    [DataField("Inv_Entry_Input_Cost")]
    public decimal InputCost {
      get; set;
    } = 0;


    [DataField("Inv_Entry_Output_Qty")]
    public decimal OutputQuantity {
      get; set;
    } = 0;


    [DataField("Inv_Entry_Output_Cost")]
    public decimal OutputCost {
      get; set;
    } = 0;


    [DataField("Inv_Entry_Time")]
    public DateTime EntryTime {
      get; set;
    }


    [DataField("Inv_Entry_Tags")]
    public string Tags {
      get; set;
    } = string.Empty;


    [DataField("Inv_Entry_Ext_Data")]
    public string ExtData {
      get; set;
    } = string.Empty;


    [DataField("Inv_Entry_Keywords")]
    public string InventoryEntryKeywords {
      get; set;
    } = string.Empty;


    [DataField("Inv_Entry_Position")]
    public int Position {
      get; set;
    } = 0;


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

    internal void AddEntry(InventoryEntryFields fields) {

      this.InputQuantity = fields.Quantity;
      this.Product = Patcher.Patch(fields.ProductUID, this.Product);
      this.Location = Patcher.Patch(fields.LocationUID, this.Location);
      this.InputCost = fields.Cost;
    }


    protected override void OnSave() {

      if (IsNew) {
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
        this.EntryTime = DateTime.Now;
      }
      InventoryOrderData.WriteInventoryEntry(this);
    }


    internal void OutputEntry(InventoryEntryFields fields) {

      this.OutputQuantity = fields.Quantity;
      this.Product = Patcher.Patch(fields.ProductUID, this.Product);
      this.Location = Patcher.Patch(fields.LocationUID, this.Location);
      this.OutputCost = fields.Cost;
    }


    internal void OutputEntry(decimal cost) {

      this.OutputQuantity = this.OrderItem.Quantity;
      this.Product = this.OrderItem.Product;
      this.Location = Location.Empty;
      this.OutputCost = cost;
    }

    internal void InputEntry(decimal inputCost, Location location) {

      this.InputQuantity = this.OrderItem.Quantity;
      this.Product = this.OrderItem.Product;
      this.Location = location;
      this.InputCost = inputCost;
      this.Position = this.OrderItem.Position;
    }


    internal void Update(InventoryEntryFields fields) {

      this.InputQuantity = fields.Quantity;
      this.Product = Patcher.Patch(fields.ProductUID, this.Product);
      this.Location = Patcher.Patch(fields.LocationUID, this.Location);
      this.Sku = ProductSku.Empty;
      this.InputCost = 0;
      this.OutputQuantity = 0;
      this.OutputCost = 0;
      this.Tags = string.Empty;
      this.ExtData = string.Empty;
      this.Position = 0;
      this.Status = InventoryStatus.Abierto;
    }


    internal void UpdatePosition(int position) {
      this.Position = position;
    }

    #endregion Private methods

  } // class InventoryEntry

} // namespace Empiria.Inventory
