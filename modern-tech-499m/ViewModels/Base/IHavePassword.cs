using System.Security;

namespace modern_tech_499m.ViewModels
{
    /// <summary>
    /// An interface for the class that can provide a secure password
    /// </summary>
    public interface IHavePassword
    {
        SecureString SecurePassword { get; }
    }
}
