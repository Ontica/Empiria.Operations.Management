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

      var order = PayableOrder.Parse(orderUID);

      var bills = Bill.GetListFor(order);

      Assertion.Require(bills.Count > 0, "No se han agregado los comprobantes.");

      var subTotalBilled = bills.Sum(x => x.BillType.Name.Contains("CreditNote") ? -1 * x.Subtotal : x.Subtotal);
      var taxes = bills.Sum(x => x.BillType.Name.Contains("CreditNote") ? -1 * x.Taxes : x.Taxes);

      Assertion.Require(subTotalBilled > 0, "El importe total de los comprobantes debe ser mayor a cero.");

      var paymentType = PaymentType.Parse(fields.PaymentTypeUID);

      if (paymentType.NeedsBudgetApproval) {
        Assertion.Require(order.Items.Count > 0, "No se han cargado los conceptos.");
        Assertion.Require(order.Subtotal == subTotalBilled,
                          "El importe antes de impuestos de los comprobantes no coincide con el importe de los conceptos.");
      }

      var paymentOrder = new PaymentOrder(paymentType, order.Provider, order, subTotalBilled + taxes);

      var paymentMethod = paymentOrder.PaymentMethod;

      fields.PayableEntityTypeUID = order.GetEmpiriaType().UID;
      fields.PayableEntityUID = order.UID;
      fields.RequestedByUID = order.RequestedBy.UID;
      fields.Description = order.Name;
      fields.Observations = fields.Description;

      fields.PayToUID = order.Provider.UID;
      fields.CurrencyUID = order.Currency.UID;
      fields.Total = subTotalBilled + taxes;

      paymentOrder.Update(fields);

      paymentOrder.Save();

      return PayableOrderMapper.Map((PayableOrder) order);
    }

    #endregion Use cases

  }  // class PaymentsProcurementUseCases

}  // namespace Empiria.Operations.Integration.Payments.UseCases
