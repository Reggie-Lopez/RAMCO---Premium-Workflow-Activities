// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Diagnostics.TraceXPathNavigator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Microsoft.IdentityModel.Diagnostics
{
  [DebuggerDisplay("TraceXPathNavigator")]
  internal class TraceXPathNavigator : XPathNavigator
  {
    private bool _closed;
    private TraceXPathNavigator.TraceNode _current;
    private TraceXPathNavigator.ElementNode _root;
    private XPathNodeType _state = XPathNodeType.Element;

    public void AddAttribute(string name, string value, string xmlns, string prefix)
    {
      if (this._closed || this.CurrentElement == null)
        return;
      this.CurrentElement.Attributes.Add(new TraceXPathNavigator.AttributeNode(name, prefix, value, xmlns, this.CurrentElement));
    }

    public void AddComment(string text)
    {
      if (this._closed || this.CurrentElement == null)
        return;
      this.CurrentElement.Add((TraceXPathNavigator.TraceNode) new TraceXPathNavigator.CommentNode(text, this.CurrentElement));
    }

    public void AddElement(string prefix, string name, string xmlns)
    {
      if (this._closed)
        return;
      TraceXPathNavigator.ElementNode elementNode = new TraceXPathNavigator.ElementNode(name, prefix, this.CurrentElement, xmlns);
      if (this.CurrentElement == null)
      {
        this._root = elementNode;
        this._current = (TraceXPathNavigator.TraceNode) this._root;
      }
      else
      {
        if (this._closed)
          return;
        this.CurrentElement.Add((TraceXPathNavigator.TraceNode) elementNode);
        this._current = (TraceXPathNavigator.TraceNode) elementNode;
      }
    }

    public void AddProcessingInstruction(string name, string text)
    {
      if (this.CurrentElement == null)
        return;
      this.CurrentElement.Add((TraceXPathNavigator.TraceNode) new TraceXPathNavigator.ProcessingInstructionNode(name, text, this.CurrentElement));
    }

    public void AddText(string value)
    {
      if (this._closed || this.CurrentElement == null)
        return;
      if (this.CurrentElement.TextNode == null)
      {
        this.CurrentElement.TextNode = new TraceXPathNavigator.TextNode(value, this.CurrentElement);
      }
      else
      {
        if (string.IsNullOrEmpty(value))
          return;
        this.CurrentElement.TextNode.AddText(value);
      }
    }

    public override string BaseURI => string.Empty;

    public void CloseElement()
    {
      if (this._closed)
        return;
      this._current = (TraceXPathNavigator.TraceNode) this.CurrentElement.Parent;
      if (this._current != null)
        return;
      this._closed = true;
    }

    public override XPathNavigator Clone() => (XPathNavigator) this;

    private TraceXPathNavigator.CommentNode CurrentComment => this._current as TraceXPathNavigator.CommentNode;

    private TraceXPathNavigator.ElementNode CurrentElement => this._current as TraceXPathNavigator.ElementNode;

    private TraceXPathNavigator.ProcessingInstructionNode CurrentProcessingInstruction => this._current as TraceXPathNavigator.ProcessingInstructionNode;

    [DebuggerDisplay("")]
    public override string LocalName => this.Name;

    public override string LookupPrefix(string ns) => this.LookupPrefix(ns, this.CurrentElement);

    private string LookupPrefix(string ns, TraceXPathNavigator.ElementNode node)
    {
      string str = (string) null;
      if (string.Compare(ns, node.NameSpace, StringComparison.Ordinal) == 0)
      {
        str = node.Prefix;
      }
      else
      {
        foreach (TraceXPathNavigator.AttributeNode attribute in node.Attributes)
        {
          if (string.Compare("xmlns", attribute.Prefix, StringComparison.Ordinal) == 0 && string.Compare(ns, attribute.NodeValue, StringComparison.Ordinal) == 0)
          {
            str = attribute.Name;
            break;
          }
        }
      }
      if (string.IsNullOrEmpty(str) && node.Parent != null)
        str = this.LookupPrefix(ns, node.Parent);
      return str;
    }

    public override bool IsEmptyElement
    {
      get
      {
        bool flag = true;
        if (this._current != null)
          flag = this.CurrentElement.TextNode != null || this.CurrentElement.ChildNodes.Count > 0;
        return flag;
      }
    }

    public override bool IsSamePosition(XPathNavigator other) => false;

    public override bool MoveTo(XPathNavigator other) => false;

    public override bool MoveToFirstAttribute()
    {
      if (this.CurrentElement == null)
        return false;
      bool firstAttribute = this.CurrentElement.MoveToFirstAttribute();
      if (firstAttribute)
        this._state = XPathNodeType.Attribute;
      return firstAttribute;
    }

    public override bool MoveToFirstChild()
    {
      if (this.CurrentElement == null)
        return false;
      bool flag = false;
      if (this.CurrentElement.ChildNodes != null && this.CurrentElement.ChildNodes.Count > 0)
      {
        this._current = this.CurrentElement.ChildNodes[0];
        this._state = this._current.NodeType;
        flag = true;
      }
      else if ((this.CurrentElement.ChildNodes == null || this.CurrentElement.ChildNodes.Count == 0) && this.CurrentElement.TextNode != null)
      {
        this._state = XPathNodeType.Text;
        this.CurrentElement.MovedToText = true;
        flag = true;
      }
      return flag;
    }

    public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope) => false;

    public override bool MoveToId(string id) => false;

    public override bool MoveToNext()
    {
      if (this.CurrentElement == null)
        return false;
      bool flag = false;
      if (this._state != XPathNodeType.Text)
      {
        TraceXPathNavigator.ElementNode parent = this.CurrentElement.Parent;
        if (parent != null)
        {
          TraceXPathNavigator.TraceNode next = parent.MoveToNext();
          if (next == null && parent.TextNode != null && !parent.MovedToText)
          {
            this._state = XPathNodeType.Text;
            parent.MovedToText = true;
            this._current = (TraceXPathNavigator.TraceNode) parent;
            flag = true;
          }
          else if (next != null)
          {
            this._state = next.NodeType;
            flag = true;
            this._current = next;
          }
        }
      }
      return flag;
    }

    public override bool MoveToNextAttribute()
    {
      if (this.CurrentElement == null)
        return false;
      bool nextAttribute = this.CurrentElement.MoveToNextAttribute();
      if (nextAttribute)
        this._state = XPathNodeType.Attribute;
      return nextAttribute;
    }

    public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope) => false;

    public override bool MoveToParent()
    {
      if (this.CurrentElement == null)
        return false;
      bool flag = false;
      switch (this._state)
      {
        case XPathNodeType.Element:
        case XPathNodeType.ProcessingInstruction:
        case XPathNodeType.Comment:
          if (this._current.Parent != null)
          {
            this._current = (TraceXPathNavigator.TraceNode) this._current.Parent;
            this._state = this._current.NodeType;
            flag = true;
            break;
          }
          break;
        case XPathNodeType.Attribute:
          this._state = XPathNodeType.Element;
          flag = true;
          break;
        case XPathNodeType.Namespace:
          this._state = XPathNodeType.Element;
          flag = true;
          break;
        case XPathNodeType.Text:
          this._state = XPathNodeType.Element;
          flag = true;
          break;
      }
      return flag;
    }

    public override bool MoveToPrevious() => false;

    public override void MoveToRoot()
    {
      this._current = (TraceXPathNavigator.TraceNode) this._root;
      this._state = XPathNodeType.Element;
      this._root.Reset();
    }

    [DebuggerDisplay("")]
    public override string Name
    {
      get
      {
        if (this.CurrentElement != null)
        {
          switch (this._state)
          {
            case XPathNodeType.Element:
              return this.CurrentElement.Name;
            case XPathNodeType.Attribute:
              return this.CurrentElement.CurrentAttribute.Name;
            case XPathNodeType.ProcessingInstruction:
              return this.CurrentProcessingInstruction.Name;
          }
        }
        return string.Empty;
      }
    }

    [DebuggerDisplay("")]
    public override string NamespaceURI
    {
      get
      {
        if (this.CurrentElement != null)
        {
          switch (this._state)
          {
            case XPathNodeType.Element:
              return this.CurrentElement.NameSpace;
            case XPathNodeType.Attribute:
              return this.CurrentElement.CurrentAttribute.NameSpace;
            case XPathNodeType.Namespace:
              return (string) null;
          }
        }
        return string.Empty;
      }
    }

    public override XmlNameTable NameTable => (XmlNameTable) null;

    [DebuggerDisplay("")]
    public override XPathNodeType NodeType => this._state;

    [DebuggerDisplay("")]
    public override string Prefix
    {
      get
      {
        string str = string.Empty;
        if (this.CurrentElement != null)
        {
          switch (this._state)
          {
            case XPathNodeType.Element:
              str = this.CurrentElement.Prefix;
              break;
            case XPathNodeType.Attribute:
              str = this.CurrentElement.CurrentAttribute.Prefix;
              break;
            case XPathNodeType.Namespace:
              str = (string) null;
              break;
          }
        }
        return str;
      }
    }

    public override string ToString()
    {
      this.MoveToRoot();
      StringBuilder sb = new StringBuilder();
      new XmlTextWriter((TextWriter) new StringWriter(sb, (IFormatProvider) CultureInfo.CurrentCulture)).WriteNode((XPathNavigator) this, false);
      return sb.ToString();
    }

    [DebuggerDisplay("")]
    public override string Value
    {
      get
      {
        if (this.CurrentElement != null)
        {
          switch (this._state)
          {
            case XPathNodeType.Attribute:
              return this.CurrentElement.CurrentAttribute.NodeValue;
            case XPathNodeType.Text:
              return this.CurrentElement.TextNode.NodeValue;
            case XPathNodeType.ProcessingInstruction:
              return this.CurrentProcessingInstruction.NodeValue;
            case XPathNodeType.Comment:
              return this.CurrentComment.NodeValue;
          }
        }
        return string.Empty;
      }
    }

    public WriteState WriteState
    {
      get
      {
        WriteState writeState = WriteState.Error;
        if (this.CurrentElement == null)
          writeState = WriteState.Start;
        else if (this._closed)
        {
          writeState = WriteState.Closed;
        }
        else
        {
          switch (this._state)
          {
            case XPathNodeType.Element:
              writeState = WriteState.Element;
              break;
            case XPathNodeType.Attribute:
              writeState = WriteState.Attribute;
              break;
            case XPathNodeType.Text:
              writeState = WriteState.Content;
              break;
            case XPathNodeType.Comment:
              writeState = WriteState.Content;
              break;
          }
        }
        return writeState;
      }
    }

    public abstract class TraceNode
    {
      private XPathNodeType _nodeType;
      private TraceXPathNavigator.ElementNode _parent;

      public TraceNode(XPathNodeType nodeType, TraceXPathNavigator.ElementNode parent)
      {
        this._nodeType = nodeType;
        this._parent = parent;
      }

      public XPathNodeType NodeType => this._nodeType;

      public abstract string NodeValue { get; }

      public TraceXPathNavigator.ElementNode Parent => this._parent;

      public abstract int Size { get; }
    }

    public class AttributeNode : TraceXPathNavigator.TraceNode
    {
      private string _name;
      private string _nodeValue;
      private string _prefix;
      private string _xmlns;

      public AttributeNode(
        string name,
        string prefix,
        string nodeValue,
        string xmlns,
        TraceXPathNavigator.ElementNode parent)
        : base(XPathNodeType.Attribute, parent)
      {
        this._name = name;
        this._nodeValue = nodeValue;
        this._prefix = prefix;
        this._xmlns = xmlns;
      }

      public string Name => this._name;

      public string NameSpace => this._xmlns;

      public override string NodeValue => this._nodeValue;

      public string Prefix => this._prefix;

      public override int Size
      {
        get
        {
          int num = this._name.Length + this._nodeValue.Length + 5;
          if (!string.IsNullOrEmpty(this._prefix))
            num += this._prefix.Length + 1;
          if (!string.IsNullOrEmpty(this._xmlns))
            num += this._xmlns.Length + 9;
          return num;
        }
      }
    }

    public class CommentNode : TraceXPathNavigator.TraceNode
    {
      public string _nodeValue;

      public CommentNode(string text, TraceXPathNavigator.ElementNode parent)
        : base(XPathNodeType.Comment, parent)
        => this._nodeValue = text;

      public override string NodeValue => this._nodeValue;

      public override int Size => this._nodeValue.Length + 8;
    }

    public class ElementNode : TraceXPathNavigator.TraceNode
    {
      private int _attributeIndex;
      private int _elementIndex;
      private string _name;
      private string _prefix;
      private string _xmlns;
      private List<TraceXPathNavigator.TraceNode> _childNodes = new List<TraceXPathNavigator.TraceNode>();
      private List<TraceXPathNavigator.AttributeNode> _attributes = new List<TraceXPathNavigator.AttributeNode>();
      private TraceXPathNavigator.TextNode _textNode;
      private bool _movedToText;

      public ElementNode(
        string name,
        string prefix,
        TraceXPathNavigator.ElementNode parent,
        string xmlns)
        : base(XPathNodeType.Element, parent)
      {
        this._name = name;
        this._prefix = prefix;
        this._xmlns = xmlns;
      }

      public void Add(TraceXPathNavigator.TraceNode node) => this._childNodes.Add(node);

      public List<TraceXPathNavigator.AttributeNode> Attributes => this._attributes;

      public List<TraceXPathNavigator.TraceNode> ChildNodes => this._childNodes;

      public TraceXPathNavigator.AttributeNode CurrentAttribute => this._attributes[this._attributeIndex];

      public IEnumerable<TraceXPathNavigator.ElementNode> FindSubnodes(
        string[] headersPath)
      {
        if (headersPath != null)
        {
          TraceXPathNavigator.ElementNode node = this;
          if (string.CompareOrdinal(node._name, headersPath[0]) != 0)
            node = (TraceXPathNavigator.ElementNode) null;
          TraceXPathNavigator.ElementNode subNode;
          for (int i = 0; node != null && ++i < headersPath.Length; node = subNode)
          {
            subNode = (TraceXPathNavigator.ElementNode) null;
            if (node._childNodes != null)
            {
              foreach (TraceXPathNavigator.TraceNode childNode1 in node._childNodes)
              {
                if (childNode1.NodeType == XPathNodeType.Element)
                {
                  TraceXPathNavigator.ElementNode childNode = childNode1 as TraceXPathNavigator.ElementNode;
                  if (childNode != null && string.CompareOrdinal(childNode._name, headersPath[i]) == 0)
                  {
                    if (headersPath.Length == i + 1)
                    {
                      yield return childNode;
                    }
                    else
                    {
                      subNode = childNode;
                      break;
                    }
                  }
                }
              }
            }
          }
        }
      }

      public bool MoveToFirstAttribute()
      {
        this._attributeIndex = 0;
        return this._attributes != null && this._attributes.Count > 0;
      }

      public TraceXPathNavigator.TraceNode MoveToNext()
      {
        TraceXPathNavigator.TraceNode traceNode = (TraceXPathNavigator.TraceNode) null;
        if (this._elementIndex + 1 < this._childNodes.Count)
        {
          ++this._elementIndex;
          traceNode = this._childNodes[this._elementIndex];
        }
        return traceNode;
      }

      public bool MoveToNextAttribute()
      {
        bool flag = false;
        if (this._attributeIndex + 1 < this._attributes.Count)
        {
          ++this._attributeIndex;
          flag = true;
        }
        return flag;
      }

      public bool MovedToText
      {
        get => this._movedToText;
        set => this._movedToText = value;
      }

      public string Name => this._name;

      public string NameSpace => this._xmlns;

      public override string NodeValue => string.Empty;

      public string Prefix => this._prefix;

      public void Reset()
      {
        this._attributeIndex = 0;
        this._elementIndex = 0;
        this._movedToText = false;
        if (this._childNodes == null)
          return;
        foreach (TraceXPathNavigator.TraceNode childNode in this._childNodes)
        {
          if (childNode.NodeType == XPathNodeType.Element && childNode is TraceXPathNavigator.ElementNode elementNode)
            elementNode.Reset();
        }
      }

      public override int Size
      {
        get
        {
          int num = 2 * this._name.Length + 6;
          if (!string.IsNullOrEmpty(this._prefix))
            num += this._prefix.Length + 1;
          if (!string.IsNullOrEmpty(this._xmlns))
            num += this._xmlns.Length + 9;
          return num;
        }
      }

      public TraceXPathNavigator.TextNode TextNode
      {
        get => this._textNode;
        set => this._textNode = value;
      }
    }

    public class ProcessingInstructionNode : TraceXPathNavigator.TraceNode
    {
      private string _name;
      private string _nodeValue;

      public ProcessingInstructionNode(
        string name,
        string text,
        TraceXPathNavigator.ElementNode parent)
        : base(XPathNodeType.ProcessingInstruction, parent)
      {
        this._name = name;
        this._nodeValue = text;
      }

      public string Name => this._name;

      public override string NodeValue => this._nodeValue;

      public override int Size => this._name.Length + this._nodeValue.Length + 12;
    }

    public class TextNode : TraceXPathNavigator.TraceNode
    {
      private string _nodeValue;

      public TextNode(string nodeValue, TraceXPathNavigator.ElementNode parent)
        : base(XPathNodeType.Text, parent)
        => this._nodeValue = nodeValue;

      public void AddText(string text) => this._nodeValue += text;

      public override string NodeValue => this._nodeValue;

      public override int Size => this._nodeValue.Length;
    }
  }
}
