using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute()]
	public class ES3UserType_PositionPainter : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_PositionPainter() : base(typeof(PT.Garden.PositionPainter)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (PT.Garden.PositionPainter)obj;
			
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (PT.Garden.PositionPainter)obj;
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


	public class ES3UserType_PositionPainterArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_PositionPainterArray() : base(typeof(PT.Garden.PositionPainter[]), ES3UserType_PositionPainter.Instance)
		{
			Instance = this;
		}
	}
}