/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Operations.Reporting.Core.dll         Pattern   : Report builder                       *
*  Type     : AssetsTransactionVoucherBuilder               License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a Pdf file with a voucher for a assets transaction.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Budgeting.Transactions;
using Empiria.Inventory.Assets;
using Empiria.Office;
using Empiria.Storage;
using System;
using System.IO;
using System.Text;


namespace Empiria.Inventory.Reporting {

    /// <summary>Builds a Pdf file with a voucher for a assets transaction.</summary>
    internal class AssetsTransactionVoucherBuilder {

        private readonly FileTemplateConfig _templateConfig;
        private readonly string _htmlTemplate;

        public AssetsTransactionVoucherBuilder(FileTemplateConfig templateConfig) {
            Assertion.Require(templateConfig, nameof(templateConfig));

            _templateConfig = templateConfig;
            _htmlTemplate = File.ReadAllText(_templateConfig.TemplateFullPath);
        }


        internal FileDto CreateVoucher(AssetTransaction transaction) {
            Assertion.Require(transaction, nameof(transaction));

            string filename = GetVoucherPdfFileName(transaction);

            string html = BuildVoucherHtml(transaction);

            SaveHtmlAsPdf(html, filename);

            return ToFileDto(filename);
        }

        #region Builders

        private string BuildVoucherHtml(AssetTransaction transaction) {
            StringBuilder html = new StringBuilder(_htmlTemplate);

            html = BuildHeader(html, transaction);
            html = BuildEntries(html, transaction);

            return html.ToString();
        }


        private StringBuilder BuildEntries(StringBuilder html, AssetTransaction transaction) {
            string TEMPLATE = GetEntriesTemplate();

            var entriesHtml = new StringBuilder();

            foreach (var entry in transaction.Entries) {
                var entryHtml = new StringBuilder(TEMPLATE.Replace("{{ASSET.ASSET_NO}}", entry.Asset.AssetNo));

                entryHtml.Replace("{{ASSET.NAME}}", entry.Asset.Name);
                entryHtml.Replace("{{ASSET.CONDITION}}", entry.Asset.CurrentCondition);
                entryHtml.Replace("{{DESCRIPTION}}", entry.Description);

                entriesHtml.Append(entryHtml);
            }

            return ReplaceEntriesTemplate(html, entriesHtml);
        }


        private StringBuilder BuildHeader(StringBuilder html, AssetTransaction txn) {
            const string AUTHORIZATION_NO_VALID = "<span class='warning'> CÉDULA DE ACTIVOS FIJOS PENDIENTE DE AUTORIZAR </span>";
            const string APPLICATION_NO_VALID = "<span class='warning'> CÉDULA DE ACTIVOS FIJOS PENDIENTE DE APLICAR </span>";

            html.Replace("{{SYSTEM.DATETIME}}", $"Impresión: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
            html.Replace("{{REPORT.TITLE}}",
                          txn.AuthorizedBy.IsEmptyInstance ? AUTHORIZATION_NO_VALID : _templateConfig.Title);
            html.Replace("{{TRANSACTION_NUMBER}}", txn.TransactionNo);
            html.Replace("{{TRANSACTION_TYPE.NAME}}", txn.AssetTransactionType.MapToNamedEntity().Name);
            html.Replace("{{ASSIGNED_TO.NAME}}", txn.AssignedTo.MapToNamedEntity().Name);
            html.Replace("{{ASSIGNED_TO_ORG_UNIT.NAME}}", txn.AssignedToOrgUnit.MapToNamedEntity().Name);
            html.Replace("{{RELEASED_BY.NAME}}", txn.ReleasedBy.MapToNamedEntity().Name);
            html.Replace("{{RELEASED_BY_ORG_UNIT.NAME}}", txn.ReleasedByOrgUnit.MapToNamedEntity().Name);
            html.Replace("{{BASE_LOCATION.NAME}}", txn.BaseLocation.FullName);
            html.Replace("{{DESCRIPTION}}", txn.Description);
            html.Replace("{{RECORDED_BY.NAME}}", txn.RecordedBy.MapToNamedEntity().Name);
            html.Replace("{{RECORDING_TIME}}", txn.RecordingDate.ToString("dd/MMM/yyyy"));
            html.Replace("{{APPLIED_BY.NAME}}", txn.AppliedBy.MapToNamedEntity().Name);
            html.Replace("{{APPLICATION_DATE}}",
                          txn.AppliedBy.IsEmptyInstance ? APPLICATION_NO_VALID : txn.ApplicationDate.ToString("dd/MMM/yyyy"));
            html.Replace("{{AUTHORIZED_BY.NAME}}", txn.AuthorizedBy.MapToNamedEntity().Name);
            html.Replace("{{AUTHORIZATION_TIME}}",
                          txn.AuthorizedBy.IsEmptyInstance ? AUTHORIZATION_NO_VALID : txn.AuthorizationTime.ToString("dd/MMM/yyyy"));

            return html;
        }


        private string GetEntriesTemplate() {
            int startIndex = _htmlTemplate.IndexOf("{{TRANSACTION_ENTRY.TEMPLATE.START}}");
            int endIndex = _htmlTemplate.IndexOf("{{TRANSACTION_ENTRY.TEMPLATE.END}}");

            var template = _htmlTemplate.Substring(startIndex, endIndex - startIndex);

            return template.Replace("{{TRANSACTION_ENTRY.TEMPLATE.START}}", string.Empty);
        }


        private StringBuilder ReplaceEntriesTemplate(StringBuilder html,
                                                     StringBuilder entriesHtml) {
            int startIndex = html.ToString().IndexOf("{{TRANSACTION_ENTRY.TEMPLATE.START}}");
            int endIndex = html.ToString().IndexOf("{{TRANSACTION_ENTRY.TEMPLATE.END}}");

            html.Remove(startIndex, endIndex - startIndex);

            return html.Replace("{{TRANSACTION_ENTRY.TEMPLATE.END}}", entriesHtml.ToString());
        }

        #endregion

        #region Helpers

        private string GetFileName(string filename) {
            return Path.Combine(FileTemplateConfig.GenerationStoragePath + "/assets.transactions/", filename);
        }


        private string GetVoucherPdfFileName(AssetTransaction txn) {
            return $"cedula.activo.fijo.{txn.TransactionNo}.{DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")}.pdf";
        }


        private void SaveHtmlAsPdf(string html, string filename) {
            string fullpath = GetFileName(filename);

            var pdfConverter = new HtmlToPdfConverter();

            var options = new PdfConverterOptions {
                BaseUri = FileTemplateConfig.TemplatesStoragePath
            };

            pdfConverter.Convert(html, fullpath, options);
        }


        private FileDto ToFileDto(string filename) {
            return new FileDto(FileType.Pdf, FileTemplateConfig.GeneratedFilesBaseUrl + "/assets.transactions/" + filename);
        }

        #endregion

    } // class AssetsTransactionVoucherBuilder

} // namespace Empiria.Inventory.Reporting
