﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;
using Paper.Media.Design.Papers;
using Paper.Media.Design.Papers.Rendering;
using System.Reflection;

namespace Paper.Media.Design.Mappings
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
  public class FieldProviderAttribute : FieldAttribute
  {
    public string Href { get; }
    public Type ProviderType { get; }
    public string[] Keys { get; }

    public FieldProviderAttribute(string href, params string[] keys)
    {
      Href = href;
      Keys = keys;
    }

    public FieldProviderAttribute(Type providerType, params string[] keys)
    {
      ProviderType = providerType;
      Keys = keys;
    }

    internal override void RenderField(Field field, PropertyInfo property, object host, PaperContext ctx)
    {
      if (ProviderType != null)
      {
        // TODO: implementar...
        throw new NotImplementedException();
      }
      else if (Href != null)
      {
        field.AddProvider(Href, Keys);
      }
    }
  }
}