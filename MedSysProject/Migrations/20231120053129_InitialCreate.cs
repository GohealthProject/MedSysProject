using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedSysProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogCategory",
                columns: table => new
                {
                    BlogClassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogCategory = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCat", x => x.BlogClassID);
                });

            migrationBuilder.CreateTable(
                name: "BlogManagement",
                columns: table => new
                {
                    BlogPermissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogManagement", x => x.BlogPermissionID);
                });

            migrationBuilder.CreateTable(
                name: "Corporation",
                columns: table => new
                {
                    taxID = table.Column<int>(type: "int", nullable: false),
                    corporation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    discount = table.Column<double>(type: "float", nullable: true),
                    contactnumber = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    middleman = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corporation", x => x.taxID);
                });

            migrationBuilder.CreateTable(
                name: "OrderPay",
                columns: table => new
                {
                    PayID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPay", x => x.PayID);
                });

            migrationBuilder.CreateTable(
                name: "OrderShip",
                columns: table => new
                {
                    shipID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shipName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderShip", x => x.shipID);
                });

            migrationBuilder.CreateTable(
                name: "OrderState",
                columns: table => new
                {
                    stateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    stateDetailed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderState", x => x.stateID);
                });

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    planId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    planName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    planDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.planId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: true),
                    License = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ingredient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photo = table.Column<byte[]>(type: "image", nullable: true),
                    UnitsInStock = table.Column<int>(type: "int", nullable: true),
                    Discontinued = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "ProductsCategories",
                columns: table => new
                {
                    CategoriesID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriesName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCategories", x => x.CategoriesID);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    projectID = table.Column<int>(type: "int", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    projectContent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    projectPrice = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subproject", x => x.projectID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeClass",
                columns: table => new
                {
                    EmployeeClassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Class = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BlogPermissionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeClass", x => x.EmployeeClassID);
                    table.ForeignKey(
                        name: "FK_EmployeeClass_BlogManagement",
                        column: x => x.BlogPermissionID,
                        principalTable: "BlogManagement",
                        principalColumn: "BlogPermissionID");
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    memberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberName = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    memberGender = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    memberBirthdate = table.Column<DateTime>(type: "date", nullable: true),
                    memberPhone = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    memberEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    memberAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    memberContactNumber = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    memberNickname = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    memberPassword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    taxID = table.Column<int>(type: "int", nullable: true),
                    memberAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.memberId);
                    table.ForeignKey(
                        name: "FK_Member_Corporation",
                        column: x => x.taxID,
                        principalTable: "Corporation",
                        principalColumn: "taxID");
                });

            migrationBuilder.CreateTable(
                name: "ProductsClassification",
                columns: table => new
                {
                    ProductsClassification = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    CategoriesID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsClassification", x => x.ProductsClassification);
                    table.ForeignKey(
                        name: "FK_ProductsClassification_Products",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                    table.ForeignKey(
                        name: "FK_ProductsClassification_ProductsCategories",
                        column: x => x.CategoriesID,
                        principalTable: "ProductsCategories",
                        principalColumn: "CategoriesID");
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    itemID = table.Column<int>(type: "int", nullable: false),
                    itemName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    itemComment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    itemPrice = table.Column<int>(type: "int", nullable: true),
                    projectID = table.Column<int>(type: "int", nullable: true),
                    itemUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    itemRangeMin = table.Column<int>(type: "int", nullable: true),
                    itemRangeMax = table.Column<int>(type: "int", nullable: true),
                    Mmin = table.Column<int>(type: "int", nullable: true),
                    Mmax = table.Column<int>(type: "int", nullable: true),
                    Fmin = table.Column<int>(type: "int", nullable: true),
                    Fmax = table.Column<int>(type: "int", nullable: true),
                    itemRange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item$", x => x.itemID);
                    table.ForeignKey(
                        name: "FK_Item$_subproject",
                        column: x => x.projectID,
                        principalTable: "Project",
                        principalColumn: "projectID");
                });

            migrationBuilder.CreateTable(
                name: "PlanRef",
                columns: table => new
                {
                    planbridgeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    planID = table.Column<int>(type: "int", nullable: false),
                    projectID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planbrigee", x => x.planbridgeId);
                    table.ForeignKey(
                        name: "FK_PlanRef_Plan",
                        column: x => x.planID,
                        principalTable: "Plan",
                        principalColumn: "planId");
                    table.ForeignKey(
                        name: "FK_Planbrigee_subproject",
                        column: x => x.projectID,
                        principalTable: "Project",
                        principalColumn: "projectID");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeClassID = table.Column<int>(type: "int", nullable: false),
                    EmployeeBirthDate = table.Column<DateTime>(type: "date", nullable: true),
                    EmployeePhoneNum = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmployeeEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeePassWord = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeePhoto = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employee_EmployeeClass",
                        column: x => x.EmployeeClassID,
                        principalTable: "EmployeeClass",
                        principalColumn: "EmployeeClassID");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    orderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderDate = table.Column<DateTime>(type: "date", nullable: false),
                    memberId = table.Column<int>(type: "int", nullable: true),
                    shipID = table.Column<int>(type: "int", nullable: true),
                    payID = table.Column<int>(type: "int", nullable: true),
                    stateID = table.Column<int>(type: "int", nullable: true),
                    shipDate = table.Column<DateTime>(type: "date", nullable: true),
                    deliveryDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_1", x => x.orderID);
                    table.ForeignKey(
                        name: "FK_Order_Member",
                        column: x => x.memberId,
                        principalTable: "Members",
                        principalColumn: "memberId");
                    table.ForeignKey(
                        name: "FK_Order_OrderPay",
                        column: x => x.payID,
                        principalTable: "OrderPay",
                        principalColumn: "PayID");
                    table.ForeignKey(
                        name: "FK_Order_OrderShip",
                        column: x => x.shipID,
                        principalTable: "OrderShip",
                        principalColumn: "shipID");
                    table.ForeignKey(
                        name: "FK_Order_OrderState",
                        column: x => x.stateID,
                        principalTable: "OrderState",
                        principalColumn: "stateID");
                });

            migrationBuilder.CreateTable(
                name: "Reserve",
                columns: table => new
                {
                    ReserveID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberID = table.Column<int>(type: "int", nullable: true),
                    planID = table.Column<int>(type: "int", nullable: true),
                    ReserveDate = table.Column<DateTime>(type: "date", nullable: true),
                    ReserveState = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserve", x => x.ReserveID);
                    table.ForeignKey(
                        name: "FK_Reserve_Members",
                        column: x => x.memberID,
                        principalTable: "Members",
                        principalColumn: "memberId");
                    table.ForeignKey(
                        name: "FK_Reserve_Plan",
                        column: x => x.planID,
                        principalTable: "Plan",
                        principalColumn: "planId");
                });

            migrationBuilder.CreateTable(
                name: "subProjectBridges",
                columns: table => new
                {
                    SubProjectBridgeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<int>(type: "int", nullable: true),
                    ItemID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subProjectBridges", x => x.SubProjectBridgeID);
                    table.ForeignKey(
                        name: "FK_subProjectBridges_Item$",
                        column: x => x.ItemID,
                        principalTable: "Item",
                        principalColumn: "itemID");
                    table.ForeignKey(
                        name: "FK_subProjectBridges_subproject",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "projectID");
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    BlogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ArticleClassID = table.Column<int>(type: "int", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlogImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.BlogID);
                    table.ForeignKey(
                        name: "FK_Blog_ArticleCat",
                        column: x => x.ArticleClassID,
                        principalTable: "BlogCategory",
                        principalColumn: "BlogClassID");
                    table.ForeignKey(
                        name: "FK_Blog_Employee",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderID = table.Column<int>(type: "int", nullable: true),
                    productID = table.Column<int>(type: "int", nullable: true),
                    unitPrice = table.Column<decimal>(type: "money", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailID);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Order",
                        column: x => x.orderID,
                        principalTable: "Order",
                        principalColumn: "orderID");
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products",
                        column: x => x.productID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "HealthReport",
                columns: table => new
                {
                    ReportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: true),
                    ReportDate = table.Column<DateTime>(type: "date", nullable: true),
                    PlanID = table.Column<int>(type: "int", nullable: true),
                    ReserveID = table.Column<int>(type: "int", nullable: true),
                    Paymentstatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthReport", x => x.ReportID);
                    table.ForeignKey(
                        name: "FK_HealthReport_Member",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "memberId");
                    table.ForeignKey(
                        name: "FK_HealthReport_Plan",
                        column: x => x.PlanID,
                        principalTable: "Plan",
                        principalColumn: "planId");
                    table.ForeignKey(
                        name: "FK_HealthReport_Reserve",
                        column: x => x.ReserveID,
                        principalTable: "Reserve",
                        principalColumn: "ReserveID");
                });

            migrationBuilder.CreateTable(
                name: "ReservedSub",
                columns: table => new
                {
                    subreservedID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reservedID = table.Column<int>(type: "int", nullable: false),
                    itemID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedSub", x => x.subreservedID);
                    table.ForeignKey(
                        name: "FK_ReservedSub_Item$",
                        column: x => x.itemID,
                        principalTable: "Item",
                        principalColumn: "itemID");
                    table.ForeignKey(
                        name: "FK_ReservedSub_Reserve",
                        column: x => x.reservedID,
                        principalTable: "Reserve",
                        principalColumn: "ReserveID");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogID = table.Column<int>(type: "int", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: true),
                    EmployeeID = table.Column<int>(type: "int", nullable: true),
                    ParentCommentID = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "smalldatetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comments__C3B4DFAA7AC8255B", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_Comments_Members1",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "memberId");
                    table.ForeignKey(
                        name: "FK__Comments__BlogID__4924D839",
                        column: x => x.BlogID,
                        principalTable: "Blogs",
                        principalColumn: "BlogID");
                    table.ForeignKey(
                        name: "FK__Comments__Employ__4B0D20AB",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK__Comments__Parent__4C0144E4",
                        column: x => x.ParentCommentID,
                        principalTable: "Comments",
                        principalColumn: "CommentID");
                });

            migrationBuilder.CreateTable(
                name: "ReportDetail",
                columns: table => new
                {
                    ReportDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportID = table.Column<int>(type: "int", nullable: true),
                    itemID = table.Column<int>(type: "int", nullable: true),
                    result = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reportDetail$", x => x.ReportDetailID);
                    table.ForeignKey(
                        name: "FK_ReportDetail_HealthReport",
                        column: x => x.ReportID,
                        principalTable: "HealthReport",
                        principalColumn: "ReportID");
                    table.ForeignKey(
                        name: "FK_ReportDetail_Item",
                        column: x => x.itemID,
                        principalTable: "Item",
                        principalColumn: "itemID");
                });

            migrationBuilder.CreateTable(
                name: "ReportTest",
                columns: table => new
                {
                    ReportDetailID = table.Column<int>(type: "int", nullable: false),
                    ReportID = table.Column<int>(type: "int", nullable: false),
                    itemID = table.Column<int>(type: "int", nullable: true),
                    result = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_ReportTest_HealthReport",
                        column: x => x.ReportID,
                        principalTable: "HealthReport",
                        principalColumn: "ReportID");
                    table.ForeignKey(
                        name: "FK_ReportTest_Item",
                        column: x => x.itemID,
                        principalTable: "Item",
                        principalColumn: "itemID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_ArticleClassID",
                table: "Blogs",
                column: "ArticleClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_EmployeeID",
                table: "Blogs",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogID",
                table: "Comments",
                column: "BlogID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_EmployeeID",
                table: "Comments",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MemberID",
                table: "Comments",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentID",
                table: "Comments",
                column: "ParentCommentID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClass_BlogPermissionID",
                table: "EmployeeClass",
                column: "BlogPermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeClassID",
                table: "Employees",
                column: "EmployeeClassID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthReport_MemberID",
                table: "HealthReport",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthReport_PlanID",
                table: "HealthReport",
                column: "PlanID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthReport_ReserveID",
                table: "HealthReport",
                column: "ReserveID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_projectID",
                table: "Item",
                column: "projectID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_taxID",
                table: "Members",
                column: "taxID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_memberId",
                table: "Order",
                column: "memberId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_payID",
                table: "Order",
                column: "payID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_shipID",
                table: "Order",
                column: "shipID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_stateID",
                table: "Order",
                column: "stateID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_orderID",
                table: "OrderDetails",
                column: "orderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_productID",
                table: "OrderDetails",
                column: "productID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanRef_planID",
                table: "PlanRef",
                column: "planID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanRef_projectID",
                table: "PlanRef",
                column: "projectID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsClassification_CategoriesID",
                table: "ProductsClassification",
                column: "CategoriesID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsClassification_ProductID",
                table: "ProductsClassification",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDetail_itemID",
                table: "ReportDetail",
                column: "itemID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDetail_ReportID",
                table: "ReportDetail",
                column: "ReportID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportTest_itemID",
                table: "ReportTest",
                column: "itemID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportTest_ReportID",
                table: "ReportTest",
                column: "ReportID");

            migrationBuilder.CreateIndex(
                name: "IX_Reserve_memberID",
                table: "Reserve",
                column: "memberID");

            migrationBuilder.CreateIndex(
                name: "IX_Reserve_planID",
                table: "Reserve",
                column: "planID");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedSub_itemID",
                table: "ReservedSub",
                column: "itemID");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedSub_reservedID",
                table: "ReservedSub",
                column: "reservedID");

            migrationBuilder.CreateIndex(
                name: "IX_subProjectBridges_ItemID",
                table: "subProjectBridges",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_subProjectBridges_ProjectID",
                table: "subProjectBridges",
                column: "ProjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "PlanRef");

            migrationBuilder.DropTable(
                name: "ProductsClassification");

            migrationBuilder.DropTable(
                name: "ReportDetail");

            migrationBuilder.DropTable(
                name: "ReportTest");

            migrationBuilder.DropTable(
                name: "ReservedSub");

            migrationBuilder.DropTable(
                name: "subProjectBridges");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductsCategories");

            migrationBuilder.DropTable(
                name: "HealthReport");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "BlogCategory");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "OrderPay");

            migrationBuilder.DropTable(
                name: "OrderShip");

            migrationBuilder.DropTable(
                name: "OrderState");

            migrationBuilder.DropTable(
                name: "Reserve");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "EmployeeClass");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.DropTable(
                name: "BlogManagement");

            migrationBuilder.DropTable(
                name: "Corporation");
        }
    }
}
