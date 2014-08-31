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
using System.Linq;

namespace Sharpend.Search
{
	public class LuceneSearcher<T> : IDisposable where T:class
	{
		protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

		public List<LuceneResult<T>> Search(String[] fields, String querystring)
		{
			//querystring = "\"" + querystring + "\""; 

			log.Debug("start new search for multiple fields " + querystring);
			
			Searcher searcher = new IndexSearcher(reader);
			Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
			
			MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30,fields,analyzer);
			parser.DefaultOperator = QueryParser.Operator.AND;
		
			Query query = parser.Parse(querystring);
			
			AnonymousClassCollector streamingHitCollector = new AnonymousClassCollector(searcher);
			
			searcher.Search(query,streamingHitCollector);
			
			parser = null;
			query = null;
			
			List<LuceneResult<T>> ret = streamingHitCollector.GetSortedResult();
			log.Debug("resultcount: " + ret.Count);
			return ret;
		}


		public List<LuceneResult<T>> Search(String[] fields, String querystring, 
		                                    String datecol, String lowerdate, String uppderdate)
		{
			log.Debug("start new search for multiple fields and range");
			//RangeFilter filter = new RangeFilter(datecol, lowerDate, upperDate, true, true);
			Searcher searcher = new IndexSearcher(reader);
			Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

			String low = lowerdate;
			String up = uppderdate;

			DateTime dt1 = DateTime.MinValue;
			if (DateTime.TryParse (lowerdate,out dt1)) {
				low = Sharpend.Utils.Utils.getDateTimeForIndex(dt1);
			}

			DateTime dt2 = DateTime.MinValue;
			if (DateTime.TryParse (uppderdate,out dt2)) {
				up = Sharpend.Utils.Utils.getDateTimeForIndex(dt2);
			}

			Query query;

			if (String.IsNullOrEmpty (querystring)) {
				query = new TermRangeQuery(datecol,low,up,true,true);
			} else {
				MultiFieldQueryParser parser = new MultiFieldQueryParser (Lucene.Net.Util.Version.LUCENE_30, fields, analyzer);
				parser.DefaultOperator = QueryParser.Operator.AND;
				query = parser.Parse (querystring);
			}

			AnonymousClassCollector streamingHitCollector = new AnonymousClassCollector(searcher);

			if (String.IsNullOrEmpty (querystring)) {
				searcher.Search (query, streamingHitCollector);
			} else {
				TermRangeFilter filter = new TermRangeFilter (datecol, low, up, true, true);
				searcher.Search (query, filter, streamingHitCollector);
			}

			query = null;
			
			List<LuceneResult<T>> ret = streamingHitCollector.GetSortedResult();
			log.Debug("resultcount: " + ret.Count);
			return ret;
		}

		public List<LuceneResult<T>> Search(String field, String querystring)
		{
			log.Debug("start new search for field " + field + " query: " + querystring);

			Searcher searcher = new IndexSearcher(reader);
			Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

			QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30,field,analyzer);
            Query query = parser.Parse(querystring);

			AnonymousClassCollector streamingHitCollector = new AnonymousClassCollector(searcher);

			searcher.Search(query,streamingHitCollector);
            
            parser = null;
            query = null;

			List<LuceneResult<T>> ret = streamingHitCollector.GetSortedResult();
			log.Debug("resultcount: " + ret.Count);
			return ret;
		}

		public IEnumerable<IGrouping<string,LuceneResult<T>>> GroupResult(List<LuceneResult<T>> result,String field)
		{
			return result.GroupBy (item => item.Doc.Get (field));
		}

		public String GetData(LuceneResult<T> result, String field)
		{
			return result.Doc.Get (field);
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

