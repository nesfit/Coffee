using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.Accounting.Migrations
{
    public partial class SpendingOfUsersView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR REPLACE VIEW SpendingOfUsers AS
            SELECT
                s.Id AS SaleId,
                NULL AS PaymentId,
                -s.Cost AS Amount,
                s.Created AS Created,
                s.UserId AS UserId,
                s.AccountingGroupId AS AccountingGroupId,
                s.PointOfSaleId AS PointOfSaleId
            FROM Sales AS s
            WHERE s.Id NOT IN (SELECT x.SaleId FROM SaleStateChange AS x WHERE x.SaleId=s.Id AND x.State=2)   

            UNION ALL

            SELECT
                NULL AS SaleId,
                p.Id AS PaymentId,
                p.Amount AS Amount,
                p.Created AS Created,
                p.UserId AS UserId,
                NULL AS AccountingGroupId,                
                NULL AS PointOfSaleId
                
            FROM Payments AS p");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW SpendingOfUsers");
        }
    }
}
