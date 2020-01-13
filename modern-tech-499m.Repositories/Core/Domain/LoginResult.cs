namespace modern_tech_499m.Repositories.Core.Domain
{
    /// <summary>
    /// Possible user login results
    /// </summary>
    public enum LoginResult
    {
        /// <summary>
        /// User with given username does not exist
        /// </summary>
        UsernameNotExists = 1,

        /// <summary>
        /// Username is correct, but password is not
        /// </summary>
        WrongPassword,

        /// <summary>
        /// Successful login
        /// </summary>
        Success
    }
}
