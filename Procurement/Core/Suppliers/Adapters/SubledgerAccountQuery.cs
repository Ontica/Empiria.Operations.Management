/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Input query DTO                         *
*  Type     : SubledgerAccountQuery                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query used to search suppliers subledger accounts.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Procurement.Suppliers.Adapters {

  /// <summary>Input query used to search suppliers subledger accounts.</summary>
  public class SubledgerAccountQuery {

    public string UID {
      get; set;
    } = string.Empty;


    public string Name {
      get; set;
    } = string.Empty;


    public string TaxCode {
      get; set;
    } = string.Empty;

  }  // class SubledgerAccountQuery

}  // namespace Empiria.Procurement.Suppliers.Adapters
