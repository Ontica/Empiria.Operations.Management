
using System.Web.Http;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.UseCases;
using Empiria.WebApi;

namespace Empiria.Inventory.WebApi {

  public class InventoryOrderController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v8/order-management/inventory-orders/search")]
    public SingleObjectModel GetInventoryOrderList([FromBody] InventoryOrderQuery query) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryOrderDataDto inventoryOrderDto = usecases.SearchInventoryOrder(query);

        return new SingleObjectModel(this.Request, inventoryOrderDto);
      }
    }


    [HttpGet]
    [Route("v8/order-management/inventory-orders/{orderUID:guid}")]
    public SingleObjectModel GetInventoryOrder([FromUri] string orderUID) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.GetInventoryOrder(orderUID);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }


    [HttpGet]
    [Route("v8/order-management/inventory-types")]
    public CollectionModel GetInventoryOrder() {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> orderTypes = usecases.GetOrderTypes();

        return new CollectionModel(this.Request, orderTypes);
      }
    }


    [HttpGet]
    [Route("v8/order-management/wareHouses")]
    public CollectionModel GetWareHouses() {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> wareHouses = usecases.GetWareHouses();

        return new CollectionModel(this.Request, wareHouses);
      }
    }


    [HttpPost]
    [Route("v8/order-management/inventory-orders/{orderUID}/items/{itemUID}/entries")]
    public SingleObjectModel CreateInventoryEntry([FromUri] string orderUID,
                                                  [FromUri] string itemUID,
                                                  [FromBody] InventoryEntryFields fields) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.CreateInventoryEntry(orderUID, itemUID, fields);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }


    [HttpPost]
    [Route("v8/order-management/inventory-orders")]
    public SingleObjectModel CreateInventoryOrder([FromBody] InventoryOrderFields fields) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.CreateInventoryOrder(fields.WarehouseUID, fields);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }


    [HttpPost]
    [Route("v8/order-management/inventory-orders/{orderUID}/close")]
    public SingleObjectModel CloseInventoryEntry([FromUri] string orderUID) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.CloseInventoryEntries(orderUID);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }


    [HttpDelete]
    [Route("v8/order-management/inventory-orders/{orderUID}/items/{itemUID}/entries/{entryUID}")]
    public SingleObjectModel DeleteInventoryEntry([FromUri] string orderUID,
                                                  [FromUri] string itemUID,
                                                  [FromUri] string entryUID) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.DeleteInventoryEntry(orderUID, itemUID, entryUID);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }

    #endregion Web Apis

  }
}
