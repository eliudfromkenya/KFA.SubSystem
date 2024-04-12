using Ardalis.Result;
using Autofac;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Interfaces;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Core.Services;
using KFA.SubSystem.Globals;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Globals.Models;
using KFA.SubSystem.Infrastructure.Data;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Models.Create;
using KFA.SubSystem.UseCases.Models.Delete;
using KFA.SubSystem.UseCases.Models.Get;
using KFA.SubSystem.UseCases.Models.List;
using KFA.SubSystem.UseCases.Models.Patch;
using KFA.SubSystem.UseCases.Models.Update;
using KFA.SubSystem.UseCases.Users;
using KFA.SubSystem.UseCases.Xs.Get;
using KFA.SubSystem.Web.UserEndPoints;
using MediatR;

namespace KFA.SubSystem.Infrastructure;

internal static class RegisterEntities
{
  public static void RegisterQueries(ContainerBuilder builder)
  {
    //builder.RegisterType<ListContributorsQueryService>()
    //  .As<IListContributorsQueryService>()
    //  .InstancePerLifetimeScope();

    var classes = new[]
    {
       System.Reflection.Assembly.GetAssembly(typeof(BaseModel)),
       System.Reflection.Assembly.GetAssembly(typeof(CostCentre))
         } /*AppDomain.CurrentDomain.GetAssemblies()*/
        .SelectMany(s => s?.GetTypes() ?? [])
    .Where(typeof(BaseModel).IsAssignableFrom)
        .Where(c => c != typeof(BaseModel)).ToList();

    RegisterDataServices(builder);
    RegisterUserServices(builder);
    RegisterCreateModels(builder, classes);
    RegisterDeleteModels(builder, classes);
    RegisterUpdateModels(builder, classes);
    RegisterByIdModels(builder, classes);
    RegisterByIdsModels(builder, classes);
    RegisterListsModels(builder, classes);
    RegisterDynamicListsModels(builder, classes);
    RegisterPatchModels(builder, classes);
  }


  private static void RegisterUserServices(ContainerBuilder builder)
  {
    builder.RegisterType<UserAddRightsHandler>()
           .As<IRequestHandler<UserAddRightsCommand, Result<UserRightDTO[]>>>()
           .InstancePerLifetimeScope();
    builder.RegisterType<UserChangePasswordHandler>()
         .As<IRequestHandler<UserChangePasswordCommand, Result>>()
         .InstancePerLifetimeScope();
    builder.RegisterType<UserChangeRoleHandler>()
         .As<IRequestHandler<UserChangeRoleCommand, Result>>()
         .InstancePerLifetimeScope();
    builder.RegisterType<UserClearRightsHandler>()
         .As<IRequestHandler<UserClearRightsCommand, Result<string[]>>>()
         .InstancePerLifetimeScope();
    builder.RegisterType<UserRegisterHandler>()
         .As<IRequestHandler<UserRegisterCommand, Result<(SystemUserDTO user, string? loginId, string?[]? rights)>>>()
         .InstancePerLifetimeScope();
    builder.RegisterType<UserRegisterDeviceHandler>()
         .As<IRequestHandler<UserRegisterDeviceCommand, Result<DataDeviceDTO>>>()
         .InstancePerLifetimeScope();
    builder.RegisterType<UserLoginHandler>()
         .As<IRequestHandler<UserLoginCommand, Result<LoginResult>>>()
         .InstancePerLifetimeScope();
  }
    private static void RegisterDataServices(ContainerBuilder builder)
  {
    builder.RegisterType<IdGenerator>()
           .As<IIdGenerator>()
           .SingleInstance();
    builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
    builder.RegisterType<UserManagementService>().As<IUserManagementService>().InstancePerLifetimeScope();

    Declarations.IdGenerator = new IdGenerator();
  }

