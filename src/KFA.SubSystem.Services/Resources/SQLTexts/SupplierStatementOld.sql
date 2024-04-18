-- SET @fromMonth = '2021-07';
-- SET @toMonth = '2022-06';
-- SET @supplierPrefix = 'S3A';


SELECT
	*
FROM
	kfa_sub_systems.vw_all_cash_receipts Where kfa_sub_systems.vw_all_cash_receipts.`month` >=  @fromMonth AND kfa_sub_systems.vw_all_cash_receipts.`month` <=  @toMonth AND credit_ledger_account_code LIKE CONCAT(@supplierPrefix, '%')
	ORDER BY month, date, kfa_sub_systems.vw_all_cash_receipts.cash_receipt_detail_id;
	
SELECT
	*
FROM
	kfa_sub_systems.vw_all_journals Where kfa_sub_systems.vw_all_journals.`month` >=  @fromMonth AND kfa_sub_systems.vw_all_journals.`month` <=  @toMonth AND (credit_ledger_account_code LIKE CONCAT(@supplierPrefix, '%') OR debit_ledger_account_code LIKE CONCAT(@supplierPrefix, '%'))
	ORDER BY month, date, kfa_sub_systems.vw_all_journals.general_ledger_detail_id;
	
	
SELECT
	*
FROM
	kfa_sub_systems.vw_all_cheques Where kfa_sub_systems.vw_all_cheques.`month` >=  @fromMonth AND kfa_sub_systems.vw_all_cheques.`month` <=  @toMonth AND debit_ledger_account_code LIKE CONCAT(@supplierPrefix, '%')
	ORDER BY month, date, kfa_sub_systems.vw_all_cheques.cheque_id;
	

SELECT
	*
FROM
	kfa_sub_systems.vw_all_petty_cash Where kfa_sub_systems.vw_all_petty_cash.`month` >=  @fromMonth AND kfa_sub_systems.vw_all_petty_cash.`month` <=  @toMonth AND debit_ledger_account_code LIKE CONCAT(@supplierPrefix, '%')
	ORDER BY month, date, kfa_sub_systems.vw_all_petty_cash.petty_cash_detail_id;
	
	
SELECT
	*
FROM
	kfa_sub_systems.vw_all_purchases Where kfa_sub_systems.vw_all_purchases.`month` >=  @fromMonth AND kfa_sub_systems.vw_all_purchases.`month` <=  @toMonth AND supplier_code LIKE CONCAT(@supplierPrefix, '%')
	ORDER BY month, date, kfa_sub_systems.vw_all_purchases.order_record_id;
	
SELECT
	*
FROM
	kfa_sub_systems.vw_all_purchases_invoices Where kfa_sub_systems.vw_all_purchases_invoices.`month` >=  @fromMonth AND kfa_sub_systems.vw_all_purchases_invoices.`month` <=  @toMonth AND supplier_code LIKE CONCAT(@supplierPrefix, '%') ORDER BY month, date, kfa_sub_systems.vw_all_purchases_invoices.supplier_code;
	
	
	
SELECT cost_centre_code, description, supplier_code_prefix FROM kfa_sub_systems.tbl_cost_centres;


SELECT
	tbl_stock_items.item_code, 
	tbl_stock_items.item_name
FROM
	tbl_stock_items;
	
	
SELECT
	tbl_ledger_accounts.ledger_account_code, 
	tbl_ledger_accounts.description
FROM
	tbl_ledger_accounts;