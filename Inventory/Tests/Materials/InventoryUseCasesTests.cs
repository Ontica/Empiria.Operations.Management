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
    public void CreateInventoryEntriesTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      string orderUID = "";
      string orderItemUID = "";

      InventoryEntryFields fields = new InventoryEntryFields {
        
        Location = "245A291C-B961-4A33-8B57-C8E6ED2CD473",
        Product = "0b994feb-1fbe-4cfe-b7e6-436d6e106805",
        InputQuantity = 1
      };

      InventoryEntryDto sut = usecase.CreateInventoryEntry(orderUID, orderItemUID, fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void GetInventoryOrderItemByOrderUIDTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();

      InventoryHolderDto sut = usecase.GetInventoryOrderByUID("d82a75ff-96bf-4f40-8fdc-f9ec63aec978");

      Assert.NotNull(sut);
    }


    [Fact]
    public void SearchInventoryOrderTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();
      
      InventoryOrderQuery query = new InventoryOrderQuery {
        Keywords = "",
        Status = StateEnums.EntityStatus.Deleted
      };

      InventoryOrderDataDto sut = usecase.SearchInventoryOrder(query);

      Assert.NotNull(sut);
    }


  } // class InventoryUseCasesTests

} // namespace Empiria.Tests.Inventory
