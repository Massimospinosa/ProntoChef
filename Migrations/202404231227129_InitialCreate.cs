namespace ProntoChef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OrderItemId = c.Int(nullable: false, identity: true),
                        OrderSummaryId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ItemPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderItemId)
                .ForeignKey("dbo.OrderSummaries", t => t.OrderSummaryId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderSummaryId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.OrderSummaries",
                c => new
                    {
                        OrderSummaryId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        OrderDate = c.String(),
                        OrderAddress = c.String(),
                        Note = c.String(),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        State = c.String(),
                    })
                .PrimaryKey(t => t.OrderSummaryId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 30),
                        Surname = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(nullable: false),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false, maxLength: 255),
                        ProductImage = c.String(nullable: false, maxLength: 1000),
                        ProductPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreparationTime = c.String(nullable: false, maxLength: 50),
                        Ingredients = c.String(nullable: false, maxLength: 1000),
                        Category = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderSummaries", "UserId", "dbo.Users");
            DropForeignKey("dbo.OrderItems", "OrderSummaryId", "dbo.OrderSummaries");
            DropIndex("dbo.OrderSummaries", new[] { "UserId" });
            DropIndex("dbo.OrderItems", new[] { "ProductId" });
            DropIndex("dbo.OrderItems", new[] { "OrderSummaryId" });
            DropTable("dbo.Products");
            DropTable("dbo.Users");
            DropTable("dbo.OrderSummaries");
            DropTable("dbo.OrderItems");
        }
    }
}
