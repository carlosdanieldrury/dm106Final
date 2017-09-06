namespace dm106CarlosDrury.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesOnProductAndOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "largura", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "largura");
        }
    }
}
