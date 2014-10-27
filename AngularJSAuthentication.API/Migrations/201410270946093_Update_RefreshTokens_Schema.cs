namespace AngularJSAuthentication.API.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Update_RefreshTokens_Schema : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RefreshTokens", "UserName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.RefreshTokens", "Subject");
            DropColumn("dbo.RefreshTokens", "IssuedUtc");
            DropColumn("dbo.RefreshTokens", "ProtectedTicket");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RefreshTokens", "ProtectedTicket", c => c.String(nullable: false));
            AddColumn("dbo.RefreshTokens", "IssuedUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.RefreshTokens", "Subject", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.RefreshTokens", "UserName");
        }
    }
}
