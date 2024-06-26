﻿using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core;
using KFA.SubSystem.Globals;
using KFA.SubSystem.UseCases.ModelCommandsAndQueries;

namespace KFA.SubSystem.UseCases.Models.List;

public record DynamicsListModelsQuery<T, X>(EndPointUser user, ListParam param) : IQuery<Result<string>> where T : BaseDTO<X>, new() where X : BaseModel, new();

// public ModelByParamSpec(IDbQuery<T> queryGenerator, ListParam param)
  //{   
  //  DynamicParam<T>.Process(queryGenerator, param);
  //}

// {"FilterParam":{"OrderByConditions":["Description","SupplierCodePrefix"],"SelectColumns":"new {Id, Description, SupplierCodePrefix}","Predicate":"SupplierCodePrefix.Trim().StartsWith(@0) and Id >= @1","Parameters":["S3","3100"]},"Skip":0,"Take":3}
