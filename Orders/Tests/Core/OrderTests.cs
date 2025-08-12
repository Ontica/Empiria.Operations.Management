/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Test cases                              *
*  Assembly : Empiria.Orders.Tests.dll                   Pattern   : Unit Tests                              *
*  Type     : OrderTests                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for Order type.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Orders;
using Empiria.Orders.Adapters;
using Empiria.Orders.UseCases;


namespace Empiria.Tests.Orders {

  /// <summary>Unit tests for Order type.</summary>
  public class OrderTests {

    #region Facts

    [Fact]
    public void Should_Create_SalesOrder() {

      TestsCommonMethods.Authenticate();

        SalesOrderFields fields = new SalesOrderFields {
        CategoryUID = "4A9C79FE-AF5B-4F1A-BFE0-2CE86C3B9A0D",
        Description = "Orden 30 de Julio para pruebas de CAPA",
        BeneficiaryUID = "0a384dc7-9c68-407c-afe1-d73b71d260cd",
        ResponsibleUID = "68188d1b-2b69-461a-86cb-f1e7386c4cb1",
      };

      var orderType = OrderType.Parse(4011);

      SalesOrder order = new SalesOrder(orderType);

      fields.OrderTypeUID = orderType.UID;

      order.Update(fields);

      order.Save();

      Assert.NotNull(order);
    }


    [Fact]
    public void ActivateOrder_ShouldUpdateAndReturnDto() {

    
      string UID = "8d857a56-4bf6-48da-b208-0b4ab56c932e";

      var useCase = SalesOrderUseCases.CreateInstance<SalesOrderUseCases>();

      var result = useCase.ActivateOrder(UID);

      Assert.NotNull(result);
    }


    [Fact]
    public void DeleteOrder_ShouldUpdateAndReturnDto() {


      string UID = "8d857a56-4bf6-48da-b208-0b4ab56c932e";

      var useCase = SalesOrderUseCases.CreateInstance<SalesOrderUseCases>();

      var result = useCase.DeleteOrder(UID);

      Assert.NotNull(result);
    }


    [Fact]
    public void SuspendOrder_ShouldUpdateAndReturnDto() {


      string UID = "8d857a56-4bf6-48da-b208-0b4ab56c932e";

      var useCase = SalesOrderUseCases.CreateInstance<SalesOrderUseCases>();

      var result = useCase.SuspendOrder(UID);

      Assert.NotNull(result);
    }


    [Fact]
    public void UpdateOrder_WithValidFields_ShouldUpdateAndReturnDto() {

      var fields = new SalesOrderFields {
        UID = "aa347634-d26b-4149-a7e5-d23e981a3d83",
        OrderTypeUID = "ObjectTypeInfo.Order.SalesOrder",
        CategoryUID = "Empty",
        Description = "Order Updated by  HAGE 22 julio 2025 15:50pm",//a0069243 01723 ov00145277 ag ic00187574
        ResponsibleUID = "494d5db3-68e2-4061-9f13-e40f5ff6dfd0", //1636
        BeneficiaryUID = "000f81b8-bc6d-45a0-9ded-2d73f7604e34", //1426
      };

      var useCase = SalesOrderUseCases.CreateInstance<SalesOrderUseCases>();

      var result = useCase.UpdateOrder(fields);

      Assert.NotNull(result);
    }


    [Fact]
    public void GetSalesOrderByUIDTest() {

      var usecase = SalesOrderUseCases.UseCaseInteractor();

      string salesOrderUID = "aa347634-d26b-4149-a7e5-d23e981a3d83";

      SalesOrderHolderDto sut = usecase.GetSalesOrderByUID(salesOrderUID);

      Assert.NotNull(sut);
    }

    [Fact]
    public void Should_Get_SalesOrders() {
      SalesOrder order = SalesOrder.Parse(31);
      var orderItems = order.GetItems<SalesOrderItem>();
      Assert.True(orderItems.Count > 0);
      Assert.NotNull(order);
    }


    [Fact]
    public void Should_Read_All_Orders() {
      var orders = BaseObject.GetFullList<SalesOrder>();

      foreach (var sut in orders) {
        Assert.NotNull(sut.Responsible);
        Assert.NotNull(sut.Beneficiary);
        Assert.NotNull(sut.Provider);
      }
    }


    [Fact]
    public void Should_Read_Empty_Order() {
      var sut = Order.Empty;

      Assert.NotNull(sut);
      Assert.NotNull(sut.Responsible);
      Assert.NotNull(sut.Beneficiary);
      Assert.NotNull(sut.Provider);
    }

    #endregion Facts

  }  // class OrderTests

}  // namespace Empiria.Tests.Orders
