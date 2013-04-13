using System;
using Xwt;
//using Sharpend;
using Sharpend.Utils;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;

namespace Sharpend.Xwt
{

	/// <summary>
	/// Xwt tree list.
	/// 
	/// Wrapper around a Xwt Tree and a Xwt Datastore
	/// 
	/// </summary>
	public class XwtTreeList : Sharpend.Utils.VirtualTreeList
	{

		/// Container for Datafields
		private class DatafieldContainer
		{
			public IDataField Field {
				get;
				private set;
			}

			public String Typename {
				get;
				private set;
			}

			public String Title {
				get;
				private set;
			}

			public DatafieldContainer(IDataField field, String title, String typename)
			{
				Field = field;
				Title = title;
				Typename = typename;
			}
		}

		private DatafieldContainer[] Fields {
			get;
			set;
		}

		public TreeView Tree {
			get;
			private set;
		}

		protected TreeStore DataStore {
			get;
			private set;
		}

		public XwtTreeList (TreeView tree) : base()
		{
			Tree = tree;
		}

		private Dictionary<TreeNavigator,VirtualGridRow> navigators;

		/// <summary>
		/// Initializes a new instance of the <see cref="Sharpend.Xwt.XwtTreeList"/> class.
		/// </summary>
		/// <param name='tree'>
		/// Tree.
		/// </param>
		/// <param name='xml'>
		/// xml definition for the grid or the resource name for the xml
		/// </param>
		/// <param name='isResource'>
		/// if true param xml is a resourcename
		/// </param>
		public XwtTreeList (TreeView tree,String xml, bool isResource=true) : this(tree)
		{
			Fields = createDataFields(xml,isResource);
			navigators = new Dictionary<TreeNavigator, VirtualGridRow>(100);
			//create the treestore
			IDataField[] df = new IDataField[Fields.Length+1];
			for (int i=0;i<Fields.Length;i++)
			{
				df[i] = Fields[i].Field;			
			}
			df[Fields.Length] = new DataField<VirtualTreeRow>();
			DataStore = new TreeStore(df);

			//add columns to the tree
			//foreach (DatafieldContainer dc in Fields)
			for (int i=0;i<(Fields.Length-1);i++)
			{
				DatafieldContainer dc = Fields[i];
				Tree.Columns.Add(dc.Title,dc.Field);
				//tree.
				//Tree.Columns.Add(itm);
				//Tree.ButtonPressed += HandleButtonPressed;
			}


			Tree.DataSource = DataStore;
		}

		protected void SetValue(TreeNavigator navigator,String fieldname, object value)
		{
			foreach (DatafieldContainer df in Fields)
			{
				if (df.Title.Equals(fieldname,StringComparison.OrdinalIgnoreCase))
				{
					SetValue(navigator,new object[] {df.Field, value }, df.Typename);
					return;
				}
			}
		}

		protected object GetValue(TreeNavigator navigator,String fieldname)
		{
			if (navigator.CurrentPosition == null)
			{
				return null;
			}

			foreach (DatafieldContainer df in Fields)
			{
				if (df.Title.Equals(fieldname,StringComparison.OrdinalIgnoreCase))
				{
					return GetValue(navigator,df.Field,df.Typename);
				}
			}
			return null;
		}


//		void HandleButtonPressed (object sender, ButtonEventArgs e)
//		{
//			if (e.MultiplePress == 2)
//			{
//				TreeView tv = sender as TreeView;
//
//				TreePosition tp = tv.SelectedRow;
//				TreeNavigator tn = DataStore.GetNavigatorAt (tp);
//				//SetValue(tn,new object[] { true },"System.Boolean");
//
//				//SetValue(tn,
//
//				SetValue(tn,new object[] {Fields[2].Field,true},"System.Boolean");
//
//
//				//Tree.Columns[0].Views[0]
//			}
//		
//		}


