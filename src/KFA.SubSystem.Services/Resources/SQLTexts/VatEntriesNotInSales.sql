-- SET @dateFrom = '2024-03-01';
-- SET @dateFrom = '2024-03-31';

SELECT
	`Posting Date`, 
	`Document Type`, 
	`Document No.`, 
	`G/L Account No.`, 
	`G/L Account Name`, 
	Description, 
	`Job No.`, 
	`Branch Code`, 
	`Dept Code`, 
	`IC Partner Code`, 
	`Gen. Posting Type`, 
	`Gen. Bus. Posting Group`, 
	`Gen. Prod. Posting Group`, 
	Quantity, 
	Amount, 
	`Additional-Currency Amount`, 
	`VAT Amount`, 
	`Bal. Account Type`, 
	`Bal. Account No.`, 
	`User ID`, 
	`Source Code`, 
	`Reason Code`, 
	Reversed, 
	`Reversed by Entry No.`, 
	`Reversed Entry No.`, 
	`FA Entry Type`, 
	`FA Entry No.`, 
	`Entry No.`, 
	`Dimension Set ID`, 
	`External Document No.`
FROM
	dynamics_tims.tbl_general_ledger_entries
WHERE `Posting Date` > @dateFrom AND `Posting Date` < @dateTo AND 
`Document No.` NOT IN (SELECT `No.` FROM dynamics_tims.tbl_posted_sales_invoices);