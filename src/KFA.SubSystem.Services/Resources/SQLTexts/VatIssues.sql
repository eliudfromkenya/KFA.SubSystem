
-- SET @dateFrom = '2024-04-06';

-- SET @dateTo = '2024-04-14';

-- SELECT @dateTo, @dateFrom ;

DROP TABLE
IF
	EXISTS vx_vat;
	
DROP TABLE
IF
	EXISTS vx_vat_input;
	
DROP TABLE
IF
	EXISTS vx_kra_vat;
	
DROP TABLE
IF
	EXISTS vx_purchases;
	
DROP TABLE
IF
	EXISTS vx_sales;
	
CREATE TABLE vx_sales AS SELECT
tbl_posted_sales_invoices.`No.`,
tbl_posted_sales_invoices.`Branch Code`,
tbl_posted_sales_invoices.`Posting Date`,
tbl_posted_sales_invoices.Amount,
tbl_posted_sales_invoices.`Customer No.`,
tbl_posted_sales_invoices.Customer,
tbl_posted_sales_invoices.`Dept Code`,
tbl_posted_sales_invoices.`Due Date`,
tbl_posted_sales_invoices.`Amount Including VAT` 
FROM
	dynamics_tims.tbl_posted_sales_invoices 
WHERE
	amount <> `Amount Including VAT` 
	AND `Due Date` >= @dateFrom 
	AND `Due Date` <= @dateTo;
	
CREATE TABLE vx_purchases AS SELECT
	tbl_posted_purchases_invoices.`No.`, 
	tbl_posted_purchases_invoices.`Branch Code`, 
	tbl_posted_purchases_invoices.`Vendor Invoice No.`, 
	tbl_posted_purchases_invoices.`Vendor No.`, 
	tbl_posted_purchases_invoices.Vendor, 
	tbl_posted_purchases_invoices.Amount, 
	tbl_posted_purchases_invoices.`Amount Including VAT`, 
	tbl_posted_purchases_invoices.`Location Code`, 
	tbl_posted_purchases_invoices.`Due Date`, 
	tbl_posted_purchases_invoices.Corrective
FROM
	dynamics_tims.tbl_posted_purchases_invoices
	WHERE
	Amount <> `Amount Including VAT` 
	AND `Due Date` >= @dateFrom 
	AND `Due Date` <= @dateTo;
	
CREATE TABLE vx_vat_input AS SELECT
tbl_general_ledger_entries.`Posting Date`,
tbl_general_ledger_entries.`Document No.`,
tbl_general_ledger_entries.`Branch Code`,
tbl_general_ledger_entries.`Dept Code`,
tbl_general_ledger_entries.Amount 
FROM
	dynamics_tims.tbl_general_ledger_entries 
WHERE 
    `G/L Account No.` = '42416'
	AND `Posting Date` >= @dateFrom 
	AND `Posting Date` <= @dateTo;
	
CREATE TABLE vx_vat AS SELECT
tbl_general_ledger_entries.`Posting Date`,
tbl_general_ledger_entries.`Document No.`,
tbl_general_ledger_entries.`Branch Code`,
tbl_general_ledger_entries.`Dept Code`,
tbl_general_ledger_entries.Amount 
FROM
	dynamics_tims.tbl_general_ledger_entries 
WHERE 
    `G/L Account No.` = '42415'
	AND `Posting Date` >= @dateFrom 
	AND `Posting Date` <= @dateTo;

CREATE TABLE vx_kra_vat AS SELECT * FROM dynamics_tims.tbl_kra_portal_confirmations WHERE dynamics_tims.tbl_kra_portal_confirmations.dated >= @dateFrom 
AND dynamics_tims.tbl_kra_portal_confirmations.dated <= @dateTo;

SELECT
	* 
FROM
	vx_purchases 
WHERE
	TRIM( `No.` ) NOT IN ( SELECT TRIM( `Document No.` ) FROM dynamics_tims.tbl_general_ledger_entries WHERE `G/L Account No.` = '42416');

SELECT
	* 
FROM
	vx_vat_input 
WHERE
	TRIM( `Document No.` ) NOT IN ( SELECT TRIM( `No.` ) FROM dynamics_tims.tbl_posted_purchases_invoices );
	
	
SELECT
	* 
FROM
	vx_sales 
WHERE
	TRIM( `No.` ) NOT IN ( SELECT TRIM( `Document No.` ) FROM dynamics_tims.tbl_general_ledger_entries  WHERE `G/L Account No.` = '42415');

SELECT
	* 
FROM
	vx_vat 
WHERE
	TRIM( `Document No.` ) NOT IN ( SELECT TRIM( `No.` ) FROM dynamics_tims.tbl_posted_sales_invoices );

SELECT
	* 
FROM
	vx_sales 
WHERE
	TRIM( `No.` ) NOT IN ( SELECT TRIM( `cash_sale_number` ) FROM dynamics_tims.tbl_kra_portal_confirmations );

SELECT
	A.*,
	B.Amount 
FROM
	vx_sales A,
	vx_vat B 
WHERE
	A.`No.` = B.`Document No.` 
	AND ROUND( A.Amount - A.`Amount Including VAT` ) <> ROUND( B.Amount );
	
SELECT
	A.*,
	B.Amount 
FROM
	vx_purchases A,
	vx_vat_input B 
WHERE
	A.`No.` = B.`Document No.` 
	AND ROUND( A.Amount - A.`Amount Including VAT` ) <> ROUND( B.Amount );

SELECT
	A.*,
	B.total_tax_amount,
	A.`Amount Including VAT` - A.Amount sales_vat 
FROM
	vx_sales A,
	vx_kra_vat B 
WHERE
	A.`No.` = B.`cash_sale_number` 
	AND ROUND( A.`Amount Including VAT` - A.Amount ) <> ROUND( B.total_tax_amount ) 
ORDER BY
	ROUND( B.total_tax_amount ) - ROUND( A.`Amount Including VAT` - A.Amount ) DESC;

SELECT
	`Document No.`,
	COUNT(*) Count 
FROM
	vx_vat_input 
GROUP BY
	`Document No.` 
HAVING
	COUNT(*) > 1;
	
SELECT
	`Document No.`,
	COUNT(*) Count 
FROM
	vx_vat 
GROUP BY
	`Document No.` 
HAVING
	COUNT(*) > 1;

SELECT
	A.* 
FROM
	vx_sales A 
WHERE
	ABS( A.`Amount Including VAT` ) - FLOOR(
	ABS( A.`Amount Including VAT` ));

SELECT
	CONCAT(MONTHNAME( dated ),' ',	YEAR ( dated )) `month`,
	total_tax_amount,
	total_taxable_amount,
	total_invoice_amount 
FROM
	(
	SELECT
		MAX( dated ) dated,
		SUM( total_tax_amount ) total_tax_amount,
		SUM( total_taxable_amount ) total_taxable_amount,
		SUM( total_invoice_amount ) total_invoice_amount 
	FROM
		dynamics_tims.tbl_kra_portal_confirmations 
	GROUP BY
		YEAR ( dated ),
		MONTH ( dated ) 
	ORDER BY
		dated 
	) A;

DROP TABLE
IF
	EXISTS vx_vat;
DROP TABLE
IF
	EXISTS vx_vat_input;
DROP TABLE
IF
	EXISTS vx_kra_vat;
DROP TABLE
IF
	EXISTS vx_purchases;
DROP TABLE
IF
	EXISTS vx_sales;