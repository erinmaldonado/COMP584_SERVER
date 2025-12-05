namespace COMP584_SERVER.DTOs
{
    public class LoginResponse
    {
        public Boolean Success { get; set; }
        public required String Message { get; set; }
        public required String Token { get; set; }

    }
}
