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


    public string EmployeeNo {
      get; set;
    } = string.Empty;


    public string SubledgerAccount {
      get; set;
    } = string.Empty;


    public string SubledgerAccountName {
      get; set;
    } = string.Empty;


    internal void EnsureValid() {
      Name = EmpiriaString.Clean(Name).ToUpper();
      TaxCode = EmpiriaString.Clean(TaxCode).ToUpper();
      EmployeeNo = EmpiriaString.Clean(EmployeeNo);
      SubledgerAccount = EmpiriaString.Clean(SubledgerAccount);

      Assertion.Require(Name, "Requiero el nombre del beneficiario.");

      Assertion.Require(TypeUID, "Requiero el tipo de beneficiario.");

      Assertion.Require(SubledgerAccount, "Requiero el auxiliar contable asociado al beneficiario.");
      Assertion.Require(SubledgerAccount.Length == 17 || SubledgerAccount.Length == 18,
                        $"El auxiliar contable debe contener 17 o 18 dígitos.");
      Assertion.Require(EmpiriaString.AllDigits(SubledgerAccount),
                        $"El auxiliar contable debe constar únicamente de dígitos.");

      Assertion.Require(TaxCode, "Requiero el RFC del beneficiario.");
      Assertion.Require(TaxCode.Length == 12 ||
                        TaxCode.Length == 13, "El RFC debe contener 12 o 13 caracteres.");

      Assertion.Require(EmployeeNo.Length == 0 || EmployeeNo.Length == 6,
                        "El número de empleado consta de 6 dígitos.");

    }

  }  // class SupplierFields

} // namespace Empiria.Procurement.Suppliers
