/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Use cases Layer                         *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : FixedAssetTransactionUseCases              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for fixed assets transactions.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Inventory.FixedAssets.Adapters;
using Empiria.Inventory.FixedAssets.Data;

namespace Empiria.Inventory.FixedAssets.UseCases {

  /// <summary>Use cases for fixed assets transactions.</summary>
  public class FixedAssetTransactionUseCases : UseCase {

    #region Constructors and parsers

    protected FixedAssetTransactionUseCases() {
      // no-op
    }

    static public FixedAssetTransactionUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FixedAssetTransactionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedAssetTransactionHolderDto GetFixedAssetTransaction(string transactionUID) {
      Assertion.Require(transactionUID, nameof(transactionUID));

      var transaction = FixedAssetTransaction.Parse(transactionUID);

      return FixedAssetTransactionMapper.Map(transaction);
    }


    public FixedList<NamedEntityDto> GetFixedAssetTransactionTypes() {
      var transactionTypes = FixedAssetTransactionType.GetList();

      return transactionTypes.MapToNamedEntityList();
    }


    public FixedList<FixedAssetTransactionDescriptorDto> SearchFixedAssetTransactions(FixedAssetTransactionQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();

      string sort = query.MapToSortString();

      FixedList<FixedAssetTransaction> transactions = FixedAssetsData.SearchTransactions(filter, sort);

      return FixedAssetTransactionMapper.Map(transactions);
    }


    public FixedList<NamedEntityDto> SearchFixedAssetTransactionsParties(TransactionPartiesQuery query) {
      var persons = BaseObject.GetList<Person>();

      return persons.MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class FixedAssetTransactionUseCases

}  // namespace Empiria.Inventory.FixedAssets.UseCases
