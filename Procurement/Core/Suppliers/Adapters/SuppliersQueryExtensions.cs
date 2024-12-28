/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Type Extensions                         *
*  Type     : SuppliersQueryExtensions                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for SuppliersQuery type.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Procurement.Suppliers.Adapters {

  /// <summary>Extension methods for SuppliersQuery type.</summary>
  static internal class SuppliersQueryExtensions {

    #region Extension Methods
    static internal void EnsureIsValid(this SuppliersQuery query) {
      // no - op
    }

    static internal string MapToFilterString(this SuppliersQuery query) {

      string roleFilter = BuildRoleFilter();

      string statusFilter = BuildStatusFilter(query.Status);

      string keywordFilter = BuildKeywordsFilter(query.Keywords);

      var filter = new Filter(roleFilter);

      filter.AppendAnd(statusFilter);

      filter.AppendAnd(keywordFilter);

      return filter.ToString();
    }


    internal static string MapToSortString(this SuppliersQuery query) {

      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      }

      return "PARTY_NAME";
    }

    #endregion Extension Methods

    #region Helpers

    static private string BuildKeywordsFilter(string keywords) {
      if (keywords == string.Empty) {
        return string.Empty;
      }

      return SearchExpression.ParseAndLike("PARTY_KEYWORDS", keywords);
    }


    static private string BuildRoleFilter() {
      return "PARTY_ROLES LIKE '%supplier%'";
    }


    static private string BuildStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return "PARTY_STATUS <> 'X' ";
      }

      return $"PARTY_STATUS = '{(char) status}'";
    }

    #endregion Helpers

  }  // class SuppliersQueryExtensions

} // namespace Empiria.Procurement.Suppliers.Adapters
