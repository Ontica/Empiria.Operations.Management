/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrderItemFields                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represent Inventory Order Item Fields                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Locations;
using Empiria.Orders;

namespace Empiria.Inventory {

  /// <summary>Represents an inventory order item.</summary>
  public class InventoryOrderItemFields : OrderItemFields {
    
    public string Location { 
      get; set; 
    }

    public string Product {
      get; set;
    }

    public override void EnsureValid() {
      base.EnsureValid(); 

      Assertion.Require(Location, "Necesito la localizacion del producto.");
    
      //_ = Location.Parse(Location);

    }

  } // class InventoryOrderItemFields


} // namespace Empiria.Inventory