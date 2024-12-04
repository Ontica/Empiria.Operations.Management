﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Fields DTO                              *
*  Type     : ContractFields                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used for update contracts information.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Input fields DTO used for update contracts information.</summary>
  public class ContractFields {

    public string ContractTypeUID {
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


    public string BudgetTypeUID {
      get; set;
    } = string.Empty;


    public string ManagedByOrgUnitUID {
      get; set;
    } = string.Empty;


    public DateTime FromDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime ToDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime SignDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public string SupplierUID {
      get; set;
    } = string.Empty;


    public string CurrencyUID {
      get; set;
    } = string.Empty;

    public decimal Total {
      get; set;
    } = 0.00m;


    internal void EnsureValid() {
      Assertion.Require(ContractNo, "Se requiere el número de contrato.");
      Assertion.Require(ContractTypeUID, "Necesito el tipo de contrato.");
      Assertion.Require(Name, "Necesito el nombre del contrato.");
      Assertion.Require(Description, "Necesito la descripción del contrato.");
      Assertion.Require(CurrencyUID, "Necesito la moneda del contrato.");
      Assertion.Require(Total, "Necesito el importe del contrato.");
      Assertion.Require(FromDate != ExecutionServer.DateMinValue,
                        "Necesito la fecha del inicio del contrato");
      Assertion.Require(ToDate != ExecutionServer.DateMinValue,
                        "Necesito la fecha del finalización del contrato");
      Assertion.Require(FromDate <= ToDate,
                        "La fecha de finalización del contrato no puede ser " +
                        "anterior a la fecha de inicio.");
      Assertion.Require(SignDate != ExecutionServer.DateMinValue,
                        "Necesito la fecha del firma del contrato");
      Assertion.Require(BudgetTypeUID, "Necesito tipo de presupuesto del contrato.");
      Assertion.Require(SupplierUID, "Necesito el proveedor del contrato.");

    }

  }  // class ContractFields

}  // namespace Empiria.Procurement.Contracts.Adapters
