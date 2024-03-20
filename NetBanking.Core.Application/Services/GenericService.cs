using AutoMapper;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;

namespace NetBanking.Core.Application.Services
{
    public class GenericService<BaseVm, AddVm, Entity> : IGenericService<BaseVm, AddVm, Entity>
        where BaseVm : class
        where AddVm : class
        where Entity : class
    {

        private readonly IMapper _mapper;
        private readonly IGenericRepository<Entity> _repository;

        public GenericService(IMapper mapper,
            IGenericRepository<Entity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<AddVm> AddAsync(AddVm vm)
        {
            Entity entity = _mapper.Map<Entity>(vm);
            entity = await _repository.AddAsync(entity);

            AddVm addvm = _mapper.Map<AddVm>(entity);
            return addvm;
        }

        public async Task Delete(int Id)
        {
            var entity = await _repository.GeEntityByIDAsync(Id);
            await _repository.DeleteAsync(entity);
        }

        public async Task<List<BaseVm>> GetAllAsync()
        {
            var entiylist = await _repository.GetAllAsync();
            return _mapper.Map<List<BaseVm>>(entiylist);
        }

        public async Task<AddVm> GetByIdAsync(int Id)
        {
            var entity = await _repository.GeEntityByIDAsync(Id);
            AddVm vm = _mapper.Map<AddVm>(entity);
            return vm;
        }

        public async Task Update(AddVm vm, int Id)
        {
            Entity entity = _mapper.Map<Entity>(_mapper.Map<Entity>(vm));
            await _repository.UpdateAsync(entity, Id);
        }
    }
}
