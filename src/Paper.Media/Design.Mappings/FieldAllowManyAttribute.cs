﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;
using System.Reflection;
using Paper.Media.Rendering_Obsolete;
using Paper.Media.Routing;

namespace Paper.Media.Design.Mappings
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class FieldAllowManyAttribute : FieldAttribute
  {
    public bool Allow { get; }

    public FieldAllowManyAttribute(bool allow = true)
    {
      Allow = allow;
    }

    internal override void RenderField(Field field, PropertyInfo property, object host, PaperContext ctx)
    {
      field.AddAllowMany(Allow);
    }

    internal override void RenderField(Field field, PropertyInfo property, object host)
    {
      field.AddAllowMany(Allow);
    }
  }
}