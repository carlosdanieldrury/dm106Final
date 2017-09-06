namespace dm106CarlosDrury.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesOnProductAndOrder1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "deliveryDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "deliveryDate", c => c.DateTime(nullable: false));
        }
    }
}
