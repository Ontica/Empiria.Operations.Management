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


    public InventoryEntry(string inventoryOrderUID, InventoryEntryFields fields) {

      MapToInventoryOrderItem(inventoryOrderUID, fields);
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
    internal InventoryOrder InventoryOrder {
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
    public string ObservationNotes {
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
    internal int PostedById {
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


    #endregion Properties


    #region Private methods

    protected override void OnSave() {

      if (IsNew) {
        this.PostedById = 1; //Party.Parse(1);
        //this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }
      InventoryOrderData.WriteInventoryEntry(this);
    }


    private void MapToInventoryOrderItem(string inventoryOrderUID,
                                         InventoryEntryFields fields) {

      this.InventoryEntryTypeId = fields.InventoryEntryTypeId;
      this.InventoryOrder = InventoryOrder.Parse(inventoryOrderUID);
      this.ProductId = fields.ProductId;
      this.SkuId = fields.SkuId;
      this.LocationId = fields.LocationId;
      this.ObservationNotes = fields.ObservationNotes;
      this.UnitId = fields.UnitId;
      this.InputQuantity = fields.InputQuantity;
      this.InputCost = fields.InputCost;
      this.OutputQuantity = fields.OutputQuantity;
      this.OutputCost = fields.OutputCost;
      this.EntryTime = new DateTime(2049, 01, 01);
      this.Tags = string.Empty;
      this.ExtData = string.Empty;
      this.Status = InventoryStatus.Abierto;
      

    }


    #endregion Private methods

  } // class InventoryEntry

} // namespace Empiria.Inventory
