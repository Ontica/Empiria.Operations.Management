/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryUseCasesTests                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for InventoryUseCases.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Inventory;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.UseCases;
using Empiria.Orders;
using Empiria.Orders.Data;
using Empiria.Parties;
using Xunit;

namespace Empiria.Tests.Inventory {

  /// <summary>Unit tests for InventoryUseCases.</summary>
  public class InventoryUseCasesTests {

    #region Initialization

    public InventoryUseCasesTests() {
      TestsCommonMethods.Authenticate();
    }

    #endregion Initialization

    [Fact]
    public void GetInventoryEntryByUIDTest() {

      var usecase = InventoryEntryUseCases.UseCaseInteractor();

      string inventoryEntryUID = "0986119b-d72a-4107-902d-f70199554ec6";

      InventoryEntryDto sut = usecase.GetInventoryEntryByUID(inventoryEntryUID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void CloseInventoryEntryTest() {

      var usecase = InventoryEntryUseCases.UseCaseInteractor();
      string orderUID = "829b237e-354a-4c06-8da9-ac5e23b704e1";

      InventoryHolderDto sut = usecase.CloseInventoryEntries(orderUID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void CreateInventoryEntriesTest() {

      var usecase = InventoryEntryUseCases.UseCaseInteractor();
      string orderUID = "a7a99924-0efc-41b9-9b65-5b78d31bf329";
      string orderItemUID = "f4874ac2-52f4-4061-87f4-28a554a5a0d7";

      InventoryEntryFields fields = new InventoryEntryFields {

        Location = "A-001-01-23",
        Product = "ASF24",
        Quantity = 5
      };

      InventoryHolderDto sut = usecase.CreateInventoryEntry(orderUID, orderItemUID, fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void CreateInventoryOrderTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();

      InventoryOrderFields fields = new InventoryOrderFields {
        Description = "ABCDE",
        InventoryTypeUID = "F6C83B25-4857-41E3-BB10-79959F37B247",
        RequestedByUID= "72b902de-8840-4985-81aa-46700d915ea7",
        ResponsibleUID= "d5527139-02e5-49b1-9e8f-827c5b8630ca",
        WarehouseUID= "C5D74E47-CFEE-4B31-81B8-D9B102EDDE8F"
      };

      InventoryHolderDto sut = usecase.CreateInventoryOrder(fields.WarehouseUID, fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void DeleteInventoryEntryTest() {

      var usecase = InventoryEntryUseCases.UseCaseInteractor();
      string orderUID = "829b237e-354a-4c06-8da9-ac5e23b704e1";
      string orderItemUID = "3f52c3bc-1a84-4390-9bb4-7850fb79f38e";
      string entryUID = "30dd291d-3d7c-4520-b8a1-11d644d37f80";

      InventoryHolderDto sut = usecase.DeleteInventoryEntry(orderUID, orderItemUID, entryUID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void DeleteInventoryOrderTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      string orderUID = "7f969825-2b14-464e-b11e-c52c8f69204d";

      usecase.DeleteInventoryOrder(orderUID);

      Assert.True(true);
    }


    [Fact]
    public void GetInventoryHolderTest() {
      TestsCommonMethods.Authenticate();
      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      InventoryHolderDto sut = usecase.GetInventoryOrder("f54744dd-81d7-4eb6-8587-424431c60e45");
      Assert.NotNull(sut);
    }


    [Fact]
    public void GetInventoryOrderItemTest() {

      InventoryOrderItem sut = InventoryOrderItem.Parse("3f52c3bc-1a84-4390-9bb4-7850fb79f38e");

      Assert.NotNull(sut);
    }


    [Fact]
    public void GetInventoryOrderTest() {

      InventoryOrder sut = InventoryOrder.Parse("7f969825-2b14-464e-b11e-c52c8f69204d");
      Assert.NotNull(sut);
    }


    [Fact]
    public void SearchInventoryOrderTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();

      InventoryOrderQuery query = new InventoryOrderQuery {
        Keywords = "",
        WarehouseUID = "",
        InventoryTypeUID = "0eb5a072-b857-4071-8b06-57a34822ec64",
        Status = StateEnums.EntityStatus.All
      };

      InventoryOrderDataDto sut = usecase.SearchInventoryOrder(query);

      Assert.NotNull(sut);
    }


    [Fact]
    public void UpdateInventoryItemTest() {
      TestsCommonMethods.Authenticate();
      var usecase = InventoryOrderUseCases.UseCaseInteractor();

      string orderUID = "f54744dd-81d7-4eb6-8587-424431c60e45";

      InventoryOrderItemFields fields = new InventoryOrderItemFields() {
        Product = "ASF24",
        Location = "A-029-01-08",
        Quantity = 1,
        RequestedByUID = "72b902de-8840-4985-81aa-46700d915ea7",
      };
      
      InventoryHolderDto sut = usecase.CreateInventoryOrderItem(orderUID, fields);
      Assert.NotNull(sut);
    }


    [Fact]
    public void UpdateInventoryOrderTest() {
      TestsCommonMethods.Authenticate();
      var usecase = InventoryOrderUseCases.UseCaseInteractor();

      string orderUID = "f54744dd-81d7-4eb6-8587-424431c60e45";

      InventoryOrderFields fields = new InventoryOrderFields() {
        Description = "previo",
        InventoryTypeUID = "F6C83B25-4857-41E3-BB10-79959F37B247",
        RequestedByUID= "72b902de-8840-4985-81aa-46700d915ea7",
        ResponsibleUID= "55334424-2871-4350-8d77-1973b6b9aa91",
        WarehouseUID= "C5D74E47-CFEE-4B31-81B8-D9B102EDDE8F"
      };

      InventoryHolderDto sut = usecase.UpdateInventoryOrder(orderUID, fields);
      Assert.NotNull(sut);
    }

  } // class InventoryUseCasesTests

} // namespace Empiria.Tests.Inventory
