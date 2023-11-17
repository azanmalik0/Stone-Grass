using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("maxChickenTray_CR", "maxCowTray_CR", "cowNumbers_CR", "chickenNumbers_CR", "maxStorageCapacity_CR", "maxFarmerCapacity_CR")]
	public class ES3UserType_FarmUpgradeManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_FarmUpgradeManager() : base(typeof(FarmUpgradeManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (FarmUpgradeManager)obj;
			
			writer.WriteProperty("maxChickenTray_CR", instance.maxChickenTray_CR, ES3Type_int.Instance);
			writer.WriteProperty("maxCowTray_CR", instance.maxCowTray_CR, ES3Type_int.Instance);
			writer.WriteProperty("cowNumbers_CR", instance.cowNumbers_CR, ES3Type_int.Instance);
			writer.WriteProperty("chickenNumbers_CR", instance.chickenNumbers_CR, ES3Type_int.Instance);
			writer.WriteProperty("maxStorageCapacity_CR", instance.maxStorageCapacity_CR, ES3Type_int.Instance);
			writer.WriteProperty("maxFarmerCapacity_CR", instance.maxFarmerCapacity_CR, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (FarmUpgradeManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "maxChickenTray_CR":
						instance.maxChickenTray_CR = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "maxCowTray_CR":
						instance.maxCowTray_CR = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "cowNumbers_CR":
						instance.cowNumbers_CR = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "chickenNumbers_CR":
						instance.chickenNumbers_CR = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "maxStorageCapacity_CR":
						instance.maxStorageCapacity_CR = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "maxFarmerCapacity_CR":
						instance.maxFarmerCapacity_CR = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_FarmUpgradeManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_FarmUpgradeManagerArray() : base(typeof(FarmUpgradeManager[]), ES3UserType_FarmUpgradeManager.Instance)
		{
			Instance = this;
		}
	}
}