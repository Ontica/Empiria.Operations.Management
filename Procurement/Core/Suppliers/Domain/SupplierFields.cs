/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                         Component : Domain Layer                          *
*  Assembly : Empiria.Procurement.Core.dll                 Pattern   : Input fields DTO                      *
*  Type     : SupplierFields                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields DTO used to create and update suppliers data.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

namespace Empiria.Procurement.Suppliers {

  /// <summary>Input fields DTO used to create and update suppliers data.</summary>
  public class SupplierFields : PartyFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string TypeUID {
      get; set;
    } = string.Empty;


    public string TaxCode {
      get; set;
    } = string.Empty;


    public string SubledgerAccount {
      get; set;
    } = string.Empty;


    public string SubledgerAccountName {
      get; set;
    } = string.Empty;

  }  // class SupplierFields

} // namespace Empiria.Procurement.Suppliers
