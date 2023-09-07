namespace WebApi.Dtos;

public class OrderDto
{
    public string Email { get; set; }

    public List<long> BookIds { get; set; }
}