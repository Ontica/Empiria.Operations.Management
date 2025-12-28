/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Use Cases Layer                         *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Use case interactor class               *
*  Type     : RequisitionUseCases                       License   : Please read LICENSE.txt file             *
*                                                                                                            *
*  Summary  : Use cases used to update and return requisitions.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.Services;

using Empiria.Budgeting;

using Empiria.Orders.Adapters;
using Empiria.Orders.Data;

namespace Empiria.Orders.UseCases {

  /// <summary>Use cases used to update and return requisitions.</summary>
  public class RequisitionUseCases : UseCase {

    #region Constructors and parsers

    protected RequisitionUseCases() {
      // no-op
    }

    static public RequisitionUseCases UseCaseInteractor() {
      return CreateInstance<RequisitionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public RequisitionHolderDto Activate(string requisitionUID) {
      Assertion.Require(requisitionUID, nameof(requisitionUID));

      var requisition = Requisition.Parse(requisitionUID);

      requisition.Activate();

      requisition.Save();

      return RequisitionMapper.Map(requisition);
    }


    public FixedList<RequisitionDescriptor> AvailableRequisitions(Party requestedBy) {
      Assertion.Require(requestedBy, nameof(requestedBy));

      var requisitions = Requisition.GetList()
                                    .FindAll(x => x.RequestedBy.Equals(requestedBy) ||
                                                  x.IsForMultipleBeneficiaries);

      return RequisitionMapper.Map(requisitions);
    }


    public RequisitionHolderDto Create(RequisitionFields fields) {
      Assertion.Require(fields, nameof(fields));
      Assertion.Require(fields.OrderTypeUID, nameof(fields.OrderTypeUID));

      fields.EnsureValid();

      var requisitionType = OrderType.Parse(fields.OrderTypeUID);

      var requisition = new Requisition(requisitionType);

      requisition.Update(fields);

      requisition.Save();

      return RequisitionMapper.Map(requisition);
    }


    public PayableOrderItemDto CreateItem(string requisitionUID,
                                          PayableOrderItemFields fields) {

      Assertion.Require(requisitionUID, nameof(requisitionUID));
      Assertion.Require(fields, nameof(fields));

      var requisition = Requisition.Parse(requisitionUID);

      var item = new PayableOrderItem(OrderItemType.PurchaseOrderItemType, requisition);

      item.Update(fields);

      EnsureCanAddItem(requisition, item);

      requisition.Items.Add(item);

      item.Save();

      return PayableOrderMapper.Map(item);
    }


    public RequisitionHolderDto Delete(string requisitionUID) {
      Assertion.Require(requisitionUID, nameof(requisitionUID));

      var requisition = Requisition.Parse(requisitionUID);

      requisition.Delete();

      requisition.Save();

      return RequisitionMapper.Map(requisition);
    }


    public PayableOrderItemDto DeleteItem(string requisitionUID, string itemUID) {
      Assertion.Require(requisitionUID, nameof(requisitionUID));
      Assertion.Require(itemUID, nameof(itemUID));

      var requisition = Requisition.Parse(requisitionUID);

      var item = requisition.GetItem<PayableOrderItem>(itemUID);

      requisition.Items.Remove(item);

      item.Save();

      return PayableOrderMapper.Map(item);
    }


    public RequisitionHolderDto Get(string requisitionUID) {
      Assertion.Require(requisitionUID, nameof(requisitionUID));

      var requisition = Requisition.Parse(requisitionUID);

      return RequisitionMapper.Map(requisition);
    }


    public FixedList<PayableOrderItemDto> GetAvailableOrderItems(Requisition requisition) {
      Assertion.Require(requisition, nameof(requisition));

      return PayableOrderMapper.Map(requisition.GetItems<PayableOrderItem>());
    }


    public FixedList<RequisitionDescriptor> Search(OrdersQuery query) {
      Assertion.Require(query, nameof(query));

      query.OrderTypeUID = Requisition.Empty.GetEmpiriaType().Name;

      query.EnsureIsValid();

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      FixedList<Requisition> requisitions = OrdersData.Search<Requisition>(filter, sort);

      return RequisitionMapper.Map(requisitions);
    }


    public RequisitionHolderDto Suspend(string requisitionUID) {
      Assertion.Require(requisitionUID, nameof(requisitionUID));

      var requisition = Requisition.Parse(requisitionUID);

      requisition.Suspend();

      requisition.Save();

      return RequisitionMapper.Map(requisition);
    }


    public RequisitionHolderDto Update(RequisitionFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var requisition = Requisition.Parse(fields.UID);

      requisition.Update(fields);

      requisition.Save();

      return RequisitionMapper.Map(requisition);
    }


    public PayableOrderItemDto UpdateItem(string requisitionUID, string itemUID,
                                          PayableOrderItemFields fields) {
      Assertion.Require(requisitionUID, nameof(requisitionUID));
      Assertion.Require(fields, nameof(fields));

      var requisition = Requisition.Parse(requisitionUID);

      var item = requisition.GetItem<PayableOrderItem>(itemUID);

      EnsureCanUpdateItem(requisition, item, BudgetAccount.Parse(fields.BudgetAccountUID));

      item.Update(fields);

      item.Save();

      return PayableOrderMapper.Map(item);
    }


    #endregion Use cases

    #region Business rules

    private void EnsureCanAddItem(Requisition requisition, PayableOrderItem orderItem) {
      if (!requisition.IsMultiYear) {
        return;
      }

      if (requisition.Items.Count == 0) {
        return;
      }

      var firstItem = requisition.Items.GetItems()[0] as PayableOrderItem;

      Assertion.Require(orderItem.BudgetAccount.Equals(firstItem.BudgetAccount),
                        "La cuenta presupuestal de todos los elementos de la requisición " +
                        "debe ser la misma, debido a que las suficiencias presupuestales plurianuales " +
                        "deben cumplir con dicho requisito.");
    }


    private void EnsureCanUpdateItem(Requisition requisition, PayableOrderItem item,
                                     BudgetAccount budgetAccount) {
      if (!requisition.IsMultiYear) {
        return;
      }

      if (requisition.Items.Count == 1) {
        return;
      }

      var distinctItems = requisition.Items.GetItems()
                                           .FindAll(x => x.BudgetAccount.Distinct(budgetAccount));

      Assertion.Require(distinctItems.Count == 0,
                        "La cuenta presupuestal de todos los elementos de la requisición " +
                        "debe ser la misma, debido a que las suficiencias presupuestales plurianuales " +
                        "deben cumplir con dicho requisito.");

    }

    #endregion Business rules

  }  // class RequisitionUseCases

}  // namespace Empiria.Orders.UseCases 
