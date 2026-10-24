namespace ConsultoriosApi.Application.Contracts.Authentication
{
    public class AuthResultDTO
    {
        public required string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
