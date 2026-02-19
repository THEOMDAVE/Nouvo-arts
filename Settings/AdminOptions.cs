namespace NouvoStudio.Settings
{
    public class AdminOptions
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        
        // For backward compatibility during migration
        [Obsolete("Use PasswordHash instead")]
        public string? Password { get; set; }
    }
}


