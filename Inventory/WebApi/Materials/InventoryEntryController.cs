/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Inventory Management                         Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Query Api Controller                  *
*  Type     : InventoryEntryController                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve inventory entries data.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.UseCases;
using Empiria.WebApi;

namespace Empiria.Inventory.WebApi {

  /// Web API used to retrieve inventory entries data.  
  public class InventoryEntryController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v8/order-management/inventory-orders/{orderUID}/items/{itemUID}/entries")]
    public SingleObjectModel CreateInventoryEntry([FromUri] string orderUID,
                                                  [FromUri] string itemUID,
                                                  [FromBody] InventoryEntryFields fields) {

      using (var usecases = InventoryEntryUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.CreateInventoryEntry(orderUID, itemUID, fields);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }
    

    [HttpPost]
    [Route("v8/order-management/inventory-orders/{orderUID}/close-entries")]
    public SingleObjectModel CloseInventoryEntry([FromUri] string orderUID) {

      using (var usecases = InventoryEntryUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.CloseInventoryEntries(orderUID);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }


    [HttpDelete]
    [Route("v8/order-management/inventory-orders/{orderUID}/items/{itemUID}/entries/{entryUID}")]
    public SingleObjectModel DeleteInventoryEntry([FromUri] string orderUID,
                                                  [FromUri] string itemUID,
                                                  [FromUri] string entryUID) {

      using (var usecases = InventoryEntryUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.DeleteInventoryEntry(orderUID, itemUID, entryUID);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }
    
    #endregion Web Apis

  }
}
