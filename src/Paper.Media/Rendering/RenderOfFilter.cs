﻿using System;
using System.Collections.Generic;
using System.Linq;
using Paper.Media.Design;
using Paper.Media.Design.Papers;
using Paper.Media.Routing;
using Toolset;
using Toolset.Reflection;

namespace Paper.Media.Rendering
{
  static class RenderOfFilter
  {
    /// <summary>
    /// Renderizador de propriedades de filtragem.
    /// </summary>
    /// <param name="paper">A instância do Paper com as definições de rederização da rota.</param>
    /// <param name="context">O contexto de rederização do Paper.</param>
    public static void SetArgs(IPaper paper, IContext context)
    {
      if (!paper._Has("Filter"))
        return;

      var filter = paper._Get<IFilter>("Filter");
      if (filter == null)
      {
        if (!paper._CanWrite("Filter"))
          return;

        filter = filter._SetNew<IFilter>("Filter");
      }

      var args = context.RequestArgs;
      foreach (var name in args.Names)
      {
        if (filter._Has(name))
        {
          var value = args[name];
          filter._Set(name, value);
        }
      }
    }

    internal static void Render(IPaper paper, IContext context, Entity entity)
    {
      if (!paper._Has("Filter"))
        return;

      var filter = paper._Get<IFilter>("Filter");
      if (filter == null)
        return;

      var action = EntityActionConverter.ConvertToEntityAction(filter, context);
      action.Name = "__filter";
      action.Title = "Filtro";
      action.Method = MethodNames.Get;
      action.Href = ".";
      entity.AddAction(action);
    }

    //public static void SetFilter(RenderContext ctx)
    //{
    //  var filter = ctx.Query.Get("Filter");
    //  if (filter == null)
    //  {
    //    if (!ctx.Query.IsWritable("Filter"))
    //      return;

    //    filter = ctx.Query.SetNew("Filter");
    //  }

    //  var args =
    //    from arg in ctx.QueryArgs.Concat(ctx.PathArgs)
    //    where !UriUtil.AllReservedArgs.Contains(arg.Key)
    //    select arg;

    //  foreach (var arg in args)
    //  {
    //    var key = arg.Key;
    //    var value = arg.Value;

    //    if (filter.Has<Widget>(key))
    //    {
    //      var widget = filter.Get(key);
    //      if (widget == null)
    //      {
    //        if (!filter.IsWritable(key))
    //          continue;

    //        widget = filter.SetNew(key, key);
    //      }
    //      widget.Set("Value", value);
    //    }
    //    else
    //    {
    //      if (!filter.IsWritable(key))
    //        continue;

    //      filter.Set(key, value);
    //    }
    //  }
    //}

    //public static void RenderFilter(RenderContext ctx)
    //{
    //  var filter = ctx.Query.Get("Filter");
    //  if (filter == null)
    //    return;

    //  var widgets =
    //    from property in filter.GetType().GetProperties().Select(x => x.Name)
    //    let widget = filter.Get(property) as Widget
    //    where widget != null
    //    select widget;

    //  if (!widgets.Any())
    //    return;

    //  if (ctx.Entity.Actions == null)
    //  {
    //    ctx.Entity.Actions = new EntityActionCollection();
    //  }

    //  var filterAction = ctx.Entity.Actions.FirstOrDefault(x => x.Name.EqualsIgnoreCase("Filter"));
    //  if (filterAction == null)
    //  {
    //    var filterArgs = Enumerable.Empty<KeyValuePair<string, object>>();
    //    var href = QueryPathBuilder.BuildPath(ctx, ctx.Query, ctx.PathTemplate, null, filterArgs);

    //    // removendo informacao de paginacao
    //    href = new Route(href).UnsetArgs("limit", "offset");

    //    filterAction = new EntityAction();
    //    filterAction.Name = "Filters";
    //    filterAction.Href = href;
    //    filterAction.Method = "GET";
    //    filterAction.Title = "Filtros";
    //    ctx.Entity.Actions.Add(filterAction);
    //  }

    //  if (filterAction.Fields == null)
    //  {
    //    filterAction.Fields = new FieldCollection();
    //  }

    //  foreach (var widget in widgets)
    //  {
    //    var field = widget.ToMediaField();

    //    if (field.Title == null)
    //      field.Title = field?.Name.ChangeCase(TextCase.ProperCase);

    //    filterAction.Fields.Add(field);
    //  }
    //}
  }
}