// THIS FILE IS AUTOMATICALLY GENERATED BY SPACETIMEDB. EDITS TO THIS FILE
// WILL NOT BE SAVED. MODIFY TABLES IN RUST INSTEAD.
// <auto-generated />

#nullable enable

using System;
using SpacetimeDB;

namespace SpacetimeDB.Types
{
	// This class is only used in Unity projects.
	// Attach this to a gameobject in your scene to use SpacetimeDB.
	#if UNITY_5_3_OR_NEWER
	public class UnityNetworkManager : UnityEngine.MonoBehaviour
	{
		private void OnDestroy() => SpacetimeDBClient.instance.Close();
		private void Update() => SpacetimeDBClient.instance.Update();
	}
	#endif
}
