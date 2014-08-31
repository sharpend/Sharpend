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
	public interface ILuceneResult<T> where T:class
	{
		float Score {get;}
		T GetData();
	}

	/// <summary>
	/// Lucene result.
	/// </summary>
	
}
