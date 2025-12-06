/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Mapper                                  *
*  Type     : SupplierMapper                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps suppliers to their DTOs.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial.Services;

namespace Empiria.Procurement.Suppliers.Adapters {

  /// <summary>Maps suppliers to their DTOs.</summary>
  static internal class SupplierMapper {

    static internal FixedList<SupplierDescriptor> Map(FixedList<Party> suppliers) {
      return suppliers.Select(x => MapToDescriptor(x))
                      .ToFixedList();
    }


    static internal SupplierHolderDto Map(Party supplier) {
      var bills = Billing.Bill.GetListFor(supplier);

      return new SupplierHolderDto {
        Supplier = MapToDto(supplier),
        PaymentAccounts = PaymentAccountServices.GetPaymentAccounts(supplier.UID),
        Bills = Billing.Adapters.BillMapper.MapToBillDto(bills),
        Documents = DocumentServices.GetAllEntityDocuments(supplier),
        History = HistoryServices.GetEntityHistory(supplier),
        Actions = new BaseActions() {
          CanUpdate = true,
          CanEditDocuments = true,
          CanDelete = true
        }
      };
    }

    #region Helpers

    static private SupplierDescriptor MapToDescriptor(Party supplier) {
      ITaxableParty taxable = supplier as ITaxableParty;

      return new SupplierDescriptor {
        UID = supplier.UID,
        Name = supplier.Name,
        TypeName = taxable.TaxData.TaxEntityKind,
        TaxCode = supplier.Code,
        SubledgerAccount = taxable.TaxData.SubledgerAccount,
        StatusName = supplier.Status.GetName()
      };
    }


    static private SupplierDto MapToDto(Party supplier) {
      ITaxableParty taxable = supplier as ITaxableParty;

      return new SupplierDto {
        UID = supplier.UID,
        Name = supplier.Name,
        Type = new NamedEntityDto(taxable.TaxData.TaxEntityKind),
        TaxCode = supplier.Code,
        SubledgerAccount = taxable.TaxData.SubledgerAccount,
        Status = supplier.Status.MapToDto(),
      };
    }

    #endregion Helpers

  }  // class SupplierMapper

}  // namespace Empiria.Procurement.Suppliers.Adapters
