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
    public void CreateInventoryOrderTest() {

      var usecase = InventoryOrderUseCases.UseCaseInteractor();

      InventoryOrderFields fields = GetInventoryFields();

      InventoryHolderDto sut = usecase.CreateInventoryOrder(fields, "");

      Assert.NotNull(sut);
    }


    #region Helpers

    private InventoryOrderFields GetInventoryFields() {
      return new InventoryOrderFields {
        ReferenceUID = "2b0153fb-dee5-47f0-9de6-b9865b9075af",
        ResponsibleUID = "5f781a0f-221a-44e4-9b24-7dc7fa8744a4",
        AssignedToUID = "1e88a373-9939-44e0-a301-5426cd01cf4c",
        Notes = "ESTE REGISTRO ES UNA PRUEBA",
        Status = InventoryStatus.Abierto,
        Items = GetItemsFields()
      };
    }


    private FixedList<InventoryEntryFields> GetItemsFields() {
      var items = new List<InventoryEntryFields>();

      Random r = new Random();

      for (int i = 0; i < 5; i++) {

        var productId = r.Next(1, 2000);

        var fields = new InventoryEntryFields {
          InventoryEntryTypeId = -1,
          ProductId = productId,
          SkuId = -1,
          LocationId = -1,
          ObservationNotes = $"asignacion num {productId}",
          UnitId = -1,
          InputQuantity = r.Next(1, 20),
          InputCost = r.Next(1, 200),
          OutputQuantity = 0,
          OutputCost = r.Next(201, 400)
        };
        items.Add(fields);
      }
      return items.ToFixedList();
    }

    #endregion Helpers
  } // class InventoryUseCasesTests

} // namespace Empiria.Tests.Inventory
