/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Data Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data service                            *
*  Type     : InventoryOrderData                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for inventory order instances.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Data;

namespace Empiria.Inventory.Data {

  /// <summary>Provides data read and write methods for inventory order instances.</summary>
  static internal class InventoryOrderData {

    internal static void WriteInventoryOrder(InventoryOrder order) {
      
      var op = DataOperation.Parse("writeInventoryOrder",
          order.InventoryOrderId, order.InventoryOrderUID,
          order.InventoryOrderTypeId, order.InventoryOrderNo,
          order.Reference.Id, order.Responsible.Id,
          order.AssignedTo.Id, order.Notes,
          order.InventoryOrderExtData, order.Keywords,
          order.ScheduledTime, order.ClosingTime,
          order.PostingTime, order.PostedBy.Id,
          (char) order.Status);

      DataWriter.Execute(op);
    }

  } // class InventoryOrderData

} // namespace Empiria.Inventory.Data