		public override void afterSetData (VirtualGridRow row, string columnName, object data)
		{
			base.afterSetData (row, columnName, data);

			if (data == null)
			{
				throw new Exception("data is null");
			}

			TreeNavigator tn = GetNavigator(row);
			if (tn != null)
			{
				SetValue(tn,columnName,data);
			}
		}

		public override Sharpend.Utils.VirtualGridRow newRow ()
		{
			return base.newRow ();
		}

		protected override void addRow (Sharpend.Utils.VirtualGridRow row)
		{
			base.addRow (row);
		}

		public override void setData (Sharpend.Utils.VirtualGridRow row, string columnName, string data)
		{	
			base.setData (row, columnName, data);
		}

		/// <summary>
		/// create a xwt datafield
		/// </summary>
		/// <returns>
		/// The data field.
		/// </returns>
		/// <param name='typename'>
		/// Typename.
		/// </param>
		private static IDataField createDataField(String typename)
		{
			Type t = Type.GetType(typename);
			Type dfType = typeof(DataField<>);

			Type gt = dfType.MakeGenericType(new Type[] { t });
			object df = Activator.CreateInstance(gt,new object[] {});
			return (df as IDataField);
		}

		/// <summary>
		/// set a value of a xwt treenavigator
		/// </summary>
		/// <returns>
		/// The value.
		/// </returns>
		/// <param name='navigator'>
		/// Navigator.
		/// </param>
		/// <param name='data'>
		/// Data.
		/// </param>
		/// <param name='typenmae'>
		/// Typenmae.
		/// </param>
		private TreeNavigator SetValue(TreeNavigator navigator, object[] data, String typenmae)
		{
			//DataStore.AddNode().SetValue
			MethodInfo method = typeof(TreeNavigator).GetMethod("SetValue");
			MethodInfo generic = method.MakeGenericMethod(Type.GetType(typenmae));

			return (generic.Invoke(navigator,data) as TreeNavigator);
		}

		private object GetValue(TreeNavigator navigator, IDataField datafield, String typenmae)
		{
			//DataStore.AddNode().SetValue
			MethodInfo method = typeof(TreeNavigator).GetMethod("GetValue");
			MethodInfo generic = method.MakeGenericMethod(Type.GetType(typenmae));

			return (generic.Invoke(navigator,new object[] {datafield}));
		}

		/// <summary>
		/// creates a xwt datastore
		/// </summary>
		/// <returns>
		/// The data store.
		/// </returns>
		private TreeStore CreateDataStore()
		{
			IDataField[] fields = new IDataField[HeaderColumns.Count];

			int i=0;
			foreach (VirtualGridHeaderColumn col in HeaderColumns)
			{
				String tp = "String";

				if (!String.IsNullOrEmpty(col.ColumnType))
				{
					tp = col.ColumnType;
				}

				fields[i] = createDataField(tp);
				i++;
			}

			return new TreeStore(fields);
		}

		private static XmlDocument getXmlDocument(String xml, bool fromResource)
		{
			String xmlStr = xml;
			if (fromResource)
			{
				xmlStr = Sharpend.Utils.Utils.getResourceString(xml);
			}

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xmlStr);
			return doc;
		}

		private DatafieldContainer[] createDataFields(String xml, bool fromResource)
		{
			XmlDocument doc = getXmlDocument(xml,fromResource);
			XmlNodeList lst = doc.SelectNodes("//field");
			DatafieldContainer[] ret = new DatafieldContainer[lst.Count + 1];
			int idx=0;
			foreach (XmlNode nd in lst)
			{
				String typename = nd.Attributes["type"].Value;
				IDataField df = createDataField(typename);
				String title = nd.Attributes["title"].Value;
				ret[idx] = new DatafieldContainer(df,title,typename);

				VirtualGridHeaderColumn c = new VirtualGridHeaderColumn(this,title);
				addHeaderColumn(c);

				idx++;
			}
			//VirtualGridRow
			ret[idx] = new DatafieldContainer(createDataField("Sharpend.Utils.VirtualGridRow,Sharpend"),"row","Sharpend.Utils.VirtualGridRow,Sharpend");
			return ret;
		}

