using Empiria.Data;

namespace Empiria.Inventory.Assets.Data {

  internal class ImportedAsset {

    static internal FixedList<ImportedAsset> GetList() {
      var op = DataOperation.Parse("SELECT * FROM Z_ACTIVO_FIJO WHERE ASSET_TYPE_ID <> -2");

      return DataReader.GetPlainObjectFixedList<ImportedAsset>(op);
    }


    static internal void Clean(ImportedAsset a) {
      var sql = "UPDATE Z_ACTIVO_FIJO " +
      $"SET AREA = '{EmpiriaString.Clean(a.AREA)}', " +
      $"NOMBRE_AREA = '{EmpiriaString.Clean(a.NOMBRE_AREA)}', " +
      $"NUM_ANTERIOR = '{EmpiriaString.Clean(a.NUM_ANTERIOR)}', " +
      $"NUM_ACTUAL = '{EmpiriaString.Clean(a.NUM_ACTUAL)}', " +
      $"DESCRIPCION = '{EmpiriaString.Clean(a.DESCRIPCION)}', " +
      $"TIPO_ACTIVO = '{EmpiriaString.Clean(a.TIPO_ACTIVO)}', " +
      $"SERIE = '{EmpiriaString.Clean(a.SERIE)}', " +
      $"EDIFICIO = '{EmpiriaString.Clean(a.EDIFICIO)}', " +
      $"PISO = '{EmpiriaString.Clean(a.PISO)}', " +
      $"LUGAR = '{EmpiriaString.Clean(a.LUGAR)}', " +
      $"ESTADO = '{EmpiriaString.Clean(a.ESTADO)}', " +
      $"MARCA = '{EmpiriaString.Clean(a.MARCA)}', " +
      $"MODELO = '{EmpiriaString.Clean(a.MODELO)}', " +
      $"RESGUARDATARIO = '{EmpiriaString.Clean(a.RESGUARDATARIO)}', " +
      $"ANO_ADQUISICION = '{EmpiriaString.Clean(a.ANO_ADQUISICION)}', " +
      $"FACTURA = '{EmpiriaString.Clean(a.FACTURA)}', " +
      $"PROVEEDOR = '{EmpiriaString.Clean(a.PROVEEDOR)}', " +
      $"EN_USO = '{EmpiriaString.Clean(a.EN_USO)}', " +
      $"ESTADO2 = '{EmpiriaString.Clean(a.ESTADO2)}', " +
      $"OBSERVACIONES = '{EmpiriaString.Clean(a.OBSERVACIONES)}', " +
      $"PARTIDA_GLOBAL = '{EmpiriaString.Clean(a.PARTIDA_GLOBAL)}', " +
      $"EXPEDIENTE = '{EmpiriaString.Clean(a.EXPEDIENTE)}', " +
      $"ASSET_TYPE_ID = -2 " +
      $"WHERE ID = {a.ID}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    [DataField("ID")]
    public int ID {
      get; private set;
    }


    [DataField("CVO")]
    public int CVO {
      get; private set;
    }


    [DataField("EDIFICIO")]
    public string EDIFICIO {
      get; private set;
    }


    [DataField("AREA")]
    public string AREA {
      get; private set;
    }


    [DataField("NOMBRE_AREA")]
    public string NOMBRE_AREA {
      get; private set;
    }


    [DataField("NUM_ANTERIOR")]
    public string NUM_ANTERIOR {
      get; private set;
    }


    [DataField("NUM_ACTUAL")]
    public string NUM_ACTUAL {
      get; private set;
    }


    [DataField("DESCRIPCION")]
    public string DESCRIPCION {
      get; private set;
    }


    [DataField("TIPO_ACTIVO")]
    public string TIPO_ACTIVO {
      get; private set;
    }


    [DataField("SERIE")]
    public string SERIE {
      get; private set;
    }


    [DataField("PISO")]
    public string PISO {
      get; private set;
    }


    [DataField("LUGAR")]
    public string LUGAR {
      get; private set;
    }


    [DataField("ESTADO")]
    public string ESTADO {
      get; private set;
    }


    [DataField("MARCA")]
    public string MARCA {
      get; private set;
    }


    [DataField("MODELO")]
    public string MODELO {
      get; private set;
    }


    [DataField("RESGUARDATARIO")]
    public string RESGUARDATARIO {
      get; private set;
    }


    [DataField("ANO_ADQUISICION")]
    public string ANO_ADQUISICION {
      get; private set;
    }


    [DataField("FACTURA")]
    public string FACTURA {
      get; private set;
    }


    [DataField("PROVEEDOR")]
    public string PROVEEDOR {
      get; private set;
    }


    [DataField("EN_USO")]
    public string EN_USO {
      get; private set;
    }


    [DataField("ESTADO2")]
    public string ESTADO2 {
      get; private set;
    }


    [DataField("OBSERVACIONES")]
    public string OBSERVACIONES {
      get; private set;
    }


    [DataField("VALOR_HISTORICO")]
    public decimal VALOR_HISTORICO {
      get; private set;
    }


    [DataField("PARTIDA_GLOBAL")]
    public string PARTIDA_GLOBAL {
      get; private set;
    }


    [DataField("EXPEDIENTE")]
    public string EXPEDIENTE {
      get; private set;
    }


    [DataField("ORG_UNIT_ID")]
    public int ORG_UNIT_ID {
      get; private set;
    }


    [DataField("PERSON_ID")]
    public int PERSON_ID {
      get; private set;
    }

    [DataField("BUILDING_ID")]
    public int BUILDING_ID {
      get; private set;
    }

    [DataField("FLOOR_ID")]
    public int FLOOR_ID {
      get; private set;
    }

    [DataField("OFFICE_ID")]
    public int OFFICE_ID {
      get; private set;
    }

    [DataField("ASSET_TYPE_ID")]
    public int ASSET_TYPE_ID {
      get; private set;
    }

    [DataField("ACCOUNT_ID")]
    public int ACCOUNT_ID {
      get; private set;
    }

    [DataField("SKU_ID")]
    public int SKU_ID {
      get; private set;
    }

  }  // class ImportedAsset

}  // namespace Empiria.Inventory.Assets.Data
