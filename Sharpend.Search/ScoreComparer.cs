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
	/// <summary>
	/// Lucene result.
	/// </summary>

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
	
}
