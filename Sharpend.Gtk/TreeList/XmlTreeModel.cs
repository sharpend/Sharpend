//
// XmlTreeModel.cs
//
//  Author:
//       Dirk Lehmeier <sharpend_develop@yahoo.de>
//
//  Copyright (c) 2012 Dirk Lehmeier
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Gtk;
using GLib;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections;

namespace Sharpend.GtkSharp
{
	
	/// <summary>
	/// Xml tree model.
	/// 
	/// this is currently not working under linux ... 
	/// getting some "GCHandle value belongs to a different domain linux" errors
	/// 
	/// </summary>
	/// <exception cref='ArgumentNullException'>
	/// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
	/// </exception>
	/// <exception cref='NotImplementedException'>
	/// Is thrown when a requested operation is not implemented for a given type.
	/// </exception>
	public class XmlTreeModel : GLib.Object, TreeModelImplementor 
	{
		private String xmlfile;
		private XmlDocument xmldoc;
		Hashtable node_hash = new Hashtable ();
		
		public XmlTreeModel (IntPtr raw) : base(raw)
		{
			xmldoc = new XmlDocument();
		}
		
		public XmlTreeModel () : base()
		{
			xmldoc = new XmlDocument();
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="GtkTest.XmlTreeModel"/> class.
		/// Loads the xmldocument with given filename
		/// </summary>
		/// <param name='filename'>
		/// Filename.
		/// </param>
		public XmlTreeModel (String filename)
		{
			xmlfile = filename;
			xmldoc = new XmlDocument();
			xmldoc.Load(xmlfile);
		}

		public void LoadData(String filename)
		{
			xmlfile = filename;
			xmldoc.Load(xmlfile);
		}
			
		/// <summary>
		/// returns iter from given xml node
		/// </summary>
		/// <returns>
		/// The from node.
		/// </returns>
		/// <param name='node'>
		/// Node.
		/// </param>
		TreeIter IterFromNode (object node)
		{
			GCHandle gch;
			if (node_hash [node] != null)
				gch = (GCHandle) node_hash [node];
			else {		
				gch = GCHandle.Alloc (node);
				node_hash[node] = gch;
			}
			
			TreeIter result = TreeIter.Zero;
			result.UserData = (IntPtr) gch;
			return result;
		}
		
		/// <summary>
		/// returns node from given iter
		/// </summary>
		/// <returns>
		/// The from iter.
		/// </returns>
		/// <param name='iter'>
		/// Iter.
		/// </param>
		object NodeFromIter (TreeIter iter)
		{
			if (iter.UserData == IntPtr.Zero)
			{
				return null;
			}
			
			GCHandle gch = (GCHandle) iter.UserData;
			return gch.Target;
		}
		
		/// <summary>
		/// returns the node from given treepath
		/// </summary>
		/// <returns>
		/// The node at path.
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		object GetNodeAtPath (TreePath path)
		{
			if (path.Indices.Length > 0) {			 
				String xpath = "/tree";
				
				for (int i=0;i<path.Indices.Length;i++)
				{
					xpath += "/children/child[position() = "+ (path.Indices[i] + 1) +"]";
				}
								
				XmlNode nd = xmldoc.SelectSingleNode(xpath);
				return nd;			
			} else
				return null;
		}
		
		/// <summary>
		/// childcount of given node
		/// </summary>
		/// <returns>
		/// The count.
		/// </returns>
		/// <param name='node'>
		/// Node.
		/// </param>
		int ChildCount (object node)
		{
			return (node as XmlNode).SelectNodes("./children/child").Count;
		}
		
		
		#region TreeModelImplementor implementation
		
		/// <summary>
		/// TODO GetColumnType description
		/// </summary>
		/// <returns>
		/// The column type.
		/// </returns>
		/// <param name='index_'>
		/// Index_.
		/// </param>
		public GType GetColumnType (int index_)
		{
			return (GType.String);
		}
		
		/// <summary>
		/// Gets the iter fom given path ... returns true if iter is found otherwise false
		/// </summary>
		/// <returns>
		/// The iter.
		/// </returns>
		/// <param name='iter'>
		/// If set to <c>true</c> iter.
		/// </param>
		/// <param name='path'>
		/// If set to <c>true</c> path.
		/// </param>
		/// <exception cref='ArgumentNullException'>
		/// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
		/// </exception>
		public bool GetIter (out TreeIter iter, TreePath path)
		{			
			if (path == null)
				throw new ArgumentNullException ("path");

			iter = TreeIter.Zero;
						
			object node = GetNodeAtPath (path);
			if (node == null)
				return false;

			iter = IterFromNode (node);
			return true;
		}
		
		/// <summary>
		/// TODO GetPath
		/// </summary>
		/// <returns>
		/// The path.
		/// </returns>
		/// <param name='iter'>
		/// Iter.
		/// </param>
		/// <exception cref='NotImplementedException'>
		/// Is thrown when a requested operation is not implemented for a given type.
		/// </exception>
		public TreePath GetPath (TreeIter iter)
		{
			throw new NotImplementedException ();
		}
		
		/// <summary>
		/// returns the nodevalue of given node
		/// </summary>
		/// <param name='iter'>
		/// Iter.
		/// </param>
		/// <param name='column'>
		/// Column.
		/// </param>
		/// <param name='val'>
		/// Value.
		/// </param>
		public void GetValue (TreeIter iter, int column, ref Value val)
		{
			object node = NodeFromIter (iter);
			if (node == null)
				return;
			
			if (node is XmlNode)
			{		
				val = new GLib.Value((node as XmlNode).SelectSingleNode("data").InnerText);
			}
		}
		
		/// <summary>
		/// next sibling
		/// </summary>
		/// <returns>
		/// The next.
		/// </returns>
		/// <param name='iter'>
		/// If set to <c>true</c> iter.
		/// </param>
		public bool IterNext (ref TreeIter iter)
		{		
			object node = NodeFromIter (iter);
			if (node == null)
				return false;
						
			if (node is XmlNode)
			{
				XmlNode nd = (node as XmlNode).NextSibling;
						
				if ((nd != null) && (nd.Name == "child"))
				{			
					iter = IterFromNode(nd);
					return true;
				} else
				{
					return false;
				}
				
			}
			return false;
		}
		
		/// <summary>
		/// first child of given parent iter
		/// </summary>
		/// <returns>
		/// The children.
		/// </returns>
		/// <param name='child'>
		/// If set to <c>true</c> child.
		/// </param>
		/// <param name='parent'>
		/// If set to <c>true</c> parent.
		/// </param>
		public bool IterChildren (out TreeIter child, TreeIter parent)
		{					
			child = TreeIter.Zero;
			object node = NodeFromIter (parent);
			if (node == null || ChildCount (node) <= 0)
				return false;
			
			if (node is XmlNode)
			{
				XmlNode elem = (node as XmlNode).SelectSingleNode("./children/child[1]");
				if (elem != null)
				{		
					child = IterFromNode(elem);
					return true;
				}
			}
			
			return false;
		}
		
		/// <summary>
		/// true if iter has a child
		/// </summary>
		/// <returns>
		/// The has child.
		/// </returns>
		/// <param name='iter'>
		/// If set to <c>true</c> iter.
		/// </param>
		public bool IterHasChild (TreeIter iter)
		{
			object node = NodeFromIter (iter);
			if (node == null || ChildCount (node) <= 0)
					return false;

			return true;
		}
		
		/// <summary>
		/// childcount
		/// </summary>
		/// <returns>
		/// The N children.
		/// </returns>
		/// <param name='iter'>
		/// Iter.
		/// </param>
		/// <exception cref='NotImplementedException'>
		/// Is thrown when a requested operation is not implemented for a given type.
		/// </exception>
		public int IterNChildren (TreeIter iter)
		{
			throw new NotImplementedException ();
		}
		
		/// <summary>
		/// nth child of an iter
		/// </summary>
		/// <returns>
		/// The nth child.
		/// </returns>
		/// <param name='child'>
		/// If set to <c>true</c> child.
		/// </param>
		/// <param name='parent'>
		/// If set to <c>true</c> parent.
		/// </param>
		/// <param name='n'>
		/// If set to <c>true</c> n.
		/// </param>
		public bool IterNthChild (out TreeIter child, TreeIter parent, int n)
		{						
			child = TreeIter.Zero;
			object node = NodeFromIter (parent);
			if (node == null || ChildCount (node) <= 0)
				return false;
			
			if (node is XmlNode)
			{
				XmlNode elem = (node as XmlNode).SelectSingleNode("./children/child["+(n+1)+"]");
				if (elem != null)
				{	
					child = IterFromNode(elem);
					return true;
				}
			}
			
			return false;
		}
		
		/// <summary>
		/// parent iter ... returns true if exists, false otherwise
		/// </summary>
		/// <returns>
		/// The parent.
		/// </returns>
		/// <param name='parent'>
		/// If set to <c>true</c> parent.
		/// </param>
		/// <param name='child'>
		/// If set to <c>true</c> child.
		/// </param>
		public bool IterParent (out TreeIter parent, TreeIter child)
		{					
			parent = TreeIter.Zero;
			
			object node = NodeFromIter (child);
			if (node == null)
				return false;
			
			if (node is XmlNode)
			{
				XmlNode nd = (node as XmlNode);	
				if (nd.ParentNode.ParentNode.Name == "child")
				{
					parent = IterFromNode(nd.ParentNode.ParentNode);	
					return true;
				}
			}	
			return false;
		}

		/// <summary>
		/// TODO RefNode ... do we need this ?
		/// </summary>
		/// <param name='iter'>
		/// Iter.
		/// </param>
		public void RefNode (TreeIter iter)
		{		
		}
		
		/// <summary>
		/// TODO UnrefNode ... do we need this ??
		/// </summary>
		/// <param name='iter'>
		/// Iter.
		/// </param>
		public void UnrefNode (TreeIter iter)
		{
		}
		
		
		public TreeModelFlags Flags {
			get {
				return TreeModelFlags.ItersPersist;
			}
		}
		
		/// <summary>
		/// TODO NColumns description
		/// </summary>
		/// <value>
		/// The N columns.
		/// </value>
		public int NColumns {
			get {
				return 2; //TODO
			}
		}
		#endregion


		#region TreeModelImplementor implementation
		GType TreeModelImplementor.GetColumnType (int index_)
		{
			throw new NotImplementedException ();
		}

		TreePath TreeModelImplementor.GetPath (TreeIter iter)
		{
			throw new NotImplementedException ();
		}

		public bool IterPrevious (ref TreeIter iter)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

