using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("UnlockOnce1", "UnlockOnce2", "percentage")]
	public class ES3UserType_PercentageChecker : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_PercentageChecker() : base(typeof(PT.Garden.PercentageChecker)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (PT.Garden.PercentageChecker)obj;
			
			writer.WritePrivateField("UnlockOnce1", instance);
			writer.WritePrivateField("UnlockOnce2", instance);
			writer.WritePrivateField("percentage", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (PT.Garden.PercentageChecker)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "UnlockOnce1":
					instance = (PT.Garden.PercentageChecker)reader.SetPrivateField("UnlockOnce1", reader.Read<System.Boolean>(), instance);
					break;
					case "UnlockOnce2":
					instance = (PT.Garden.PercentageChecker)reader.SetPrivateField("UnlockOnce2", reader.Read<System.Boolean>(), instance);
					break;
					case "percentage":
					instance = (PT.Garden.PercentageChecker)reader.SetPrivateField("percentage", reader.Read<System.Single>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_PercentageCheckerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_PercentageCheckerArray() : base(typeof(PT.Garden.PercentageChecker[]), ES3UserType_PercentageChecker.Instance)
		{
			Instance = this;
		}
	}
}