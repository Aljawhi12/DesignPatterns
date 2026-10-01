namespace _7_Builder.Models
{
    public class User
    {
        public required string Name { get; init; }
        public required string Email { get; init; }
        public bool IsAdmin { get; init; }
    }
}
