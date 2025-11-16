/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Information Holder                      *
*  Type     : ContractCategory                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a contract category.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Procurement.Contracts {

  /// <summary>Describes a contract category.</summary>
  public class FormerContractCategory : GeneralObject {

    #region Constructors and parsers

    static internal FormerContractCategory Parse(int id) {
      return ParseId<FormerContractCategory>(id);
    }

    static internal FormerContractCategory Parse(string uid) {
      return ParseKey<FormerContractCategory>(uid);
    }

    static internal FixedList<FormerContractCategory> GetList() {
      return GetList<FormerContractCategory>().ToFixedList();
    }

    static internal FormerContractCategory Empty => ParseEmpty<FormerContractCategory>();

    #endregion Constructors and parsers

  }  // class ContractCategory

}  // namespace Empiria.Procurement.Contracts