		public VirtualGridRow GetSelectedRow()
		{
			TreePosition tp = Tree.SelectedRow;
			TreeNavigator tn = DataStore.GetNavigatorAt(tp);

			object o = GetValue(tn,"row");
			return (VirtualGridRow)o;
		}

		private TreeNavigator GetNavigator(VirtualGridRow row,TreeNavigator parent=null)
		{	
			TreeNavigator first = null;
			if (parent != null)
			{
				first = parent;
			} else
			{
				first = DataStore.GetFirstNode();
			}

			if (first.CurrentPosition != null)
			{
				do
				{
					VirtualGridRow o = (VirtualGridRow)GetValue(first,"row");

					if (o == row)
					{
						return first;
					}

					if (first.MoveToChild())
					{
						TreeNavigator tn = GetNavigator(row,first);
						if (tn != null)
						{
							return tn;
						}
						first.MoveToParent();
					}

				} while(first.MoveNext());
			}

			return null;
		}

		private TreeNavigator setData(TreeNavigator tn, object[] data)
		{
			TreeNavigator ret = null;

			int i=0;
			foreach (DatafieldContainer dc in Fields)
			{
				ret = SetValue(tn,new object[] {dc.Field,data[i]},dc.Typename);
				i++;
			}

			return ret;
		}

//		public VirtualGridRow GetSelectedGridRow()
//		{
//			TreePosition tp = Tree.SelectedRow;
//
//			foreach (VirtualGridRow row in Rows)
//			{
//				TreeNavigator tn = DataStore.GetNavigatorAt (tp);
//				//DataStore.
//				//VirtualGridRow rw = (VirtualGridRow)GetValue(tn,"");
//				//tn.GetValue(
//			}
//			return null;
//		}


		public void addData(TreeNavigator navigator, VirtualTreeRow row)
		{
			if (navigator == null)
			{
				throw new ArgumentNullException("navigator");
			}

			if (row == null)
			{
				throw new ArgumentNullException("row");
			}

			//add children
			if ((row.Children != null) && (row.Children.Count > 0))
			{
				TreeNavigator current = navigator;
				int idx = 0;
				foreach (KeyValuePair<String,VirtualTreeRow> kp in row.Children)
				{
					if (idx > 0)
					{
						current.InsertAfter();
					}

					setData(current,kp.Value.Data);

					if (kp.Value.Children.Count > 0)
					{
						addData(current.AddChild(),kp.Value);
						current.MoveToParent();
					} else
					{
						if (kp.Value.Rows.Count > 0)
						{
							addData(current.AddChild(),kp.Value);
							current.MoveToParent();
						}
					}

					idx++;
				}
			} 

			//if no children available add the assigned rows
			if ((row.Children == null) || (row.Children.Count < 1))
			{
				int idx = 0;
				foreach (VirtualGridRow rw in row.Rows)
				{
					if (idx > 0)
					{
						navigator.InsertAfter();
					}

					setData(navigator,rw.Datas);

					idx++;
				}
			}
		}

		/// <summary>
		/// Reloads the datastore.
		/// </summary>
		public void reloadDatastore()
		{
			DataStore.Clear();
			if ((Root != null) && (Root.RootRow != null))
			{			
				Root.RootRow.clearRows();
                Root.RootRow.clearChildren();
				Root.RootRow.clearChache(true);
			}

			if (Root != null)
			{
				TreeNavigator tn = DataStore.AddNode();
				addData(tn,Root.RootRow);
			} else
			{
				foreach (VirtualGridRow row in Rows)
				{
					TreeNavigator tn = DataStore.AddNode();
					setData(tn,row.Datas);
				}
			}
		}

	}
}

