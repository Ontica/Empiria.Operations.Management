/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Use Cases Layer                         *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Use case interactor class               *
*  Type     : OrderBillsUseCases                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services use for associate orders with their corresponding bills.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.IO;

using Empiria.Documents;
using Empiria.Financial;
using Empiria.Services;
using Empiria.Storage;

using Empiria.Billing;
using Empiria.Billing.UseCases;

namespace Empiria.Orders.UseCases {

  /// <summary>Provides services use for associate orders with their corresponding bills.</summary>
  public class OrderBillsUseCases : UseCase {

    #region Constructors and parsers

    protected OrderBillsUseCases() {
      // no-op
    }

    static public OrderBillsUseCases UseCaseInteractor() {
      return CreateInstance<OrderBillsUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public Bill AddBillToOrder(Order order, DocumentFields documentFields, InputFileCollection inputFiles) {
      Assertion.Require(order, nameof(order));
      Assertion.Require(documentFields, nameof(documentFields));
      Assertion.Require(inputFiles, nameof(inputFiles));

      var documentProduct = DocumentProduct.Parse(documentFields.DocumentProductUID);

      if (!documentProduct.Attributes.Get("isCFDI", false)) {
        return AddVoucherBillToOrder(order, documentProduct, documentFields, inputFiles);
      }

      return AddCfdiToOrder(order, documentProduct, documentFields, inputFiles);
    }


    public FixedList<Bill> GetBillsFor(Order order) {
      Assertion.Require(order, nameof(order));

      return Bill.GetListFor((IPayableEntity) order);
    }


    public void RemoveBillFromOrder(Order order, Bill bill) {
      Assertion.Require(order, nameof(order));
      Assertion.Require(bill, nameof(bill));

      bill.Delete();

      bill.Save();

      DocumentServices.RemoveAllDocuments(bill);
    }


    public FixedList<DocumentProduct> GetBillTypes() {
      return DocumentProduct.GetList<DocumentProduct>()
                            .FindAll(x => x.InternalCode.StartsWith("BILL-"))
                            .ToFixedList();
    }

    #endregion Use cases

    #region Helpers

    private Bill AddVoucherBillToOrder(Order order, DocumentProduct documentProduct,
                                        DocumentFields documentFields, InputFileCollection inputFiles) {
      Assertion.Require(documentFields.DocumentNumber, "Se requiere proporcionar el número del oficio o documento.");
      Assertion.Require(documentFields.Total > 0, "Se requiere proporcionar el total del comprobante.");

      var usecases = BillUseCases.UseCaseInteractor();

      InputFile voucherPdfFile = inputFiles[documentProduct.Name];

      var voucherBill = usecases.CreateVoucherBill((IPayableEntity) order, documentFields);

      DocumentServices.StoreDocument(voucherPdfFile, voucherBill, documentFields);

      return voucherBill;
    }


    private Bill AddCfdiToOrder(Order order, DocumentProduct documentProduct,
                                 DocumentFields documentFields, InputFileCollection inputFiles) {
      Assertion.Require(inputFiles.ContainsKey("xml"), "Se requiere proprocionar el archivo XML del comprobante fiscal");

      var usecases = BillUseCases.UseCaseInteractor();

      InputFile xmlFile = inputFiles["xml"];
      InputFile pdfFile = inputFiles.ContainsKey("pdf") ? inputFiles["pdf"] : null;

      var xmlReader = new StreamReader(xmlFile.Stream);

      var xmlAsString = xmlReader.ReadToEnd();

      string billNo = usecases.ExtractCFDINo(xmlAsString);

      var bill = Bill.TryParseWithBillNo(billNo);

      Assertion.Require(bill == null, $"El comprobante con folio fiscal '{billNo}' ya existe en el sistema.");

      bill = usecases.CreateCFDI(xmlAsString, (IPayableEntity) order, documentProduct);

      var xmlDocument = DocumentServices.StoreDocument(xmlFile, bill, documentFields);

      var fields = new DocumentFields {
        UID = xmlDocument.UID,
        DocumentProductUID = documentFields.DocumentProductUID,
        Name = documentFields.Name,
        DocumentNumber = bill.BillNo,
        DocumentDate = bill.IssueDate,
        SourcePartyUID = bill.IssuedBy.UID,
        TargetPartyUID = bill.IssuedTo.UID,
        Description = order.Description
      };

      DocumentServices.UpdateDocument(bill, xmlDocument, fields);

      if (pdfFile != null) {
        DocumentDto pdfDocument = DocumentServices.StoreDocument(pdfFile, bill, fields);

        fields.UID = pdfDocument.UID;
        DocumentServices.UpdateDocument(bill, pdfDocument, fields);
      }

      return bill;
    }

    #endregion Helpers

  }  // class OrderBillsUseCases

}  // namespace Empiria.Orders.UseCases
