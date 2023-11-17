using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("currentSawBlades", "currentWheels", "SawBlades_CR", "rotationSpeed_CR", "wheels_CR", "maxCarCapacity_CR")]
	public class ES3UserType_TruckUpgradeManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_TruckUpgradeManager() : base(typeof(TruckUpgradeManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (TruckUpgradeManager)obj;
			
			writer.WritePrivateField("currentSawBlades", instance);
			writer.WritePrivateField("currentWheels", instance);
			writer.WriteProperty("SawBlades_CR", instance.SawBlades_CR, ES3Type_int.Instance);
			writer.WriteProperty("rotationSpeed_CR", instance.rotationSpeed_CR, ES3Type_int.Instance);
			writer.WriteProperty("wheels_CR", instance.wheels_CR, ES3Type_int.Instance);
			writer.WriteProperty("maxCarCapacity_CR", instance.maxCarCapacity_CR, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (TruckUpgradeManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "currentSawBlades":
					instance = (TruckUpgradeManager)reader.SetPrivateField("currentSawBlades", reader.Read<System.Int32>(), instance);
					break;
					case "currentWheels":
					instance = (TruckUpgradeManager)reader.SetPrivateField("currentWheels", reader.Read<System.Int32>(), instance);
					break;
					case "SawBlades_CR":
						instance.SawBlades_CR = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "rotationSpeed_CR":
						instance.rotationSpeed_CR = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "wheels_CR":
						instance.wheels_CR = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "maxCarCapacity_CR":
						instance.maxCarCapacity_CR = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_TruckUpgradeManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_TruckUpgradeManagerArray() : base(typeof(TruckUpgradeManager[]), ES3UserType_TruckUpgradeManager.Instance)
		{
			Instance = this;
		}
	}
}