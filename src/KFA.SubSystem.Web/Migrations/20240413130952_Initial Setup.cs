using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFA.SubSystem.Web.Migrations;

  /// <inheritdoc />
  public partial class InitialSetup : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.AlterDatabase()
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_command_details",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  command_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  action = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  active_state = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  category = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  command_name = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  command_text = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  image_id = table.Column<long>(type: "bigint", nullable: true),
                  image_path = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  is_enabled = table.Column<bool>(type: "tinyint(1)", nullable: true),
                  is_published = table.Column<bool>(type: "tinyint(1)", nullable: true),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  shortcut_key = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_command_details", x => x.command_id);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_communication_messages",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  message_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  attachments = table.Column<byte[]>(type: "longblob", nullable: true),
                  details = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  from = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  message = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  message_type = table.Column<byte>(type: "tinyint unsigned", maxLength: 255, nullable: true),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  status = table.Column<byte>(type: "tinyint unsigned", maxLength: 255, nullable: true),
                  title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  to = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_communication_messages", x => x.message_id);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_cost_centres",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  description = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  is_active = table.Column<bool>(type: "tinyint(1)", nullable: true),
                  region = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  supplier_code_prefix = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_cost_centres", x => x.cost_centre_code);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_default_access_rights",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  right_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  rights = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_default_access_rights", x => x.right_id);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_device_guids",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  guid = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_device_guids", x => x.guid);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_item_groups",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  group_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  parent_group_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_item_groups", x => x.group_id);
                  table.ForeignKey(
                      name: "FK_tbl_item_groups_tbl_item_groups_parent_group_id",
                      column: x => x.parent_group_id,
                      principalTable: "tbl_item_groups",
                      principalColumn: "group_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_password_safes",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  password_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  details = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  reminder = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  users_visible_to = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_password_safes", x => x.password_id);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_payroll_groups",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  group_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  group_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_payroll_groups", x => x.group_id);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_project_issues",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  project_issue_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  category = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  effect = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  registered_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                  sub_category = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_project_issues", x => x.project_issue_id);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_staff_groups",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  group_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_staff_groups", x => x.group_number);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_system_rights",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  right_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  is_compulsory = table.Column<bool>(type: "tinyint(1)", nullable: false),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  right_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_system_rights", x => x.right_id);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_tims_machines",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  machine_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  class_type = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  current_status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                  domain_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  external_ip_address = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  external_port_number = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  internal_ip_address = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  internal_port_number = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  ready_for_use = table.Column<bool>(type: "tinyint(1)", nullable: false),
                  serial_number = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  tims_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_tims_machines", x => x.machine_id);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_user_roles",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  role_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  expiration_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  maturity_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  role_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_user_roles", x => x.role_id);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_verification_types",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  verification_type_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  category = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  verification_type_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_verification_types", x => x.verification_type_id);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_actual_budget_variances_batch_headers",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  batch_key = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  approved_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  batch_number = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  cash_sales_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  computer_number_of_records = table.Column<short>(type: "smallint", nullable: false),
                  computer_total_actual_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  computer_total_budget_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  month = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  number_of_records = table.Column<short>(type: "smallint", nullable: false),
                  prepared_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  purchaseses_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  total_actual_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  total_budget_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_actual_budget_variances_batch_headers", x => x.batch_key);
                  table.ForeignKey(
                      name: "FK_tbl_actual_budget_variances_batch_headers_tbl_cost_centres_c~",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code",
                      onDelete: ReferentialAction.Cascade);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_computer_anydesks",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  anydesk_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  anydesk_number = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  name_of_user = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  device_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  type = table.Column<byte>(type: "tinyint unsigned", maxLength: 255, nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_computer_anydesks", x => x.anydesk_id);
                  table.ForeignKey(
                      name: "FK_tbl_computer_anydesks_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_count_sheet_batches",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  batch_key = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  batch_number = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  class_of_card = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  computer_number_of_records = table.Column<short>(type: "smallint", nullable: false),
                  computer_total_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  month = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  no_of_records = table.Column<short>(type: "smallint", nullable: false),
                  total_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_count_sheet_batches", x => x.batch_key);
                  table.ForeignKey(
                      name: "FK_tbl_count_sheet_batches_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_data_devices",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  device_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  device_caption = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  device_code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  device_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  device_number = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  device_right = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  station = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  type_of_device = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_data_devices", x => x.device_id);
                  table.ForeignKey(
                      name: "FK_tbl_data_devices_tbl_cost_centres_station",
                      column: x => x.station,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_expense_budget_batch_headers",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  batch_key = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  approved_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  batch_number = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  computer_number_of_records = table.Column<short>(type: "smallint", nullable: false),
                  computer_total_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  month_from = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  month_to = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  number_of_records = table.Column<short>(type: "smallint", nullable: false),
                  prepared_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  total_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_expense_budget_batch_headers", x => x.batch_key);
                  table.ForeignKey(
                      name: "FK_tbl_expense_budget_batch_headers_tbl_cost_centres_cost_centr~",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code",
                      onDelete: ReferentialAction.Cascade);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_purchases_budget_batch_headers",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  batch_key = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  approved_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  batch_number = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  computer_number_of_records = table.Column<short>(type: "smallint", nullable: false),
                  computer_total_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  month_from = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  month_to = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  number_of_records = table.Column<short>(type: "smallint", nullable: false),
                  prepared_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  total_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  total_quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_purchases_budget_batch_headers", x => x.batch_key);
                  table.ForeignKey(
                      name: "FK_tbl_purchases_budget_batch_headers_tbl_cost_centres_cost_cen~",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_qr_codes_requests",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  qr_code_request_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  is_duplicate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  request_data = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  response_data = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  response_status = table.Column<byte>(type: "tinyint unsigned", maxLength: 255, nullable: true),
                  time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  tims_machine_used = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  vat_class = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_qr_codes_requests", x => x.qr_code_request_id);
                  table.ForeignKey(
                      name: "FK_tbl_qr_codes_requests_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_sales_budget_batch_headers",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  batch_key = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  approved_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  batch_number = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  computer_number_of_records = table.Column<short>(type: "smallint", nullable: false),
                  computer_total_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  month_from = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  month_to = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  number_of_records = table.Column<short>(type: "smallint", nullable: false),
                  prepared_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  total_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  total_quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_sales_budget_batch_headers", x => x.batch_key);
                  table.ForeignKey(
                      name: "FK_tbl_sales_budget_batch_headers_tbl_cost_centres_cost_centre_~",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_vendor_codes_requests",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  vendor_code_request_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  attanded_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  requesting_user = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  status = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  time_attended = table.Column<DateTime>(type: "datetime(6)", maxLength: 255, nullable: true),
                  time_of_request = table.Column<DateTime>(type: "datetime(6)", maxLength: 255, nullable: true),
                  vendor_code = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  vendor_type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_vendor_codes_requests", x => x.vendor_code_request_id);
                  table.ForeignKey(
                      name: "FK_tbl_vendor_codes_requests_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_stock_items",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  item_code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  barcode = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  group_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  item_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_stock_items", x => x.item_code);
                  table.ForeignKey(
                      name: "FK_tbl_stock_items_tbl_item_groups_group_id",
                      column: x => x.group_id,
                      principalTable: "tbl_item_groups",
                      principalColumn: "group_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_issues_attachments",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  attachment_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  attachment_type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  data = table.Column<byte[]>(type: "longblob", nullable: true),
                  description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  file = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  issue_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  time = table.Column<DateTime>(type: "datetime(6)", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_issues_attachments", x => x.attachment_id);
                  table.ForeignKey(
                      name: "FK_tbl_issues_attachments_tbl_project_issues_issue_id",
                      column: x => x.issue_id,
                      principalTable: "tbl_project_issues",
                      principalColumn: "project_issue_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_issues_progresses",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  progress_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  issue_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  reported_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  status = table.Column<byte>(type: "tinyint unsigned", maxLength: 255, nullable: true),
                  time = table.Column<DateTime>(type: "datetime(6)", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_issues_progresses", x => x.progress_id);
                  table.ForeignKey(
                      name: "FK_tbl_issues_progresses_tbl_project_issues_issue_id",
                      column: x => x.issue_id,
                      principalTable: "tbl_project_issues",
                      principalColumn: "project_issue_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_issues_submissions",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  submission_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  issue_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  status = table.Column<byte>(type: "tinyint unsigned", maxLength: 255, nullable: true),
                  submitted_to = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  submitting_user = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  time_submitted = table.Column<DateTime>(type: "datetime(6)", maxLength: 255, nullable: true),
                  type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_issues_submissions", x => x.submission_id);
                  table.ForeignKey(
                      name: "FK_tbl_issues_submissions_tbl_project_issues_issue_id",
                      column: x => x.issue_id,
                      principalTable: "tbl_project_issues",
                      principalColumn: "project_issue_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_employee_details",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  employee_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  amount_due = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  classfication = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  date = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  full_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  gender = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  group_number = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  id_number = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  payroll_group_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  payroll_number = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  phone_number = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  rejoin_date = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  remarks = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  retiree_amount = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  retrenchment_amount = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  retrenchment_date = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  status = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_employee_details", x => x.employee_id);
                  table.ForeignKey(
                      name: "FK_tbl_employee_details_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
                  table.ForeignKey(
                      name: "FK_tbl_employee_details_tbl_payroll_groups_payroll_group_id",
                      column: x => x.payroll_group_id,
                      principalTable: "tbl_payroll_groups",
                      principalColumn: "group_id");
                  table.ForeignKey(
                      name: "FK_tbl_employee_details_tbl_staff_groups_group_number",
                      column: x => x.group_number,
                      principalTable: "tbl_staff_groups",
                      principalColumn: "group_number");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_system_users",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  user_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  contact = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  email_address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  expiration_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                  is_active = table.Column<bool>(type: "tinyint(1)", nullable: true),
                  maturity_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                  name_of_the_user = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  password_hash = table.Column<byte[]>(type: "longblob", nullable: true),
                  password_salt = table.Column<byte[]>(type: "longblob", nullable: true),
                  role_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  username = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_system_users", x => x.user_id);
                  table.ForeignKey(
                      name: "FK_tbl_system_users_tbl_user_roles_role_id",
                      column: x => x.role_id,
                      principalTable: "tbl_user_roles",
                      principalColumn: "role_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_actual_budget_variances",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  actual_budget_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  actual_value = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  batch_key = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  budget_value = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  comment = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  field_1 = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  field_2 = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  field_3 = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  ledger_code = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  ledger_cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_actual_budget_variances", x => x.actual_budget_id);
                  table.ForeignKey(
                      name: "FK_tbl_actual_budget_variances_tbl_actual_budget_variances_batc~",
                      column: x => x.batch_key,
                      principalTable: "tbl_actual_budget_variances_batch_headers",
                      principalColumn: "batch_key");
                  table.ForeignKey(
                      name: "FK_tbl_actual_budget_variances_tbl_cost_centres_ledger_cost_cen~",
                      column: x => x.ledger_cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_expenses_budget_details",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  expense_budget_detail_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  basis_of_calculation = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  batch_key = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  ledger_account_code = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  month_01 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_02 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_03 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_04 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_05 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_06 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_07 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_08 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_09 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_10 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_11 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_12 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_expenses_budget_details", x => x.expense_budget_detail_id);
                  table.ForeignKey(
                      name: "FK_tbl_expenses_budget_details_tbl_expense_budget_batch_headers~",
                      column: x => x.batch_key,
                      principalTable: "tbl_expense_budget_batch_headers",
                      principalColumn: "batch_key");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_qr_request_items",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  sale_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  cash_sale_number = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  hs_code = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  hs_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  item_code = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  item_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  percentage_discount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  request_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  total_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  unit_of_measure = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  unit_price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  vat_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  vat_class = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_qr_request_items", x => x.sale_id);
                  table.ForeignKey(
                      name: "FK_tbl_qr_request_items_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
                  table.ForeignKey(
                      name: "FK_tbl_qr_request_items_tbl_qr_codes_requests_request_id",
                      column: x => x.request_id,
                      principalTable: "tbl_qr_codes_requests",
                      principalColumn: "qr_code_request_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_price_change_requests",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  request_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  attanded_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  batch_number = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  cost_price = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  item_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  requesting_user = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  selling_price = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  status = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  time_attended = table.Column<DateTime>(type: "datetime(6)", maxLength: 255, nullable: true),
                  time_of_request = table.Column<DateTime>(type: "datetime(6)", maxLength: 255, nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_price_change_requests", x => x.request_id);
                  table.ForeignKey(
                      name: "FK_tbl_price_change_requests_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
                  table.ForeignKey(
                      name: "FK_tbl_price_change_requests_tbl_stock_items_item_code",
                      column: x => x.item_code,
                      principalTable: "tbl_stock_items",
                      principalColumn: "item_code");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_purchases_budget_details",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  purchases_budget_detail_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  batch_key = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  buying_price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  item_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  month = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                  month_01_quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_02__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_03__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_04__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_05__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_06__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_07__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_08__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_09__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_10__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_11__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_12__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  unit_cost_price = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_purchases_budget_details", x => x.purchases_budget_detail_id);
                  table.ForeignKey(
                      name: "FK_tbl_purchases_budget_details_tbl_stock_items_item_code",
                      column: x => x.item_code,
                      principalTable: "tbl_stock_items",
                      principalColumn: "item_code");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_sales_budget_details",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  sales_budget_detail_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  batch_key = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  item_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  month = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                  month_01_quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_02__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_03__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_04__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_05__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_06__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_07__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_08__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_09__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_10__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_11__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  month_12__quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  selling_price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  unit_cost_price = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_sales_budget_details", x => x.sales_budget_detail_id);
                  table.ForeignKey(
                      name: "FK_tbl_sales_budget_details_tbl_sales_budget_batch_headers_batc~",
                      column: x => x.batch_key,
                      principalTable: "tbl_sales_budget_batch_headers",
                      principalColumn: "batch_key");
                  table.ForeignKey(
                      name: "FK_tbl_sales_budget_details_tbl_stock_items_item_code",
                      column: x => x.item_code,
                      principalTable: "tbl_stock_items",
                      principalColumn: "item_code");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_stock_count_sheets",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  count_sheet_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  actual = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  average_age_months = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  batch_key = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  count_sheet_document_id = table.Column<long>(type: "bigint", nullable: false),
                  document_number = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  item_code = table.Column<string>(type: "varchar(20)", nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  quantity_on_hand = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  quantity_sold_last_12_months = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  selling_price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  stocks_over = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  stocks_short = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  unitcostprice = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_stock_count_sheets", x => x.count_sheet_id);
                  table.ForeignKey(
                      name: "FK_tbl_stock_count_sheets_tbl_stock_items_item_code",
                      column: x => x.item_code,
                      principalTable: "tbl_stock_items",
                      principalColumn: "item_code",
                      onDelete: ReferentialAction.Cascade);
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_stock_item_codes_requests",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  item_code_request_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  attanded_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  cost_price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  distributor = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  item_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  requesting_user = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  selling_price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  status = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  supplier = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  time_attended = table.Column<DateTime>(type: "datetime(6)", maxLength: 255, nullable: true),
                  time_of_request = table.Column<DateTime>(type: "datetime(6)", maxLength: 255, nullable: true),
                  unit_of_measure = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_stock_item_codes_requests", x => x.item_code_request_id);
                  table.ForeignKey(
                      name: "FK_tbl_stock_item_codes_requests_tbl_cost_centres_cost_centre_c~",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
                  table.ForeignKey(
                      name: "FK_tbl_stock_item_codes_requests_tbl_stock_items_item_code",
                      column: x => x.item_code,
                      principalTable: "tbl_stock_items",
                      principalColumn: "item_code");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_dues_payment_details",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  payment_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  amount = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  date = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  document_no = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  paid_to = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  is_final_payment = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  employee_id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  opening_balance = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  payment_type = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  processed_by = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  EmployeeDetailId = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_dues_payment_details", x => x.payment_id);
                  table.ForeignKey(
                      name: "FK_tbl_dues_payment_details_tbl_employee_details_EmployeeDetail~",
                      column: x => x.EmployeeDetailId,
                      principalTable: "tbl_employee_details",
                      principalColumn: "employee_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_user_logins",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  login_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  device_id = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  from_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  upto_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  user_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_user_logins", x => x.login_id);
                  table.ForeignKey(
                      name: "FK_tbl_user_logins_tbl_system_users_user_id",
                      column: x => x.user_id,
                      principalTable: "tbl_system_users",
                      principalColumn: "user_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_user_rights",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  user_right_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  object_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  command_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  right_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  role_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  user_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  user_activities = table.Column<short>(type: "smallint", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_user_rights", x => x.user_right_id);
                  table.ForeignKey(
                      name: "FK_tbl_user_rights_tbl_command_details_command_id",
                      column: x => x.command_id,
                      principalTable: "tbl_command_details",
                      principalColumn: "command_id");
                  table.ForeignKey(
                      name: "FK_tbl_user_rights_tbl_system_rights_right_id",
                      column: x => x.right_id,
                      principalTable: "tbl_system_rights",
                      principalColumn: "right_id");
                  table.ForeignKey(
                      name: "FK_tbl_user_rights_tbl_system_users_user_id",
                      column: x => x.user_id,
                      principalTable: "tbl_system_users",
                      principalColumn: "user_id");
                  table.ForeignKey(
                      name: "FK_tbl_user_rights_tbl_user_roles_role_id",
                      column: x => x.role_id,
                      principalTable: "tbl_user_roles",
                      principalColumn: "role_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_verification_rights",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  verification_right_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  device_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  user_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  user_role_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  verification_type_id = table.Column<long>(type: "bigint", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_verification_rights", x => x.verification_right_id);
                  table.ForeignKey(
                      name: "FK_tbl_verification_rights_tbl_data_devices_device_id",
                      column: x => x.device_id,
                      principalTable: "tbl_data_devices",
                      principalColumn: "device_id");
                  table.ForeignKey(
                      name: "FK_tbl_verification_rights_tbl_system_users_user_id",
                      column: x => x.user_id,
                      principalTable: "tbl_system_users",
                      principalColumn: "user_id");
                  table.ForeignKey(
                      name: "FK_tbl_verification_rights_tbl_user_roles_user_role_id",
                      column: x => x.user_role_id,
                      principalTable: "tbl_user_roles",
                      principalColumn: "role_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_user_audit_trails",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  audit_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  activity_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  activity_enum_number = table.Column<short>(type: "smallint", nullable: false),
                  category = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  command_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  data = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  login_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  old_values = table.Column<string>(type: "longtext", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_user_audit_trails", x => x.audit_id);
                  table.ForeignKey(
                      name: "FK_tbl_user_audit_trails_tbl_command_details_command_id",
                      column: x => x.command_id,
                      principalTable: "tbl_command_details",
                      principalColumn: "command_id");
                  table.ForeignKey(
                      name: "FK_tbl_user_audit_trails_tbl_user_logins_login_id",
                      column: x => x.login_id,
                      principalTable: "tbl_user_logins",
                      principalColumn: "login_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_verifications",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  verification_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  date_of_verification = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  login_id = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  record_id = table.Column<long>(type: "bigint", nullable: false),
                  table_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  verification_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  verification_record_id = table.Column<long>(type: "bigint", nullable: false),
                  verification_type_id = table.Column<long>(type: "bigint", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_verifications", x => x.verification_id);
                  table.ForeignKey(
                      name: "FK_tbl_verifications_tbl_user_logins_login_id",
                      column: x => x.login_id,
                      principalTable: "tbl_user_logins",
                      principalColumn: "login_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_leased_properties_accounts",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  leased_property_account_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  commencement_rent = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  current_rent = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  landlord_address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  last_review_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  leased_on = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  ledger_account_code = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_leased_properties_accounts", x => x.leased_property_account_id);
                  table.ForeignKey(
                      name: "FK_tbl_leased_properties_accounts_tbl_cost_centres_cost_centre_~",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_ledger_accounts",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  ledger_account_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  group_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  increase_with_debit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                  ledger_account_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  main_group = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  ActualBudgetVarianceId = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  ExpensesBudgetDetailId = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_ledger_accounts", x => x.ledger_account_id);
                  table.ForeignKey(
                      name: "FK_tbl_ledger_accounts_tbl_actual_budget_variances_ActualBudget~",
                      column: x => x.ActualBudgetVarianceId,
                      principalTable: "tbl_actual_budget_variances",
                      principalColumn: "actual_budget_id");
                  table.ForeignKey(
                      name: "FK_tbl_ledger_accounts_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
                  table.ForeignKey(
                      name: "FK_tbl_ledger_accounts_tbl_expenses_budget_details_ExpensesBudg~",
                      column: x => x.ExpensesBudgetDetailId,
                      principalTable: "tbl_expenses_budget_details",
                      principalColumn: "expense_budget_detail_id");
                  table.ForeignKey(
                      name: "FK_tbl_ledger_accounts_tbl_leased_properties_accounts_ledger_ac~",
                      column: x => x.ledger_account_code,
                      principalTable: "tbl_leased_properties_accounts",
                      principalColumn: "leased_property_account_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_let_properties_accounts",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  let_property_account_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  commencement_rent = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  current_rent = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                  last_review_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  ledger_account_code = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  let_on = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  tenant_address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_let_properties_accounts", x => x.let_property_account_id);
                  table.ForeignKey(
                      name: "FK_tbl_let_properties_accounts_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
                  table.ForeignKey(
                      name: "FK_tbl_let_properties_accounts_tbl_ledger_accounts_ledger_accou~",
                      column: x => x.ledger_account_code,
                      principalTable: "tbl_ledger_accounts",
                      principalColumn: "ledger_account_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateTable(
              name: "tbl_suppliers",
              columns: table => new
              {
                  date_added = table.Column<long>(type: "bigint", nullable: true),
                  date_updated = table.Column<long>(type: "bigint", nullable: true),
                  originator_id = table.Column<long>(type: "bigint", nullable: true),
                  is_currently_enabled = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                  supplier_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  contact_person = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  cost_centre_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  narration = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  postal_code = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  supplier_code = table.Column<string>(type: "varchar(20)", nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  telephone = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4"),
                  town = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                      .Annotation("MySql:CharSet", "utf8mb4")
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_tbl_suppliers", x => x.supplier_id);
                  table.ForeignKey(
                      name: "FK_tbl_suppliers_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
                  table.ForeignKey(
                      name: "FK_tbl_suppliers_tbl_ledger_accounts_supplier_code",
                      column: x => x.supplier_code,
                      principalTable: "tbl_ledger_accounts",
                      principalColumn: "ledger_account_id");
              })
              .Annotation("MySql:CharSet", "utf8mb4");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_actual_budget_variances_batch_key",
              table: "tbl_actual_budget_variances",
              column: "batch_key");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_actual_budget_variances_ledger_cost_centre_code",
              table: "tbl_actual_budget_variances",
              column: "ledger_cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_actual_budget_variances_batch_headers_cost_centre_code",
              table: "tbl_actual_budget_variances_batch_headers",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_computer_anydesks_cost_centre_code",
              table: "tbl_computer_anydesks",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_count_sheet_batches_cost_centre_code",
              table: "tbl_count_sheet_batches",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_data_devices_station",
              table: "tbl_data_devices",
              column: "station");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_dues_payment_details_EmployeeDetailId",
              table: "tbl_dues_payment_details",
              column: "EmployeeDetailId");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_employee_details_cost_centre_code",
              table: "tbl_employee_details",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_employee_details_group_number",
              table: "tbl_employee_details",
              column: "group_number");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_employee_details_payroll_group_id",
              table: "tbl_employee_details",
              column: "payroll_group_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_expense_budget_batch_headers_cost_centre_code",
              table: "tbl_expense_budget_batch_headers",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_expenses_budget_details_batch_key",
              table: "tbl_expenses_budget_details",
              column: "batch_key");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_issues_attachments_issue_id",
              table: "tbl_issues_attachments",
              column: "issue_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_issues_progresses_issue_id",
              table: "tbl_issues_progresses",
              column: "issue_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_issues_submissions_issue_id",
              table: "tbl_issues_submissions",
              column: "issue_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_item_groups_parent_group_id",
              table: "tbl_item_groups",
              column: "parent_group_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_leased_properties_accounts_cost_centre_code",
              table: "tbl_leased_properties_accounts",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_leased_properties_accounts_ledger_account_code",
              table: "tbl_leased_properties_accounts",
              column: "ledger_account_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_ledger_accounts_ActualBudgetVarianceId",
              table: "tbl_ledger_accounts",
              column: "ActualBudgetVarianceId");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_ledger_accounts_cost_centre_code",
              table: "tbl_ledger_accounts",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_ledger_accounts_ExpensesBudgetDetailId",
              table: "tbl_ledger_accounts",
              column: "ExpensesBudgetDetailId");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_ledger_accounts_ledger_account_code",
              table: "tbl_ledger_accounts",
              column: "ledger_account_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_let_properties_accounts_cost_centre_code",
              table: "tbl_let_properties_accounts",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_let_properties_accounts_ledger_account_code",
              table: "tbl_let_properties_accounts",
              column: "ledger_account_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_price_change_requests_cost_centre_code",
              table: "tbl_price_change_requests",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_price_change_requests_item_code",
              table: "tbl_price_change_requests",
              column: "item_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_purchases_budget_batch_headers_cost_centre_code",
              table: "tbl_purchases_budget_batch_headers",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_purchases_budget_details_item_code",
              table: "tbl_purchases_budget_details",
              column: "item_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_qr_codes_requests_cost_centre_code",
              table: "tbl_qr_codes_requests",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_qr_request_items_cost_centre_code",
              table: "tbl_qr_request_items",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_qr_request_items_request_id",
              table: "tbl_qr_request_items",
              column: "request_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_sales_budget_batch_headers_cost_centre_code",
              table: "tbl_sales_budget_batch_headers",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_sales_budget_details_batch_key",
              table: "tbl_sales_budget_details",
              column: "batch_key");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_sales_budget_details_item_code",
              table: "tbl_sales_budget_details",
              column: "item_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_stock_count_sheets_item_code",
              table: "tbl_stock_count_sheets",
              column: "item_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_stock_item_codes_requests_cost_centre_code",
              table: "tbl_stock_item_codes_requests",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_stock_item_codes_requests_item_code",
              table: "tbl_stock_item_codes_requests",
              column: "item_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_stock_items_group_id",
              table: "tbl_stock_items",
              column: "group_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_suppliers_cost_centre_code",
              table: "tbl_suppliers",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_suppliers_supplier_code",
              table: "tbl_suppliers",
              column: "supplier_code",
              unique: true);

          migrationBuilder.CreateIndex(
              name: "IX_tbl_system_users_role_id",
              table: "tbl_system_users",
              column: "role_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_user_audit_trails_command_id",
              table: "tbl_user_audit_trails",
              column: "command_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_user_audit_trails_login_id",
              table: "tbl_user_audit_trails",
              column: "login_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_user_logins_user_id",
              table: "tbl_user_logins",
              column: "user_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_user_rights_command_id",
              table: "tbl_user_rights",
              column: "command_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_user_rights_right_id",
              table: "tbl_user_rights",
              column: "right_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_user_rights_role_id",
              table: "tbl_user_rights",
              column: "role_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_user_rights_user_id",
              table: "tbl_user_rights",
              column: "user_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_vendor_codes_requests_cost_centre_code",
              table: "tbl_vendor_codes_requests",
              column: "cost_centre_code");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_verification_rights_device_id",
              table: "tbl_verification_rights",
              column: "device_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_verification_rights_user_id",
              table: "tbl_verification_rights",
              column: "user_id");

          migrationBuilder.CreateIndex(
              name: "IX_tbl_verification_rights_user_role_id",
              table: "tbl_verification_rights",
              column: "user_role_id",
              unique: true);

          migrationBuilder.CreateIndex(
              name: "IX_tbl_verifications_login_id",
              table: "tbl_verifications",
              column: "login_id");

          migrationBuilder.AddForeignKey(
              name: "FK_tbl_leased_properties_accounts_tbl_ledger_accounts_ledger_ac~",
              table: "tbl_leased_properties_accounts",
              column: "ledger_account_code",
              principalTable: "tbl_ledger_accounts",
              principalColumn: "ledger_account_id");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropForeignKey(
              name: "FK_tbl_actual_budget_variances_tbl_actual_budget_variances_batc~",
              table: "tbl_actual_budget_variances");

          migrationBuilder.DropForeignKey(
              name: "FK_tbl_actual_budget_variances_tbl_cost_centres_ledger_cost_cen~",
              table: "tbl_actual_budget_variances");

          migrationBuilder.DropForeignKey(
              name: "FK_tbl_expense_budget_batch_headers_tbl_cost_centres_cost_centr~",
              table: "tbl_expense_budget_batch_headers");

          migrationBuilder.DropForeignKey(
              name: "FK_tbl_leased_properties_accounts_tbl_cost_centres_cost_centre_~",
              table: "tbl_leased_properties_accounts");

          migrationBuilder.DropForeignKey(
              name: "FK_tbl_ledger_accounts_tbl_cost_centres_cost_centre_code",
              table: "tbl_ledger_accounts");

          migrationBuilder.DropForeignKey(
              name: "FK_tbl_expenses_budget_details_tbl_expense_budget_batch_headers~",
              table: "tbl_expenses_budget_details");

          migrationBuilder.DropForeignKey(
              name: "FK_tbl_leased_properties_accounts_tbl_ledger_accounts_ledger_ac~",
              table: "tbl_leased_properties_accounts");

          migrationBuilder.DropTable(
              name: "tbl_communication_messages");

          migrationBuilder.DropTable(
              name: "tbl_computer_anydesks");

          migrationBuilder.DropTable(
              name: "tbl_count_sheet_batches");

          migrationBuilder.DropTable(
              name: "tbl_default_access_rights");

          migrationBuilder.DropTable(
              name: "tbl_device_guids");

          migrationBuilder.DropTable(
              name: "tbl_dues_payment_details");

          migrationBuilder.DropTable(
              name: "tbl_issues_attachments");

          migrationBuilder.DropTable(
              name: "tbl_issues_progresses");

          migrationBuilder.DropTable(
              name: "tbl_issues_submissions");

          migrationBuilder.DropTable(
              name: "tbl_let_properties_accounts");

          migrationBuilder.DropTable(
              name: "tbl_password_safes");

          migrationBuilder.DropTable(
              name: "tbl_price_change_requests");

          migrationBuilder.DropTable(
              name: "tbl_purchases_budget_batch_headers");

          migrationBuilder.DropTable(
              name: "tbl_purchases_budget_details");

          migrationBuilder.DropTable(
              name: "tbl_qr_request_items");

          migrationBuilder.DropTable(
              name: "tbl_sales_budget_details");

          migrationBuilder.DropTable(
              name: "tbl_stock_count_sheets");

          migrationBuilder.DropTable(
              name: "tbl_stock_item_codes_requests");

          migrationBuilder.DropTable(
              name: "tbl_suppliers");

          migrationBuilder.DropTable(
              name: "tbl_tims_machines");

          migrationBuilder.DropTable(
              name: "tbl_user_audit_trails");

          migrationBuilder.DropTable(
              name: "tbl_user_rights");

          migrationBuilder.DropTable(
              name: "tbl_vendor_codes_requests");

          migrationBuilder.DropTable(
              name: "tbl_verification_rights");

          migrationBuilder.DropTable(
              name: "tbl_verification_types");

          migrationBuilder.DropTable(
              name: "tbl_verifications");

          migrationBuilder.DropTable(
              name: "tbl_employee_details");

          migrationBuilder.DropTable(
              name: "tbl_project_issues");

          migrationBuilder.DropTable(
              name: "tbl_qr_codes_requests");

          migrationBuilder.DropTable(
              name: "tbl_sales_budget_batch_headers");

          migrationBuilder.DropTable(
              name: "tbl_stock_items");

          migrationBuilder.DropTable(
              name: "tbl_command_details");

          migrationBuilder.DropTable(
              name: "tbl_system_rights");

          migrationBuilder.DropTable(
              name: "tbl_data_devices");

          migrationBuilder.DropTable(
              name: "tbl_user_logins");

          migrationBuilder.DropTable(
              name: "tbl_payroll_groups");

          migrationBuilder.DropTable(
              name: "tbl_staff_groups");

          migrationBuilder.DropTable(
              name: "tbl_item_groups");

          migrationBuilder.DropTable(
              name: "tbl_system_users");

          migrationBuilder.DropTable(
              name: "tbl_user_roles");

          migrationBuilder.DropTable(
              name: "tbl_actual_budget_variances_batch_headers");

          migrationBuilder.DropTable(
              name: "tbl_cost_centres");

          migrationBuilder.DropTable(
              name: "tbl_expense_budget_batch_headers");

          migrationBuilder.DropTable(
              name: "tbl_ledger_accounts");

          migrationBuilder.DropTable(
              name: "tbl_actual_budget_variances");

          migrationBuilder.DropTable(
              name: "tbl_expenses_budget_details");

          migrationBuilder.DropTable(
              name: "tbl_leased_properties_accounts");
      }
  }
