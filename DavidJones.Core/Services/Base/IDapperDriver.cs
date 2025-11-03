using DavidJones.Core.Models.DapperDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.ProductSyncer.Helpers.services;


public interface IDapperDriver
{
    SpResult<IEnumerable<OUTPUT>> Query<INPUT, OUTPUT>(INPUT model, SpOptions options)
      where INPUT : class
      where OUTPUT : class;
    //query sql with parameter
    IEnumerable<OUTPUT> Query<INPUT, OUTPUT>(string sql, INPUT model)
        where INPUT : class
        where OUTPUT : class;
    //query sql without parameter
    IEnumerable<OUTPUT> Query<OUTPUT>(string sql) where OUTPUT : class;
    Task<SpResult<IEnumerable<OUTPUT>>> QueryAsync<INPUT, OUTPUT>(INPUT model, SpOptions options)
        where INPUT : class
        where OUTPUT : class;
    Task<IEnumerable<OUTPUT>> QueryAsync<INPUT, OUTPUT>(string sql, INPUT model)
        where INPUT : class
        where OUTPUT : class;
    Task<IEnumerable<OUTPUT>> QueryAsync<OUTPUT>(string sql) where OUTPUT : class;
    Task<SpResult> ExecuteAsync<INPUT>(INPUT model, SpOptions options) where INPUT : class;
    Task ExecuteAsync<INPUT>(string sql, INPUT model) where INPUT : class;
    Task ExecuteAsync(string sql);
    SpResult Execute<INPUT>(INPUT model, SpOptions options) where INPUT : class;
    //command with parameter
    void Execute<INPUT>(string sql, INPUT model) where INPUT : class;
    //command without parameter
    void Execute(string sql);
}

