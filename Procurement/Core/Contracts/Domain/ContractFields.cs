/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Fields DTO                              *
*  Type     : ContractFields                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used for update contracts information.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Budgeting;

namespace Empiria.Procurement.Contracts {

  /// <summary>Input fields DTO used for update contracts information.</summary>
  public class ContractFields {

    [Newtonsoft.Json.JsonProperty(PropertyName = "ContractTypeUID")]
    public string ContractCategoryUID {
      get; set;
    } = string.Empty;


    public string RequisitionUID {
      get; set;
    } = string.Empty;


    public string ContractNo {
      get; set;
    } = string.Empty;


    public string Name {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string Justification {
      get; set;
    } = string.Empty;


    public string Notes {
      get; set;
    } = string.Empty;


    public string BudgetTypeUID {
      get; set;
    } = string.Empty;


    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public string[] BudgetsUIDs {
      get; set;
    } = new string[0];


    public string RequestedByUID {
      get; set;
    } = string.Empty;


    public string ResponsibleUID {
      get; set;
    } = string.Empty;


    public string BeneficiaryUID {
      get; set;
    } = string.Empty;

    public bool IsForMultipleBeneficiaries {
      get; set;
    } = false;


    public string ProviderUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; set;
    } = string.Empty;


    public DateTime FromDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public DateTime ToDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public DateTime SignDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    internal void EnsureValid() {
      Assertion.Require(ContractCategoryUID, "Necesito la clasificación o categoría del contrato.");
      Assertion.Require(RequisitionUID, "Necesito el número del requisición.");
      Assertion.Require(ContractNo, "Necesito el número del contrato.");
      Assertion.Require(Name, "Necesito el nombre del contrato.");

      Assertion.Require(CurrencyUID, "Necesito la moneda del contrato.");
      Assertion.Require(BudgetsUIDs.Length > 0, "Necesito se seleccione el presupuesto o presupuestos " +
                                                "asociados al contrato");

      BudgetTypeUID = BudgetTypeUID.Length == 0 ? Budget.Parse(BudgetsUIDs[0]).UID : BudgetTypeUID;

      Assertion.Require(FromDate, "Necesito la fecha de inicio del contrato.");
      Assertion.Require(ToDate, "Necesito la fecha de fin del contrato.");
      Assertion.Require(SignDate, "Necesito la fecha de firma del contrato.");

      Assertion.Require(ProviderUID, "Necesito se proporcione al proveedor del contrato.");
    }

  }  // class ContractFields

}  // namespace Empiria.Procurement.Contracts
