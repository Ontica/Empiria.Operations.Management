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
using Empiria.StateEnums;

using Empiria.Billing;
using Empiria.Billing.Adapters;

using Empiria.Financial.Services;

namespace Empiria.Procurement.Suppliers.Adapters {

  /// <summary>Maps suppliers to their DTOs.</summary>
  static internal class SupplierMapper {

    static internal FixedList<SupplierDescriptor> Map(FixedList<Supplier> suppliers) {
      return suppliers.Select(x => MapToDescriptor(x))
                      .ToFixedList();
    }


    static internal SupplierHolderDto Map(Supplier supplier) {
      var bills = Bill.GetListFor(supplier);

      return new SupplierHolderDto {
        Supplier = MapToDto(supplier),
        PaymentAccounts = PaymentAccountServices.GetPaymentAccounts(supplier),
        Bills = BillMapper.MapToBillStructure(bills),
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

    static private SupplierDescriptor MapToDescriptor(Supplier supplier) {

      return new SupplierDescriptor {
        UID = supplier.UID,
        Name = supplier.Name,
        TypeName = supplier.SupplierType.Name,
        TaxCode = supplier.Code,
        EmployeeNo = supplier.EmployeeNo,
        SubledgerAccount = supplier.SubledgerAccount,
        StatusName = supplier.Status.GetName()
      };
    }


    static private SupplierDto MapToDto(Supplier supplier) {

      return new SupplierDto {
        UID = supplier.UID,
        Name = supplier.Name,
        Type = supplier.SupplierType.MapToNamedEntity(),
        TaxCode = supplier.Code,
        EmployeeNo = supplier.EmployeeNo,
        Tags = supplier.Tags,
        SubledgerAccount = supplier.SubledgerAccount,
        Status = supplier.Status.MapToDto(),
      };
    }

    #endregion Helpers

  }  // class SupplierMapper

}  // namespace Empiria.Procurement.Suppliers.Adapters
