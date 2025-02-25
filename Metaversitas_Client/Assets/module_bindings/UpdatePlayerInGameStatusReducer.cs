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
		public delegate void UpdatePlayerInGameStatusHandler(ReducerEvent reducerEvent, bool inGame);
		public static event UpdatePlayerInGameStatusHandler OnUpdatePlayerInGameStatusEvent;

		public static void UpdatePlayerInGameStatus(bool inGame)
		{
			var _argArray = new object[] {inGame};
			var _message = new SpacetimeDBClient.ReducerCallRequest {
				fn = "update_player_in_game_status",
				args = _argArray,
			};
			SpacetimeDBClient.instance.InternalCallReducer(Newtonsoft.Json.JsonConvert.SerializeObject(_message, _settings));
		}

		[ReducerCallback(FunctionName = "update_player_in_game_status")]
		public static bool OnUpdatePlayerInGameStatus(ClientApi.Event dbEvent)
		{
			if(OnUpdatePlayerInGameStatusEvent != null)
			{
				var args = ((ReducerEvent)dbEvent.FunctionCall.CallInfo).UpdatePlayerInGameStatusArgs;
				OnUpdatePlayerInGameStatusEvent((ReducerEvent)dbEvent.FunctionCall.CallInfo
					,(bool)args.InGame
				);
				return true;
			}
			return false;
		}

		[DeserializeEvent(FunctionName = "update_player_in_game_status")]
		public static void UpdatePlayerInGameStatusDeserializeEventArgs(ClientApi.Event dbEvent)
		{
			var args = new UpdatePlayerInGameStatusArgsStruct();
			var bsatnBytes = dbEvent.FunctionCall.ArgBytes;
			using var ms = new System.IO.MemoryStream();
			ms.SetLength(bsatnBytes.Length);
			bsatnBytes.CopyTo(ms.GetBuffer(), 0);
			ms.Position = 0;
			using var reader = new System.IO.BinaryReader(ms);
			var args_0_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.Bool), reader);
			args.InGame = args_0_value.AsBool();
			dbEvent.FunctionCall.CallInfo = new ReducerEvent(ReducerType.UpdatePlayerInGameStatus, "update_player_in_game_status", dbEvent.Timestamp, Identity.From(dbEvent.CallerIdentity.ToByteArray()), Address.From(dbEvent.CallerAddress.ToByteArray()), dbEvent.Message, dbEvent.Status, args);
		}
	}

	public partial class UpdatePlayerInGameStatusArgsStruct
	{
		public bool InGame;
	}

}
