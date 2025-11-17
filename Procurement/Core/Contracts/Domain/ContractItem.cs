/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Partitioned Type                        *
*  Type     : ContractItem                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract item.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Commands;
using Empiria.Parties;
using Empiria.Orders;

namespace Empiria.Procurement.Contracts {

  public class ContractItem : OrderItem, IPositionable, INamedEntity {

    #region Constructors and parsers

    private ContractItem(OrderItemType contractItemType) : base(contractItemType) {
      // Required by Empiria Framework.
    }


    public ContractItem(OrderItemType contractItemType,
                        Contract contract, ContractItemFields fields) : this(contractItemType) {
      Assertion.Require(contract, nameof(contract));
      Assertion.Require(fields, nameof(fields));

      Contract = contract;

      Update(fields);
    }


    static internal new ContractItem Parse(string contractItemUID) => ParseKey<ContractItem>(contractItemUID);

    static internal new ContractItem Parse(int id) => ParseId<ContractItem>(id);

    static public new ContractItem Empty => ParseEmpty<ContractItem>();

    #endregion Constructors and parsers

    #region Properties

    public new Contract Contract {
      get {
        return (Contract) base.Contract;
      }
      private set {
        base.Contract = value;
      }
    }

    public decimal MinTotal {
      get {
        return MinQuantity * UnitPrice;
      }
    }


    public decimal MaxTotal {
      get {
        return MaxQuantity * UnitPrice;
      }
    }

    #endregion Properties

    #region Methods

    internal void SetProvider(Party provider) {
      Assertion.Require(provider, nameof(provider));

      base.Provider = provider;

      MarkAsDirty();
    }


    internal void Update(ContractItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Contract = Contract.Parse(fields.ContractUID);
      RequestedBy = Patcher.Patch(fields.RequestedByUID, Order.RequestedBy);
      Provider = Patcher.Patch(fields.ProviderUID, Contract.Provider);

      MinQuantity = fields.MinQuantity == 0 ? fields.Quantity : fields.MinQuantity;
      MaxQuantity = fields.MaxQuantity == 0 ? fields.Quantity : fields.MaxQuantity;

      base.Update(fields);

      MarkAsDirty();
    }

    #endregion Methods

  }  // class ContractItem

}  // namespace Empiria.Procurement.Contracts
