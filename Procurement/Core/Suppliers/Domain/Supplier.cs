/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                         Component : Domain Layer                          *
*  Assembly : Empiria.Procurement.Core.dll                 Pattern   : Information Holder                    *
*  Type     : Supplier                                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a supplier of goods or services.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Parties;

namespace Empiria.Procurement.Suppliers {

  /// <summary>Represents a supplier of goods or services.</summary>  
  public class Supplier : Party {

    #region Constructors and parsers

    protected Supplier(PartyType partyType) : base(partyType) {
      // Required by Empiria Framework.
    }

    internal Supplier(string taxCode) {
      taxCode = BuildAndValidateTaxCode(taxCode);
    }

    static public new Supplier Parse(int id) => ParseId<Supplier>(id);

    static public new Supplier Parse(string uid) => ParseKey<Supplier>(uid);

    static public new Supplier Empty => ParseEmpty<Supplier>();

    #endregion Constructors and parsers

    #region Properties

    public string SubledgerAccount {
      get {
        return ExtendedData.Get("taxData/subledgerAccount", string.Empty);
      }
      private set {
        ExtendedData.SetIfValue("taxData/subledgerAccount", value);
      }
    }


    public string TaxCode {
      get {
        return Code;
      }
      private set {
        Code = value;
      }
    }

    #endregion Properties

    #region Methods

    public void Update(SupplierFields fields) {
      Assertion.Require(fields, nameof(fields));

      TaxCode = Patcher.Patch(BuildAndValidateTaxCode(fields.TaxCode), TaxCode);
      SubledgerAccount = Patcher.Patch(fields.SubledgerAccount, SubledgerAccount);

      if (fields.UID.Length == 0) {
        fields.StartDate = DateTime.Today;
      } else {
        fields.StartDate = base.StartDate;
      }

      fields.EndDate = ExecutionServer.DateMaxValue;

      fields.Roles = EmpiriaString.Tagging("supplier")
                                  .ToArray();

      base.Update(fields);
    }

    #endregion Methods

    #region Helpers

    private string BuildAndValidateTaxCode(string taxCode) {
      taxCode = EmpiriaString.Clean(taxCode);

      Assertion.Require(taxCode, nameof(taxCode));

      if (taxCode.Length < 12) {
        Assertion.RequireFail($"El RFC proporcionado '{taxCode}' es inválido. " +
                              $"Debe contener al menos 12 caracteres.");
      }

      return taxCode.ToUpper();
    }

    #endregion Helpers

  } // class Supplier

} // namespace Empiria.Procurement.Suppliers
