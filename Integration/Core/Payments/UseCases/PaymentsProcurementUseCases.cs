/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Payments Integration               Component : Use cases Layer                      *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Use case interactor class            *
*  Type     : PaymentsProcurementUseCases                   License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to integrate organization's procurement operations with the payments system.    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Financial;
using Empiria.Parties;
using Empiria.Services;

using Empiria.Billing;

using Empiria.Orders;
using Empiria.Orders.Adapters;

using Empiria.Payments;

namespace Empiria.Operations.Integration.Payments.UseCases {

  /// <summary>Use cases used to integrate organization's procurement operations
  /// with the payments system.</summary>
  public class PaymentsProcurementUseCases : UseCase {

    #region Constructors and parsers

    protected PaymentsProcurementUseCases() {
      // no-op
    }

    static public PaymentsProcurementUseCases UseCaseInteractor() {
      return CreateInstance<PaymentsProcurementUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public PayableOrderHolderDto RequestPayment(string orderUID, PaymentOrderFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(fields, nameof(fields));

      var order = (IPayableEntity) PayableOrder.Parse(orderUID);

      var paymentOrder = new PaymentOrder(order);

      var accounts = PaymentAccount.GetListFor((Party) order.PayTo);

      Assertion.Require(accounts.Count > 0, "El proveedor no tiene cuentas asignadas.");

      var paymentAccount = accounts[0];

      // ToDo: Use bill type operation toknow if adds or substracts
      var totalBilled = Bill.GetListFor(order)
                            .Sum(x => x.BillType.Name.Contains("CreditNote") ? -1 * x.Total : x.Total);

      fields.PayToUID = order.PayTo.UID;
      fields.CurrencyUID = order.Currency.UID;
      fields.Description = order.Name;
      fields.Observations = fields.Description;
      fields.RequestedByUID = order.OrganizationalUnit.UID;
      fields.ReferenceNumber = order.EntityNo;
      fields.Total = totalBilled;
      fields.PayableEntityTypeUID = order.GetEmpiriaType().UID;
      fields.PayableEntityUID = order.UID;

      paymentOrder.Update(fields);

      paymentOrder.Save();

      return PayableOrderMapper.Map((PayableOrder) order);
    }

    #endregion Use cases

  }  // class PaymentsProcurementUseCases

}  // namespace Empiria.Operations.Integration.Payments.UseCases