  private static void RegisterListsModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var dtoAssemblyName = typeof(CostCentreDTO).Assembly.GetName()?.Name;
        var dtoType = Type.GetType($"{dtoAssemblyName}.DTOs.{type.Name}DTO, {dtoAssemblyName}")!;

        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(ListModelsQuery<,>);
        var listType = typeof(List<>).MakeGenericType(dtoType);
        var resultType = typeof(Result<>);// .MakeGenericType(listType);
        var genericCommandType = modelCommandType.MakeGenericType(dtoType, type);
        var genericResultType = resultType.MakeGenericType(listType);
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, genericResultType);

        Type genericHandlerType = typeof(ListModelsHandler<,>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType(dtoType, type);

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch (Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }


  private static void RegisterDynamicListsModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var dtoAssemblyName = typeof(CostCentreDTO).Assembly.GetName()?.Name;
        var dtoType = Type.GetType($"{dtoAssemblyName}.DTOs.{type.Name}DTO, {dtoAssemblyName}")!;

        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(DynamicsListModelsQuery<,>);
        //var listType = typeof(List<>).MakeGenericType(dtoType);
        var resultType = typeof(Result<>);// .MakeGenericType(listType);
        var genericCommandType = modelCommandType.MakeGenericType(dtoType, type);
        var genericResultType = resultType.MakeGenericType(typeof(string));
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, genericResultType);

        Type genericHandlerType = typeof(DynamicsListModelsHandler<,>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType(dtoType, type);

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch (Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }

  private static void RegisterByIdModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var dtoAssemblyName = typeof(CostCentreDTO).Assembly.GetName()?.Name;
        var dtoType = Type.GetType($"{dtoAssemblyName}.DTOs.{type.Name}DTO, {dtoAssemblyName}")!;

        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(GetModelQuery<,>);
        var resultType = typeof(Result<>);
        var genericCommandType = modelCommandType.MakeGenericType(dtoType, type);
        var genericResultType = resultType.MakeGenericType(dtoType);
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, genericResultType);

        Type genericHandlerType = typeof(GetModelHandler<,>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType([dtoType, type]);

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch (Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }

  private static void RegisterByIdsModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var dtoAssemblyName = typeof(CostCentreDTO).Assembly.GetName()?.Name;
        var dtoType = Type.GetType($"{dtoAssemblyName}.DTOs.{type.Name}DTO, {dtoAssemblyName}")!;

        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(GetModelsByIdsQuery<,>);
        var resultType = typeof(Result<>);
        var genericCommandType = modelCommandType.MakeGenericType(dtoType, type);
        var genericResultType = resultType.MakeGenericType(dtoType.MakeArrayType());
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, genericResultType);

        Type genericHandlerType = typeof(GetModelsByIdsHandler<,>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType([dtoType, type]);

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch (Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }

  private static void RegisterUpdateModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var dtoAssemblyName = typeof(CostCentreDTO).Assembly.GetName()?.Name;
        var dtoType = Type.GetType($"{dtoAssemblyName}.DTOs.{type.Name}DTO, {dtoAssemblyName}")!;

        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(UpdateModelCommand<,>);
        var resultType = typeof(Result<>);
        var genericCommandType = modelCommandType.MakeGenericType(dtoType, type);
        var genericResultType = resultType.MakeGenericType(dtoType);
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, genericResultType);

        Type genericHandlerType = typeof(UpdateModelHandler<,>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType([dtoType, type]);

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch (Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }

  private static void RegisterPatchModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var dtoAssemblyName = typeof(CostCentreDTO).Assembly.GetName()?.Name;
        var dtoType = Type.GetType($"{dtoAssemblyName}.DTOs.{type.Name}DTO, {dtoAssemblyName}")!;

        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(PatchModelCommand<,>);
        var resultType = typeof(Result<>);
        var genericCommandType = modelCommandType.MakeGenericType(dtoType, type);
        var genericResultType = resultType.MakeGenericType(dtoType);
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, genericResultType);

        Type genericHandlerType = typeof(PatchModelHandler<,>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType([dtoType, type]);

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch (Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }

  private static void RegisterDeleteModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(DeleteModelCommand<>);
        var resultType = typeof(Result);
        var genericCommandType = modelCommandType.MakeGenericType(type);
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, resultType);

        Type genericHandlerType = typeof(DeleteModelHandler<>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType([type]);

        builder.RegisterType(typeof(DeleteModelService<>).MakeGenericType(type))
         .As(typeof(IDeleteModelService<>).MakeGenericType(type))
         .InstancePerLifetimeScope();

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch (Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }

  private static void RegisterCreateModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var dtoAssemblyName = typeof(CostCentreDTO).Assembly.GetName()?.Name;
        var dtoType = Type.GetType($"{dtoAssemblyName}.DTOs.{type.Name}DTO, {dtoAssemblyName}")!;

        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(CreateModelCommand<,>);
        var resultType = typeof(Result<>);
        var genericCommandType = modelCommandType.MakeGenericType(dtoType, type);
        var genericResultType = resultType.MakeGenericType(dtoType.MakeArrayType());
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, genericResultType);

        Type genericHandlerType = typeof(CreateModelHandler<,>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType([dtoType, type]);

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch (Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }
}
