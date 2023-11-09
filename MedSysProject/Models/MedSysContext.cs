﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedSysProject.Models;

public partial class MedSysContext : DbContext
{
    public MedSysContext()
    {
    }

    public MedSysContext(DbContextOptions<MedSysContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogCategory> BlogCategories { get; set; }

    public virtual DbSet<BlogManagement> BlogManagements { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Corporation> Corporations { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeClass> EmployeeClasses { get; set; }

    public virtual DbSet<HealthReport> HealthReports { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderPay> OrderPays { get; set; }

    public virtual DbSet<OrderShip> OrderShips { get; set; }

    public virtual DbSet<OrderState> OrderStates { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<PlanRef> PlanRefs { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductsCategory> ProductsCategories { get; set; }

    public virtual DbSet<ProductsClassification> ProductsClassifications { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ReportDetail> ReportDetails { get; set; }

    public virtual DbSet<ReportTest> ReportTests { get; set; }

    public virtual DbSet<Reserve> Reserves { get; set; }

    public virtual DbSet<ReservedSub> ReservedSubs { get; set; }

    public virtual DbSet<SubProjectBridge> SubProjectBridges { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=anrouter9203.asuscomm.com,1433;Initial Catalog=MedSys;User ID=sa;Password=123a@;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK_Blog");

            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.ArticleClassId).HasColumnName("ArticleClassID");
            entity.Property(e => e.CreatedAt).HasColumnType("smalldatetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.ArticleClass).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.ArticleClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Blog_ArticleCat");

            entity.HasOne(d => d.Employee).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Blog_Employee");
        });

        modelBuilder.Entity<BlogCategory>(entity =>
        {
            entity.HasKey(e => e.BlogClassId).HasName("PK_ArticleCat");

            entity.ToTable("BlogCategory");

            entity.Property(e => e.BlogClassId).HasColumnName("BlogClassID");
            entity.Property(e => e.BlogCategory1)
                .HasMaxLength(40)
                .HasColumnName("BlogCategory");
        });

        modelBuilder.Entity<BlogManagement>(entity =>
        {
            entity.HasKey(e => e.BlogPermissionId);

            entity.ToTable("BlogManagement");

            entity.Property(e => e.BlogPermissionId).HasColumnName("BlogPermissionID");
            entity.Property(e => e.PermissionType).HasMaxLength(50);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFAA7AC8255B");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.CreatedAt).HasColumnType("smalldatetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.ParentCommentId).HasColumnName("ParentCommentID");

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__BlogID__4924D839");

            entity.HasOne(d => d.Employee).WithMany(p => p.Comments)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Comments__Employ__4B0D20AB");

            entity.HasOne(d => d.Member).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_Comments_Members1");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK__Comments__Parent__4C0144E4");
        });

        modelBuilder.Entity<Corporation>(entity =>
        {
            entity.HasKey(e => e.TaxId);

            entity.ToTable("Corporation");

            entity.Property(e => e.TaxId)
                .ValueGeneratedNever()
                .HasColumnName("taxID");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .HasColumnName("address");
            entity.Property(e => e.Contactnumber).HasColumnName("contactnumber");
            entity.Property(e => e.Corporation1)
                .HasMaxLength(50)
                .HasColumnName("corporation");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.Middleman)
                .HasMaxLength(50)
                .HasColumnName("middleman");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK_Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeBirthDate).HasColumnType("date");
            entity.Property(e => e.EmployeeClassId).HasColumnName("EmployeeClassID");
            entity.Property(e => e.EmployeeEmail).HasMaxLength(50);
            entity.Property(e => e.EmployeeName).HasMaxLength(50);
            entity.Property(e => e.EmployeePassWord).HasMaxLength(50);
            entity.Property(e => e.EmployeePhoneNum).HasMaxLength(20);
            entity.Property(e => e.EmployeePhoto).HasColumnType("image");

            entity.HasOne(d => d.EmployeeClass).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeeClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_EmployeeClass");
        });

        modelBuilder.Entity<EmployeeClass>(entity =>
        {
            entity.ToTable("EmployeeClass");

            entity.Property(e => e.EmployeeClassId).HasColumnName("EmployeeClassID");
            entity.Property(e => e.BlogPermissionId).HasColumnName("BlogPermissionID");
            entity.Property(e => e.Class).HasMaxLength(50);

            entity.HasOne(d => d.BlogPermission).WithMany(p => p.EmployeeClasses)
                .HasForeignKey(d => d.BlogPermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeClass_BlogManagement");
        });

        modelBuilder.Entity<HealthReport>(entity =>
        {
            entity.HasKey(e => e.ReportId);

            entity.ToTable("HealthReport");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.ReportDate).HasColumnType("date");
            entity.Property(e => e.ReserveId).HasColumnName("ReserveID");

            entity.HasOne(d => d.Member).WithMany(p => p.HealthReports)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_HealthReport_Member");

            entity.HasOne(d => d.Plan).WithMany(p => p.HealthReports)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_HealthReport_Plan");

            entity.HasOne(d => d.Reserve).WithMany(p => p.HealthReports)
                .HasForeignKey(d => d.ReserveId)
                .HasConstraintName("FK_HealthReport_Reserve");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK_Item$");

            entity.ToTable("Item");

            entity.Property(e => e.ItemId)
                .ValueGeneratedNever()
                .HasColumnName("itemID");
            entity.Property(e => e.ItemComment)
                .HasMaxLength(255)
                .HasColumnName("itemComment");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .HasColumnName("itemName");
            entity.Property(e => e.ItemPrice).HasColumnName("itemPrice");
            entity.Property(e => e.ItemRange)
                .HasMaxLength(50)
                .HasColumnName("itemRange");
            entity.Property(e => e.ItemRangeMax).HasColumnName("itemRangeMax");
            entity.Property(e => e.ItemRangeMin).HasColumnName("itemRangeMin");
            entity.Property(e => e.ItemUnit)
                .HasMaxLength(50)
                .HasColumnName("itemUnit");
            entity.Property(e => e.ProjectId).HasColumnName("projectID");

            entity.HasOne(d => d.Project).WithMany(p => p.Items)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_Item$_subproject");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK_Member");

            entity.Property(e => e.MemberId).HasColumnName("memberId");
            entity.Property(e => e.MemberAccount)
                .HasMaxLength(50)
                .HasColumnName("memberAccount");
            entity.Property(e => e.MemberAddress)
                .HasMaxLength(50)
                .HasColumnName("memberAddress");
            entity.Property(e => e.MemberBirthdate)
                .HasColumnType("date")
                .HasColumnName("memberBirthdate");
            entity.Property(e => e.MemberContactNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("memberContactNumber");
            entity.Property(e => e.MemberEmail)
                .HasMaxLength(50)
                .HasColumnName("memberEmail");
            entity.Property(e => e.MemberGender)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("memberGender");
            entity.Property(e => e.MemberName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("memberName");
            entity.Property(e => e.MemberNickname)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("memberNickname");
            entity.Property(e => e.MemberPassword)
                .HasMaxLength(50)
                .HasColumnName("memberPassword");
            entity.Property(e => e.MemberPhone)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("memberPhone");
            entity.Property(e => e.TaxId).HasColumnName("taxID");

            entity.HasOne(d => d.Tax).WithMany(p => p.Members)
                .HasForeignKey(d => d.TaxId)
                .HasConstraintName("FK_Member_Corporation");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK_Order_1");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.DeliveryDate)
                .HasColumnType("date")
                .HasColumnName("deliveryDate");
            entity.Property(e => e.MemberId).HasColumnName("memberId");
            entity.Property(e => e.OrderDate)
                .HasColumnType("date")
                .HasColumnName("orderDate");
            entity.Property(e => e.PayId).HasColumnName("payID");
            entity.Property(e => e.ShipDate)
                .HasColumnType("date")
                .HasColumnName("shipDate");
            entity.Property(e => e.ShipId).HasColumnName("shipID");
            entity.Property(e => e.StateId).HasColumnName("stateID");

            entity.HasOne(d => d.Member).WithMany(p => p.Orders)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_Order_Member");

            entity.HasOne(d => d.Pay).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PayId)
                .HasConstraintName("FK_Order_OrderPay");

            entity.HasOne(d => d.Ship).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShipId)
                .HasConstraintName("FK_Order_OrderShip");

            entity.HasOne(d => d.State).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_Order_OrderState");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("money")
                .HasColumnName("unitPrice");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderDetails_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_OrderDetails_Products");
        });

