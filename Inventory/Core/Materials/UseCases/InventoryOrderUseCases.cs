/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryOrderUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to manage inventory order.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


using Empiria.Locations;
using Empiria.Products;
using Empiria.Services;

using Empiria.Inventory.Adapters;
using Empiria.Inventory.Data;


namespace Empiria.Inventory.UseCases {

  /// <summary>Use cases to manage inventory order.</summary>
  public class InventoryOrderUseCases : UseCase {

    private const int INVENTORYORDERTYPEID = 4010;

    #region Constructors and parsers

    protected InventoryOrderUseCases() {
      // no-op
    }

    static public InventoryOrderUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<InventoryOrderUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public InventoryHolderDto CloseInventoryEntries(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);

      FixedList<InventoryOrderItem> orderItems = order.GetItems<InventoryOrderItem>();

      InventoryUtility.EnsureIsValidToClose(orderItems);

      InventoryOrderData.UpdateEntriesStatusByOrder(order.Id, InventoryStatus.Cerrado);

      return GetInventoryOrder(orderUID);
    }


    public InventoryHolderDto CreateInventoryEntry(string orderUID, string orderItemUID,
                                                   InventoryEntryFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));
      Assertion.Require(fields, nameof(fields));

      ProductEntry productEntry = InventoryOrderData.GetProductEntryByName(fields.Product.Trim());
      LocationEntry locationEntry = InventoryOrderData.GetLocationEntryByName(fields.Location.Trim());

      fields.EnsureIsValid(productEntry.ProductId, orderItemUID);
      fields.ProductUID = Product.Parse(productEntry.ProductId).UID;
      fields.LocationUID = Location.Parse(locationEntry.LocationId).UID;

      var inventoryEntry = new InventoryEntry(orderUID, orderItemUID);

      inventoryEntry.Update(fields, orderItemUID);

      inventoryEntry.Save();

      return GetInventoryOrder(orderUID);
    }


    public InventoryHolderDto CreateInventoryOrder(int wareHouse, InventoryOrderFields fields) {
      Assertion.Require(wareHouse, nameof(wareHouse));
      Assertion.Require(fields, nameof(fields));
            
      var orderType = Orders.OrderType.Parse(INVENTORYORDERTYPEID);

      InventoryOrder order = new InventoryOrder(wareHouse, orderType);

      order.Update(fields);

      order.Save();

      return GetInventoryOrder(order.UID);
    }


    public InventoryHolderDto DeleteInventoryEntry(string orderUID, string itemUID, string entryUID) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(itemUID, nameof(itemUID));
      Assertion.Require(entryUID, nameof(entryUID));

      InventoryOrder order = InventoryOrder.Parse(orderUID);
      InventoryOrderItem orderItem = InventoryOrderItem.Parse(itemUID);
      InventoryEntry entry = InventoryEntry.Parse(entryUID);

      Assertion.Require(order.Id == entry.Order.Id && orderItem.Order.Id == entry.Order.Id,
                        $"El registro de inventario no coincide con la orden!");

      InventoryOrderData.DeleteEntryStatus(order.Id, orderItem.Id,
                                           entry.Id, InventoryStatus.Deleted);

      return GetInventoryOrder(orderUID);
    }


    public InventoryHolderDto GetInventoryOrder(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      InventoryOrder inventoryOrder = InventoryUtility.GetInventoryOrder(orderUID);

      InventoryOrderActions actions = InventoryUtility.GetActions(inventoryOrder.Items);

      return InventoryOrderMapper.MapToHolderDto(inventoryOrder, actions);
    }


    public InventoryEntryDto GetInventoryEntryByUID(string inventoryEntryUID) {
      Assertion.Require(inventoryEntryUID, nameof(inventoryEntryUID));

      InventoryEntry entry = InventoryEntry.Parse(inventoryEntryUID);

      return InventoryOrderMapper.MapToInventoryEntryDto(entry);
    }


    public InventoryOrderDataDto SearchInventoryOrder(InventoryOrderQuery query) {
      Assertion.Require(query, nameof(query));

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      var orders = InventoryOrderData.SearchInventoryOrders(filter, sort);

      return InventoryOrderMapper.InventoryOrderDataDto(orders, query);
    }
    
    #endregion Use cases

    #region Helpers


    #endregion Helpers

  } // class InventoryOrderUseCases

} // namespace Empiria.Inventory.UseCases
