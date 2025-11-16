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
  [Powertype(typeof(FormerContract))]
  public sealed class FormerContractType : Powertype {

    #region Constructors and parsers

    private FormerContractType() {
      // Empiria powertype types always have this constructor.
    }

    static public new FormerContractType Parse(int typeId) => Parse<FormerContractType>(typeId);

    static public new FormerContractType Parse(string typeName) => Parse<FormerContractType>(typeName);

    static public FixedList<FormerContractType> GetList() {
      return Empty.GetAllSubclasses()
            .Select(x => (FormerContractType) x)
            .ToFixedList();
    }

    static public FormerContractType Empty => Parse("ObjectTypeInfo.Contract");

    static public FormerContractType Procurement => Parse("ObjectTypeInfo.Contract.Procurement");

    #endregion Constructors and parsers

  } // class ContractType

}  // namespace Empiria.Procurement.Contracts
