using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("hayCollected", "gridOffset", "previousPositions", "currentR", "currentC")]
	public class ES3UserType_HayStack : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_HayStack() : base(typeof(HayStack)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (HayStack)obj;
			
			writer.WriteProperty("hayCollected", instance.hayCollected, ES3Type_int.Instance);
			writer.WritePrivateField("gridOffset", instance);
			writer.WriteProperty("previousPositions", instance.previousPositions, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<UnityEngine.Vector3>)));
			writer.WriteProperty("currentR", instance.currentR, ES3Type_int.Instance);
			writer.WriteProperty("currentC", instance.currentC, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (HayStack)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "hayCollected":
						instance.hayCollected = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "gridOffset":
					instance = (HayStack)reader.SetPrivateField("gridOffset", reader.Read<UnityEngine.Vector3>(), instance);
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


	public class ES3UserType_HayStackArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_HayStackArray() : base(typeof(HayStack[]), ES3UserType_HayStack.Instance)
		{
			Instance = this;
		}
	}
}