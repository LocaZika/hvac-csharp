namespace hvac_backend.global.interfaces;

public interface IServiceRouteBase<TQueryParams, TFindAll, TFindOne, TCreateDto, TUpdateDto> {
  Task<List<TFindAll>?> FindAll(TQueryParams queryParams);
  Task<TFindOne?> FindOne(int id);
  Task<bool> Create(TCreateDto CreateDto);
  Task<bool> Update(int id, TUpdateDto UpdateDto);
  Task<bool> Remove(int id);
}
