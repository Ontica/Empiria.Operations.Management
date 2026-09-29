/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data Transfer Object                    *
*  Type     : AvailableOrderItemDto                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return available order items.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.FinancialAccounting.Transactions.Adapters;

namespace Empiria.Orders.Adapters {

  /// <summary>Data transfer object used to return available order items.</summary>
  public class AvailableOrderItemDto : OrderItemDto {

    protected internal AvailableOrderItemDto(AvailableOrderItem availableOrderItem,
                                             FixedList<AccountingClassificationDto> classifications) :
                                                                      base(availableOrderItem.OrderItem) {
      RequestedTotal = availableOrderItem.RequestedTotal;
      AvailableTotal = availableOrderItem.AvailableTotal;

      AccountingClassifications =
          classifications.FindAll(x => x.SelectorValue == availableOrderItem.OrderItem.BudgetAccount.AccountNo)
                         .SelectFlat(x => x.Classifications);
    }

    public decimal RequestedTotal {
      get;
    }

    public decimal AvailableTotal {
      get;
    }

    public FixedList<NamedEntityDto> AccountingClassifications {
      get;
    }

  }  // class AvailableOrderItemDto



  /// <summary>Maps available order items to their corresponding DTOs.</summary>
  static public class AvailableOrderItemMapper {

    static public FixedList<AvailableOrderItemDto> Map(FixedList<AvailableOrderItem> availableItems,
                                                       FixedList<AccountingClassificationDto> classifications) {
      return availableItems.Select(x => new AvailableOrderItemDto(x, classifications))
                           .ToFixedList();
    }

  }  // class AvailableOrderItemMapper

}  // namespace Empiria.Orders.Adapters
