/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                            Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Query Api Controller                  *
*  Type     : AssetsAssignmentController                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve asset assignments data.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Inventory.Assets.Adapters;
using Empiria.Inventory.Assets.UseCases;

using Empiria.Inventory.Reporting;

namespace Empiria.Inventory.Assets.WebApi {

  /// <summary>Web API used to retrieve asset assignments data.</summary>
  public class AssetsAssignmentController : WebApiController {

    #region Query web apis

    [HttpPost]
    [Route("v2/assets/assignments/export")]
    public SingleObjectModel ExportAssetsAssignmentsToExcel([FromBody] AssetsAssignmentsQuery query) {

      using (var usecases = AssetAssignmentUseCases.UseCaseInteractor()) {
        FixedList<AssetAssignment> assignments = usecases.SearchAssetsAssignments(query);

        var reportingService = AssetsReportingService.ServiceInteractor();

        FileDto excelFile = reportingService.ExportAssetsAssignmentsToExcel(assignments);

        return new SingleObjectModel(base.Request, excelFile);
      }
    }


    [HttpGet]
    [Route("v2/assets/assignments/{assignmentUID}")]
    public SingleObjectModel GetAssetAssignments([FromUri] string assignmentUID) {

      using (var usecases = AssetAssignmentUseCases.UseCaseInteractor()) {
        AssetAssignmentHolder assignment = usecases.GetAssetAssignment(assignmentUID);

        return new SingleObjectModel(base.Request, assignment);
      }
    }


    [HttpPost]
    [Route("v2/assets/assignments/search")]
    public CollectionModel SearchAssetsAssignments([FromBody] AssetsAssignmentsQuery query) {

      using (var usecases = AssetAssignmentUseCases.UseCaseInteractor()) {
        FixedList<AssetAssignment> assignments = usecases.SearchAssetsAssignments(query);

        return new CollectionModel(base.Request, AssetAssignmentMapper.Map(assignments));
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v2/assets/assignments/bulk-operation/{operationID}")]
    public SingleObjectModel BulkOperations([FromUri] string operationID,
                                            [FromBody] AssignmentBulkCommand command) {

      command.TransactionType = AssetTransactionType.ParseWithOperation(operationID);

      using (var usecases = AssetAssignmentUseCases.UseCaseInteractor()) {

        FixedList<AssetTransaction> txns = usecases.CreateBulkTransactions(command);

        FileDto report;

        using (var reporting = AssetsReportingService.ServiceInteractor()) {
          report = reporting.ExportAssetsTransactionToPdf(txns[0]);
        }

        var result = new FileResultDto(report,
            $"Se generaron {txns.Count} transacciones de activo fijo.");

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpPost]
    [Route("v2/assets/assignments/{assignmentUID}/bulk-operation/{operationID}")]
    public SingleObjectModel BulkOperations([FromUri] string assignmentUID,
                                            [FromUri] string operationID,
                                            [FromBody] AssignmentBulkCommand command) {

      command.TransactionType = AssetTransactionType.ParseWithOperation(operationID);

      using (var usecases = AssetAssignmentUseCases.UseCaseInteractor()) {

        AssetTransaction txn = usecases.CreateBulkTransaction(assignmentUID, command);

        FileDto report;

        using (var reporting = AssetsReportingService.ServiceInteractor()) {
          report = reporting.ExportAssetsTransactionToPdf(txn);
        }

        var result = new FileResultDto(report,
            "La transacción de activo fijo fue generada satisfactoriamente.");

        return new SingleObjectModel(base.Request, result);
      }
    }

    #endregion Command web apis

  }  // class AssetsAssignmentController

}  // namespace Empiria.Inventory.Assets.WebApi
