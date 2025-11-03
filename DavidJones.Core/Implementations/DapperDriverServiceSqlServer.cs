using Dapper;
using DavidJones.Core.Contexts;
using DavidJones.Core.Models.DapperDriver;
using DavidJones.ProductSyncer.Helpers.services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Implementations;

public class DapperDriverServiceSqlServer : IDapperDriver
{
    private readonly IDapperContext context;
    public DapperDriverServiceSqlServer() => context = new DapperContext();

    public SpResult Execute<INPUT>(INPUT model, SpOptions options) where INPUT : class
    {
        var conn = context.CreateConnection();
        var param = new DynamicParameters();
        try
        {
            if (model != null)
            {
                var propertyList = model.GetType().GetProperties();
                foreach (var item in propertyList)
                {
                    if (item.PropertyType == typeof(byte[]))
                        param.Add(item.Name, dbType: DbType.Binary, value: item.GetValue(model, null), direction: ParameterDirection.Input, size: -1);
                    else
                        param.Add(item.Name, item.GetValue(model, null));
                }
                param.Add("ErrorNumber", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("ErrorMessage", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

                if (options.HasOutputJson)
                    param.Add("OutputJson", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                if (options.HasOutputId)
                    param.Add("OutputId", dbType: DbType.Int64, direction: ParameterDirection.Output);

            }

            var effectedId = conn.Execute(
                options.Name,
                param,
                null,
                options.Timeout,
                commandType: CommandType.StoredProcedure);

            var result = new SpResult
            {
                ErrorHandler = new ErrorManager((param.Get<int?>("ErrorNumber") ?? 0), (param.Get<string>("ErrorMessage") ?? "عملیات با موفقیت انجام شد")),
                Id = options.HasOutputId ? param.Get<long?>("OutputId") : null,
                OutputJson = options.HasOutputJson ? param.Get<string?>("OutputJson") : null,
            };

            result.IsSuccess = true;
            return result;
        }
        catch (Exception ex)
        {
            return new SpResult
            {
                ErrorHandler = new ErrorManager((1000), (ex.Message)),
                Id = null,
                OutputJson = null,
                IsSuccess = false
            };
        }
    }


    public void Execute<INPUT>(string sql, INPUT model) where INPUT : class
    {
        var conn = context.CreateConnection();
        var param = new DynamicParameters();
        try
        {
            if (model != null)
            {
                var propertyList = model.GetType().GetProperties();
                foreach (var item in propertyList)
                {
                    if (item.PropertyType == typeof(byte[]))
                        param.Add(item.Name, dbType: DbType.Binary, value: item.GetValue(model, null), direction: ParameterDirection.Input, size: -1);
                    else
                        param.Add(item.Name, item.GetValue(model, null));
                }
            }

            conn.Execute(sql, param, null, commandTimeout: 300, commandType: CommandType.Text);

        }
        catch (Exception ex)
        {
            throw; //return new SpResult<OUTPUT> { ErrorHandler = new CErrorHandler(3, ex.Message) };
        }
    }

    public void Execute(string sql)
    {
        var conn = context.CreateConnection();
        try
        {
            conn.Execute(sql, 300, commandType: CommandType.Text);
        }
        catch (Exception ex)
        {
            throw; //return new SpResult<OUTPUT> { ErrorHandler = new CErrorHandler(3, ex.Message) };
        }
    }

    public async Task<SpResult> ExecuteAsync<INPUT>(INPUT model, SpOptions options) where INPUT : class
    {
        var conn = context.CreateConnection();
        var param = new DynamicParameters();

        try
        {
            if (model != null)
            {
                var propertyList = model.GetType().GetProperties();
                foreach (var item in propertyList)
                {
                    if (item.PropertyType == typeof(byte[]))
                        param.Add(item.Name, dbType: DbType.Binary, value: item.GetValue(model, null), direction: ParameterDirection.Input, size: -1);
                    else
                        param.Add(item.Name, item.GetValue(model, null));
                }
                param.Add("ErrorNumber", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("ErrorMessage", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

                if (options.HasOutputJson)
                    param.Add("OutputJson", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                if (options.HasOutputId)
                    param.Add("OutputId", dbType: DbType.Int64, direction: ParameterDirection.Output);

            }

            var effectedId = await conn.ExecuteAsync(
                options.Name,
                param,
                null,
                options.Timeout,
                commandType: CommandType.StoredProcedure);

            var result = new SpResult
            {
                ErrorHandler = new ErrorManager((param.Get<int?>("ErrorNumber") ?? 0), (param.Get<string>("ErrorMessage") ?? "عملیات با موفقیت انجام شد")),
                Id = options.HasOutputId ? param.Get<long?>("OutputId") : null,
                OutputJson = options.HasOutputJson ? param.Get<string?>("OutputJson") : null,
            };

            result.IsSuccess = true;
            return result;
        }
        catch (Exception ex)
        {
            return new SpResult
            {
                ErrorHandler = new ErrorManager((1000), (ex.Message)),
                Id = null,
                OutputJson = null,
                IsSuccess = false
            };
        }
    }

    public async Task ExecuteAsync<INPUT>(string sql, INPUT model) where INPUT : class
    {
        var conn = context.CreateConnection();
        var param = new DynamicParameters();

        try
        {
            if (model != null)
            {
                var propertyList = model.GetType().GetProperties();
                foreach (var item in propertyList)
                {
                    if (item.PropertyType == typeof(byte[]))
                        param.Add(item.Name, dbType: DbType.Binary, value: item.GetValue(model, null), direction: ParameterDirection.Input, size: -1);
                    else
                        param.Add(item.Name, item.GetValue(model, null));
                }
            }

            await conn.ExecuteAsync(sql, param, null, commandTimeout: 300, commandType: CommandType.Text);

        }
        catch (Exception ex)
        {
            throw;
        }

    }
    public async Task ExecuteAsync(string sql)
    {
        var conn = context.CreateConnection();
        try
        {
            await conn.ExecuteAsync(sql, 300, commandType: CommandType.Text);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public SpResult<IEnumerable<OUTPUT>> Query<INPUT, OUTPUT>(INPUT model, SpOptions options)
      where INPUT : class
      where OUTPUT : class
    {
        var conn = context.CreateConnection();
        var param = new DynamicParameters();
        try
        {
            if (model != null)
            {
                var propertyList = model.GetType().GetProperties();
                foreach (var item in propertyList)
                {
                    if (item.PropertyType == typeof(byte[]))
                        param.Add(item.Name, dbType: DbType.Binary, value: item.GetValue(model, null), direction: ParameterDirection.Input, size: -1);
                    else
                        param.Add(item.Name, item.GetValue(model, null));
                }
                param.Add("ErrorNumber", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("ErrorMessage", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

                if (options.HasTotalCount)
                    param.Add("TotalCount", dbType: DbType.Int64, direction: ParameterDirection.Output, size: int.MaxValue);

            }

            var list = conn.Query<OUTPUT>(
                options.Name,
                param,
                null,
                true,
                options.Timeout,
                commandType: CommandType.StoredProcedure);

            var result = new SpResult<IEnumerable<OUTPUT>>
            {
                ErrorHandler = new ErrorManager((param.Get<int?>("ErrorNumber") ?? 0), (param.Get<string>("ErrorMessage") ?? "عملیات با موفقیت انجام شد")),
                TotalCount = options.HasTotalCount ? param.Get<long?>("TotalCount") : null,
                Value = list
            };
            result.IsSuccess = true;
            return result;
        }
        catch (Exception ex)
        {
            return new SpResult<IEnumerable<OUTPUT>> { ErrorHandler = new ErrorManager(1000, ex.Message), Value = null, IsSuccess = false };
        }

    }

    public IEnumerable<OUTPUT> Query<INPUT, OUTPUT>(string sql, INPUT model)
        where INPUT : class
        where OUTPUT : class
    {
        var conn = context.CreateConnection();
        var param = new DynamicParameters();
        try
        {
            if (model != null)
            {
                var propertyList = model.GetType().GetProperties();
                foreach (var item in propertyList)
                {
                    if (item.PropertyType == typeof(byte[]))
                        param.Add(item.Name, dbType: DbType.Binary, value: item.GetValue(model, null), direction: ParameterDirection.Input, size: -1);
                    else
                        param.Add(item.Name, item.GetValue(model, null));
                }
            }

            return conn.Query<OUTPUT>(sql, param, null, commandTimeout: 300, commandType: CommandType.Text);

        }
        catch (Exception ex)
        {
            throw;//new DatabaseException(ex, "خطایی پایگاه داده لطفا با پشتیبانی ارتبط بگیرید");
        }
    }

    public IEnumerable<OUTPUT> Query<OUTPUT>(string sql) where OUTPUT : class
    {
        var conn = context.CreateConnection();
        try
        {
            return conn.Query<OUTPUT>(sql, 300, commandType: CommandType.Text);
        }
        catch (Exception ex)
        {
            throw; //new DatabaseException(ex, "خطایی پایگاه داده لطفا با پشتیبانی ارتبط بگیرید");
        }
    }

    public async Task<SpResult<IEnumerable<OUTPUT>>> QueryAsync<INPUT, OUTPUT>(INPUT model, SpOptions options)
        where INPUT : class
        where OUTPUT : class
    {
        var conn = context.CreateConnection();
        var param = new DynamicParameters();
        try
        {
            if (model != null)
            {
                var propertyList = model.GetType().GetProperties();
                foreach (var item in propertyList)
                {
                    if (item.PropertyType == typeof(byte[]))
                        param.Add(item.Name, dbType: DbType.Binary, value: item.GetValue(model, null), direction: ParameterDirection.Input, size: -1);
                    else
                        param.Add(item.Name, item.GetValue(model, null));
                }
                param.Add("ErrorNumber", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("ErrorMessage", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

                if (options.HasTotalCount)
                    param.Add("TotalCount", dbType: DbType.Int64, direction: ParameterDirection.Output, size: int.MaxValue);

            }

            var list = await conn.QueryAsync<OUTPUT>(
                options.Name,
                param,
                null,
                options.Timeout,
                commandType: CommandType.StoredProcedure);

            var result = new SpResult<IEnumerable<OUTPUT>>
            {
                ErrorHandler = new ErrorManager((param.Get<int?>("ErrorNumber") ?? 0), (param.Get<string>("ErrorMessage") ?? "عملیات با موفقیت انجام شد")),
                TotalCount = options.HasTotalCount ? param.Get<long?>("TotalCount") : null,
                Value = list
            };
            result.IsSuccess = true;
            return result;
        }
        catch (Exception ex)
        {
            return new SpResult<IEnumerable<OUTPUT>> { ErrorHandler = new ErrorManager(1000, ex.Message), Value = null, IsSuccess = false };
        }

    }

    public async Task<IEnumerable<OUTPUT>> QueryAsync<INPUT, OUTPUT>(string sql, INPUT model)
        where INPUT : class
        where OUTPUT : class
    {
        var conn = context.CreateConnection();
        var param = new DynamicParameters();

        try
        {
            if (model != null)
            {
                var propertyList = model.GetType().GetProperties();
                foreach (var item in propertyList)
                {
                    if (item.PropertyType == typeof(byte[]))
                        param.Add(item.Name, dbType: DbType.Binary, value: item.GetValue(model, null), direction: ParameterDirection.Input, size: -1);
                    else
                        param.Add(item.Name, item.GetValue(model, null));
                }
            }

            return await conn.QueryAsync<OUTPUT>(sql, param, null, 300, commandType: CommandType.Text);

        }
        catch (Exception ex)
        {
            throw;// new DatabaseException(ex, "خطایی پایگاه داده لطفا با پشتیبانی ارتبط بگیرید");
        }
    }

    public async Task<IEnumerable<OUTPUT>> QueryAsync<OUTPUT>(string sql) where OUTPUT : class
    {
        var conn = context.CreateConnection();
        try
        {
            return await conn.QueryAsync<OUTPUT>(sql,
                null,
                null,
                300, commandType: CommandType.Text);

        }
        catch (DbException ex)
        {
            throw;
        }
    }
}
