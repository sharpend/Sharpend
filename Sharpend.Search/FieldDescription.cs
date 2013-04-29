using System;
using Lucene.Net.Documents;
using System.Xml;

namespace Sharpend.Search
{
	public static class FieldDescription
	{
		public static Field CreateInstance(object data, XmlNode node)
		{
			String name = node.Attributes["name"].Value;
			String datasource = node.Attributes["datasource"].Value;
			String store = node.Attributes["store"].Value;
			String index = node.Attributes["index"].Value;

			Lucene.Net.Documents.Field.Store st;	
			Enum.TryParse<Lucene.Net.Documents.Field.Store>(store,out st);

			Lucene.Net.Documents.Field.Index idx ;
			Enum.TryParse<Lucene.Net.Documents.Field.Index>(index,out idx);

			String value = getData(data,datasource);
			Field ret = new Field(name,value,st,idx);

			return ret;
		}

		private static String getData(object data, String datasource)
		{
			object dt = data.GetType().GetProperty(datasource).GetValue(data,null);
			return Convert.ToString(dt);
		}
	}
}

