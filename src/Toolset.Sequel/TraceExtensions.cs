﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toolset.Sequel
{
  public static class TraceExtensions
  {
    private static string Dump(Sql defaultSql)
    {
      var sql = defaultSql.Clone();

      if (SequelSettings.QueryTemplateEnabled)
        sql.ApplyTemplate();

      var text = sql.Beautify().ToString();
      var lines = text.Split('\n', '\r').Where(x => !string.IsNullOrWhiteSpace(x));

      var builder = new StringBuilder();
      builder.AppendLine("Sql {");
      builder.AppendLine("  Text {");
      foreach (var line in lines)
      {
        builder.Append("    ");
        builder.AppendLine(line);
      }
      builder.AppendLine("  }");
      builder.AppendLine("  Args {");
      foreach (var parameter in sql.ParameterNames)
      {
        builder.Append("    ");
        builder.Append(string.IsNullOrEmpty(parameter) ? "{many}" : parameter);
        builder.Append(" := ");

        var value = sql[parameter];
        var any = value as Any;
        if (any?.IsRange == true)
        {
          builder.AppendLine($"{any.Range}");
        }
        else if (any?.IsList == true)
        {
          var item = any.List.FirstOrDefault();
          var isMany = item.IsEnumerable();
          var isBinary = item is byte;
          var isEmpty = !any.List.Any();

          if (isEmpty)
          {
            builder.AppendLine("null");
          }
          else if (isMany)
          {
            builder.AppendLine("{ ... }");
          }
          else if (isBinary)
          {
            builder.AppendLine("binary");
          }
          else
          {
            value = Commander.CreateSqlCompatibleValue(value);
            builder.AppendLine($"{{ {value} }}");
          }
        }
        else
        {
          value = Commander.CreateSqlCompatibleValue(value);
          builder.AppendLine(value is DBNull ? "(null)" : $"\"{value}\"");
        }

      }
      builder.AppendLine("  }");
      builder.AppendLine("}");
      return builder.ToString();
    }

    public static Sql Trace(this Sql sql)
    {
      var text = Dump(sql);
      try
      {
        System.Diagnostics.Trace.WriteLine(text);
      }
      catch
      {
        // nada a fazer
      }
      return sql;
    }

    public static Sql Echo(this Sql sql)
    {
      var text = Dump(sql.ApplyTemplate());
      try
      {
        System.Diagnostics.Trace.WriteLine(text);
      }
      catch
      {
        // nada a fazer
      }
      return sql;
    }
  }
}
