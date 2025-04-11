/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrderUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to manage inventory order.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using Empiria.Inventory.Adapters;
using Empiria.Services;

namespace Empiria.Inventory.UseCases {

  /// <summary>Use cases to manage inventory order.</summary>
  internal class InventoryOrderUseCases : UseCase {

    #region Constructors and parsers

    protected InventoryOrderUseCases() {
      // no-op
    }

    static public InventoryOrderUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<InventoryOrderUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    internal InventoryHolderDto CreateInventoryOrder(InventoryOrderFields fields,
      string inventoryOrderUID) {

      var inventoryOrder = new InventoryOrder(fields, inventoryOrderUID);
      
      inventoryOrder.Save();

      inventoryOrder.Items = CreateInventoryEntries(inventoryOrder.UID, fields.Items);

      return InventoryOrderMapper.Map(inventoryOrder);
    }


    internal FixedList<InventoryEntry> CreateInventoryEntries(string inventoryOrderUID,
                                                              FixedList<InventoryEntryFields> inventoryEntryFields) {
      var returnedItems = new List<InventoryEntry>();

      foreach (var fields in inventoryEntryFields) {

        returnedItems.Add(CreateInventoryEntry(inventoryOrderUID, fields));
      }

      return returnedItems.ToFixedList();
    }


    internal InventoryEntry CreateInventoryEntry(string inventoryOrderUID, InventoryEntryFields fields) {

      var inventoryEntry = new InventoryEntry(inventoryOrderUID, fields);

      inventoryEntry.Save();

      return inventoryEntry;
    }

    #endregion Use cases

    #region Private methods




    #endregion Private methods

  } // class InventoryOrderUseCases

} // namespace Empiria.Inventory.UseCases
