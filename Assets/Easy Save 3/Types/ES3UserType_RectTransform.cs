using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("anchorMin", "anchorMax", "anchoredPosition", "sizeDelta", "pivot", "offsetMin", "offsetMax", "localPosition", "localRotation", "localScale", "parent")]
	public class ES3UserType_RectTransform : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_RectTransform() : base(typeof(UnityEngine.RectTransform)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UnityEngine.RectTransform)obj;
			
			writer.WriteProperty("anchorMin", instance.anchorMin, ES3Type_Vector2.Instance);
			writer.WriteProperty("anchorMax", instance.anchorMax, ES3Type_Vector2.Instance);
			writer.WriteProperty("anchoredPosition", instance.anchoredPosition, ES3Type_Vector2.Instance);
			writer.WriteProperty("sizeDelta", instance.sizeDelta, ES3Type_Vector2.Instance);
			writer.WriteProperty("pivot", instance.pivot, ES3Type_Vector2.Instance);
			writer.WriteProperty("offsetMin", instance.offsetMin, ES3Type_Vector2.Instance);
			writer.WriteProperty("offsetMax", instance.offsetMax, ES3Type_Vector2.Instance);
			writer.WriteProperty("localPosition", instance.localPosition, ES3Type_Vector3.Instance);
			writer.WriteProperty("localRotation", instance.localRotation, ES3Type_Quaternion.Instance);
			writer.WriteProperty("localScale", instance.localScale, ES3Type_Vector3.Instance);
			writer.WritePropertyByRef("parent", instance.parent);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UnityEngine.RectTransform)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "anchorMin":
						instance.anchorMin = reader.Read<UnityEngine.Vector2>(ES3Type_Vector2.Instance);
						break;
					case "anchorMax":
						instance.anchorMax = reader.Read<UnityEngine.Vector2>(ES3Type_Vector2.Instance);
						break;
					case "anchoredPosition":
						instance.anchoredPosition = reader.Read<UnityEngine.Vector2>(ES3Type_Vector2.Instance);
						break;
					case "sizeDelta":
						instance.sizeDelta = reader.Read<UnityEngine.Vector2>(ES3Type_Vector2.Instance);
						break;
					case "pivot":
						instance.pivot = reader.Read<UnityEngine.Vector2>(ES3Type_Vector2.Instance);
						break;
					case "offsetMin":
						instance.offsetMin = reader.Read<UnityEngine.Vector2>(ES3Type_Vector2.Instance);
						break;
					case "offsetMax":
						instance.offsetMax = reader.Read<UnityEngine.Vector2>(ES3Type_Vector2.Instance);
						break;
					case "localPosition":
						instance.localPosition = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "localRotation":
						instance.localRotation = reader.Read<UnityEngine.Quaternion>(ES3Type_Quaternion.Instance);
						break;
					case "localScale":
						instance.localScale = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "parent":
						instance.parent = reader.Read<UnityEngine.Transform>(ES3Type_Transform.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_RectTransformArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_RectTransformArray() : base(typeof(UnityEngine.RectTransform[]), ES3UserType_RectTransform.Instance)
		{
			Instance = this;
		}
	}
}