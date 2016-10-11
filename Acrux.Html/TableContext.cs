using System;
using System.Collections.Generic;
using System.Text;
using Acrux.Html.Specialized.Html401;
using System.Diagnostics;
using Acrux.Html.Specialized;

namespace Acrux.Html
{
    internal interface IHtmlParserStatus
    {
        HtmlDocument Document { get; }
        HtmlNode CurrentNode { get; set; }
        HtmlNode LastAddedElement { get; set; }
        Stack<HtmlNode> CurrentNodeStack { get; }
    }

    internal enum TablePosition
    {
        NotInTable,
        InTable,
        InTbody,
        InTfoot,
        InThead,
        InTableRow,
        InTableCell,
    }

    internal class TableContext
    {
        private Stack<TableContext> m_TableContextStack = new Stack<TableContext>();
        private IHtmlParserStatus m_Parser = null;

        private HtmlNode m_TableNode;
        private HtmlNode m_TableCellNode;
        private HtmlNode m_TableRowNode;
        private HtmlNode m_CurrTableBlockNode; /* the current TBODY, THEAD or TFOOT */

        private TablePosition m_Position;

        internal TablePosition Position
        {
            get { return Current.m_Position; }
        }

        internal HtmlNode TableNode
        {
            get { return Current.m_TableNode; }
        }

        internal HtmlNode TableCellNode
        {
            get { return Current.m_TableCellNode; }
        }


        internal HtmlNode TableRowNode
        {
            get { return Current.m_TableRowNode; }
        }

        private TableContext Current
        {
            get
            {
                return m_TableContextStack.Peek();
            }
        }

        internal TableContext()
        {
            Reset(null);
        }

        internal void Reset(IHtmlParserStatus parser)
        {
            m_Position = TablePosition.NotInTable;
            m_TableNode = null;
            m_TableCellNode = null;
            m_TableRowNode = null;
            m_CurrTableBlockNode = null;

            m_Parser = parser;
            m_TableContextStack.Clear();
            m_TableContextStack.Push(this);
        }

        internal void EnterTable(HtmlNode tableNode)
        {
            m_TableContextStack.Push(new TableContext());

            Current.m_Position = TablePosition.InTable;
            Current.m_TableNode = tableNode;
        }

        internal void ExitTable()
        {
            if (m_TableContextStack.Count > 0)
                m_TableContextStack.Pop();

            if (m_TableContextStack.Count == 0)
                Reset(m_Parser);
        }

        internal void EnterTableRow(HtmlNode tableRowNode)
        {
            EnsureTable();
            EnsureTableBlock();

            Current.m_Position = TablePosition.InTableRow;
            Current.m_TableRowNode = tableRowNode;
        }

        internal void ExitTableRow()
        {
            Current.m_Position = TablePosition.InTbody;
            Current.m_TableRowNode = null;
            Current.m_TableCellNode = null;
        }

        internal void EnterTableCell(HtmlNode tableCellNode)
        {
            EnsureTable();
            EnsureTableRowIfNeeded();

            Current.m_Position = TablePosition.InTableCell;
            Current.m_TableCellNode = tableCellNode;
        }

        internal void ExitTableCell()
        {
            Current.m_Position = TablePosition.InTableRow;
            Current.m_TableCellNode = null;
        }

        internal void EnterTableInnerBlock(HtmlNode tableBodyNode)
        {
            Current.m_CurrTableBlockNode = tableBodyNode;

            if ("tfoot" == tableBodyNode.Name)
                Current.m_Position = TablePosition.InTfoot;
            else if ("thead" == tableBodyNode.Name)
                Current.m_Position = TablePosition.InThead;
            else
            {
                Debug.Assert("tbody" == tableBodyNode.Name);
                Current.m_Position = TablePosition.InTbody;
            }
        }

        internal void ExitTableInnerBlock()
        {
            Current.m_Position = TablePosition.InTable;

            Current.m_CurrTableBlockNode = null;
            Current.m_TableRowNode = null;
            Current.m_TableCellNode = null;
        }

        internal bool AddTextNodeOutsideTable(HtmlTextElement textNode)
        {
            if (textNode != null &&
                Current.m_TableNode != null &&
                Current.m_TableNode.ParentNode != null)
            {
                Current.m_TableNode.ParentNode.InsertBefore(textNode, Current.m_TableNode);
                
                // NOTE: The parser current node and stack will not change
                //       we simply insert the text node before the table

                return true;
            }

            return false;
        }

        private void EnsureTable()
        {
            if (Current.Position == TablePosition.NotInTable)
            {
                Table newTable = new Table(null, "table", null, m_Parser.Document, false, NodePosition.ReadOnly);

                AddToParent(m_Parser.CurrentNode, newTable);
                EnterTable(newTable);
            }
        }

