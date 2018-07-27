﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Paper.Media.Design;

namespace Paper.Media.Design.Papers
{
  public interface IPaperRows : IPaper
  {
    Page Page { get; }

    Sort Sort { get; }

    IFilter Filter { get; }

    string GetTitle();

    DataTable GetRows();

    IEnumerable<HeaderInfo> GetRowHeaders(DataTable rows);

    IEnumerable<ILink> GetRowLinks(DataRow row);
  }
}