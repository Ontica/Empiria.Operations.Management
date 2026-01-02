/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data Transfer Object                    *
*  Type     : OrderItemDto                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract DTO used to return OrderItem abstract type data.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.StateEnums;

namespace Empiria.Orders.Adapters {

  /// <summary>Abstract DTO used to return OrderItem abstract type data.</summary>
  abstract public class OrderItemDto {

    protected internal OrderItemDto(OrderItem item) {
      UID = item.UID;
      OrderItemType = item.OrderItemType.MapToNamedEntity();
      Order = item.Order.MapToNamedEntity();
      Product = item.Product.MapToNamedEntity();
      ProductCode = item.ProductCode;
      Description = item.Description;
      Justification = item.Justification;
      ProductUnit = item.ProductUnit.MapToNamedEntity();
      RequestedQty = item.RequestedQuantity;
      Currency = item.Currency.MapToNamedEntity();
      Quantity = item.Quantity;
      UnitPrice = item.UnitPrice;
      Discount = item.Discount;
      PenaltyDiscount = item.PenaltyDiscount;
      Subtotal = item.Subtotal;
      RequestedBy = item.RequestedBy.MapToNamedEntity();
      Budget = item.Budget.MapToNamedEntity();
      BudgetAccount = item.BudgetAccount.MapToNamedEntity();
      BudgetControlNo = item.BudgetEntry.ControlNo;
      Project = item.Project.MapToNamedEntity();
      Requisition = item.Requisition.MapToNamedEntity();
      if (!item.RequisitionItem.IsEmptyInstance && item.RequisitionItem is PayableOrderItem rqp) {
        RequisitionItem = PayableOrderMapper.Map(rqp);
      }
      RelatedItem = item.RelatedItem.MapToNamedEntity();
      OriginCountry = item.OriginCountry.MapToNamedEntity();
      StartDate = item.StartDate;
      EndDate = item.EndDate;
      RequiredTime = item.RequiredTime;
      Status = item.Status.MapToDto();
    }

    public string UID {
      get;
    }

    public NamedEntityDto OrderItemType {
      get;
    }

    public NamedEntityDto Order {
      get;
    }

    public NamedEntityDto Product {
      get;
    }

    public string ProductCode {
      get;
    }

    public string Description {
      get;
    }

    public string Justification {
      get;
    }

    public NamedEntityDto ProductUnit {
      get; set;
    }

    public decimal RequestedQty {
      get;
    }

    public NamedEntityDto Currency {
      get;
    }

    public decimal Quantity {
      get; set;
    }

    public decimal UnitPrice {
      get;
    }

    public decimal Discount {
      get;
    }

    public decimal PenaltyDiscount {
      get;
    }

    public decimal Subtotal {
      get;
    }

    public NamedEntityDto RequestedBy {
      get;
    }

    public NamedEntityDto Budget {
      get;
    }

    public NamedEntityDto BudgetAccount {
      get;
    }

    public string BudgetControlNo {
      get;
    }

    public NamedEntityDto Project {
      get;
    }

    public NamedEntityDto Requisition {
      get;
    }

    public PayableOrderItemDto RequisitionItem {
      get;
    }

    public NamedEntityDto RelatedItem {
      get;
    }

    public NamedEntityDto OriginCountry {
      get;
    }

    public DateTime StartDate {
      get;
    }

    public DateTime EndDate {
      get;
    }

    public DateTime RequiredTime {
      get;
    }

    public NamedEntityDto Status {
      get; private set;
    }

  }  // class OrderItemDto

}  // namespace Empiria.Orders.Adapters
