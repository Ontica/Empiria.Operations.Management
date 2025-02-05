/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration              Component : Adapters Layer                       *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Input Query DTO                      *
*  Type     : BudgetAccountsForProductQuery                 License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve the budget accounts that can apply for a given product.       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Operations.Integration.Budgeting.Adapters {

  /// <summary>Input query DTO used to retrieve the budget accounts that can apply for a
  /// given product.</summary>
  public class BudgetAccountsForProductQuery {

    public string ProductUID {
      get; set;
    } = string.Empty;


    public string BudgetUID {
      get; set;
    } = string.Empty;


    public string OrgUnitUID {
      get; set;
    } = string.Empty;

  }  // class BudgetAccountsForProductQuery



  /// <summary>Extension methods for BudgetAccountsForProductQuery type.</summary>
  static internal class BudgetAccountsForProductQueryExtensions {

    static internal void EnsureValid(this BudgetAccountsForProductQuery query) {
      Assertion.Require(query.ProductUID, nameof(query.ProductUID));
      Assertion.Require(query.BudgetUID, nameof(query.BudgetUID));
      Assertion.Require(query.OrgUnitUID, nameof(query.OrgUnitUID));
    }
  }

}  // namespace Empiria.Operations.Integration.Budgeting.Adapters
