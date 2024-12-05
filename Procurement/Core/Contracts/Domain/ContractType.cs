/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Power type                              *
*  Type     : ContractType                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a contract.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;

namespace Empiria.Procurement.Contracts {

  /// <summary>Power type that describes a contract.</summary>
  [Powertype(typeof(Contract))]
  public sealed class ContractType : Powertype {

    #region Constructors and parsers

    private ContractType() {
      // Empiria powertype types always have this constructor.
    }

    static public new ContractType Parse(int typeId) => Parse<ContractType>(typeId);

    static public new ContractType Parse(string typeName) => Parse<ContractType>(typeName);

    static public FixedList<ContractType> GetList() {
      return Empty.GetAllSubclasses()
            .Select(x => (ContractType) x)
            .ToFixedList();
    }

    static public ContractType Empty => Parse("ObjectTypeInfo.Contract");

    static public ContractType Procurement => Parse("ObjectTypeInfo.Contract.Procurement");

    #endregion Constructors and parsers

  } // class ContractType

}  // namespace Empiria.Procurement.Contracts
