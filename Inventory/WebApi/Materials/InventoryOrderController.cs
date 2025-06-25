
using System.Web.Http;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.UseCases;
using Empiria.WebApi;
using Empiria.Inventory;

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
    [Route("v8/order-management/warehouses")]
    public CollectionModel GetWarehouses() {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> warehouses = usecases.GetWarehouses();

        return new CollectionModel(this.Request, warehouses);
      }
    }


    [HttpGet]
    [Route("v8/order-management/inventory-orders/inventory-supervisor")]
    public CollectionModel GetResponsibles() {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> parties = usecases.GetPartiesByRol("Inventory-manager");

        return new CollectionModel(this.Request, parties);
      }
    }

    [HttpGet]
    [Route("v8/order-management/inventory-orders/warehousemen")]
    public CollectionModel GetWarehouseman() {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> parties = usecases.GetPartiesByRol("Warehouseman");

        return new CollectionModel(this.Request, parties);
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
    [Route("v8/order-management/inventory-orders/{orderUID}/items")]
    public SingleObjectModel CreateInventoryOrderItem([FromUri] string orderUID, [FromBody] InventoryOrderItemFields fields) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.CreateInventoryOrderItem(orderUID, fields);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }


    [HttpPost]
    [Route("v8/order-management/inventory-orders/{orderUID}/close-entries")]
    public SingleObjectModel CloseInventoryEntry([FromUri] string orderUID) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.CloseInventoryEntries(orderUID);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }


    [HttpPut]
    [Route("v8/order-management/inventory-orders/{orderUID:guid}/close")]
    public SingleObjectModel CloseInventoryOrder([FromUri] string orderUID) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.CloseInventoryOrder(orderUID);

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


    [HttpDelete]
    [Route("v8/order-management/inventory-orders/{orderUID:guid}")]
    public NoDataModel DeleteInventoryOrder([FromUri] string orderUID) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        usecases.DeleteInventoryOrder(orderUID);

        return new NoDataModel(this.Request);
      }
    }


    [HttpDelete]
    [Route("v8/order-management/inventory-orders/{orderUID:guid}/items/{orderItemUID:guid}")]
    public SingleObjectModel DeleteInventoryOrderItem([FromUri] string orderUID,
                                                [FromUri] string orderItemUID) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

       InventoryHolderDto inventoryOrder =  usecases.DeleteInventoryOrderItem(orderUID, orderItemUID);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }


    [HttpPut]
    [Route("v8/order-management/inventory-orders/{orderUID:guid}")]

    public SingleObjectModel UpdateInventoryOrder([FromUri] string orderUID,
                                                  [FromBody] InventoryOrderFields fields) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.UpdateInventoryOrder(orderUID, fields);

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }

    #endregion Web Apis

  }
}
