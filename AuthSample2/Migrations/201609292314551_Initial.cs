namespace AuthSample2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.auth_users",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 255),
                        login_name = c.String(maxLength: 255),
                        password_hash = c.String(maxLength: 255),
                        hash_key = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.auth_users");
        }
    }
}
