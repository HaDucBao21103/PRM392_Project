using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vaccine.Repo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    admin_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    dob = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    blood_type = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Admin__43AA41412325BEAC", x => x.admin_id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    dob = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    phone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    blood_type = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    user_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__CD65CB85E799178F", x => x.customer_id);
                });

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    doctor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    age = table.Column<int>(type: "int", nullable: true),
                    gender = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    phone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    degree = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    experience_years = table.Column<int>(type: "int", nullable: true),
                    dob = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Doctor__F399356456957BF5", x => x.doctor_id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    staff_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    dob = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    phone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    user_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    degree = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    experience_years = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Staff__1963DD9CF478652A", x => x.staff_id);
                });

            migrationBuilder.CreateTable(
                name: "Vaccine",
                columns: table => new
                {
                    vaccine_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    max_late_date = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    internal_duration_doses = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Vaccine__B593EFB3591D646C", x => x.vaccine_id);
                });

            migrationBuilder.CreateTable(
                name: "VaccineBatch",
                columns: table => new
                {
                    batch_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    manufacturer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    manufacture_date = table.Column<DateOnly>(type: "date", nullable: false),
                    expiry_date = table.Column<DateOnly>(type: "date", nullable: false),
                    country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VaccineB__56E378360CEC7A57", x => x.batch_number);
                });

            migrationBuilder.CreateTable(
                name: "VaccineCombo",
                columns: table => new
                {
                    combo_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VaccineC__18F74AA37256A032", x => x.combo_id);
                });

            migrationBuilder.CreateTable(
                name: "Holiday",
                columns: table => new
                {
                    holiday_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    admin_id = table.Column<int>(type: "int", nullable: true),
                    date_from = table.Column<DateOnly>(type: "date", nullable: false),
                    date_to = table.Column<DateOnly>(type: "date", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Holiday__253884EA6DBA9AD5", x => x.holiday_id);
                    table.ForeignKey(
                        name: "FK__Holiday__admin_i__2EDAF651",
                        column: x => x.admin_id,
                        principalTable: "Admin",
                        principalColumn: "admin_id");
                });

            migrationBuilder.CreateTable(
                name: "Child",
                columns: table => new
                {
                    child_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    dob = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    blood_type = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Child__015ADC05C293D48F", x => x.child_id);
                    table.ForeignKey(
                        name: "FK__Child__customer___6383C8BA",
                        column: x => x.customer_id,
                        principalTable: "Customer",
                        principalColumn: "customer_id");
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    invoice_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Invoice__F58DFD493CA53699", x => x.invoice_id);
                    table.ForeignKey(
                        name: "FK__Invoice__custome__17F790F9",
                        column: x => x.customer_id,
                        principalTable: "Customer",
                        principalColumn: "customer_id");
                });

            migrationBuilder.CreateTable(
                name: "VaccineBatchDetail",
                columns: table => new
                {
                    batch_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    vaccine_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    PreOrderQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VaccineB__7DBA46CD69A10EBA", x => new { x.batch_number, x.vaccine_id });
                    table.ForeignKey(
                        name: "FK__VaccineBa__batch__00200768",
                        column: x => x.batch_number,
                        principalTable: "VaccineBatch",
                        principalColumn: "batch_number");
                    table.ForeignKey(
                        name: "FK__VaccineBa__vacci__01142BA1",
                        column: x => x.vaccine_id,
                        principalTable: "Vaccine",
                        principalColumn: "vaccine_id");
                });

            migrationBuilder.CreateTable(
                name: "VaccineComboDetail",
                columns: table => new
                {
                    combo_id = table.Column<int>(type: "int", nullable: false),
                    vaccine_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineComboDetail", x => new { x.combo_id, x.vaccine_id });
                    table.ForeignKey(
                        name: "FK__VaccineCo__combo__05D8E0BE",
                        column: x => x.combo_id,
                        principalTable: "VaccineCombo",
                        principalColumn: "combo_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__VaccineCo__vacci__06CD04F7",
                        column: x => x.vaccine_id,
                        principalTable: "Vaccine",
                        principalColumn: "vaccine_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VaccineVaccineCombo",
                columns: table => new
                {
                    CombosComboId = table.Column<int>(type: "int", nullable: false),
                    VaccinesVaccineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineVaccineCombo", x => new { x.CombosComboId, x.VaccinesVaccineId });
                    table.ForeignKey(
                        name: "FK_VaccineVaccineCombo_VaccineCombo_CombosComboId",
                        column: x => x.CombosComboId,
                        principalTable: "VaccineCombo",
                        principalColumn: "combo_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaccineVaccineCombo_Vaccine_VaccinesVaccineId",
                        column: x => x.VaccinesVaccineId,
                        principalTable: "Vaccine",
                        principalColumn: "vaccine_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    appointment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    child_id = table.Column<int>(type: "int", nullable: false),
                    staff_id = table.Column<int>(type: "int", nullable: true),
                    doctor_id = table.Column<int>(type: "int", nullable: true),
                    vaccine_type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    vaccine_id = table.Column<int>(type: "int", nullable: true),
                    combo_id = table.Column<int>(type: "int", nullable: true),
                    appointment_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    batchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Appointm__A50828FCF69887E8", x => x.appointment_id);
                    table.ForeignKey(
                        name: "FK_Appointment_Doctor",
                        column: x => x.doctor_id,
                        principalTable: "Doctor",
                        principalColumn: "doctor_id");
                    table.ForeignKey(
                        name: "FK_Appointment_Staff",
                        column: x => x.staff_id,
                        principalTable: "Staff",
                        principalColumn: "staff_id");
                    table.ForeignKey(
                        name: "FK_Appointment_VaccineBatch",
                        column: x => x.batchNumber,
                        principalTable: "VaccineBatch",
                        principalColumn: "batch_number");
                    table.ForeignKey(
                        name: "FK__Appointme__child__0D7A0286",
                        column: x => x.child_id,
                        principalTable: "Child",
                        principalColumn: "child_id");
                    table.ForeignKey(
                        name: "FK__Appointme__combo__114A936A",
                        column: x => x.combo_id,
                        principalTable: "VaccineCombo",
                        principalColumn: "combo_id");
                    table.ForeignKey(
                        name: "FK__Appointme__custo__0C85DE4D",
                        column: x => x.customer_id,
                        principalTable: "Customer",
                        principalColumn: "customer_id");
                    table.ForeignKey(
                        name: "FK__Appointme__vacci__10566F31",
                        column: x => x.vaccine_id,
                        principalTable: "Vaccine",
                        principalColumn: "vaccine_id");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    review_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    doctor_id = table.Column<int>(type: "int", nullable: true),
                    staff_id = table.Column<int>(type: "int", nullable: true),
                    vaccine_id = table.Column<int>(type: "int", nullable: true),
                    appointment_id = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__60883D90766CC40A", x => x.review_id);
                    table.ForeignKey(
                        name: "FK_Feedback_Staff_staff_id",
                        column: x => x.staff_id,
                        principalTable: "Staff",
                        principalColumn: "staff_id");
                    table.ForeignKey(
                        name: "FK_Feedback_Vaccine_vaccine_id",
                        column: x => x.vaccine_id,
                        principalTable: "Vaccine",
                        principalColumn: "vaccine_id");
                    table.ForeignKey(
                        name: "FK__Feedback__appoin__2BFE89A6",
                        column: x => x.appointment_id,
                        principalTable: "Appointment",
                        principalColumn: "appointment_id");
                    table.ForeignKey(
                        name: "FK__Feedback__custom__2A164134",
                        column: x => x.customer_id,
                        principalTable: "Customer",
                        principalColumn: "customer_id");
                    table.ForeignKey(
                        name: "FK__Feedback__doctor__2B0A656D",
                        column: x => x.doctor_id,
                        principalTable: "Doctor",
                        principalColumn: "doctor_id");
                });

            migrationBuilder.CreateTable(
                name: "HealthRecord",
                columns: table => new
                {
                    record_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    staff_id = table.Column<int>(type: "int", nullable: true),
                    appointment_id = table.Column<int>(type: "int", nullable: false),
                    doctor_id = table.Column<int>(type: "int", nullable: true),
                    blood_pressure = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    heart_rate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    height = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    weight = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    temperature = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    ate_before_vaccine = table.Column<bool>(type: "bit", nullable: true),
                    condition_ok = table.Column<bool>(type: "bit", nullable: true),
                    reaction_notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HealthRe__BFCFB4DD4AA89881", x => x.record_id);
                    table.ForeignKey(
                        name: "FK__HealthRec__appoi__236943A5",
                        column: x => x.appointment_id,
                        principalTable: "Appointment",
                        principalColumn: "appointment_id");
                    table.ForeignKey(
                        name: "FK__HealthRec__docto__245D67DE",
                        column: x => x.doctor_id,
                        principalTable: "Doctor",
                        principalColumn: "doctor_id");
                    table.ForeignKey(
                        name: "FK__HealthRec__staff__22751F6C",
                        column: x => x.staff_id,
                        principalTable: "Staff",
                        principalColumn: "staff_id");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetail",
                columns: table => new
                {
                    detail_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    invoice_id = table.Column<int>(type: "int", nullable: false),
                    vaccine_id = table.Column<int>(type: "int", nullable: true),
                    appointment_id = table.Column<int>(type: "int", nullable: true),
                    combo_id = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__InvoiceD__38E9A224183CAA31", x => x.detail_id);
                    table.ForeignKey(
                        name: "FK__InvoiceDe__appoi__1DB06A4F",
                        column: x => x.appointment_id,
                        principalTable: "Appointment",
                        principalColumn: "appointment_id");
                    table.ForeignKey(
                        name: "FK__InvoiceDe__combo__1EA48E88",
                        column: x => x.combo_id,
                        principalTable: "VaccineCombo",
                        principalColumn: "combo_id");
                    table.ForeignKey(
                        name: "FK__InvoiceDe__invoi__1BC821DD",
                        column: x => x.invoice_id,
                        principalTable: "Invoice",
                        principalColumn: "invoice_id");
                    table.ForeignKey(
                        name: "FK__InvoiceDe__vacci__1CBC4616",
                        column: x => x.vaccine_id,
                        principalTable: "Vaccine",
                        principalColumn: "vaccine_id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Admin__7C9273C419142EEB",
                table: "Admin",
                column: "user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Admin__AB6E6164E5C92D16",
                table: "Admin",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_batchNumber",
                table: "Appointment",
                column: "batchNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_child_id",
                table: "Appointment",
                column: "child_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_combo_id",
                table: "Appointment",
                column: "combo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_customer_id",
                table: "Appointment",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_doctor_id",
                table: "Appointment",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_staff_id",
                table: "Appointment",
                column: "staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_vaccine_id",
                table: "Appointment",
                column: "vaccine_id");

            migrationBuilder.CreateIndex(
                name: "IX_Child_customer_id",
                table: "Child",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__7C9273C488BB26AE",
                table: "Customer",
                column: "user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__AB6E61641F332227",
                table: "Customer",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__B43B145F7904B365",
                table: "Customer",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Doctor__B43B145F3F38322A",
                table: "Doctor",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_appointment_id",
                table: "Feedback",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_customer_id",
                table: "Feedback",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_doctor_id",
                table: "Feedback",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_staff_id",
                table: "Feedback",
                column: "staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_vaccine_id",
                table: "Feedback",
                column: "vaccine_id");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecord_appointment_id",
                table: "HealthRecord",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecord_doctor_id",
                table: "HealthRecord",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecord_staff_id",
                table: "HealthRecord",
                column: "staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_admin_id",
                table: "Holiday",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_customer_id",
                table: "Invoice",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetail_appointment_id",
                table: "InvoiceDetail",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetail_combo_id",
                table: "InvoiceDetail",
                column: "combo_id");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetail_invoice_id",
                table: "InvoiceDetail",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetail_vaccine_id",
                table: "InvoiceDetail",
                column: "vaccine_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Staff__7C9273C4DBD54384",
                table: "Staff",
                column: "user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Staff__AB6E616411F382A9",
                table: "Staff",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Staff__B43B145F9977ADF9",
                table: "Staff",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VaccineBatchDetail_vaccine_id",
                table: "VaccineBatchDetail",
                column: "vaccine_id");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineComboDetail_vaccine_id",
                table: "VaccineComboDetail",
                column: "vaccine_id");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineVaccineCombo_VaccinesVaccineId",
                table: "VaccineVaccineCombo",
                column: "VaccinesVaccineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "HealthRecord");

            migrationBuilder.DropTable(
                name: "Holiday");

            migrationBuilder.DropTable(
                name: "InvoiceDetail");

            migrationBuilder.DropTable(
                name: "VaccineBatchDetail");

            migrationBuilder.DropTable(
                name: "VaccineComboDetail");

            migrationBuilder.DropTable(
                name: "VaccineVaccineCombo");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "VaccineBatch");

            migrationBuilder.DropTable(
                name: "Child");

            migrationBuilder.DropTable(
                name: "VaccineCombo");

            migrationBuilder.DropTable(
                name: "Vaccine");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
