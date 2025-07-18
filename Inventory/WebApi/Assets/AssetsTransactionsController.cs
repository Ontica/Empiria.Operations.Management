/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                            Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Query Api Controller                  *
*  Type     : AssetsTransactionsController                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve assets transactions data.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.StateEnums;
using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Inventory.Assets.Adapters;
using Empiria.Inventory.Assets.Data;
using Empiria.Inventory.Assets.UseCases;

using Empiria.Inventory.Reporting;

namespace Empiria.Inventory.Assets.WebApi {

  /// <summary>Web API used to retrieve assets transactions data.</summary>
  public class AssetsTransactionsController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/assets/transactions/clean")]
    public NoDataModel CleanTransactions() {
      var transactions = AssetTransaction.GetList();

      foreach (var txn in transactions) {
        AssetsTransactionsData.Clean(txn);
      }

      return new NoDataModel(base.Request);
    }


    [HttpPost]
    [Route("v2/assets/transactions/clean-entries")]
    public NoDataModel CleanTransactionEntries() {
      var entries = AssetTransactionEntry.GetFullList<AssetTransactionEntry>();

      foreach (var entry in entries) {
        AssetsTransactionsData.Clean(entry);
      }

      return new NoDataModel(base.Request);
    }


    [HttpPost]
    [Route("v2/assets/transactions/{transactionUID:guid}/clone-for/{transactionTypeUID}")]
    public SingleObjectModel CloneAssetTransaction([FromUri] string transactionUID,
                                                   [FromUri] string transactionTypeUID) {

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        AssetTransactionHolderDto transaction = usecases.CloneAssetTransaction(transactionUID, transactionTypeUID);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPost]
    [Route("v2/assets/transactions/{transactionUID:guid}/close")]
    public SingleObjectModel CloseAssetTransaction([FromUri] string transactionUID) {

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        AssetTransactionHolderDto transaction = usecases.CloseAssetTransaction(transactionUID);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPost]
    [Route("v2/assets/transactions")]
    public SingleObjectModel CreateAssetTransaction([FromBody] AssetTransactionFields fields) {

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        AssetTransactionHolderDto transaction = usecases.CreateAssetTransaction(fields);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpDelete]
    [Route("v2/assets/transactions/{transactionUID:guid}")]
    public NoDataModel DeleteAssetTransaction([FromUri] string transactionUID) {

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        _ = usecases.DeleteAssetTransaction(transactionUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPost]
    [Route("v2/assets/transactions/export")]
    public SingleObjectModel ExportAssetsAssignmentsToExcel([FromBody] AssetsTransactionsQuery query) {

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        FixedList<AssetTransaction> transactions = usecases.SearchAssetTransactions(query);

        var reportingService = AssetsReportingService.ServiceInteractor();

        FileDto excelFile = reportingService.ExportAssetsTransactionsToExcel(transactions);

        return new SingleObjectModel(base.Request, excelFile);
      }
    }


    [HttpGet]
    [Route("v2/assets/transactions/types")]
    public CollectionModel GetAssetTransactionTypes() {

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> transactionTypes = usecases.GetAssetTransactionTypes();

        return new CollectionModel(base.Request, transactionTypes);
      }
    }


    [HttpGet]
    [Route("v2/assets/transactions/{transactionUID:guid}")]
    public SingleObjectModel GetAssetTransaction([FromUri] string transactionUID) {

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        AssetTransactionHolderDto transaction = usecases.GetAssetTransaction(transactionUID);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpGet]
    [Route("v2/assets/transactions/assignees")]
    public CollectionModel GetAssetTransactionsAssignees([FromUri] string keywords = "") {

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> assignees = usecases.GetAssetTransactionsAssignees(keywords);

        return new CollectionModel(base.Request, assignees);
      }
    }


    [HttpGet]
    [Route("v2/assets/transactions/managers")]
    public CollectionModel GetAssetTransactionsManagers([FromUri] string keywords = "") {

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> managers = usecases.GetAssetTransactionsManagers(keywords);

        return new CollectionModel(base.Request, managers);
      }
    }


    [HttpGet]
    [Route("v2/assets/transactions/{transactionUID:guid}/print")]
    public SingleObjectModel PrintAssetTransaction([FromUri] string transactionUID) {

        var transaction = AssetTransaction.Parse(transactionUID);

        using (var reportingService = AssetsReportingService.ServiceInteractor()) {
            FileDto file = reportingService.ExportAssetsTransactionToPdf(transaction);

            return new SingleObjectModel(base.Request, file);
        }
    }


    [HttpPost]
    [Route("v2/assets/transactions/search")]
    public CollectionModel SearchAssetTransactions([FromBody] AssetsTransactionsQuery query) {

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        FixedList<AssetTransaction> transactions = usecases.SearchAssetTransactions(query);

        return new CollectionModel(base.Request, AssetTransactionMapper.Map(transactions));
      }
    }


    [HttpPost]
    [Route("v2/assets/transactions/parties")]
    public CollectionModel SearchAssetTransactionParties([FromBody] TransactionPartiesQuery query) {

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> parties = usecases.SearchAssetTransactionsParties(query);

        return new CollectionModel(base.Request, parties);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/assets/transactions/{transactionUID:guid}")]
    public SingleObjectModel UpdateAssetTransaction([FromUri] string transactionUID,
                                                    [FromBody] AssetTransactionFields fields) {

      fields.TransactionTypeUID = transactionUID;

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        AssetTransactionHolderDto transaction = usecases.UpdateAssetTransaction(transactionUID, fields);

        return new SingleObjectModel(base.Request, transaction);
      }
    }

    #endregion Web Apis

  }  // class AssetsTransactionsController

}  // namespace Empiria.Inventory.Assets.WebApi
