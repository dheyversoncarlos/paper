﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;

namespace Toolset.Sequel
{
  internal static class Commander
  {
    public static DbCommand CreateCommand(DbConnection cn, Sql sql)
    {
      sql.ApplyTemplate();

      var command = cn.CreateCommand();
      command.CommandType = CommandType.Text;
      command.CommandText = sql.Text;
      command.CommandTimeout = (cn.ConnectionTimeout << 1);
      command.Transaction = SequelTransactionScope.GetTransactionFor(cn);

      var names =
        from name in sql.ParameterNames
        orderby name.Length descending, name
        select name;
      foreach (var name in names)
      {
        var value = CreateSqlCompatibleValue(sql[name]);

        var parameterName = name;
        var parameter = command.CreateParameter();
        parameter.ParameterName = parameterName;
        parameter.Value = value;
        command.Parameters.Add(parameter);
      }

      if (SequelSettings.TraceQueries)
      {
        var message = "---\n" + sql.Text.Beautify() + "\n---\n";
        System.Diagnostics.Trace.WriteLine(message);
      }

      return command;
    }

    public static object CreateSqlCompatibleValue(object value)
    {
      if (value == null || (value as Any)?.IsNull == true)
      {
        return DBNull.Value;
      }

      if ((value as Any)?.IsRaw == true)
      {
        value = ((Any)value).Raw;
      }

      if (value is Sql)
      {
        return DBNull.Value;
      }

      if (value is XNode)
      {
        var xml = ((XNode)value).ToString(SaveOptions.DisableFormatting);
        return xml;
      }

      if ((value as Any)?.IsText == true)
      {
        return ((Any)value).Text;
      }

      if ((value as Any)?.IsRange == true)
      {
        return DBNull.Value;
      }

      if ((value as Any)?.IsList == true)
      {
        var list = ((Any)value).List;

        if (!list.Any())
        {
          return DBNull.Value;
        }

        if (list.First() is byte)
        {
          // Temos um array de bytes.
          // Array de bytes ├⌐ usado para dados bin├írios.
          // Deve ser repassado como est├í.
          return list.Cast<byte>().ToArray();
        }

        var sample = list.FirstOrDefault();
        if (sample is string)
        {
          list = list.Select(x => "'" + x + "'");
        }
        var text = string.Join(",", list);
        return text;
      }

      return value;
    }
  }
}
