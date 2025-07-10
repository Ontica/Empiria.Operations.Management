/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Operations.Reporting.Core.dll         Pattern   : Report builder                       *
*  Type     : AssetsTransactionsToExcelBuilder              License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with assets transactions information.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.StateEnums;
using Empiria.Storage;

using Empiria.Inventory.Assets;

namespace Empiria.Inventory.Reporting {

  ///<summary>Builds an Excel file with assets transactions information.</summary>
  internal class AssetsTransactionsToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;


    public AssetsTransactionsToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(FixedList<AssetTransaction> transactions) {
      Assertion.Require(transactions, nameof(transactions));

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader();

      FillOut(transactions);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }


    private void SetHeader() {
      _excelFile.SetCell(_templateConfig.TitleCell, _templateConfig.Title);
      _excelFile.SetCell(_templateConfig.CurrentTimeCell,
                         $"Generado el: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
    }


    private void FillOut(FixedList<AssetTransaction> transactions) {
      int i = _templateConfig.FirstRowIndex;

      foreach (var txn in transactions) {
        foreach (var entry in txn.Entries) {
          var asset = entry.Asset;
          _excelFile.SetCell($"A{i}", txn.TransactionNo);
          _excelFile.SetCell($"B{i}", txn.AssetTransactionType.DisplayName);
          _excelFile.SetCell($"C{i}", txn.RecordingDate);
          _excelFile.SetCell($"D{i}", txn.RecordedBy.Name);
          _excelFile.SetCell($"E{i}", txn.ApplicationDate);
          _excelFile.SetCell($"F{i}", txn.AppliedBy.Name);
          _excelFile.SetCell($"G{i}", txn.Status.GetName());
          _excelFile.SetCell($"H{i}", txn.AssignedToOrgUnit.Code);
          _excelFile.SetCell($"I{i}", txn.AssignedToOrgUnit.Name);
          _excelFile.SetCell($"J{i}", txn.AssignedTo.Code);
          _excelFile.SetCell($"K{i}", txn.AssignedTo.Name);
          _excelFile.SetCell($"L{i}", txn.ReleasedBy.Code);
          _excelFile.SetCell($"M{i}", txn.ReleasedBy.Name);
          _excelFile.SetCell($"N{i}", asset.AssetNo);
          _excelFile.SetCell($"O{i}", asset.Name);
          _excelFile.SetCell($"P{i}", asset.AssetType.Name);
          _excelFile.SetCell($"Q{i}", asset.Condition);
          _excelFile.SetCell($"R{i}", asset.Condition);
          _excelFile.SetCell($"S{i}", txn.Building.Name);
          _excelFile.SetCell($"T{i}", txn.Floor.Name);
          _excelFile.SetCell($"U{i}", txn.Place.Name);
          _excelFile.SetCell($"V{i}", asset.Brand);
          _excelFile.SetCell($"W{i}", asset.Model);
          _excelFile.SetCell($"X{i}", asset.Sku.SerialNo);
          i++;
        }
      }
    }

  } // class AssetsTransactionsToExcelBuilder

} // namespace Empiria.Inventory.Reporting
