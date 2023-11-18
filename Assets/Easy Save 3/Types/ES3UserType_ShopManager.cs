using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("selectedIndex")]
	public class ES3UserType_ShopManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_ShopManager() : base(typeof(ShopManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (ShopManager)obj;
			
			writer.WritePrivateField("selectedIndex", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (ShopManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "selectedIndex":
					instance = (ShopManager)reader.SetPrivateField("selectedIndex", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_ShopManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ShopManagerArray() : base(typeof(ShopManager[]), ES3UserType_ShopManager.Instance)
		{
			Instance = this;
		}
	}
}