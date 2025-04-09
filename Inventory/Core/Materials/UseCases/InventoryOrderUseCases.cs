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

    internal InventoryOrder CreateInventoryOrder(InventoryOrderFields fields,
      string inventoryOrderUID) {

      var inventoryOrder = new InventoryOrder(fields, inventoryOrderUID);
      inventoryOrder.Save();

      return inventoryOrder;
    }

    #endregion Use cases

  } // class InventoryOrderUseCases

} // namespace Empiria.Inventory.UseCases
