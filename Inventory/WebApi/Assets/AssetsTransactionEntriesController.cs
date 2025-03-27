/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                            Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Command Web Api Controller            *
*  Type     : AssetsTransactionEntriesController           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Command web API used to update assets transactions entries.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Inventory.Assets.Adapters;
using Empiria.Inventory.Assets.UseCases;

namespace Empiria.Inventory.Assets.WebApi {

  /// <summary>Command web API used to update assets transactions entries.</summary>
  public class AssetsTransactionEntriesController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/assets/transactions/{transactionUID:guid}/entries")]
    public SingleObjectModel CreateAssetTransactionEntry([FromUri] string transactionUID,
                                                         [FromBody] AssetTransactionEntryFields fields) {

      base.RequireBody(fields);

      Assertion.Require(fields.TransactionUID == transactionUID, "transaction.UID mismatch.");

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        AssetTransactionEntryDto entry = usecases.CreateAssetTransactionEntry(fields);

        return new SingleObjectModel(base.Request, entry);
      }
    }


    [HttpDelete]
    [Route("v2/assets/transactions/{transactionUID:guid}/entries/{transactionEntryUID:guid}")]
    public NoDataModel DeleteAssetTransactionEntry([FromUri] string transactionUID,
                                                   [FromUri] string transactionEntryUID) {

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        _ = usecases.DeleteAssetTransactionEntry(transactionUID, transactionEntryUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/assets/transactions/{transactionUID:guid}/entries/{transactionEntryUID:guid}")]
    public SingleObjectModel UpdateAssetTransactionEntry([FromUri] string transactionUID,
                                                         [FromUri] string transactionEntryUID,
                                                         [FromBody] AssetTransactionEntryFields fields) {

      base.RequireBody(fields);

      Assertion.Require(fields.TransactionUID == transactionUID, "transaction.UID mismatch.");
      Assertion.Require(fields.UID == transactionEntryUID, "fields.UID mismatch.");

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        AssetTransactionEntryDto entry = usecases.UpdateAssetTransactionEntry(fields);

        return new SingleObjectModel(base.Request, entry);
      }
    }

    #endregion Web Apis

  }  // class AssetsTransactionEntriesController

}  // namespace Empiria.Inventory.Assets.WebApi
