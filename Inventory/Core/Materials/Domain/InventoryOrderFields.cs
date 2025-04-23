/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrderFields                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to create and update inventory order.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;
using Empiria.Inventory.Data;

namespace Empiria.Inventory {

  public enum InventoryStatus {

    Todos = 'T',

    Abierto = 'A',

    EnProceso = 'E',

    Cerrado = 'C',

    Deleted = 'X'

  }


  public class InventoryEntryFields {

    public string UID {
      get; set;
    }


    public string Location {
      get; set;
    }


    public string Product {
      get; set;
    }


    public decimal Quantity {
      get; set;
    }
  }


  static public class InventoryEntryFieldsExtensions {

    static internal void EnsureIsValid(this InventoryEntryFields fields,
                                       int productId,
                                       string orderItemUID) {

      var orderItem = InventoryOrderData.GetInventoryOrderItemByUID(orderItemUID);
      var entries = InventoryOrderData.GetInventoryEntriesByOrderItemId(orderItem);

      Assertion.Require(orderItem.ItemTypeId == 4311 ||
                       (fields.Quantity + entries.Sum(x => x.InputQuantity)) <= orderItem.ProductQuantity,
                        $"La cantidad de productos capturados supera los productos restantes.");

      Assertion.Require(productId == orderItem.ProductId, "El producto no coincide con el seleccionado.");
    }

  }


} // namespace Empiria.Inventory
