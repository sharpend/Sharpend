using System;
using Lucene.Net.Documents;
using System.Xml;
using System.Reflection;

namespace Sharpend.Search
{
	public static class FieldDescription
	{
		public static String GetFieldName(XmlNode node)
		{
			return node.Attributes["name"].Value.ToLower();
		}

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

        private static PropertyInfo GetFirstProperty(object source, String name)
        {
			if (source == null) {
				throw new ArgumentNullException("source");
			}

            foreach (PropertyInfo pi in source.GetType().GetProperties())
            {
                if (pi.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return pi;
                }
            }

            return null;
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

                String[] lst = datasource.Split('[');
                String index = String.Empty;
                if (lst.Length == 2)
                {
                    datasource = lst[0];
                    index = lst[1].Substring(0, lst[1].Length - 1);
                }

                PropertyInfo pi = data.GetType().GetProperty(datasource);                
                object dt = null;

                if (!String.IsNullOrEmpty(index))
                {
                    object source = pi.GetValue(data, null);
                    if (source != null)
					{
						PropertyInfo ii = GetFirstProperty(source, "Item");
	                    dt = ii.GetValue(source, new object[] { index });
					} else {
						throw new Exception("could not load value");
					}
                }
                else
                {
                    dt = pi.GetValue(data, null);
                }

                if (dt != null)
                {
                    if (dt is DateTime)
                    {
                        return GetDateTime((DateTime)dt);
                    }

                    return Convert.ToString(dt);
                }
                
				return String.Empty;
			} catch (Exception ex)
			{
				Console.WriteLine("error in getData: " + datasource);
                Console.WriteLine(ex.ToString());
				throw;
			}
		}
	}
}

