using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFA.SubSystem.Services.Models;
public record LedgerRecord(bool? IsReversed, DateTime? PostingDate, string? DocumentType, string? DocumentNo, string? BankAccountNo, string? Description, decimal Amount, string? BalAccountType, string? BalAccountNo, string? BalAccountName, string? ExternalDocumentNo, string? EntryNo,
    string? BranchCode, string? DeptCode, bool Reversed, (string monthName, string monthCode) month);
