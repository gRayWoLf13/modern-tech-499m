namespace modern_tech_499m.Repositories.Core.Domain
{
    /// <summary>
    /// Possible user register results
    /// </summary>
    public enum RegisterResult
    {
        /// <summary>
        /// User with given username already exists
        /// </summary>
        UsernameAlreadyExists = 1,

        /// <summary>
        /// Successful registration
        /// </summary>
        Success
    }
}
