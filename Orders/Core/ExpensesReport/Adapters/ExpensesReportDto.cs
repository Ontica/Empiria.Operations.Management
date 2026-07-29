/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data Transfer Object                    *
*  Type     : PayableOrderDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return payable orders information.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Orders.Adapters {

  /// <summary>Data transfer object used to return complete expenses reports information.</summary>
  public class ExpensesReportHolderDto : PayableOrderHolderDto {

    public new FixedList<ExpensesReportItemDto> Items {
      get; internal set;
    }

  }  // class ExpensesReportHolderDto



  /// <summary>Data transfer object used to return payable orders information.</summary>
  public class ExpensesReportDto : PayableOrderDto {

    protected internal ExpensesReportDto(ExpensesReport expensesReport) : base(expensesReport) {
      PayableOrder = PayableOrderMapper.MapToDescriptor(expensesReport.PayableOrder);

      // Todo: remove this hardcoded value and get it from the expensesReport instance.
      ExpensesReportType = new NamedEntityDto("Settlement", "Gasto por comprobar");
    }


    public NamedEntityDto ExpensesReportType {
      get;
    }

    public OrderDescriptor PayableOrder {
      get;
    }

  }  // class ExpensesReportDto

}  // namespace Empiria.Orders.Adapters
