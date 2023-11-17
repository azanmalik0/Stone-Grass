using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("coinsStored", "gridOffset", "previousPositions", "currentR", "currentC")]
	public class ES3UserType_MoneyStack : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_MoneyStack() : base(typeof(MoneyStack)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (MoneyStack)obj;
			
			writer.WriteProperty("coinsStored", instance.coinsStored, ES3Type_int.Instance);
			writer.WritePrivateField("gridOffset", instance);
			writer.WriteProperty("previousPositions", instance.previousPositions, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<UnityEngine.Vector3>)));
			writer.WriteProperty("currentR", instance.currentR, ES3Type_int.Instance);
			writer.WriteProperty("currentC", instance.currentC, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (MoneyStack)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "coinsStored":
						instance.coinsStored = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "gridOffset":
					instance = (MoneyStack)reader.SetPrivateField("gridOffset", reader.Read<UnityEngine.Vector3>(), instance);
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


	public class ES3UserType_MoneyStackArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_MoneyStackArray() : base(typeof(MoneyStack[]), ES3UserType_MoneyStack.Instance)
		{
			Instance = this;
		}
	}
}