﻿/* Empiria Operations ****************************************************************************************
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
using Empiria.Locations;
using Empiria.Ontology;
using Empiria.Orders;
using Xunit;

namespace Empiria.Tests.Inventory {

  /// <summary>Unit tests for InventoryUseCases.</summary>
  public class InventoryUseCasesTests {

    #region Initialization

    public InventoryUseCasesTests() {
      //TestsCommonMethods.Authenticate();
    }

    #endregion Initialization

    [Fact]
    public void GetInventoryEntryByUIDTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      
      string inventoryEntryUID = "0986119b-d72a-4107-902d-f70199554ec6";

      InventoryEntryDto sut = usecase.GetInventoryEntryByUID(inventoryEntryUID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void CloseInventoryEntryTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      string orderUID = "829b237e-354a-4c06-8da9-ac5e23b704e1";

      InventoryHolderDto sut = usecase.CloseInventoryEntries(orderUID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void CreateInventoryEntriesTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      string orderUID = "829b237e-354a-4c06-8da9-ac5e23b704e1";
      string orderItemUID = "3f52c3bc-1a84-4390-9bb4-7850fb79f38e";

      InventoryEntryFields fields = new InventoryEntryFields {

        Location = "A-001-01-23",
        Product = "AOME1",
        Quantity = 5
      };
      
      InventoryHolderDto sut = usecase.CreateInventoryEntry(orderUID, orderItemUID, fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void DeleteInventoryEntryTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      string orderUID =  "829b237e-354a-4c06-8da9-ac5e23b704e1";
      string orderItemUID = "3f52c3bc-1a84-4390-9bb4-7850fb79f38e";
      string entryUID = "30dd291d-3d7c-4520-b8a1-11d644d37f80";

      InventoryHolderDto sut = usecase.DeleteInventoryEntry(orderUID, orderItemUID, entryUID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void GetInventoryHolderTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      InventoryHolderDto sut = usecase.GetInventoryOrder("829b237e-354a-4c06-8da9-ac5e23b704e1");
      Assert.NotNull(sut);
    }


    [Fact]
    public void GetInventoryOrderItemTest() {

      InventoryOrderItem sut = InventoryOrderItem.Parse("3f52c3bc-1a84-4390-9bb4-7850fb79f38e");

      Assert.NotNull(sut);
    }


    [Fact]
    public void GetInventoryOrderTest() {

      InventoryOrder sut = InventoryOrder.Parse("829b237e-354a-4c06-8da9-ac5e23b704e1");
      Assert.NotNull(sut);
    }


    [Fact]
    public void SearchInventoryOrderTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      
      InventoryOrderQuery query = new InventoryOrderQuery {
        Keywords = "",
        WarehouseUID = "",
        InventoryTypeUID = "5ECC7A4B-9CA0-48E8-9C44-7B16F3E1E9CB",
        Status = StateEnums.EntityStatus.All
      };

      InventoryOrderDataDto sut = usecase.SearchInventoryOrder(query);

      Assert.NotNull(sut);
    }


  } // class InventoryUseCasesTests

} // namespace Empiria.Tests.Inventory
