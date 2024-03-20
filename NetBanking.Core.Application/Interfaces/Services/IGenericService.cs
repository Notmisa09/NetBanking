namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IGenericService <BaseVm , AddVm , Entity> 
        where BaseVm : class
        where Entity : class
        where AddVm : class
    {
        Task<List<BaseVm>> GetAllAsync();
        Task<AddVm> GetByIdAsync (int Id);
        Task Delete(int Id);
        Task<AddVm> AddAsync (AddVm vm);
        Task Update(AddVm vm , int Id);
    }
}
