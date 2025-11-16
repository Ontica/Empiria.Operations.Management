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
  [Powertype(typeof(FormerContractItem))]
  public sealed class FormerContractItemType : Powertype {

    #region Constructors and parsers

    private FormerContractItemType() {
      // Empiria powertype types always have this constructor.
    }

    static public new FormerContractItemType Parse(int typeId) => Parse<FormerContractItemType>(typeId);

    static public new FormerContractItemType Parse(string typeName) => Parse<FormerContractItemType>(typeName);

    static public FixedList<FormerContractItemType> GetList() {
      return Empty.GetAllSubclasses()
            .Select(x => (FormerContractItemType) x)
            .ToFixedList();
    }

    static public FormerContractItemType Empty => Parse("ObjectTypeInfo.ContractItem");

    static public FormerContractItemType NoPayable => Parse("ObjectTypeInfo.ContractItem.NoPayable");

    static public FormerContractItemType Payable => Parse("ObjectTypeInfo.ContractItem.Payable");

    #endregion Constructors and parsers

  } // class ContractItemType

}  // namespace Empiria.Procurement.Contracts
