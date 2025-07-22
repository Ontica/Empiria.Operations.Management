/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data Transfer Object                    *
*  Type     : SalesOrderItemDto                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract DTO used to return SalesOrderItem abstract type data.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Orders.Adapters {

  /// <summary>Abstract DTO used to return SalesOrderItem abstract type data.</summary>
  public class SalesOrderItemDto  {
    
    protected internal SalesOrderItemDto(OrderItem item)  {
      UID = item.UID;
      Order = item.Order.MapToNamedEntity();
      OrderType = item.Order.OrderType.MapToNamedEntity();
      Description = item.Description;
      Quantity = item.Quantity;
      ProductUnit = item.ProductUnit.MapToNamedEntity();
      Product = item.Product.MapToNamedEntity();
      Status = item.Status.MapToDto();
    }
    
    public string UID {
      get; private set;
    }

    public NamedEntityDto Order {
      get; private set;
    }

    public NamedEntityDto OrderType {
      get; private set;
    }

    public string Description {
      get; private set;
    }

    public NamedEntityDto Product {
      get; private set;
    }

    public NamedEntityDto ProductUnit {
      get; private set;
    }

    public decimal Quantity {
      get; private set;
    }

    public NamedEntityDto Status {
      get; private set;
    }

  }  // class SalesOrderItemDto

}  // namespace Empiria.Orders.Adapters
