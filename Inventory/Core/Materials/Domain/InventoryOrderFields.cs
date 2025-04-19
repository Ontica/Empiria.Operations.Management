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

    Cerrado = 'C'

  }


  public class InventoryEntryFields {

    public string UID {
      get; internal set;
    }


    public string Location {
      get; internal set;
    }


    public string Product {
      get; internal set;
    }


    public decimal Quantity {
      get; internal set;
    }
  }


  static public class InventoryEntryFieldsExtensions {

    static internal void EnsureIsValid(this InventoryEntryFields fields,
                                       int productId,
                                       string orderItemUID) {

      var orderItem = InventoryOrderData.GetInventoryOrderItemsByUID(orderItemUID);
      var entries = InventoryOrderData.GetInventoryEntriesByOrderItemId(orderItem);

      Assertion.Require((fields.Quantity + entries.Sum(x => x.InputQuantity)) <= orderItem.ProductQuantity,
                        $"La suma de productos ingresados " +
                        $"es mayor al registro en la orden de inventario!");

      Assertion.Require(productId == orderItem.ProductId, "El producto que intenta registrar " +
                        "no es el mismo que está registrado en la orden de inventario!");
    }


  }


} // namespace Empiria.Inventory
