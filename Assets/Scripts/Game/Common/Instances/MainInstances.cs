using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace Game.Common.Instances {

	public interface IGameInstance { }

	public class MainInstances : MonoBehaviour
	{
		
		
		public static MainInstances main;


		private readonly Dictionary<string, IGameInstance> gameServices = new Dictionary<string, IGameInstance>();


		private void Awake() {
			if(main != null) {
				Debug.LogWarning("MainInstance main was not cleaned up, manually cleaning up instance");
				main.Clean();
			}
			main = this;
			
			DontDestroyOnLoad(this.gameObject);

			SceneManager.sceneUnloaded += OnSceneUnloaded;
		}

		/// <summary>
		/// This allows you to statically get the current Instance of any class that has been added. 
		/// </summary>
		/// <typeparam name="T">IGameInstance</typeparam>
		/// <returns>The referenced instances. If none is found it will throw an exception</returns>
		public static T Get<T>() where T : IGameInstance {
			IGameInstance output;
			if(main.gameServices.TryGetValue(GetKey<T>(), out output)) {
				return (T)output;
			} else {
				throw new UnityException("IGameService has not been added! " + typeof(T).FullName);
			}
		}

		/// <summary>
		/// Grabs the Object version of IGameInstance of the type defined.
		/// </summary>
		/// <param name="type"></param>
		/// <returns>null is returned if not found</returns>
		public static Object GetObject(Type type) {
			if (main.gameServices.TryGetValue(GetKey(type), out var instance)) {
				return instance;
			}

			Debug.LogError("Tried grabbing an IGameInstance by type without it being added.");
			return null;
		}

		/// <summary>
		/// Adds a instance into the system. Only one instance per type is allowed
		/// </summary>
		/// <typeparam name="T">IGameInstance</typeparam>
		/// <param name="obj">IGameInstance class</param>
		public static void Add<T>(T obj) where T : IGameInstance {
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
		public static void TryAdd<T>(T obj) where T : IGameInstance {
			try {
				Add(obj);
			} catch { }
		}
		

		/// <summary>
		/// Removes an instance from the system.
		/// </summary>
		/// <typeparam name="T">IGameInstance</typeparam>
		public static void Remove<T>() where T : IGameInstance {
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



		private void OnDestroy() {
			if(main == this) {
				main = null;
			}
		}

		
		/// <summary>
		/// Checks to see if any instances are being carried over. This is reassurance
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="loadMode"></param>
		private void OnSceneUnloaded(Scene scene)
		{
			if (this.gameServices.Count > 0)
			{
				foreach(var keyPair in gameServices)
				{
					Debug.LogError( keyPair.Key.ToString() + " - Scene loaded with instances still being carried over! Use Main Persistant Instance instead.");
				}
			}
		}
	}

}