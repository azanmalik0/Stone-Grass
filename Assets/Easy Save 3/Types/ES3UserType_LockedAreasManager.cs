using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("henhouseLocked_CR", "farmLocked_CR", "barnLocked_CR", "marketLocked_CR")]
	public class ES3UserType_LockedAreasManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_LockedAreasManager() : base(typeof(LockedAreasManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (LockedAreasManager)obj;
			
			writer.WritePrivateField("henhouseLocked_CR", instance);
			writer.WritePrivateField("farmLocked_CR", instance);
			writer.WritePrivateField("barnLocked_CR", instance);
			writer.WritePrivateField("marketLocked_CR", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (LockedAreasManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "henhouseLocked_CR":
					instance = (LockedAreasManager)reader.SetPrivateField("henhouseLocked_CR", reader.Read<System.Int32>(), instance);
					break;
					case "farmLocked_CR":
					instance = (LockedAreasManager)reader.SetPrivateField("farmLocked_CR", reader.Read<System.Int32>(), instance);
					break;
					case "barnLocked_CR":
					instance = (LockedAreasManager)reader.SetPrivateField("barnLocked_CR", reader.Read<System.Int32>(), instance);
					break;
					case "marketLocked_CR":
					instance = (LockedAreasManager)reader.SetPrivateField("marketLocked_CR", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_LockedAreasManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_LockedAreasManagerArray() : base(typeof(LockedAreasManager[]), ES3UserType_LockedAreasManager.Instance)
		{
			Instance = this;
		}
	}
}