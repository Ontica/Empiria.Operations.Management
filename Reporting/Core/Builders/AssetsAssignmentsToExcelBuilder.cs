/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Operations.Reporting.Core.dll         Pattern   : Report builder                       *
*  Type     : AssetsAssignmentsToExcelBuilder               License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with assets assignments information.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.StateEnums;
using Empiria.Storage;

using Empiria.Inventory.Assets;

namespace Empiria.Inventory.Reporting {

  ///<summary>Builds an Excel file with assets assignments information.</summary>
  internal class AssetsAssignmentsToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;


    public AssetsAssignmentsToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(FixedList<AssetAssignment> assignments) {
      Assertion.Require(assignments, nameof(assignments));

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader();

      FillOut(assignments);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }


    private void SetHeader() {
      _excelFile.SetCell(_templateConfig.TitleCell, _templateConfig.Title);
      _excelFile.SetCell(_templateConfig.CurrentTimeCell,
                         $"Generado el: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
    }


    private void FillOut(FixedList<AssetAssignment> assignments) {
      int i = _templateConfig.FirstRowIndex;

      foreach (var assignment in assignments) {
        AssetTransaction txn = assignment.Transaction;
        foreach (var entry in txn.Entries) {
          var asset = entry.Asset;
          _excelFile.SetCell($"A{i}", asset.AssetNo);
          _excelFile.SetCell($"B{i}", asset.Name);
          _excelFile.SetCell($"C{i}", asset.AssetType.Name);
          _excelFile.SetCell($"D{i}", txn.ApplicationDate);
          _excelFile.SetCell($"E{i}", txn.TransactionNo);
          _excelFile.SetCell($"F{i}", txn.AssignedTo.Code);
          _excelFile.SetCell($"G{i}", txn.AssignedTo.Name);
          _excelFile.SetCell($"H{i}", txn.AssignedToOrgUnit.Code);
          _excelFile.SetCell($"I{i}", txn.AssignedToOrgUnit.Name);
          _excelFile.SetCell($"J{i}", txn.ReleasedBy.Code);
          _excelFile.SetCell($"K{i}", txn.ReleasedBy.Name);
          _excelFile.SetCell($"L{i}", txn.ReleasedByOrgUnit.Code);
          _excelFile.SetCell($"M{i}", txn.ReleasedByOrgUnit.Name);
          _excelFile.SetCell($"N{i}", entry.PreviousCondition);
          _excelFile.SetCell($"O{i}", entry.ReleasedCondition);
          _excelFile.SetCell($"P{i}", entry.Description);
          _excelFile.SetCell($"Q{i}", entry.Building.Name);
          _excelFile.SetCell($"R{i}", entry.Floor.Name);
          _excelFile.SetCell($"S{i}", entry.Place.Name);
          _excelFile.SetCell($"T{i}", asset.Brand);
          _excelFile.SetCell($"U{i}", asset.Model);
          _excelFile.SetCell($"V{i}", asset.SerialNo);
          _excelFile.SetCell($"W{i}", txn.AuthorizedBy.Name);
          _excelFile.SetCell($"X{i}", txn.RecordingDate);
          _excelFile.SetCell($"Y{i}", txn.RecordedBy.Name);
          _excelFile.SetCell($"Z{i}", txn.Status.GetName());
          i++;
        }
      }
    }

  } // class AssetsAssignmentsToExcelBuilder

} // namespace Empiria.Inventory.Reporting
