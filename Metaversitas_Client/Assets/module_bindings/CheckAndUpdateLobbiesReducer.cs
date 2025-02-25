// THIS FILE IS AUTOMATICALLY GENERATED BY SPACETIMEDB. EDITS TO THIS FILE
// WILL NOT BE SAVED. MODIFY TABLES IN RUST INSTEAD.

using System;
using ClientApi;
using Newtonsoft.Json.Linq;
using SpacetimeDB;

namespace SpacetimeDB.Types
{
	public static partial class Reducer
	{
		public delegate void CheckAndUpdateLobbiesHandler(ReducerEvent reducerEvent, ulong prevTime);
		public static event CheckAndUpdateLobbiesHandler OnCheckAndUpdateLobbiesEvent;

		public static void CheckAndUpdateLobbies(ulong prevTime)
		{
			var _argArray = new object[] {prevTime};
			var _message = new SpacetimeDBClient.ReducerCallRequest {
				fn = "check_and_update_lobbies",
				args = _argArray,
			};
			SpacetimeDBClient.instance.InternalCallReducer(Newtonsoft.Json.JsonConvert.SerializeObject(_message, _settings));
		}

		[ReducerCallback(FunctionName = "check_and_update_lobbies")]
		public static bool OnCheckAndUpdateLobbies(ClientApi.Event dbEvent)
		{
			if(OnCheckAndUpdateLobbiesEvent != null)
			{
				var args = ((ReducerEvent)dbEvent.FunctionCall.CallInfo).CheckAndUpdateLobbiesArgs;
				OnCheckAndUpdateLobbiesEvent((ReducerEvent)dbEvent.FunctionCall.CallInfo
					,(ulong)args.PrevTime
				);
				return true;
			}
			return false;
		}

		[DeserializeEvent(FunctionName = "check_and_update_lobbies")]
		public static void CheckAndUpdateLobbiesDeserializeEventArgs(ClientApi.Event dbEvent)
		{
			var args = new CheckAndUpdateLobbiesArgsStruct();
			var bsatnBytes = dbEvent.FunctionCall.ArgBytes;
			using var ms = new System.IO.MemoryStream();
			ms.SetLength(bsatnBytes.Length);
			bsatnBytes.CopyTo(ms.GetBuffer(), 0);
			ms.Position = 0;
			using var reader = new System.IO.BinaryReader(ms);
			var args_0_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.U64), reader);
			args.PrevTime = args_0_value.AsU64();
			dbEvent.FunctionCall.CallInfo = new ReducerEvent(ReducerType.CheckAndUpdateLobbies, "check_and_update_lobbies", dbEvent.Timestamp, Identity.From(dbEvent.CallerIdentity.ToByteArray()), Address.From(dbEvent.CallerAddress.ToByteArray()), dbEvent.Message, dbEvent.Status, args);
		}
	}

	public partial class CheckAndUpdateLobbiesArgsStruct
	{
		public ulong PrevTime;
	}

}
