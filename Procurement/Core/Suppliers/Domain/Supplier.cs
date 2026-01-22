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
using Empiria.StateEnums;

using Empiria.Procurement.Suppliers.Data;

namespace Empiria.Procurement.Suppliers {

  /// <summary>Represents a supplier of goods or services.</summary>  
  public class Supplier : Party, INamedEntity {

    #region Constructors and parsers

    protected Supplier(PartyType partyType) : base(partyType) {
      // Required by Empiria Framework.
    }

    internal Supplier(SupplierFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Update(fields);
    }

    static public new Supplier Parse(int id) => ParseId<Supplier>(id);

    static public new Supplier Parse(string uid) => ParseKey<Supplier>(uid);

    static public new Supplier Empty => ParseEmpty<Supplier>();

    static public FixedList<Supplier> TryGetWithTaxCode(string taxCode) {
      Assertion.Require(taxCode, nameof(taxCode));

      taxCode = EmpiriaString.Clean(taxCode).ToUpper();

      return GetList<Supplier>($"PARTY_CODE = '{taxCode}' AND PARTY_STATUS <> 'X'")
             .ToFixedList();
    }

    #endregion Constructors and parsers

    #region Properties

    public string EmployeeNo {
      get {
        return ExtendedData.Get("employeeNo", string.Empty);
      }
      private set {
        ExtendedData.SetIfValue("employeeNo", value);
      }
    }


    public string SubledgerAccount {
      get {
        return ExtendedData.Get("taxData/subledgerAccount", string.Empty);
      }
      private set {
        ExtendedData.SetIfValue("taxData/subledgerAccount", value);
      }
    }


    public string SubledgerAccountName {
      get {
        return ExtendedData.Get("taxData/subledgerAccountName", string.Empty);
      }
      private set {
        ExtendedData.SetIfValue("taxData/subledgerAccountName", value);
      }
    }


    public SupplierType SupplierType {
      get {
        return SupplierType.Parse(ExtendedData.Get("supplierType", SupplierType.Unknown.Name));
      }
      private set {
        ExtendedData.SetIfValue("supplierType", value.Name);
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

    string INamedEntity.Name {
      get {
        if (EmployeeNo.Length != 0) {
          return $"{base.Name} (No. expediente: {EmployeeNo})";
        }
        return $"{base.Name} ({TaxCode})";
      }
    }

    #endregion Properties

    #region Methods

    internal void Delete() {
      SetStatus(EntityStatus.Deleted);
    }


    protected override void OnBeforeSave() {
      if (IsNew) {
        PatchObjectId(SuppliersData.GetNextId());
      }
    }


    internal void Update(SupplierFields fields) {
      Assertion.Require(fields, nameof(fields));

      SupplierType = SupplierType.Parse(fields.TypeUID);
      TaxCode = Patcher.Patch(fields.TaxCode, TaxCode);

      SubledgerAccount = Patcher.Patch(fields.SubledgerAccount, SubledgerAccount);
      SubledgerAccountName = Patcher.Patch(fields.SubledgerAccountName, SubledgerAccountName);

      EmployeeNo = EmpiriaString.Clean(fields.EmployeeNo);

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

  } // class Supplier

} // namespace Empiria.Procurement.Suppliers
