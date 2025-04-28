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
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Inventory {

  /// <summary>Represents an inventory order.</summary>
  public class InventoryOrder {

    #region Constructors and parsers

    public InventoryOrder() {
      //no-op
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("Order_Id")]
    internal int OrderId {
      get; set;
    }


    [DataField("Order_UID")]
    internal string InventoryOrderUID {
      get; set;
    }


    [DataField("Order_Type_Id")]
    internal int InventoryOrderTypeId {
      get; set;
    }


    [DataField("Order_No")]
    internal string InventoryOrderNo {
      get; set;
    }


    [DataField("Order_Responsible_Id")]
    internal Party Responsible {
      get; set;
    }


    [DataField("Order_Description")]
    internal string Order_Description {
      get; set;
    }



    [DataField("Order_Posted_By_Id")]
    internal Party PostedBy {
      get; set;
    }


    [DataField("Order_Posting_Time")]
    internal DateTime PostingTime {
      get; set;
    }


    [DataField("Order_Closing_Time")]
    internal DateTime ClosingTime {
      get; set;
    }


    [DataField("Order_Status", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; set;
    }


    public FixedList<InventoryOrderItem> Items {
      get; internal set;
    }


    internal string Keywords {
      get {
        return EmpiriaString.BuildKeywords(

          InventoryOrderUID, InventoryOrderNo, Order_Description
        );
      }
    }

    #endregion Properties


    #region Public methods


    #endregion

    #region Private methods



    #endregion Private methods

  } // class InventoryOrder

} // namespace Empiria.Inventory
