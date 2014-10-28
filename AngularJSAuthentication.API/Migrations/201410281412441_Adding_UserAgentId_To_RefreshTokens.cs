namespace AngularJSAuthentication.API.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Adding_UserAgentId_To_RefreshTokens : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RefreshTokens", "UserAgentId", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RefreshTokens", "UserAgentId");
        }
    }
}
