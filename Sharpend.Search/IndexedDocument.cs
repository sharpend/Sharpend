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
		protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private IndexWriter writer = null;
		private bool _disposed;

		private bool verboseMode = false;
		public bool VerboseMode {
			get
			{
				return verboseMode;
			}
			set
			{
				verboseMode = value;
			}
		}

		public String IndexDir {
			get;
			private set;
		}

		public bool CreateIndex {
			get;
			private set;
		}

		public IndexedDocument (String indexDir,bool initializeWriter,bool createIndex)
		{
			IndexDir = indexDir;
			CreateIndex = createIndex;
			if (initializeWriter)
			{
				initWriter();
			}
		}

		public void initWriter()
		{
			writer = new IndexWriter(FSDirectory.Open(IndexDir), new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), CreateIndex, IndexWriter.MaxFieldLength.LIMITED);
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

			try
			{
				Document doc = createDocument(data);
				writer.AddDocument(doc);
				return true;
			} catch (Exception ex)
			{
				log.Error(ex);
				throw;
			}
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
				if (!String.IsNullOrEmpty(id))
				{
					log.Debug("update document with id: " + id + " idfield: " + fieldname);
					writer.UpdateDocument(new Lucene.Net.Index.Term(fieldname,id),doc);
					//writer.UpdateDocument(new Lucene.Net.Index.Term(
					return true;
				} else
				{
					log.Warn("Update: could not find id");
				}
			}
			return false;
		}

		public bool Remove(T data)
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
				if (!String.IsNullOrEmpty(id))
				{
					log.Debug("update document with id: " + id + " idfield: " + fieldname);
					//writer.UpdateDocument(new Lucene.Net.Index.Term(fieldname,id),doc);
					writer.DeleteDocuments(new Lucene.Net.Index.Term(fieldname,id));
					//writer.UpdateDocument(new Lucene.Net.Index.Term(
					return true;
				} else
				{
					log.Warn("Update: could not find id");
				}
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

			if (nd != null) {
				fieldname = nd.Attributes ["name"].Value.ToLower ();
				String datasource = nd.Attributes ["datasource"].Value;
				object dt = data.GetType ().GetProperty (datasource).GetValue (data, null);
				id = Convert.ToString (dt);

				if (!String.IsNullOrEmpty (id) && !String.IsNullOrEmpty (fieldname)) {
					return true;
				}
			} else {
				throw new Exception("could not load searchdescription.xml");
			}
			return false;
		}

		private Document createDocument(T data)
		{
			try
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

				log.Debug("create new document for class " + className + " assembly " + assembly);

				XmlNodeList lst = Sharpend.Configuration.ConfigurationManager.getValues("searchdescription.xml","//field[(../../@class='"+className+"') and (../../@assembly='"+assembly+"')]");

				if (lst.Count == 0)
				{
					log.Warn("there are no matching fields specified for this class: " + className  + " assembly" + assembly );
				}

				foreach (XmlNode nd in lst)
				{
					Field f = FieldDescription.CreateInstance(data, nd);
					if (VerboseMode)
					{
						log.Debug("add new field: Name: " + f.Name  +" ToString: " + f.ToString());
					}
					ret.Add(f);
				}
				
				return ret;
			} catch (Exception ex)
			{
				log.Error(ex);
				throw;
			}
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

