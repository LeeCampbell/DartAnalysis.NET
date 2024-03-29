﻿using System;
using System.Web.Script.Serialization;

namespace DanTup.DartAnalysis
{
	/// <summary>
	/// Serialises and deserialises objects to/from JSON.
	/// </summary>
	class JsonSerialiser
	{
		readonly JavaScriptSerializer serialiser = new JavaScriptSerializer();

		/// <summary>
		/// Serialises the provided object into JSON.
		/// </summary>
		/// <param name="obj">The object to serialise.</param>
		/// <returns>String of JSON representing the provided object.</returns>
		public string Serialise(object obj)
		{
			return serialiser.Serialize(obj);
		}

		/// <summary>
		/// Deserialises the provided JSON into an object of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type to deserialise into.</typeparam>
		/// <param name="json">The string of JSON to deserialise.</param>
		/// <returns>A concrete object built from the provided JSON.</returns>
		public T Deserialise<T>(string json)
		{
			return (T)Deserialise(json, typeof(T));
		}

		/// <summary>
		/// Deserialises the provided JSON into an object of the provided type.
		/// </summary>
		/// <param name="json">The string of JSON to deserialise.</param>
		/// <param name="t">The Type to be deserialised into.</param>
		/// <returns>A concrete object built from the provided JSON.</returns>
		public object Deserialise(string json, Type t)
		{
			return serialiser.Deserialize(json, t);
		}
	}
}
