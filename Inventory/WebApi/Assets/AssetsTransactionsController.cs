﻿/* Empiria Operations ****************************************************************************************
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
using Empiria.WebApi;

using Empiria.Inventory.Assets.Adapters;
using Empiria.Inventory.Assets.UseCases;

namespace Empiria.Inventory.Assets.WebApi {

  /// <summary>Web API used to retrieve assets transactions data.</summary>
  public class AssetsTransactionsController : WebApiController {

    #region Web Apis

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


    [HttpPost]
    [Route("v2/assets/transactions/search")]
    public CollectionModel SearchAssetTransactions([FromBody] AssetsTransactionsQuery query) {

      using (var usecases = AssetTransactionUseCases.UseCaseInteractor()) {
        FixedList<AssetTransactionDescriptorDto> transactions = usecases.SearchAssetTransactions(query);

        return new CollectionModel(base.Request, transactions);
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
    #endregion Web Apis

  }  // class AssetsTransactionsController

}  // namespace Empiria.Inventory.Assets.WebApi
