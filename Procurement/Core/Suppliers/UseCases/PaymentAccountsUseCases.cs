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

using Empiria.Financial.Adapters;
using Empiria.Financial.Services;
using Empiria.Financial;

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

    public FixedList<PaymentAccountDto> GetPaymentAccounts(string supplierUID) {
      Assertion.Require(supplierUID, nameof(supplierUID));

      var supplier = Supplier.Parse(supplierUID);

      return PaymentAccountServices.GetPaymentAccounts(supplier);
    }


    public PaymentAccountDto UpdatePaymentAccount(PaymentAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      var account = PaymentAccount.Parse(fields.UID);

      account.Update(fields);

      account.Save();

      return new PaymentAccountDto(account);
    }

    #endregion Use cases

  }  // class PaymentAccountsUseCases

}  // namespace Empiria.Procurement.Contracts.UseCases
