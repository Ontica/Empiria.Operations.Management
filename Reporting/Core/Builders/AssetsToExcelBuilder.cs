/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Operations.Reporting.Core.dll         Pattern   : Report builder                       *
*  Type     : AssetsToExcelBuilder                          License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with assets information.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.StateEnums;
using Empiria.Storage;

using Empiria.Inventory.Assets;

namespace Empiria.Inventory.Reporting {

  ///<summary>Builds an Excel file with assets information.</summary>
  internal class AssetsToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;


    public AssetsToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(FixedList<Asset> assets) {
      Assertion.Require(assets, nameof(assets));

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader();

      FillOut(assets);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }


    private void SetHeader() {
      _excelFile.SetCell(_templateConfig.TitleCell, _templateConfig.Title);
      _excelFile.SetCell(_templateConfig.CurrentTimeCell,
                         $"Generado el: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
    }


    private void FillOut(FixedList<Asset> assets) {
      int i = _templateConfig.FirstRowIndex;

      foreach (var asset in assets) {
        _excelFile.SetCell($"A{i}", asset.AssetNo);
        _excelFile.SetCell($"B{i}", EmpiriaString.Tagging(asset.Identificators));
        _excelFile.SetCell($"C{i}", asset.Name);
        _excelFile.SetCell($"D{i}", asset.AssetType.Name);
        _excelFile.SetCell($"E{i}", asset.CurrentCondition);
        _excelFile.SetCell($"F{i}", asset.Building.Name);
        _excelFile.SetCell($"G{i}", asset.Floor.Name);
        _excelFile.SetCell($"H{i}", asset.Place.Name);
        _excelFile.SetCell($"I{i}", asset.AssignedToOrgUnit.Code);
        _excelFile.SetCell($"J{i}", asset.AssignedToOrgUnit.Name);
        _excelFile.SetCell($"K{i}", asset.AssignedTo.Code);
        _excelFile.SetCell($"L{i}", asset.AssignedTo.Name);
        _excelFile.SetCell($"M{i}", asset.Brand);
        _excelFile.SetCell($"N{i}", asset.Model);
        _excelFile.SetCell($"O{i}", asset.SerialNo);
        _excelFile.SetCell($"P{i}", asset.AcquisitionDate);
        _excelFile.SetCell($"Q{i}", asset.SupplierName);
        _excelFile.SetCell($"R{i}", asset.InvoiceNo);
        _excelFile.SetCell($"S{i}", asset.AccountingTag);
        _excelFile.SetCell($"T{i}", asset.HistoricalValue);
        _excelFile.SetCell($"U{i}", asset.InUse.GetName());
        _excelFile.SetCell($"V{i}", asset.LastAssignmentTransactionNo);
        _excelFile.SetCell($"W{i}", asset.LastUpdate);
        i++;
      }
    }

  } // class AssetsToExcelBuilder

} // namespace Empiria.Inventory.Reporting
