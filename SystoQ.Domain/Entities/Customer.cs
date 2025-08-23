namespace SystoQ.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = string.Empty;
        public string? Email { get; private set; }
        public string? PhoneNumber { get; private set; }
        
        public Customer(string name, string email, string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome é obrigatório.", nameof(name));
            
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("Email inválido.", nameof(email));
            
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber ?? string.Empty;
        }
        
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Nome é obrigatório.", nameof(newName));
            
            Name = newName;
        }
        
        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains("@"))
                throw new ArgumentException("Email inválido.", nameof(newEmail));
            
            Email = newEmail;
        }
        
        public void UpdatePhoneNumber(string? newPhoneNumber)
        {
            PhoneNumber = newPhoneNumber ?? string.Empty;
        }
    }
}
