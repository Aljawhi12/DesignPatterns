namespace _7_Builder.Models
{
    public class UserBuilder
    {
        private string? _name;
        private string? _email;
        private bool _isAdmin;

        public UserBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public UserBuilder SetEmail(string email)
        {
            _email = email;
            return this;
        }

        public UserBuilder MakeAdmin()
        {
            _isAdmin = true;
            return this;
        }

        // Validates and returns a new, immutable User on every call.
        public User Build()
        {
            if (string.IsNullOrWhiteSpace(_name))
                throw new InvalidOperationException("Name is required.");

            if (string.IsNullOrWhiteSpace(_email))
                throw new InvalidOperationException("Email is required.");

            return new User
            {
                Name = _name,
                Email = _email,
                IsAdmin = _isAdmin
            };
        }
    }
}
