using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Game.Common.Instances
{

	public interface IGamePersistantInstance { }

	
	public class MainPersistantInstances : MonoBehaviour
	{

		public static MainPersistantInstances main;


		private readonly Dictionary<string, IGamePersistantInstance> gameServices= new Dictionary<string, IGamePersistantInstance>();


		private void Awake() {
			if(main != null) {
				Debug.LogWarning("MainInstance main was not cleaned up, manually cleaning up instance");
				main.Clean();
			}
			main = this;
			DontDestroyOnLoad(this.gameObject);
		}

		/// <summary>
		/// This allows you to statically get the current Instance of any class that has been added. 
		/// </summary>
		/// <typeparam name="T">IGamePersistantInstance</typeparam>
		/// <returns>The referenced instances. If none is found it will throw an exception</returns>
		public static T Get<T>() where T : IGamePersistantInstance {
			IGamePersistantInstance output;
			if(main.gameServices.TryGetValue(GetKey<T>(), out output)) {
				return (T)output;
			} else
			{
				return default(T);
				//throw new UnityException("IGameService has not been added! " + typeof(T).FullName);
			}
		}

		/// <summary>
		/// Try get value
		/// </summary>
		/// <param name="obj"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static bool TryGet<T>(out T obj) where T : IGamePersistantInstance
		{
			if (HasType(typeof(T)))
			{
				obj = Get<T>();
				return true;
			}
			else
			{
				obj = default(T);
				return false;
			}
		}

		/// <summary>
		/// Grabs the Object version of IGamePersistantInstance of the type defined.
		/// </summary>
		/// <param name="type"></param>
		/// <returns>null is returned if not found</returns>
		public static Object GetObject(Type type) {

			IGamePersistantInstance instance;
			if (main.gameServices.TryGetValue(GetKey(type), out instance)) {
				return instance;
			}

			Debug.LogError("Tried grabbing an IGamePersistantInstance by type without it being added.");
			return null;
		}

		/// <summary>
		/// Adds a instance into the system. Only one instance per type is allowed
		/// </summary>
		/// <typeparam name="T">IGamePersistantInstance</typeparam>
		/// <param name="obj">IGamePersistantInstance class</param>
		public static void Add<T>(T obj) where T : IGamePersistantInstance {
			string key = GetKey<T>();

			if (main.gameServices.ContainsKey(key)) {
				throw new UnityException("IGameService has already been added! " + key);
			}

			main.gameServices.Add(key, obj);
		}

		/// <summary>
		/// Try to add if you know this will be recalled.
		/// </summary>
		/// <param name="obj"></param>
		/// <typeparam name="T"></typeparam>
		public static void TryAdd<T>(T obj) where T : IGamePersistantInstance {
			try {
				Add(obj);
			} catch { }
		}
		

		/// <summary>
		/// Removes an instance from the system.
		/// </summary>
		/// <typeparam name="T">IGamePersistantInstance</typeparam>
		public static void Remove<T>() where T : IGamePersistantInstance {
			string key = GetKey<T>();

			//If it wasn't able to be removed
			if (!main.gameServices.Remove(key)) {
				Debug.LogWarning("IGameService was tried to be removed when none existed in dictionary!" + key);
			}
		}

		/// <summary>
		/// Check if this contains type of t
		/// </summary>
		/// <param name="t"></param>
		public static bool HasType(Type t)
		{
			return main.gameServices.ContainsKey(GetKey(t));
		}
		private static string GetKey<T>() {
			return typeof(T).FullName;
		}

		private static string GetKey(Type type) {
			return type.FullName;
		}


		public void Clean() {
			this.gameServices.Clear();
			Destroy(this.gameObject);
			Destroy(this);
		}
		
		

	}
}
