using System;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using System.IO;
using System.Xml;
using Lucene.Net.Store;
using Lucene.Net.Analysis.Standard;

namespace Sharpend.Search
{
	/// <summary>
	/// Generic Indexed Document
	/// 
	/// Allows you to index your objects.
	/// The fields for the index must be described in an xml description
	/// 
	/// </summary>
	public class IndexedDocument<T> : IDisposable where T:class
	{
		private IndexWriter writer = null;
		private bool _disposed;

		public String IndexDir {
			get;
			private set;
		}

		public IndexedDocument (String indexDir,bool initializeWriter)
		{
			IndexDir = indexDir;
			if (initializeWriter)
			{
				initWriter();
			}
		}

		public void initWriter()
		{
			writer = new IndexWriter(FSDirectory.Open(IndexDir), new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), true, IndexWriter.MaxFieldLength.LIMITED);
		}

		/// <summary>
		/// Add the specified object to the index
		/// </summary>
		/// <param name='data'>
		/// If set to <c>true</c> data.
		/// </param>
		public bool Add(T data)
		{
			if (_disposed)
			{
            	throw new ObjectDisposedException("Resource was disposed.");
			}

			if (writer == null)
			{
				throw new ArgumentNullException("indexwriter not opened!");
			}

			Document doc = createDocument(data);
			writer.AddDocument(doc);
			return true;
		}

		/// <summary>
		/// Update the specified data in the index
		/// </summary>
		/// <param name='data'>
		/// If set to <c>true</c> data.
		/// </param>
		public bool Update(T data)
		{
			if (_disposed)
			{
            	throw new ObjectDisposedException("Resource was disposed.");
			}

			if (writer == null)
			{
				throw new ArgumentNullException("indexwriter not opened!");
			}

			Document doc = createDocument(data);

			String id = String.Empty;
			String fieldname = String.Empty;

			if (getId(data,out id, out fieldname))
			{
				writer.UpdateDocument(new Lucene.Net.Index.Term(id, fieldname),doc);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Optimize the index
		/// </summary>
		public void Optimize()
		{
			if (_disposed)
			{
            	throw new ObjectDisposedException("Resource was disposed.");
			}

			if (writer == null)
			{
				throw new ArgumentNullException("indexwriter not opened!");
			}

			writer.Optimize();
		}

		private bool getId(object data, out String id, out String fieldname)
		{
			id = String.Empty;
			fieldname = String.Empty;

			String className = typeof(T).ToString();
			String assembly = typeof(T).Assembly.FullName;
			assembly = assembly.Split(',')[0];

			XmlNode nd = Sharpend.Configuration.ConfigurationManager.getValue("searchdescription.xml","//field[(../../@class='"+className+"') and (../../@assembly='"+assembly+"') and (@id='yes')]");

			if (nd != null)
			{
				fieldname = nd.Attributes["name"].Value;
				String datasource = nd.Attributes["datasource"].Value;
				object dt = data.GetType().GetProperty(datasource).GetValue(data,null);
				id = Convert.ToString(dt);

				if (!String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(fieldname))
				{
					return true;
				}
			}
			return false;
		}

		private Document createDocument(T data)
		{
			Document ret = new Document();

			FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile("searchdescription.xml");
			if (! fi.Exists)
			{
				throw new Exception("could not load searchdescription.xml");
			}
			String className = typeof(T).ToString();
			String assembly = typeof(T).Assembly.FullName;
			assembly = assembly.Split(',')[0];
			XmlNodeList lst = Sharpend.Configuration.ConfigurationManager.getValues("searchdescription.xml","//field[(../../@class='"+className+"') and (../../@assembly='"+assembly+"')]");
			foreach (XmlNode nd in lst)
			{
				Field f = FieldDescription.CreateInstance(data, nd);
				ret.Add(f);
			}
			
			return ret;
		}


		#region IDisposable implementation
		public void Dispose ()
		{
			Dispose(true);

			// Use SupressFinalize in case a subclass 
        	// of this type implements a finalizer.
        	// GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
	    {
	        // If you need thread safety, use a lock around these  
	        // operations, as well as in your methods that use the resource. 
	        if (!_disposed)
	        {
	            if (disposing) {
	                if (writer != null)
	                    writer.Dispose();
	                    //Console.WriteLine("Object disposed.");
	            }

	            // Indicate that the instance has been disposed.
	            writer = null;
	            _disposed = true;   
	        }
	    }
		#endregion

	}
}