        modelBuilder.Entity<OrderPay>(entity =>
        {
            entity.HasKey(e => e.PayId);

            entity.ToTable("OrderPay");

            entity.Property(e => e.PayId).HasColumnName("PayID");
            entity.Property(e => e.PayName).HasMaxLength(50);
        });

        modelBuilder.Entity<OrderShip>(entity =>
        {
            entity.HasKey(e => e.ShipId);

            entity.ToTable("OrderShip");

            entity.Property(e => e.ShipId).HasColumnName("shipID");
            entity.Property(e => e.ShipName)
                .HasMaxLength(50)
                .HasColumnName("shipName");
        });

        modelBuilder.Entity<OrderState>(entity =>
        {
            entity.HasKey(e => e.StateId);

            entity.ToTable("OrderState");

            entity.Property(e => e.StateId).HasColumnName("stateID");
            entity.Property(e => e.StateDetailed)
                .HasMaxLength(50)
                .HasColumnName("stateDetailed");
            entity.Property(e => e.StateName)
                .HasMaxLength(50)
                .HasColumnName("stateName");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.ToTable("Plan");

            entity.Property(e => e.PlanId).HasColumnName("planId");
            entity.Property(e => e.PlanDescription).HasColumnName("planDescription");
            entity.Property(e => e.PlanName).HasColumnName("planName");
        });

