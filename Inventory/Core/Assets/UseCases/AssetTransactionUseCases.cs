/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Use cases Layer                         *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : AssetTransactionUseCases                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for assets transactions.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Inventory.Assets.Adapters;
using Empiria.Inventory.Assets.Data;
using System;

namespace Empiria.Inventory.Assets.UseCases {

  /// <summary>Use cases for assets transactions.</summary>
  public class AssetTransactionUseCases : UseCase {

    #region Constructors and parsers

    protected AssetTransactionUseCases() {
      // no-op
    }

    static public AssetTransactionUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<AssetTransactionUseCases>();
    }


    #endregion Constructors and parsers

    #region Use cases


    public AssetTransactionHolderDto CloneAssetTransaction(string transactionUID, string transactionTypeUID) {
      Assertion.Require(transactionUID, nameof(transactionUID));
      Assertion.Require(transactionTypeUID, nameof(transactionTypeUID));

      var sourceTransaction = AssetTransaction.Parse(transactionUID);
      var transactionType = AssetTransactionType.Parse(transactionTypeUID);

      AssetTransaction transaction = sourceTransaction.Clone(transactionType);

      transaction.Save();

      return AssetTransactionMapper.Map(transaction);
    }


    public AssetTransactionHolderDto CloseAssetTransaction(string transactionUID) {
      Assertion.Require(transactionUID, nameof(transactionUID));

      var transaction = AssetTransaction.Parse(transactionUID);

      transaction.Close();

      transaction.Save();

      return AssetTransactionMapper.Map(transaction);
    }


    public AssetTransactionHolderDto CreateAssetTransaction(AssetTransactionFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var transactionType = AssetTransactionType.Parse(fields.TransactionTypeUID);

      var transaction = new AssetTransaction(transactionType);

      transaction.Update(fields);

      transaction.Save();

      return AssetTransactionMapper.Map(transaction);
    }


    public AssetTransactionHolderDto DeleteAssetTransaction(string transactionUID) {
      Assertion.Require(transactionUID, nameof(transactionUID));

      var transaction = AssetTransaction.Parse(transactionUID);

      transaction.Delete();

      transaction.Save();

      return AssetTransactionMapper.Map(transaction);
    }


    public AssetTransactionHolderDto GetAssetTransaction(string transactionUID) {
      Assertion.Require(transactionUID, nameof(transactionUID));

      var transaction = AssetTransaction.Parse(transactionUID);

      return AssetTransactionMapper.Map(transaction);
    }


    public FixedList<NamedEntityDto> GetAssetTransactionsAssignees(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<Person> assignees = AssetsTransactionsData.GetTransactionsAssignees();

      return assignees.MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> GetAssetTransactionsManagers(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<Person> managers = AssetsTransactionsData.GetTransactionsManagers();

      return managers.MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> GetAssetTransactionTypes() {
      var transactionTypes = AssetTransactionType.GetList();

      return transactionTypes.MapToNamedEntityList(false);
    }


    public FixedList<AssetTransactionDescriptorDto> SearchAssetTransactions(AssetsTransactionsQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();

      string sort = query.MapToSortString();

      FixedList<AssetTransaction> transactions = AssetsTransactionsData.SearchTransactions(filter, sort);

      return AssetTransactionMapper.Map(transactions);
    }


    public FixedList<NamedEntityDto> SearchAssetTransactionsParties(TransactionPartiesQuery query) {
      var persons = BaseObject.GetList<Person>();

      return persons.MapToNamedEntityList();
    }


    public AssetTransactionHolderDto UpdateAssetTransaction(string transactionUID,
                                                            AssetTransactionFields fields) {
      Assertion.Require(transactionUID, nameof(transactionUID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var transaction = AssetTransaction.Parse(transactionUID);

      transaction.Update(fields);

      transaction.Save();

      return AssetTransactionMapper.Map(transaction);
    }

    #endregion Use cases

    #region Entries use cases

    public AssetTransactionEntryDto CreateAssetTransactionEntry(AssetTransactionEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var transaction = AssetTransaction.Parse(fields.TransactionUID);

      AssetTransactionEntry entry = transaction.AppendEntry(fields);

      transaction.Save();

      return AssetTransactionMapper.Map(entry);
    }


    public AssetTransactionEntryDto DeleteAssetTransactionEntry(string transactionUID,
                                                                string transactionEntryUID) {
      Assertion.Require(transactionUID, nameof(transactionUID));
      Assertion.Require(transactionEntryUID, nameof(transactionEntryUID));

      var transaction = AssetTransaction.Parse(transactionUID);

      AssetTransactionEntry entry = transaction.GetEntry(transactionEntryUID);

      transaction.RemoveEntry(entry);

      entry.Save();

      transaction.Save();

      return AssetTransactionMapper.Map(entry);
    }


    public AssetTransactionEntryDto UpdateAssetTransactionEntry(AssetTransactionEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var transaction = AssetTransaction.Parse(fields.TransactionUID);

      AssetTransactionEntry entry = transaction.GetEntry(fields.UID);

      transaction.UpdateEntry(entry, fields);

      transaction.Save();

      return AssetTransactionMapper.Map(entry);
    }

    #endregion Entries use cases

  }  // class AssetTransactionUseCases

}  // namespace Empiria.Inventory.Assets.UseCases
