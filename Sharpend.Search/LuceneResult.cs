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
	public class LuceneResult<T> : ILuceneResult<T> where T:class
	{
		protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

		private Type[] GetTypes(Type nominalType)
		{
			FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile("searchdescription.xml");
			if (! fi.Exists)
			{
				throw new Exception("could not load searchdescription.xml");
			}
			
			String className = nominalType.ToString();
			String assembly = nominalType.Assembly.FullName;
			assembly = assembly.Split(',')[0];
			XmlNodeList lst = Sharpend.Configuration.ConfigurationManager.getValues("searchdescription.xml","//field[(../../@class='"+className+"') and (../../@assembly='"+assembly+"') and (@constructor='yes')]");

			if (lst.Count == 0) 
			{
				throw new Exception("no fields specified in searchdescription.xml");
			}

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

		private object[] GetValues(Type nominalType)
		{
			FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile("searchdescription.xml");
			if (! fi.Exists)
			{
				throw new Exception("could not load searchdescription.xml");
			}
			
			String className = nominalType.ToString();
			String assembly = nominalType.Assembly.FullName;
			assembly = assembly.Split(',')[0];
			XmlNodeList lst = Sharpend.Configuration.ConfigurationManager.getValues("searchdescription.xml","//field[(../../@class='"+className+"') and (../../@assembly='"+assembly+"') and (@constructor='yes')]");
			
			object[] ret = new object[lst.Count];
			int i=0;
			foreach (XmlNode nd in lst)
			{
				String name = nd.Attributes["name"].Value.ToLower();
				ret[i] = Doc.Get(name);
				i++;
			}

			return ret;
		}

		public T GetData()
		{
			Type tp = typeof(T);

			//try a CreateInstance Methode
			String nominalType = Doc.Get ("nominaltype");
			if (!String.IsNullOrWhiteSpace (nominalType)) {
				tp = Type.GetType(nominalType);
				if (tp == null)
				{
					throw new Exception("could not find nominaltype" + nominalType);
				}
			}

			Type[] types = GetTypes(tp);
			object[] values = GetValues(tp);

			MethodInfo mi = tp.GetMethod("CreateInstance",types);

            if (mi == null)
            {
                mi = tp.GetMethod("CreateInstance", BindingFlags.Static);
            }

			if (mi != null)
			{
				T dt = (T)mi.Invoke(null,values);
				return dt;
			} else
			{
				String tps = String.Empty;
				foreach (Type t in types)
				{
					tps += t.ToString() + ",";
				}
				log.Warn("could not find CreateInstance for " + tps);
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
	
}
