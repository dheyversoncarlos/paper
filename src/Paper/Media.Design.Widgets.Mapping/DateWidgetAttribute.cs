﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media.Design.Widgets.Mapping
{
  public class DateWidgetAttribute : WidgetAttribute
  {
    public DateWidgetAttribute()
      : base(DataTypeNames.Date)
    {
    }

    /// <summary>
    /// Ativa ou desativa a múltipla seleção de valores para o campo.
    /// </summary>
    public bool AllowMany { get; set; }

    /// <summary>
    /// Ativa ou desativa o suporte a intervalo, na forma "{ min=x, max=y }".
    /// </summary>
    public bool AllowRange { get; set; }
  }
}