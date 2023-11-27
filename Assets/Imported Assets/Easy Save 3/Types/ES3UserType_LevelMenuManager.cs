using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute()]
	public class ES3UserType_LevelMenuManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_LevelMenuManager() : base(typeof(LevelMenuManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (LevelMenuManager)obj;
			
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (LevelMenuManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_LevelMenuManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_LevelMenuManagerArray() : base(typeof(LevelMenuManager[]), ES3UserType_LevelMenuManager.Instance)
		{
			Instance = this;
		}
	}
}