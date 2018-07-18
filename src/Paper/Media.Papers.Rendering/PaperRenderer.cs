﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Toolset.Collections;
using Toolset.Reflection;

namespace Paper.Media.Papers.Rendering
{
  public class PaperRenderer
  {
    private readonly IServiceProvider serviceProvider;

    public PaperRenderer(IServiceProvider serviceProvider)
    {
      this.serviceProvider = serviceProvider;
    }

    public Entity RenderEntity(PaperContext ctx)
    {
      var entity = new Entity();

      //
      // Fase 2: Repassando parametros
      //
      SetArgs(ctx.Paper, ctx.QueryArgs);
      SetArgs(ctx.Paper, ctx.PathArgs);

      //
      // Fase 3: Consultando dados
      //
      CacheData(ctx.Paper, ctx.Cache);
      CacheRows(ctx.Paper, ctx.Cache);

      //
      // Fase 4: Renderizando entidade
      //
      RenderOfInfo.Render(ctx.Paper, entity, ctx);
      RenderOfData.Render(ctx.Paper, entity, ctx);
      // RenderRows(ctx.Paper, entity, ctx);

      return entity;
    }

    /// <summary>
    /// Repassa os argumentos indicados para as propriedades na instância de IPaper.
    /// </summary>
    /// <param name="paper">A instância de IPaper que será modificada.</param>
    /// <param name="pathArgs">A coleção dos argumentos atribuídos.</param>
    private void SetArgs(IPaper paper, ArgCollection pathArgs)
    {
      foreach (var arg in pathArgs)
      {
        paper._Set(arg.Key, arg.Value);
      }
    }

    /// <summary>
    /// Realiza a consulta de dados specificados no Paper e estoca os dados
    /// obtidos no cache indicado.
    /// </summary>
    /// <param name="paper">A instância de IPaper que contém as consultas a dados.</param>
    /// <param name="cache">O cache para estocagem dos dados consultados.</param>
    private void CacheData(IPaper paper, EntryCollection cache)
    {
      if (paper._Has("GetData"))
      {
        var data = paper._Call("GetData");
        cache.Set(CacheKeys.Data, data);
      }
    }

    /// <summary>
    /// Realiza a consulta de registros specificados no Paper e estoca os registros
    /// obtidos no cache indicado.
    /// </summary>
    /// <param name="paper">A instância de IPaper que contém as consultas a dados.</param>
    /// <param name="cache">O cache para estocagem dos registros consultados.</param>
    private void CacheRows(IPaper paper, EntryCollection cache)
    {
      if (paper._Has("GetRows"))
      {
        var data = paper._Call("GetRows");
        cache.Set(CacheKeys.Rows, data);
      }
    }
  }
}
