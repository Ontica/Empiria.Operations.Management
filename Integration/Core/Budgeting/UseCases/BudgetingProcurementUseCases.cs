/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Use cases Layer                      *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Use case interactor class            *
*  Type     : BudgetingProcurementUseCases                  License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to integrate organization's procurement operations with the budgeting system.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.Parties;

using Empiria.Orders;
using Empiria.Orders.Data;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;

using Empiria.Payments;
using Empiria.Payments.Adapters;

using Empiria.Operations.Integration.Budgeting.Adapters;

namespace Empiria.Operations.Integration.Budgeting.UseCases {

  /// <summary>Use cases used to integrate organization's procurement operations
  /// with the budgeting system.</summary>
  public class BudgetingProcurementUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetingProcurementUseCases() {
      // no-op
    }

    static public BudgetingProcurementUseCases UseCaseInteractor() {
      return CreateInstance<BudgetingProcurementUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<BudgetTransactionDescriptorDto> GetBudgetRequests(BudgetRequestFields fields) {
      return new FixedList<BudgetTransactionDescriptorDto>();
    }


    public BudgetTransactionDescriptorDto RequestBudget(BudgetRequestFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      bool isPaymentOrder = fields.BaseObjectTypeUID.Contains("PaymentOrder");
      Order order;


      if (isPaymentOrder) {
        var po = PaymentOrder.Parse(fields.BaseObjectUID);

        order = Order.Parse(po.PayableEntity.UID);

      } else {
        order = Order.Parse(fields.BaseObjectUID);
      }

      order.Activate();

      order.Save();

      OrderBudgetTransactionBuilder builder;

      if (isPaymentOrder) {
        builder = new OrderBudgetTransactionBuilder(BudgetTransactionType.ComprometerGastoCorriente, order);
      } else {
        builder = new OrderBudgetTransactionBuilder(BudgetTransactionType.ApartarGastoCorriente, order);
      }

      BudgetTransaction transaction = builder.Build();

      transaction.SendToAuthorization();

      transaction.Save();

      return BudgetTransactionMapper.MapToDescriptor(transaction);
    }


    public FixedList<PayableEntityDto> SearchRelatedDocumentsForTransactionEdition(RelatedDocumentsQuery query) {
      Assertion.Require(query, nameof(query));

      var filter = new Filter();

      var orgUnit = OrganizationalUnit.Parse(query.OrganizationalUnitUID);

      filter.AppendAnd($"ORDER_REQUESTED_BY_ID = {orgUnit.Id}");
      filter.AppendAnd($"ORDER_STATUS <> 'X'");

      if (query.Keywords.Length != 0) {
        filter.AppendAnd(SearchExpression.ParseAndLikeKeywords("ORDER_KEYWORDS", query.Keywords));
      }

      FixedList<Order> orders = OrdersData.Search<Order>(filter.ToString(), "ORDER_NO");

      return orders.Select(x => MapToPayableEntity(x))
                   .ToFixedList();
    }

    public BudgetValidationResultDto ValidateBudget(BudgetRequestFields fields) {
      Assertion.Require(fields, nameof(fields));

      return new BudgetValidationResultDto();
    }

    #endregion Use cases

    #region Helpers

    static private PayableEntityDto MapToPayableEntity(Order order) {
      return new PayableEntityDto {
        UID = order.UID.ToString(),
        Type = order.OrderType.MapToNamedEntity(),
        EntityNo = order.OrderNo,
        Name = order.Name,
        Description = order.Description,
        Budget = order.BaseBudget.MapToNamedEntity(),
        Currency = order.Currency.MapToNamedEntity(),
        Total = order.Total
      };
    }

    #endregion Helpers

  }  // class BudgetingProcurementUseCases

}  // namespace Empiria.Operations.Integration.Budgeting.UseCases
