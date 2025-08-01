﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Products SKU Management                    Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : ProductSku                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type that represents a product stock keeping unit (SKU).                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Products;

using Empiria.Inventory.Data;

namespace Empiria.Inventory {

  /// <summary>Partitioned type that represents a product stock keeping unit (SKU).</summary>
  [PartitionedType(typeof(ProductSkuType))]
  public class ProductSku : BaseObject, INamedEntity {

    #region Constructors and parsers

    protected ProductSku(ProductSkuType productSkuType) : base(productSkuType) {
      // Required by Empiria Framework for all partitioned types.
    }

    static internal ProductSku Parse(int id) => ParseId<ProductSku>(id);

    static public ProductSku Parse(string uid) => ParseKey<ProductSku>(uid);

    static public ProductSku Empty => ParseEmpty<ProductSku>();

    #endregion Constructors and parsers

    #region Properties

    public ProductSkuType SkuType {
      get {
        return (ProductSkuType) base.GetEmpiriaType();
      }
    }


    [DataField("SKU_NO")]
    public string SkuNo {
      get; private set;
    }


    [DataField("SKU_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }


    [DataField("SKU_NAME")]
    private string _name;

    public string Name {
      get {
        return _name.Length == 0 ? Product.Name : _name;
      }
      private set {
        _name = EmpiriaString.Clean(value);
      }
    }


    [DataField("SKU_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("SKU_UNIT_ID")]
    public ProductUnit Unit {
      get; private set;
    }


    [DataField("SKU_QTY")]
    public decimal Quantity {
      get; private set;
    }


    [DataField("SKU_ACQUISITION_DATE")]
    public DateTime AcquisitionDate {
      get; private set;
    }


    [DataField("SKU_EXPIRATION_DATE")]
    public DateTime ExpirationDate {
      get; private set;
    }


    [DataField("SKU_BRAND")]
    public string Brand {
      get; private set;
    }


    [DataField("SKU_MODEL")]
    public string Model {
      get; private set;
    }

    [DataField("SKU_SERIAL_NO")]
    public string SerialNo {
      get; private set;
    }


    [DataField("SKU_CONDITION")]
    public string Condition {
      get; private set;
    }


    [DataField("SKU_VARIANT_ATTRS")]
    protected JsonObject VariantAttrs {
      get; private set;
    }


    [DataField("SKU_IDENTIFICATORS")]
    private string _identificators = string.Empty;

    public FixedList<string> Identificators {
      get {
        return EmpiriaString.Tagging(_identificators);
      }
    }


    [DataField("SKU_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return EmpiriaString.Tagging(_tags);
      }
    }


    [DataField("SKU_START_DATE")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("SKU_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    [DataField("SKU_LAST_UPDATE")]
    public DateTime LastUpdate {
      get; private set;
    }


    [DataField("SKU_EXT_DATA")]
    protected JsonObject ExtData {
      get; private set;
    }


    [DataField("SKU_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("SKU_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("SKU_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(SkuNo, Name, Brand, Model, SerialNo,
                                           _identificators, _tags, Product.Keywords, Description);
      }
    }

    #endregion Properties

    #region Methods

    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      ProductsSkusData.WriteProductSku(this, this.VariantAttrs.ToString(), this.ExtData.ToString());
    }

    #endregion Methods

  }  // class ProductSku

}  // namespace Empiria.Inventory
