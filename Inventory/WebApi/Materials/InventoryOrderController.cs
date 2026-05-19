/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Inventory Management                         Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Query Api Controller                  *
*  Type     : InventoryOrderController                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve inventory order data.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Web.Http;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.UseCases;
using Empiria.WebApi;

namespace Empiria.Inventory.WebApi {

  public class InventoryOrderController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v8/order-management/inventory-orders/{orderUID:guid}/close_")]
    public SingleObjectModel CloseInventoryOrder([FromUri] string orderUID) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.CloseInventoryOrder(orderUID);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }


    [HttpPut]
    [Route("v8/order-management/inventory-orders/{orderUID:guid}/items/{orderItemUID:guid}_")]

    public SingleObjectModel UpdateInventoryOrderItemQuantity([FromUri] string orderUID, [FromUri] string orderItemUID,
                                                              [FromBody] InventoryOrderItemFields fields) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.UpdateInventoryOrderItemQuantity(orderUID, orderItemUID, fields);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }

    #endregion Web Apis

  }
}
