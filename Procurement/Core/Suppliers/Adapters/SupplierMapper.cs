﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Mapper                                  *
*  Type     : SupplierMapper                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps suppliers to their DTOs.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Procurement.Suppliers.Adapters {

  /// <summary>Maps suppliers to their DTOs.</summary>
  static internal class SupplierMapper {

    static internal FixedList<SupplierDescriptor> Map(FixedList<Party> suppliers) {
      return suppliers.Select(x => Map(x))
                      .ToFixedList();
    }

    #region Helpers

    static private SupplierDescriptor Map(Party party) {

      if (party is Organization) {
        return MapOrganization((Organization) party);
      }
      if (party is Person) {
        return MapPerson((Person) party);
      }
      if (party is Group) {
        return MapGroup((Group) party);
      }

      throw Assertion.EnsureNoReachThisCode($"Party type {party.PartyType.DisplayName} do not " +
                                            $"correspond to a Supplier.");
    }

    static private SupplierDescriptor MapGroup(Group group) {
      return new SupplierDescriptor {
        UID = group.UID,
        TypeUID = group.PartyType.UID,
        SupplierTypeName = "Grupo de proveedores",
        Name = group.Name,
        CommonName = string.Empty,
        TaxCode = "N/A",
        TaxEntityName = "N/A",
        TaxZipCode = "N/A",
        StatusName = group.Status.GetName()
      };
    }

    static private SupplierDescriptor MapPerson(Person person) {
      return new SupplierDescriptor {
        UID = person.UID,
        TypeUID = person.PartyType.UID,
        SupplierTypeName = "Persona física",
        Name = person.Name,
        CommonName = string.Empty,
        TaxCode = person.TaxData.TaxCode,
        TaxEntityName = person.TaxData.TaxEntityName,
        TaxZipCode = person.TaxData.TaxZipCode,
        StatusName = person.Status.GetName()
      };
    }

    static private SupplierDescriptor MapOrganization(Organization organization) {
      return new SupplierDescriptor {
        UID = organization.UID,
        TypeUID = organization.PartyType.UID,
        SupplierTypeName = "Persona moral",
        Name = organization.Name,
        CommonName = organization.CommonName,
        TaxCode = organization.TaxData.TaxCode,
        TaxEntityName = organization.TaxData.TaxEntityName,
        TaxZipCode = organization.TaxData.TaxZipCode,
        StatusName = organization.Status.GetName()
      };
    }

    #endregion Helpers

  }  // class SupplierMapper

}  // namespace Empiria.Procurement.Suppliers.Adapters