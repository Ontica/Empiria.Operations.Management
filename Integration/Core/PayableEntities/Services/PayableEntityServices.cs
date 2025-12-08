/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Services Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service interactor class                *
*  Type     : PayableEntityServices                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Services used to retrive and communicate with payable entities.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;

using Empiria.Financial;
using Empiria.Parties;
using Empiria.Services;

using Empiria.Billing;

using Empiria.Orders;
using Empiria.Orders.Adapters;

using Empiria.Payments.Adapters;
using Empiria.Payments.Payables.Adapters;

namespace Empiria.Payments.Payables.Services {

  /// <summary>Services used to retrive and communicate with payable entities.</summary>
  public class PayableEntityServices : Service {

    #region Constructors and parsers

    protected PayableEntityServices() {
      // no-op
    }

    static public PayableEntityServices ServiceInteractor() {
      return CreateInstance<PayableEntityServices>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<NamedEntityDto> GetPayableEntityTypes() {

      return OrderType.GetList()
                      .FindAll(x => x.Name.Contains("PayableOrder"))
                      .MapToNamedEntityList();
    }


    public PayableOrderHolderDto RequestPayment(string orderUID) {
      Assertion.Require(orderUID, nameof(orderUID));

      var order = (IPayableEntity) PayableOrder.Parse(orderUID);

      var paymentOrder = new PaymentOrder(order);

      var accounts = PaymentAccount.GetListFor((Party) order.PayTo);

      Assertion.Require(accounts.Count > 0, "El proveedor no tiene cuentas asignadas.");

      var paymentAccount = accounts[0];

      var totalBilled = Bill.GetListFor(order)
                            .Sum(x => x.BillType.Name.Contains("CreditNote") ? -1 * x.Total : x.Total);

      var fields = new PaymentOrderFields {
        PayToUID = order.PayTo.UID,
        CurrencyUID = order.Currency.UID,
        Description = order.Name,
        DueTime = DateTime.Now.AddDays(7),
        PaymentAccountUID = paymentAccount.UID,
        PaymentMethodUID = paymentAccount.PaymentMethod.UID,
        RequestedByUID = order.OrganizationalUnit.UID,
        RequestedTime = DateTime.Now,
        ReferenceNumber = order.EntityNo,
        Total = totalBilled,
        PayableEntityTypeUID = order.GetEmpiriaType().UID,
        PayableEntityUID = order.UID,
      };

      paymentOrder.Update(fields);

      paymentOrder.Save();

      return PayableOrderMapper.Map((PayableOrder) order);
    }


    public FixedList<PayableEntityDto> SearchPayableEntities(PayableEntitiesQuery query) {
      Assertion.Require(query, nameof(query));

      // ToDo: Change this fixed code to search for any payable entities

      var orders = BaseObject.GetFullList<PayableOrder>()
                             .ToFixedList()
                             .FindAll(x => x.Status != StateEnums.EntityStatus.Deleted);

      return PayableEntityMapper.Map(orders);
    }

    #endregion Use cases

  }  // class PayableEntityServices

}  // namespace Empiria.Payments.Payables.UseCases
