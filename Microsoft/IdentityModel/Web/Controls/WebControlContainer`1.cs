// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Controls.WebControlContainer`1
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Microsoft.IdentityModel.Web.Controls
{
  internal abstract class WebControlContainer<TWebControl> : WebControl where TWebControl : WebControl
  {
    internal const string DesignerRegionAttributeName = "_designerRegion";
    private const string _templateDesignerRegion = "0";
    private bool _renderDesignerRegion;
    private TWebControl _owner;
    private bool _ownerDesignMode;
    private Table _layoutTable;
    private Table _borderTable;

    public WebControlContainer(TWebControl owner, bool ownerDesignMode)
    {
      this._owner = owner;
      this._ownerDesignMode = ownerDesignMode;
    }

    internal Table BorderTable
    {
      get => this._borderTable;
      set => this._borderTable = value;
    }

    protected abstract bool ConvertingToTemplate { get; }

    internal Table LayoutTable
    {
      get => this._layoutTable;
      set => this._layoutTable = value;
    }

    internal TWebControl Owner => this._owner;

    internal bool RenderDesignerRegion
    {
      get => this._ownerDesignMode && this._renderDesignerRegion;
      set => this._renderDesignerRegion = value;
    }

    private bool UsingDefaultTemplate => this.BorderTable != null;

    public override sealed void Focus() => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID5014", (object) this.GetType().Name)));

    protected override void Render(HtmlTextWriter writer)
    {
      if (this.UsingDefaultTemplate)
      {
        if (!this.ConvertingToTemplate)
        {
          this.BorderTable.CopyBaseAttributes((WebControl) this);
          if (this.ControlStyleCreated)
          {
            ControlUtil.CopyBorderStyles((WebControl) this.BorderTable, this.ControlStyle);
            ControlUtil.CopyStyleToInnerControl((WebControl) this.LayoutTable, this.ControlStyle);
          }
        }
        this.LayoutTable.Height = this.Height;
        this.LayoutTable.Width = this.Width;
        this.RenderContents(writer);
      }
      else
        this.RenderContentsInUnitTable(writer);
    }

    private void RenderContentsInUnitTable(HtmlTextWriter writer)
    {
      WebControlContainer<TWebControl>.LayoutHelperTable layoutHelperTable = new WebControlContainer<TWebControl>.LayoutHelperTable(1, 1, this.Page);
      if (this.RenderDesignerRegion)
      {
        layoutHelperTable[0, 0].Attributes["_designerRegion"] = "0";
      }
      else
      {
        foreach (Control control in this.Controls)
          layoutHelperTable[0, 0].Controls.Add(control);
      }
      switch (this.Parent.ID)
      {
        case "":
        case null:
          layoutHelperTable.CopyBaseAttributes((WebControl) this);
          layoutHelperTable.ApplyStyle(this.ControlStyle);
          layoutHelperTable.CellPadding = 0;
          layoutHelperTable.CellSpacing = 0;
          layoutHelperTable.RenderControl(writer);
          break;
        default:
          layoutHelperTable.ID = this.Parent.ClientID;
          goto case "";
      }
    }

    [SupportsEventValidation]
    private class LayoutHelperTable : Table
    {
      public LayoutHelperTable(int rows, int columns, Page page)
      {
        if (rows <= 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (rows));
        if (columns <= 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (columns));
        if (page != null)
          this.Page = page;
        for (int index1 = 0; index1 < rows; ++index1)
        {
          TableRow row = new TableRow();
          this.Rows.Add(row);
          for (int index2 = 0; index2 < columns; ++index2)
          {
            TableCell cell = (TableCell) new WebControlContainer<TWebControl>.LayoutHelperTable.LayoutTableCell();
            row.Cells.Add(cell);
          }
        }
      }

      public TableCell this[int row, int column] => this.Rows[row].Cells[column];

      private class LayoutTableCell : TableCell
      {
        protected override void AddedControl(Control control, int index)
        {
          if (control.Page != null)
            return;
          control.Page = this.Page;
        }

        protected override void RemovedControl(Control control)
        {
        }
      }
    }
  }
}
