namespace UserManagementAPI.DAL.Contracts
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IUserProfileRepository UserProfileRepository { get; }
        public Task SaveAsync();
    }
}
