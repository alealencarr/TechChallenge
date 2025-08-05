namespace Application.Interfaces.DataSources
{
    public interface IPaymentDataSource
    {
        Task<byte[]> GenerateQrCodeAsync(Guid id, decimal amount);

    }
}
