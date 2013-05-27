using System;
using Lucene.Net.Documents;
using System.Xml;

namespace Sharpend.Search
{
	public static class FieldDescription
	{
		public static Field CreateInstance(object data, XmlNode node)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}

			if (node == null)
			{
				throw new ArgumentNullException("node");
			}

			String name = node.Attributes["name"].Value.ToLower();
			String datasource = node.Attributes["datasource"].Value;
			String store = node.Attributes["store"].Value;
			String index = node.Attributes["index"].Value;

			Lucene.Net.Documents.Field.Store st;	
			Enum.TryParse<Lucene.Net.Documents.Field.Store>(store,out st);

			Lucene.Net.Documents.Field.Index idx ;
			Enum.TryParse<Lucene.Net.Documents.Field.Index>(index,out idx);

			String value = getData(data,datasource);

			//name = name.ToLower(); 
			Field ret = new Field(name,value,st,idx);

			return ret;
		} 

		private static String GetDateTime(DateTime input)
		{
			return Sharpend.Utils.Utils.getDateTimeForIndex(input);
		}

		private static String getData(object data, String datasource)
		{
			try
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}

				if (datasource == null)
				{
					throw new ArgumentNullException("datasource");
				}

				object dt = data.GetType().GetProperty(datasource).GetValue(data,null);

				if (dt != null)
				{
					if (dt is DateTime)
					{
						return GetDateTime((DateTime)dt);
					}

					return Convert.ToString(dt);
				}

				return String.Empty;
			} catch
			{
				Console.WriteLine("error in getData: " + datasource);
				throw;
			}
		}
	}
}

