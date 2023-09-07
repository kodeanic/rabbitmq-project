namespace Infrastructure.IServices;

public interface IOrderService
{
    Task CreateOrder(string email, List<long> bookIds);
}