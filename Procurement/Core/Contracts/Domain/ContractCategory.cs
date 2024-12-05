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
  public class ContractCategory : GeneralObject {

    #region Constructors and parsers

    static internal ContractCategory Parse(int id) {
      return ParseId<ContractCategory>(id);
    }

    static internal ContractCategory Parse(string uid) {
      return ParseKey<ContractCategory>(uid);
    }

    static internal FixedList<ContractCategory> GetList() {
      return GetList<ContractCategory>().ToFixedList();
    }

    static internal ContractCategory Empty => ParseEmpty<ContractCategory>();

    #endregion Constructors and parsers

  }  // class ContractCategory

}  // namespace Empiria.Procurement.Contracts
