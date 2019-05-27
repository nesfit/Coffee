namespace Barista.Api.Dto
{
    public class SpendingSumDto
    {
        public SpendingSumDto(decimal spendingSum)
        {
            SpendingSum = spendingSum;
        }

        public decimal SpendingSum { get; }
    }
}
