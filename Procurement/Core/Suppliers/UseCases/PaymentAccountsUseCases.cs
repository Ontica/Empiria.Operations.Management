/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                       Component : Use cases Layer                         *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Use case interactor class               *
*  Type     : PaymentAccountsUseCases                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for payments accounts.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Financial;
using Empiria.Financial.Adapters;
using Empiria.Financial.Services;

namespace Empiria.Procurement.Suppliers.UseCases {

  /// <summary>Use cases for payments accounts.</summary>
  public class PaymentAccountsUseCases : UseCase {

    #region Constructors and parsers

    protected PaymentAccountsUseCases() {

    }

    static public PaymentAccountsUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<PaymentAccountsUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public PaymentAccountDto AddPaymentAccount(Supplier supplier, PaymentAccountFields fields) {
      Assertion.Require(supplier, nameof(supplier));
      Assertion.Require(fields, nameof(fields));

      var account = new PaymentAccount(supplier, fields);

      account.Save();

      return new PaymentAccountDto(account);
    }


    public FixedList<PaymentAccountDto> GetPaymentAccounts(string supplierUID) {
      Assertion.Require(supplierUID, nameof(supplierUID));

      var supplier = Supplier.Parse(supplierUID);

      return PaymentAccountServices.GetPaymentAccounts(supplier);
    }


    internal PaymentAccountDto RemovePaymentAccount(Supplier supplier, PaymentAccount account) {
      Assertion.Require(supplier, nameof(supplier));
      Assertion.Require(account, nameof(account));

      Assertion.Require(account.Party.Equals(supplier), "La cuenta no pertenece al proveedor especificado.");

      account.Delete();

      account.Save();

      return new PaymentAccountDto(account);
    }


    public PaymentAccountDto UpdatePaymentAccount(Supplier supplier, PaymentAccountFields fields) {
      Assertion.Require(supplier, nameof(supplier));
      Assertion.Require(fields, nameof(fields));

      var account = PaymentAccount.Parse(fields.UID);

      Assertion.Require(account.Party.Equals(supplier), "La cuenta no pertenece al proveedor especificado.");

      account.Update(fields);

      account.Save();

      return new PaymentAccountDto(account);
    }

    #endregion Use cases

  }  // class PaymentAccountsUseCases

}  // namespace Empiria.Procurement.Contracts.UseCases
