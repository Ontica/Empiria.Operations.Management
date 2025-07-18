/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Service provider                        *
*  Type     : AssetTransactionRules                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to control assets transaction's rules.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.HumanResources;
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Inventory.Assets {

  /// <summary>Provides services to control assets transaction's rules.</summary>
  internal class AssetTransactionRules {

    #region Fields

    static internal readonly string ACQUISITION_MANAGER = "acquisition-manager";
    static internal readonly string FIXED_ASSSET_MANAGER = "fixed-assets-manager";
    static internal readonly string FIXED_ASSSET_AUTHORIZER = "fixed-assets-manager";

    private readonly AssetTransaction _transaction;
    private readonly Party _currentUser;
    private readonly FixedList<string> _userRoles;
    private readonly FixedList<OrganizationalUnit> _acquisitionOrgUnits;

    #endregion Fields

    #region Constructors and parsers

    internal AssetTransactionRules(AssetTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));

      _transaction = transaction;

      _currentUser = Party.ParseWithContact(ExecutionServer.CurrentContact);

      _userRoles = Accountability.GetResponsibleRoles(_currentUser);

      _acquisitionOrgUnits = GetUserAcquisitionOrgUnits();
    }


    static internal FixedList<OrganizationalUnit> GetUserAcquisitionOrgUnits() {

      var currentUser = Party.ParseWithContact(ExecutionServer.CurrentContact);

      return Accountability.GetCommissionersFor<OrganizationalUnit>(currentUser, "fixed-assets", ACQUISITION_MANAGER);
    }

    #endregion Constructors and parsers

    #region Properties

    public bool CanAuthorize {
      get {
        if (_transaction.Status != TransactionStatus.OnAuthorization) {
          return false;
        }
        if (_userRoles.Contains(FIXED_ASSSET_MANAGER) ||
            _userRoles.Contains(FIXED_ASSSET_AUTHORIZER)) {
          return true;
        }
        if (_userRoles.Contains(ACQUISITION_MANAGER) &&
            _acquisitionOrgUnits.Contains(x => x.Equals(_transaction.ReleasedByOrgUnit))) {
          return true;
        }
        return false;
      }
    }


    public bool CanClone {
      get {
        if (_transaction.Status != TransactionStatus.Closed) {
          return false;
        }
        if (_userRoles.Contains(FIXED_ASSSET_MANAGER) ||
            _userRoles.Contains(FIXED_ASSSET_AUTHORIZER)) {
          return true;
        }
        return false;
      }
    }


    public bool CanClose {
      get {
        if (_transaction.Status != TransactionStatus.Authorized) {
          return false;
        }
        if (_userRoles.Contains(FIXED_ASSSET_MANAGER) ||
            _userRoles.Contains(FIXED_ASSSET_AUTHORIZER)) {
          return true;
        }
        return false;
      }
    }


    public bool CanDelete {
      get {
        if (_transaction.Status != TransactionStatus.Pending) {
          return false;
        }

        if (_userRoles.Contains(ACQUISITION_MANAGER) &&
            _acquisitionOrgUnits.Contains(x => x.Equals(_transaction.ReleasedByOrgUnit))) {
          return true;
        }
        if (_currentUser.Equals(_transaction.PostedBy) || _currentUser.Equals(_transaction.RecordedBy)) {
          return true;
        }
        return false;
      }
    }


    public bool CanEditDocuments {
      get {
        if (_transaction.Status != TransactionStatus.Authorized) {
          return false;
        }
        if (_userRoles.Contains(ACQUISITION_MANAGER) &&
            _acquisitionOrgUnits.Contains(x => x.Equals(_transaction.ReleasedByOrgUnit))) {
          return true;
        }
        if (_currentUser.Equals(_transaction.PostedBy) || _currentUser.Equals(_transaction.RecordedBy)) {
          return true;
        }
        return false;
      }
    }


    public bool CanReject {
      get {
        if (_transaction.Status != TransactionStatus.OnAuthorization &&
            _transaction.Status != TransactionStatus.Authorized) {
          return false;
        }
        if (_userRoles.Contains(FIXED_ASSSET_MANAGER) ||
            _userRoles.Contains(FIXED_ASSSET_AUTHORIZER)) {
          return true;
        }
        return false;
      }
    }


    public bool CanSendToAuthorization {
      get {
        if (_transaction.Status != TransactionStatus.Pending) {
          return false;
        }

        if (_transaction.Entries.Count == 0) {
          return false;
        }
        if (_transaction.AssetTransactionType.IsProtected &&
           (_userRoles.Contains(FIXED_ASSSET_MANAGER) ||
            _userRoles.Contains(FIXED_ASSSET_AUTHORIZER))) {
          return true;
        }
        if (_userRoles.Contains(ACQUISITION_MANAGER) &&
            _acquisitionOrgUnits.Contains(x => x.Equals(_transaction.ReleasedByOrgUnit))) {
          return true;
        }
        if (_currentUser.Equals(_transaction.PostedBy) || _currentUser.Equals(_transaction.RecordedBy)) {
          return true;
        }

        return false;
      }
    }


    public bool CanUpdate {
      get {
        if (_transaction.IsNew) {
          return true;
        }
        if (_transaction.Status != TransactionStatus.Pending) {
          return false;
        }
        if (_transaction.AssetTransactionType.IsProtected &&
            (_userRoles.Contains(FIXED_ASSSET_MANAGER) ||
             _userRoles.Contains(FIXED_ASSSET_AUTHORIZER))) {
          return true;
        }
        if (_userRoles.Contains(ACQUISITION_MANAGER) &&
            _acquisitionOrgUnits.Contains(x => x.Equals(_transaction.ReleasedByOrgUnit))) {
          return true;
        }
        if (_currentUser.Equals(_transaction.PostedBy) || _currentUser.Equals(_transaction.RecordedBy)) {
          return true;
        }

        return false;
      }
    }

    #endregion Properties

  }  // class AssetTransactionRules

}  // namespace Empiria.Inventory.Assets
