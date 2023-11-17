using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("feedStored", "gridOffset", "previousPositions", "currentR", "currentC")]
	public class ES3UserType_HayLoft : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_HayLoft() : base(typeof(HayLoft)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (HayLoft)obj;
			
			writer.WriteProperty("feedStored", instance.feedStored, ES3Type_int.Instance);
			writer.WritePrivateField("gridOffset", instance);
			writer.WriteProperty("previousPositions", instance.previousPositions, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<UnityEngine.Vector3>)));
			writer.WriteProperty("currentR", instance.currentR, ES3Type_int.Instance);
			writer.WriteProperty("currentC", instance.currentC, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (HayLoft)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "feedStored":
						instance.feedStored = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "gridOffset":
					instance = (HayLoft)reader.SetPrivateField("gridOffset", reader.Read<UnityEngine.Vector3>(), instance);
					break;
					case "previousPositions":
						instance.previousPositions = reader.Read<System.Collections.Generic.List<UnityEngine.Vector3>>();
						break;
					case "currentR":
						instance.currentR = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "currentC":
						instance.currentC = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_HayLoftArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_HayLoftArray() : base(typeof(HayLoft[]), ES3UserType_HayLoft.Instance)
		{
			Instance = this;
		}
	}
}