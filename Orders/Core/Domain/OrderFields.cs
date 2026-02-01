/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Input fields DTO                        *
*  Type     : PayableOrderFields                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to update payable orders information.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Financial;
using Empiria.StateEnums;
using Empiria.Time;

namespace Empiria.Orders {

  abstract public class OrderFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string OrderTypeUID {
      get; set;
    } = string.Empty;


    public string RequisitionUID {
      get; set;
    } = string.Empty;


    public string[] Budgets {
      get; set;
    } = new string[0];


    public string ContractUID {
      get; set;
    } = string.Empty;


    public string ParentOrderUID {
      get; set;
    } = string.Empty;


    public string CategoryUID {
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


    public string[] Identificators {
      get; set;
    } = new string[0];


    public string[] Tags {
      get; set;
    } = new string[0];


    public DateTime? StartDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public DateTime? EndDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


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
    }

    public string ProviderUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; private set;
    } = string.Empty;


    public Priority? Priority {
      get; set;
    }


    public string Observations {
      get; set;
    } = string.Empty;


    public string GuaranteeNotes {
      get; set;
    } = string.Empty;


    public string PenaltyNotes {
      get; set;
    } = string.Empty;


    public string DeliveryNotes {
      get; set;
    } = string.Empty;


    public int? EstimatedMonths {
      get; set;
    }


    public DateTime RequiredTime {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public decimal ExchangeRate {
      get; set;
    } = decimal.One;


    public string SourceUID {
      get; set;
    } = string.Empty;


    public string DeliveryPlaceUID {
      get; set;
    } = string.Empty;


    public virtual void EnsureValid() {
      Assertion.Require(OrderTypeUID, nameof(OrderTypeUID));

      Assertion.Require(Name, "Necesito se proporcione el nombre");

      Assertion.Require(RequestedByUID, nameof(RequestedByUID));

      Priority = Priority.HasValue ? Priority.Value : StateEnums.Priority.Normal;

      if (string.IsNullOrWhiteSpace(CurrencyUID)) {
        CurrencyUID = Currency.Default.UID;
      }

      if (ExchangeRate == decimal.Zero) {
        ExchangeRate = decimal.One;
      }

      if (CurrencyUID != Currency.Default.UID) {
        Assertion.Require(ExchangeRate > 0 && ExchangeRate != decimal.One,
                         "El tipo de cambio debe ser positivo y distinto a uno.");
      }

      if (!StartDate.HasValue) {
        StartDate = ExecutionServer.DateMaxValue;
      }

      if (!EndDate.HasValue) {
        EndDate = ExecutionServer.DateMaxValue;
      }

      Assertion.Require(StartDate.Value <= EndDate.Value,
                        $"La fecha final del período o vigencia debe ser " +
                        $"posterior a la fecha inicial.");

      if (!EstimatedMonths.HasValue) {
        EstimatedMonths = YearMonth.GetMonths(StartDate.Value, EndDate.Value);
      }

      Assertion.Require(EstimatedMonths <= YearMonth.GetMonths(StartDate.Value, EndDate.Value),
                       "La duración estimada en meses no puede sobrepasar los meses de la vigencia o período.");

      Assertion.Require(Observations.Length <= 3800,
          "El texto de las observaciones es demasiado largo. Máximo de 3800 caracteres");

      Assertion.Require(GuaranteeNotes.Length + PenaltyNotes.Length <= 3800,
          "El texto de las garantías en conjunto con el de las penalidades " +
          "es demasiado largo. Máximo de 3800 caracteres entre los dos");

      Assertion.Require(DeliveryNotes.Length <= 3800,
          "El texto de las condiciones de entrega es demasiado largo. Máximo de 3800 caracteres");
    }

  }  // class OrderFields

}  // namespace Empiria.Orders
