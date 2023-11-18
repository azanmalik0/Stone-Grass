using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("coins")]
	public class ES3UserType_CurrencyManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_CurrencyManager() : base(typeof(CurrencyManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (CurrencyManager)obj;
			
			writer.WriteProperty("coins", CurrencyManager.coins, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (CurrencyManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "coins":
						CurrencyManager.coins = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_CurrencyManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_CurrencyManagerArray() : base(typeof(CurrencyManager[]), ES3UserType_CurrencyManager.Instance)
		{
			Instance = this;
		}
	}
}