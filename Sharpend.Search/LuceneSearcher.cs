using System;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.Analysis;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using System.Collections.Generic;
using Lucene.Net.Documents;
using System.Xml;
using System.IO;
using System.Reflection;

namespace Sharpend.Search
{
	/// <summary>
	/// Lucene result.
	/// </summary>
	public class LuceneResult<T> where T:class
	{
		public Document Doc {
			get;
			private set;	
		}

		public float Score {
			get;
			private set;
		}

		public LuceneResult(Document d,float score)
		{
			Doc = d;
			Score = score;
		}

//		private T GetData()
//		{
//			FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile("searchdescription.xml");
//			if (! fi.Exists)
//			{
//				throw new Exception("could not load searchdescription.xml");
//			}
//			
//			String className = typeof(T).ToString();
//			String assembly = typeof(T).Assembly.FullName;
//			assembly = assembly.Split(',')[0];
//			XmlNodeList lst = Sharpend.Configuration.ConfigurationManager.getValues("searchdescription.xml","//field[(../../@class='"+className+"') and (../../@assembly='"+assembly+"')]");
//			foreach (XmlNode nd in lst)
//			{
//				Field f = FieldDescription.CreateInstance(data, nd);
//				ret.Add(f);
//			}
//			
//			return ret;
//		}

		private Type[] GetTypes()
		{
			FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile("searchdescription.xml");
			if (! fi.Exists)
			{
				throw new Exception("could not load searchdescription.xml");
			}
			
			String className = typeof(T).ToString();
			String assembly = typeof(T).Assembly.FullName;
			assembly = assembly.Split(',')[0];
			XmlNodeList lst = Sharpend.Configuration.ConfigurationManager.getValues("searchdescription.xml","//field[(../../@class='"+className+"') and (../../@assembly='"+assembly+"') and (@constructor='yes')]");

			Type[] ret = new Type[lst.Count];
			int i=0;
			foreach (XmlNode nd in lst)
			{
				if (nd.Attributes["type"] != null)
				{
					throw new NotImplementedException("type");
				} else
				{
					ret[i] = typeof(String);
				}
				i++;
			}
			
			return ret;
		}

		private object[] GetValues()
		{
			FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile("searchdescription.xml");
			if (! fi.Exists)
			{
				throw new Exception("could not load searchdescription.xml");
			}
			
			String className = typeof(T).ToString();
			String assembly = typeof(T).Assembly.FullName;
			assembly = assembly.Split(',')[0];
			XmlNodeList lst = Sharpend.Configuration.ConfigurationManager.getValues("searchdescription.xml","//field[(../../@class='"+className+"') and (../../@assembly='"+assembly+"') and (@constructor='yes')]");
			
			object[] ret = new object[lst.Count];
			int i=0;
			foreach (XmlNode nd in lst)
			{
				String name = nd.Attributes["name"].Value;
				ret[i] = Doc.Get(name);
				i++;
			}

			return ret;
		}

		public T GetData()
		{
			Type tp = typeof(T);

			Type[] types = GetTypes();
			object[] values = GetValues();

			//try a CreateInstance Methode
			MethodInfo mi = tp.GetMethod("CreateInstance",types);
			if (mi != null)
			{
				T dt = (T)mi.Invoke(null,values);
				return dt;
			}

			//if not we try a constructor
			ConstructorInfo ci = tp.GetConstructor(types);
			T data = (T)ci.Invoke(values);

			if (data != null)
			{
				return data;
			}
			return null;
		}
	}

	public class ScoreComparer<T> : IComparer<LuceneResult<T>> where T:class
	{
		#region IComparer implementation
		public int Compare (LuceneResult<T> x, LuceneResult<T> y)
		{
			if (x.Score == y.Score)
			{
				return 0;
			}

			if (x.Score < y.Score)
			{
				return 1;
			} else
			{
				return -1;
			}
		}
		#endregion
	}

	public class LuceneSearcher<T> : IDisposable where T:class
	{
		private class AnonymousClassCollector : Collector 
		{
			private Scorer scorer;
			private int docBase;
			public Searcher SearchSearcher { get; protected set; }
			public List<LuceneResult<T>> Result { get; protected set; }

			public AnonymousClassCollector(Searcher searcher)
				:base()
			{
				SearchSearcher = searcher;
				Result = new List<LuceneResult<T>>(10000);
			}

			public List<LuceneResult<T>> GetSortedResult()
			{
				Result.Sort(new ScoreComparer<T>());
				return Result;
			}

			// simply print docId and score of every matching document
			public override void Collect(int doc)
			{
				//Console.Out.WriteLine("doc=" + doc + docBase + " score=" + scorer.Score());
				Result.Add(new LuceneResult<T>(SearchSearcher.Doc(doc),scorer.Score()));
			}
			
			public override bool AcceptsDocsOutOfOrder
			{
                get { return true; }
			}
			
			public override void SetNextReader(IndexReader reader, int docBase)
			{
				this.docBase = docBase;
			}
			
			public override void  SetScorer(Scorer scorer)
			{
				this.scorer = scorer;
			}
		}


		private IndexReader reader;
		private bool _disposed = false;

		public String IndexDir {
			get;
			private set;
		}

		public LuceneSearcher (String indexDir,bool initializeReader)
		{
			IndexDir = indexDir;
			if (initializeReader)
			{
				initReader();
			}
		}

		public List<LuceneResult<T>> Search(String field, String querystring)
		{
			Searcher searcher = new IndexSearcher(reader);
			Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

			QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30,field,analyzer);
            Query query = parser.Parse(querystring);

			AnonymousClassCollector streamingHitCollector = new AnonymousClassCollector(searcher);

			searcher.Search(query,streamingHitCollector);
            
            parser = null;
            query = null;

			return streamingHitCollector.GetSortedResult();
		}


		public void initReader()
		{
			reader = IndexReader.Open(FSDirectory.Open(new System.IO.DirectoryInfo(IndexDir)), true); // only searching, so read-only=true
		}




		#region IDisposable implementation
		public void Dispose ()
		{
			Dispose(true);

			// Use SupressFinalize in case a subclass 
        	// of this type implements a finalizer.
        	GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
	    {
	        // If you need thread safety, use a lock around these  
	        // operations, as well as in your methods that use the resource. 
	        if (!_disposed)
	        {
	            if (disposing) {
	                if (reader != null)
	                    reader.Dispose();
	                    //Console.WriteLine("Object disposed.");
	            }

	            // Indicate that the instance has been disposed.
	            reader = null;
	            _disposed = true;   
	        }
	    }
		#endregion


	}
}
