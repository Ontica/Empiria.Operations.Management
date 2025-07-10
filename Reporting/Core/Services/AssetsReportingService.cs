/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Operations.Reporting.Core.dll         Pattern   : Service provider                     *
*  Type     : AssetsReportingService                        License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Provides services used to generate assets related reports.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.Storage;

using Empiria.Office;

using Empiria.Inventory.Assets;

namespace Empiria.Inventory.Reporting {

  /// <summary>Provides services used to generate assets related reports.</summary>
  public class AssetsReportingService : Service {

    #region Constructors and parsers

    private AssetsReportingService() {
      // no-op
    }

    static public AssetsReportingService ServiceInteractor() {
      return Service.CreateInstance<AssetsReportingService>();
    }

    #endregion Constructors and parsers

    #region Services

    public FileDto ExportAssetsToExcel(FixedList<Asset> assets) {
      Assertion.Require(assets, nameof(assets));

      var templateUID = $"{this.GetType().Name}.ExportAssetsToExcel";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new AssetsToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(assets);

      return excelFile.ToFileDto();
    }

    #endregion Services

  } // class BudgetTransactionReportingService

} // namespace Empiria.Inventory.Reporting
