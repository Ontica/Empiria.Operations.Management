/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Power type                              *
*  Type     : ContractItemType                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a contract item.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;

namespace Empiria.Procurement.Contracts {

  /// <summary>Power type that describes a contract item.</summary>
  [Powertype(typeof(ContractItem))]
  public sealed class ContractItemType : Powertype {

    #region Constructors and parsers

    private ContractItemType() {
      // Empiria powertype types always have this constructor.
    }

    static public new ContractItemType Parse(int typeId) => Parse<ContractItemType>(typeId);

    static public new ContractItemType Parse(string typeName) => Parse<ContractItemType>(typeName);

    static public FixedList<ContractItemType> GetList() {
      return Empty.GetAllSubclasses()
            .Select(x => (ContractItemType) x)
            .ToFixedList();
    }

    static public ContractItemType Empty => Parse("ObjectTypeInfo.ContractItem");

    static public ContractItemType NoPayable => Parse("ObjectTypeInfo.ContractItem.NoPayable");

    static public ContractItemType Payable => Parse("ObjectTypeInfo.ContractItem.Payable");

    #endregion Constructors and parsers

  } // class ContractItemType

}  // namespace Empiria.Procurement.Contracts
