/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                      Component : Web Api                               *
*  Assembly : Empiria.Inventory.WebApi.dll                 Pattern   : Query Api Controller                  *
*  Type     : FixedAssetsTransactionsController            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve fixed assets transactions data.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.StateEnums;
using Empiria.WebApi;

using Empiria.Inventory.FixedAssets.Adapters;
using Empiria.Inventory.FixedAssets.UseCases;

namespace Empiria.Inventory.FixedAssets.WebApi {

  /// <summary>Web API used to retrieve fixed assets transactions data.</summary>
  public class FixedAssetsTransactionsController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/fixed-assets/transactions/types")]
    public CollectionModel GetFixedAssetTransactionTypes() {

      using (var usecases = FixedAssetTransactionUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> transactionTypes = usecases.GetFixedAssetTransactionTypes();

        return new CollectionModel(base.Request, transactionTypes);
      }
    }


    [HttpGet]
    [Route("v2/fixed-assets/transactions/{transactionUID:guid}")]
    public SingleObjectModel GetFixedAssetTransaction([FromUri] string transactionUID) {

      using (var usecases = FixedAssetTransactionUseCases.UseCaseInteractor()) {
        FixedAssetTransactionHolderDto transaction = usecases.GetFixedAssetTransaction(transactionUID);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPost]
    [Route("v2/fixed-assets/transactions/search")]
    public CollectionModel SearchFixedAssetTransactions([FromBody] FixedAssetTransactionQuery query) {

      using (var usecases = FixedAssetTransactionUseCases.UseCaseInteractor()) {
        FixedList<FixedAssetTransactionDescriptorDto> transactions = usecases.SearchFixedAssetTransactions(query);

        return new CollectionModel(base.Request, transactions);
      }
    }


    [HttpPost]
    [Route("v2/fixed-assets/transactions/parties")]
    public CollectionModel SearchFixedAssetTransactionParties([FromBody] TransactionPartiesQuery query) {

      using (var usecases = FixedAssetTransactionUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> parties = usecases.SearchFixedAssetTransactionsParties(query);

        return new CollectionModel(base.Request, parties);
      }
    }
    #endregion Web Apis

  }  // class FixedAssetsTransactionsController

}  // namespace Empiria.Inventory.FixedAssets.WebApi
