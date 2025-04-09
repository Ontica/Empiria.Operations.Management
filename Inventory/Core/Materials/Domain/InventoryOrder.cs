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
using Empiria.Inventory.Data;
using Empiria.Parties;

namespace Empiria.Inventory {

  /// <summary>Represents an inventory order.</summary>
  internal class InventoryOrder : BaseObject {

    #region Constructors and parsers

    public InventoryOrder() {
      //no-op
    }


    static public InventoryOrder Parse(int id) => ParseId<InventoryOrder>(id);

    static public InventoryOrder Parse(string uid) => ParseKey<InventoryOrder>(uid);

    static public InventoryOrder Empty => ParseEmpty<InventoryOrder>();


    public InventoryOrder(InventoryOrderFields fields, string inventoryOrderUID) {

      MapToInventoryOrder(fields, inventoryOrderUID);
    }


    #endregion Constructors and parsers

    #region Properties

    [DataField("Inventory_Order_Id")]
    internal int InventoryOrderId {
      get; set;
    }


    [DataField("Inventory_Order_UID")]
    internal string InventoryOrderUID {
      get; set;
    }


    [DataField("Inventory_Order_Type_Id")]
    internal int InventoryOrderTypeId {
      get; set;
    }


    [DataField("Inventory_Order_No")]
    internal string InventoryOrderNo {
      get; set;
    }


    [DataField("Reference_Id")]
    internal Party Reference {
      get; set;
    }


    [DataField("Responsible_Id")]
    internal Party Responsible {
      get; set;
    }


    [DataField("Assigned_To_Id")]
    internal Party AssignedTo {
      get; set;
    }


    [DataField("Inventory_Order_Notes")]
    internal string Notes {
      get; set;
    }


    [DataField("Inventory_Order_Ext_Data")]
    internal string InventoryOrderExtData {
      get; set;
    } = string.Empty;


    [DataField("Inventory_Order_Keywords")]
    internal string InventoryOrderKeywords {
      get; set;
    }


    [DataField("Scheduled_Time")]
    internal DateTime ScheduledTime {
      get; set;
    }


    [DataField("Closing_Time")]
    internal DateTime ClosingTime {
      get; set;
    }


    [DataField("Posting_Time")]
    internal DateTime PostingTime {
      get; set;
    }


    [DataField("Posted_By_Id")]
    internal Party PostedBy {
      get; set;
    }


    [DataField("Inventory_Order_Status", Default = InventoryStatus.Abierto)]
    internal InventoryStatus Status {
      get; set;
    }


    public FixedList<InventoryEntry> Items {
      get; internal set;
    }


    internal string Keywords {
      get {
        return EmpiriaString.BuildKeywords(

          InventoryOrderUID, InventoryOrderNo, Notes
        );
      }
    }

    #endregion Properties


    #region Public methods



    #endregion

    #region Private methods

    protected override void OnSave() {

      if (IsNew) {
        this.PostedBy = Party.Parse(1);
        //this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
        this.InventoryOrderNo = GenerateOrderNumber();
      }

      InventoryOrderData.WriteInventoryOrder(this);
    }


    private void MapToInventoryOrder(InventoryOrderFields fields, string inventoryOrderUID) {

      if (inventoryOrderUID != string.Empty) {
        this.InventoryOrderId = Parse(inventoryOrderUID).InventoryOrderId;
        this.InventoryOrderUID = inventoryOrderUID;
      } else {
        this.PostingTime = DateTime.Now;
      }

      this.InventoryOrderTypeId = GetEmpiriaType().Id;
      this.Reference = PatchField(fields.ReferenceUID, Reference);
      this.Responsible = PatchField(fields.ResponsibleUID, Responsible);
      this.AssignedTo = PatchField(fields.AssignedToUID, AssignedTo);
      this.Notes = fields.Notes;

      this.ScheduledTime = new DateTime(2049, 01, 01); //TODO ENVIAR FECHA PROGRAMADA
      this.ClosingTime = new DateTime(2049, 01, 01);

      this.InventoryOrderExtData = "";
      this.Status = fields.Status;
    }


    private string GenerateOrderNumber() {

      string orderNumber = $"OR-INV";
        //this.InventoryOrderType.InventoryOrderTypeCode.Name;

      if (orderNumber != string.Empty) {
        return $"{orderNumber}{this.InventoryOrderId.ToString().PadLeft(12, '0')}";
      }

      return string.Empty;
    }

    #endregion Private methods

  } // class InventoryOrder

} // namespace Empiria.Inventory
