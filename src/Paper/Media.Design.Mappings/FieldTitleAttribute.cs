﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media.Design.Mappings
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
  public class FieldTitleAttribute : Attribute
  {
    public string Title { get; }

    public FieldTitleAttribute(string title)
    {
      Title = title;
    }
  }
}