        private void EnsureTableRowIfNeeded()
        {
            if (Current.Position == TablePosition.InTableRow)
                // We are already in a row
                return;

            if (Current.Position == TablePosition.InTable)
            {
                TableBody tbody = new TableBody(null, "tbody", null, m_Parser.Document, false, NodePosition.ReadOnly);

                AddToParent(m_TableNode, tbody);
                EnterTableInnerBlock(tbody);
            }

            if (Current.Position == TablePosition.InTbody)
            {
                TableRow tr = new TableRow(null, "tr", null, m_Parser.Document, false, NodePosition.ReadOnly);

                AddToParent(m_CurrTableBlockNode, tr);
                EnterTableRow(tr);
            }
            else if (
                Current.Position == TablePosition.InTfoot ||
                Current.Position == TablePosition.InThead)
            {
                // NOTE: Firefox doesn't add TR for TDs inside THEAD and TFOOT
            }
            else if (Current.Position == TablePosition.InTableCell)
            {
                Debug.Assert(m_Parser.CurrentNodeStack.Count > 3);

                HtmlNode currCell = m_Parser.CurrentNodeStack.Pop();
                if (currCell == null ||
                    "td" != currCell.Name)
                {
                    // We shouldn't be here. Something is wrong
                    Debug.Assert(false);
                    throw new InvalidOperationException("Cannot ensure a table row because the current state is invalid.");
                }

                m_Parser.CurrentNode = m_Parser.CurrentNodeStack.Peek();

                Debug.Assert(m_Parser.CurrentNode != null);

                if (m_Parser.CurrentNode == null ||
                    "tr" != m_Parser.CurrentNode.Name)
                {
                    // We shouldn't be here. Something is wrong
                    Debug.Assert(false);
                    throw new InvalidOperationException("Cannot ensure a table row because the current state is invalid.");
                }
                else
                {
                    EnterTableRow(m_Parser.CurrentNode);
                }
            }
            else
            {
                // We shouldn't be here. Something is wrong
                Debug.Assert(false);
                throw new InvalidOperationException("Cannot ensure a table row because the current state is invalid.");
            }
        }

        private void EnsureTableBlock()
        {
            if (Current.Position == TablePosition.InTbody ||
                Current.Position == TablePosition.InTfoot ||
                Current.Position == TablePosition.InThead)
                // We are already in a table block
                return;

            if (Current.Position == TablePosition.InTable)
            {
                TableBody tbody = new TableBody(null, "tbody", null, m_Parser.Document, false, NodePosition.ReadOnly);

                AddToParent(m_TableNode, tbody);
                EnterTableInnerBlock(tbody);
            }
            else if (Current.Position == TablePosition.InTableRow)
            {
                Debug.Assert(m_Parser.CurrentNodeStack.Count > 2);

                HtmlNode currCell = m_Parser.CurrentNodeStack.Pop();
                if (currCell == null ||
                    "tr" != currCell.Name)
                {
                    // We shouldn't be here. Something is wrong
                    Debug.Assert(false);
                    throw new InvalidOperationException("Cannot ensure a table block because the current state is invalid.");
                }

                m_Parser.CurrentNode = m_Parser.CurrentNodeStack.Peek();


                Debug.Assert(m_Parser.CurrentNode != null);

                if (m_Parser.CurrentNode == null ||
                    "tbody" != m_Parser.CurrentNode.Name ||
                    "thead" != m_Parser.CurrentNode.Name ||
                    "tfoot" != m_Parser.CurrentNode.Name)
                {
                    // We shouldn't be here. Something is wrong
                    Debug.Assert(false);
                    throw new InvalidOperationException("Cannot ensure a table block because the current state is invalid.");                    
                }
                else
                {
                    EnterTableInnerBlock(m_Parser.CurrentNode);
                }
            }
            else if (Current.Position == TablePosition.InTableCell)
            {
                // TODO: just exit the current cell and row?? Test with <table><tr><td> a <tr> b <td> c </td> d </tr> c </td></tr></table>
            }
            else
            {
                // We shouldn't be here. Something is wrong
                Debug.Assert(false);
                throw new InvalidOperationException("Cannot ensure a table row because the current state is invalid.");
            }
        }

        private void AddToParent(HtmlNode parent, HtmlNode child)
        {
            Debug.Assert(m_Parser != null);
            Debug.Assert(m_Parser.CurrentNode != null);
            Debug.Assert(m_Parser.CurrentNode is Table);
            Debug.Assert(m_Parser.CurrentNode.Equals(parent));
            Debug.Assert(m_Parser.CurrentNodeStack != null);

            HtmlNode parentNode = m_Parser.CurrentNodeStack.Peek();

            Debug.Assert(parentNode != null);
            Debug.Assert(m_Parser.CurrentNode.Equals(parentNode));

            parentNode.AppendChild(child);
            m_Parser.CurrentNodeStack.Push(child);
            m_Parser.CurrentNode = child;
            m_Parser.LastAddedElement = child;
        }
    }
}