        modelBuilder.Entity<PlanRef>(entity =>
        {
            entity.HasKey(e => e.PlanbridgeId).HasName("PK_Planbrigee");

            entity.ToTable("PlanRef");

            entity.Property(e => e.PlanbridgeId).HasColumnName("planbridgeId");
            entity.Property(e => e.PlanId).HasColumnName("planID");
            entity.Property(e => e.ProjectId).HasColumnName("projectID");

            entity.HasOne(d => d.Plan).WithMany(p => p.PlanRefs)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlanRef_Plan");

            entity.HasOne(d => d.Project).WithMany(p => p.PlanRefs)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_Planbrigee_subproject");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Photo).HasColumnType("image");
            entity.Property(e => e.UnitPrice).HasColumnType("money");
        });

        modelBuilder.Entity<ProductsCategory>(entity =>
        {
            entity.HasKey(e => e.CategoriesId);

            entity.Property(e => e.CategoriesId).HasColumnName("CategoriesID");
            entity.Property(e => e.CategoriesName).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductsClassification>(entity =>
        {
            entity.HasKey(e => e.ProductsClassification1);

            entity.ToTable("ProductsClassification");

            entity.Property(e => e.ProductsClassification1).HasColumnName("ProductsClassification");
            entity.Property(e => e.CategoriesId).HasColumnName("CategoriesID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Categories).WithMany(p => p.ProductsClassifications)
                .HasForeignKey(d => d.CategoriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductsClassification_ProductsCategories");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductsClassifications)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductsClassification_Products");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK_subproject");

            entity.ToTable("Project");

            entity.Property(e => e.ProjectId)
                .ValueGeneratedNever()
                .HasColumnName("projectID");
            entity.Property(e => e.ProjectContent)
                .HasMaxLength(255)
                .HasColumnName("projectContent");
            entity.Property(e => e.ProjectName).HasMaxLength(255);
            entity.Property(e => e.ProjectPrice).HasColumnName("projectPrice");
        });

        modelBuilder.Entity<ReportDetail>(entity =>
        {
            entity.HasKey(e => e.ReportDetailId).HasName("PK_reportDetail$");

            entity.ToTable("ReportDetail");

            entity.Property(e => e.ReportDetailId).HasColumnName("ReportDetailID");
            entity.Property(e => e.ItemId).HasColumnName("itemID");
            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.Result).HasColumnName("result");

            entity.HasOne(d => d.Item).WithMany(p => p.ReportDetails)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ReportDetail_Item");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportDetails)
                .HasForeignKey(d => d.ReportId)
                .HasConstraintName("FK_ReportDetail_HealthReport");
        });

        modelBuilder.Entity<ReportTest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ReportTest");

            entity.Property(e => e.ItemId).HasColumnName("itemID");
            entity.Property(e => e.ReportDetailId).HasColumnName("ReportDetailID");
            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.Result)
                .HasColumnType("text")
                .HasColumnName("result");

            entity.HasOne(d => d.Item).WithMany()
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ReportTest_Item");

            entity.HasOne(d => d.Report).WithMany()
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportTest_HealthReport");
        });

        modelBuilder.Entity<Reserve>(entity =>
        {
            entity.ToTable("Reserve");

            entity.Property(e => e.ReserveId).HasColumnName("ReserveID");
            entity.Property(e => e.MemberId).HasColumnName("memberID");
            entity.Property(e => e.PlanId).HasColumnName("planID");
            entity.Property(e => e.ReserveDate).HasColumnType("date");
            entity.Property(e => e.ReserveState).HasMaxLength(50);

            entity.HasOne(d => d.Member).WithMany(p => p.Reserves)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_Reserve_Members");

            entity.HasOne(d => d.Plan).WithMany(p => p.Reserves)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_Reserve_Plan");
        });

        modelBuilder.Entity<ReservedSub>(entity =>
        {
            entity.HasKey(e => e.SubreservedId);

            entity.ToTable("ReservedSub");

            entity.Property(e => e.SubreservedId).HasColumnName("subreservedID");
            entity.Property(e => e.ItemId).HasColumnName("itemID");
            entity.Property(e => e.ReservedId).HasColumnName("reservedID");

            entity.HasOne(d => d.Item).WithMany(p => p.ReservedSubs)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReservedSub_Item$");

            entity.HasOne(d => d.Reserved).WithMany(p => p.ReservedSubs)
                .HasForeignKey(d => d.ReservedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReservedSub_Reserve");
        });

        modelBuilder.Entity<SubProjectBridge>(entity =>
        {
            entity.ToTable("subProjectBridges");

            entity.Property(e => e.SubProjectBridgeId).HasColumnName("SubProjectBridgeID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.HasOne(d => d.Item).WithMany(p => p.SubProjectBridges)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_subProjectBridges_Item$");

            entity.HasOne(d => d.Project).WithMany(p => p.SubProjectBridges)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_subProjectBridges_subproject");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
