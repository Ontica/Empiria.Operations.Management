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
using Empiria.Inventory;
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

      InventoryOrderFields fields = new InventoryOrderFields {
        ReferenceUID = "2b0153fb-dee5-47f0-9de6-b9865b9075af",
        ResponsibleUID = "5f781a0f-221a-44e4-9b24-7dc7fa8744a4",
        AssignedToUID = "1e88a373-9939-44e0-a301-5426cd01cf4c",
        Notes = "",
        Status = InventoryStatus.Abierto
      };

      InventoryOrder sut = usecase.CreateInventoryOrder(fields, "");

      Assert.NotNull(sut);
    }

  } // class InventoryUseCasesTests

} // namespace Empiria.Tests.Inventory
