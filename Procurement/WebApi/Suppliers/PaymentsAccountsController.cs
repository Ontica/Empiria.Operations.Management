/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                         Component : Web Api                               *
*  Assembly : Empiria.Procurement.WebApi.dll               Pattern   : Web api Controller                    *
*  Type     : PaymentsAccountsController                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update payment accounts.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial;
using Empiria.Financial.Adapters;

using Empiria.Procurement.Suppliers.UseCases;
using Empiria.Procurement.Suppliers.Adapters;

namespace Empiria.Procurement.Suppliers.WebApi {

  /// <summary>Web API used to retrieve and update payment accounts.</summary>
  public class PaymentsAccountsController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v8/procurement/suppliers/{supplierUID:guid}/payment-accounts")]
    public CollectionModel GetPaymentAccounts([FromUri] string supplierUID) {

      using (var usecases = PaymentAccountsUseCases.UseCaseInteractor()) {
        FixedList<PaymentAccountDto> accounts = usecases.GetPaymentAccounts(supplierUID);

        return new CollectionModel(base.Request, accounts);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPut, HttpPatch]
    [Route("v8/procurement/suppliers/{supplierUID:guid}/payment-accounts/{accountUID:guid}")]
    public SingleObjectModel UpdatePaymentAccount([FromUri] string supplierUID,
                                                  [FromUri] string accountUID,
                                                  [FromBody] PaymentAccountFields fields) {

      var supplier = Supplier.Parse(supplierUID);

      fields.UID = accountUID;
      fields.PartyUID = supplier.UID;

      using (var usecases = PaymentAccountsUseCases.UseCaseInteractor()) {
        _ = usecases.UpdatePaymentAccount(fields);

        var supplierDto = SupplierMapper.Map(supplier);

        return new SingleObjectModel(base.Request, supplierDto);
      }
    }

    #endregion Command web apis

  }  // class PaymentsAccountsController

}  // namespace Empiria.Procurement.Suppliers.WebApi
