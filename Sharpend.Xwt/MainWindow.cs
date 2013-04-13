using System;
using Xwt;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using Sharpend.Logging;

namespace Sharpend.Xwt
{
	public class MainWindow : Window, IXmlSerializable
	{
		protected static MainWindow instance;
		
		/// <summary>
		/// Gets the location x.
		/// </summary>
		/// <value>
		/// The location x.
		/// </value>
		public int LocationX {
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the location y.
		/// </summary>
		/// <value>
		/// The location y.
		/// </value>
		public int LocationY
		{
			get;
			private set;
		}
		
		/// <summary>
		/// width, stored in xml
		/// </summary>
		/// <value>
		/// The width of the stored.
		/// </value>
		public int StoredWidth {
			get;
			private set;
		}
		
		/// <summary>
		/// height, stored in xml
		/// </summary>
		/// <value>
		/// The height of the stored.
		/// </value>
		public int StoredHeight {
			get;
			private set;
		}
		
		public String StoredTitle {
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the name of the current assembly.
		/// </summary>
		/// <value>
		/// The name of the current assembly.
		/// </value>
		public String CurrentAssemblyName {
			get
			{
				String name = this.GetType().Assembly.FullName;
				String[] l = name.Split(',');
				return l[0];
			}
		}
		
		public MainWindow ()
		{
		}
		
		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns>
		/// The instance.
		/// </returns>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public static T CreateInstance<T>() where T:MainWindow
		{
			if (instance == null)
			{
				Type t = typeof(T);
				instance = (T)Activator.CreateInstance(t);
				instance.init();
			}
			
			return (instance as T);
		}
		
		/// <summary>
		/// Restores the instance from xml
		/// </summary>
		/// <returns>
		/// The instance.
		/// </returns>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public static T RestoreInstance<T>() where T:class
		{
			try
			{
				String name = typeof(T).Assembly.FullName;
				String[] l = name.Split(',');
				String fn = l[0] + ".xml";
				
				if (File.Exists(fn))
				{
					XmlSerializer xs = new XmlSerializer(typeof(T));
					using (XmlReader reader = XmlReader.Create(fn))
					{							
						var wnd = xs.Deserialize(reader);				
						instance = (MainWindow)wnd;
						instance.init();
						return (T)wnd;
					}
				}
			} catch (Exception ex)
			{
				Logger.Log(LogLevel.Exception,ex.ToString());
			}
			return null;
		}
		
		protected virtual void init()
		{
			double x = Convert.ToDouble(LocationX);
			double y = Convert.ToDouble(LocationY);
			this.Location = new Point(x,y);
			
			this.Width = (int)StoredWidth;
			this.Height = (int)StoredHeight;
			this.Title = StoredTitle;
		}		
		
		protected override bool OnCloseRequested ()
		{
			Save();
			return base.OnCloseRequested ();
		}
		
		public void Save()
		{
			String fn = CurrentAssemblyName + ".xml";
			using (XmlWriter xw = XmlWriter.Create(fn))
			{
				XmlSerializer xs =  new XmlSerializer(this.GetType());
				xs.Serialize(xw,this);
			}
			//Console.WriteLine("Save() done");
		}
		
		protected virtual void doReadXml(XmlReader reader)
		{
			LocationX = Convert.ToInt32(reader["LocationX"]);
			LocationY = Convert.ToInt32(reader["LocationY"]);
			StoredHeight = Convert.ToInt32(reader["Height"]);
			StoredWidth = Convert.ToInt32(reader["Width"]);
			StoredTitle = reader["Title"];
		}
		
		protected virtual void doWriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("Title",BackendHost.Backend.Title);
			
			
			int wd = (int)BackendHost.Backend.Bounds.Width;
			int hg = (int)BackendHost.Backend.Bounds.Height;
			int x = (int)BackendHost.Backend.Bounds.Location.X;
			int y = (int)BackendHost.Backend.Bounds.Location.Y;
			
			writer.WriteAttributeString("LocationX",x.ToString());
			writer.WriteAttributeString("LocationY",y.ToString());
			writer.WriteAttributeString("Width",wd.ToString());
			writer.WriteAttributeString("Height",hg.ToString());
		}
		
		#region IXmlSerializable implementation
		public System.Xml.Schema.XmlSchema GetSchema ()
		{
			return null;
		}
		
		public void ReadXml (XmlReader reader)
		{
			if (reader.LocalName == this.GetType().Name)
			{
				reader.Read();
			}
			
			doReadXml(reader);
		}
		
		public void WriteXml (XmlWriter writer)
		{
			writer.WriteStartElement(this.GetType().ToString());
			doWriteXml(writer);
			writer.WriteEndElement();
		}
		#endregion
		
		
	}
}

