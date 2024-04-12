using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core.ContributorAggregate.Specifications;
using KFA.SubSystem.Core.Services;
using KFA.SubSystem.Globals;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KFA.SubSystem.UseCases.Models.List;

public class ListModelsHandler<T, X>(IDbQuery<X> dbQuery)
  : IQueryHandler<ListModelsQuery<T, X>, List<X>> where T : BaseDTO<X>, new() where X : BaseModel, new()
{
  static object objLock = new();
  public async Task<List<X>> Handle(ListModelsQuery<T, X> request, CancellationToken cancellationToken)
  {
    var query = DynamicParam<X>.GetQuery(request.user, dbQuery, request.param);
    List<X> objs = [];
    if(query != null)
      objs = await query!.ToListAsync(cancellationToken);

    return objs;

   //var tt = objs.Select(v => v.ToBaseDTO() /*as T*/).ToList();

    //List<T> values = [];
    //values.Adapt(objs, typeof(List<X>), typeof(List<T>));
    //return Result<List<T>>.Success(values);
    // var resukt = JsonConvert.SerializeObject(objs);
    // var bb = objs.Select(v => (T)v.ToBaseDTO()).ToList();
    // return Result<List<T>>.Success(JsonConvert.DeserializeObject<List<T>>(resukt));
   // return Result<List<T>>.Success(objs.Select(v => (T)v.ToBaseDTO()).ToList());
  }
}
