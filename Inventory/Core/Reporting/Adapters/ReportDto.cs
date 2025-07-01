/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Inventory Management                       Component : Interface adapters                      *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : ReportDto                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return report data.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory.Reporting.Adapters {


  /// <summary>Output DTO used to return report data.</summary>
  public class ReportDto {


  } // class ReportDto


  /// <summary>Output DTO used to return inventory report descriptor data.</summary>
  internal class ReportDescriptorDto : IReportDto {


  } // class InventoryReportDescriptorDto


  /// <summary>Output DTO used to return inventory stock descriptor data.</summary>
  public class InventoryStockDescriptorDto : IReportDto {


    public string UID {
      get; set;
    } = string.Empty;


    public string Name {
      get; set;
    } = string.Empty;


    public string Code {
      get; set;
    } = string.Empty;


    public string Presentation {
      get; set;
    } = string.Empty;


    public string WarehouseName {
      get;
      internal set;
    }


    public string Rack {
      get;
      internal set;
    }


    public string Tag {
      get; set;
    } = string.Empty;


    public decimal Stock {
      get; set;
    }


    public decimal RealStock {
      get; set;
    }


    public decimal StockInProcess {
      get; set;
    }


    //public ReportItemType ItemType {
    //  get;
    //  internal set;
    //} = ReportItemType.Entry;


  } // class InventoryStockDescriptorDto


} // namespace Empiria.Inventory.Reporting.Adapters
