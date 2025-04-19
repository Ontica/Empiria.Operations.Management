/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : InventoryUseCasesTests                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for InventoryUseCases.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using Empiria.Inventory;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.Data;
using Empiria.Inventory.UseCases;
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
    public void CloseInventoryEntryTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      string orderUID = "64a733ea-0e84-410d-a8ba-7662b6f3a23d";

      InventoryHolderDto sut = usecase.CloseInventoryEntries(orderUID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void CreateInventoryEntriesTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      string orderUID = "64a733ea-0e84-410d-a8ba-7662b6f3a23d";
      string orderItemUID = "0b2f127a-1aed-486e-b892-98238ae91f4d";

      InventoryEntryFields fields = new InventoryEntryFields {
        
        Location = "A-001-1-23",
        Product = "PPLAN10X212-200",
        Quantity = 5
      };

      LocationEntry location = InventoryOrderData.GetLocationEntryByName(fields.Location);

      InventoryHolderDto sut = usecase.CreateInventoryEntry(orderUID, orderItemUID, fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void DeleteInventoryEntryTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      string orderUID = "64a733ea-0e84-410d-a8ba-7662b6f3a23d";
      string orderItemUID = "0b2f127a-1aed-486e-b892-98238ae91f4d";
      string entryUID = "11132bf0-f125-44e5-b89a-075d71811720";

      InventoryHolderDto sut = usecase.DeleteInventoryEntry(orderUID, orderItemUID, entryUID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void GetInventoryOrderItemByOrderUIDTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();

      InventoryHolderDto sut = usecase.GetInventoryOrderByUID("64a733ea-0e84-410d-a8ba-7662b6f3a23d");

      Assert.NotNull(sut);
    }


    [Fact]
    public void SearchInventoryOrderTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      
      InventoryOrderQuery query = new InventoryOrderQuery {
        Keywords = "",
        Status = StateEnums.EntityStatus.All
      };

      InventoryOrderDataDto sut = usecase.SearchInventoryOrder(query);

      Assert.NotNull(sut);
    }


  } // class InventoryUseCasesTests

} // namespace Empiria.Tests.Inventory
