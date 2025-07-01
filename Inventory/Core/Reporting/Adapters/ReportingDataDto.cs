/*Empiria Operations *****************************************************************************************
*                                                                                                            *
*  Module   : Inventory Management                       Component : Interface adapters                      *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : FinderDataDto                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return inventory reports data.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.DynamicData;

namespace Empiria.Inventory.Reporting.Adapters {


  public interface IReportDto {

  } // interface IReportDto


  /// <summary>Output DTO used to return inventory reports data.</summary>
  public class ReportingDataDto {


    public FinderInventoryQuery Query {
      get; set;
    }


    public FixedList<DataTableColumn> Columns {
      get; set;
    } = new FixedList<DataTableColumn>();


    public FixedList<InventoryStockDescriptorDto> Entries {
      get; set;
    } = new FixedList<InventoryStockDescriptorDto>();


  } // class FinderDataDto 

} // namespace Empiria.Inventory.Reporting.Adapters
