using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("henhouseLocked_RM", "farmLocked_RM", "barnLocked_RM", "marketLocked_RM")]
	public class ES3UserType_LockedAreasManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_LockedAreasManager() : base(typeof(LockedAreasManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (LockedAreasManager)obj;
			
			writer.WritePrivateField("henhouseLocked_RM", instance);
			writer.WritePrivateField("farmLocked_RM", instance);
			writer.WritePrivateField("barnLocked_RM", instance);
			writer.WritePrivateField("marketLocked_RM", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (LockedAreasManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "henhouseLocked_RM":
					instance = (LockedAreasManager)reader.SetPrivateField("henhouseLocked_RM", reader.Read<System.Int32>(), instance);
					break;
					case "farmLocked_RM":
					instance = (LockedAreasManager)reader.SetPrivateField("farmLocked_RM", reader.Read<System.Int32>(), instance);
					break;
					case "barnLocked_RM":
					instance = (LockedAreasManager)reader.SetPrivateField("barnLocked_RM", reader.Read<System.Int32>(), instance);
					break;
					case "marketLocked_RM":
					instance = (LockedAreasManager)reader.SetPrivateField("marketLocked_RM", reader.Read<System.Int32>(), instance);
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