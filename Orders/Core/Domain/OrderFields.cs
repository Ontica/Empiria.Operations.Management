/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Input fields DTO                        *
*  Type     : PayableOrderFields                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to update payable orders information.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Orders {

  abstract public class OrderFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string OrderTypeUID {
      get; set;
    } = string.Empty;


    public string CategoryUID {
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


    public string ParentOrderUID {
      get; set;
    } = string.Empty;


    public virtual void EnsureValid() {
      Assertion.Require(OrderTypeUID, nameof(OrderTypeUID));
      Assertion.Require(CategoryUID, nameof(CategoryUID));
      Assertion.Require(Description, nameof(Description));
      Assertion.Require(RequestedByUID, nameof(RequestedByUID));
      Priority = Priority.HasValue ? Priority.Value : StateEnums.Priority.Normal;
    }

  }  // class OrderFields

}  // namespace Empiria.Orders
