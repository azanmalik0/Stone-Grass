using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute()]
	public class ES3UserType_GrassPatchActivator : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_GrassPatchActivator() : base(typeof(PT.Garden.GrassPatchActivator)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (PT.Garden.GrassPatchActivator)obj;
			
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (PT.Garden.GrassPatchActivator)obj;
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


	public class ES3UserType_GrassPatchActivatorArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_GrassPatchActivatorArray() : base(typeof(PT.Garden.GrassPatchActivator[]), ES3UserType_GrassPatchActivator.Instance)
		{
			Instance = this;
		}
	}
}