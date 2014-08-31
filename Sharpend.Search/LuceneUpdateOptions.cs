using System;
using System.Collections.Generic;

namespace Sharpend.Search
{
	public class LuceneUpdateOptions
	{
		private static LuceneUpdateOptions _empty;

		public static LuceneUpdateOptions Empty {
			get {
				if (_empty == null) {
					_empty = new LuceneUpdateOptions();
				}
				return _empty;
			}
		}

		public enum UpdateModes
		{
			Undefined = 0,
			Normal = 1,
			SelectedFields = 2
		}

		public UpdateModes UpdateMode {
			get;
			private set;
		}

		private LuceneUpdateOptions ()
		{
			UpdateMode = UpdateModes.Normal;
			Fields = new List<string> ();
		}

		public LuceneUpdateOptions(UpdateModes updateMode) {
			UpdateMode = updateMode;
			Fields = new List<string> ();
		}

		public List<String> Fields { get; private set;}
	}
}

