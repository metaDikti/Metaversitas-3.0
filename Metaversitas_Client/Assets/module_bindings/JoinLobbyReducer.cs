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
		public delegate void JoinLobbyHandler(ReducerEvent reducerEvent, string kelas, string materi, string pertemuan);
		public static event JoinLobbyHandler OnJoinLobbyEvent;

		public static void JoinLobby(string kelas, string materi, string pertemuan)
		{
			var _argArray = new object[] {kelas, materi, pertemuan};
			var _message = new SpacetimeDBClient.ReducerCallRequest {
				fn = "join_lobby",
				args = _argArray,
			};
			SpacetimeDBClient.instance.InternalCallReducer(Newtonsoft.Json.JsonConvert.SerializeObject(_message, _settings));
		}

		[ReducerCallback(FunctionName = "join_lobby")]
		public static bool OnJoinLobby(ClientApi.Event dbEvent)
		{
			if(OnJoinLobbyEvent != null)
			{
				var args = ((ReducerEvent)dbEvent.FunctionCall.CallInfo).JoinLobbyArgs;
				OnJoinLobbyEvent((ReducerEvent)dbEvent.FunctionCall.CallInfo
					,(string)args.Kelas
					,(string)args.Materi
					,(string)args.Pertemuan
				);
				return true;
			}
			return false;
		}

		[DeserializeEvent(FunctionName = "join_lobby")]
		public static void JoinLobbyDeserializeEventArgs(ClientApi.Event dbEvent)
		{
			var args = new JoinLobbyArgsStruct();
			var bsatnBytes = dbEvent.FunctionCall.ArgBytes;
			using var ms = new System.IO.MemoryStream();
			ms.SetLength(bsatnBytes.Length);
			bsatnBytes.CopyTo(ms.GetBuffer(), 0);
			ms.Position = 0;
			using var reader = new System.IO.BinaryReader(ms);
			var args_0_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.String), reader);
			args.Kelas = args_0_value.AsString();
			var args_1_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.String), reader);
			args.Materi = args_1_value.AsString();
			var args_2_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.String), reader);
			args.Pertemuan = args_2_value.AsString();
			dbEvent.FunctionCall.CallInfo = new ReducerEvent(ReducerType.JoinLobby, "join_lobby", dbEvent.Timestamp, Identity.From(dbEvent.CallerIdentity.ToByteArray()), Address.From(dbEvent.CallerAddress.ToByteArray()), dbEvent.Message, dbEvent.Status, args);
		}
	}

	public partial class JoinLobbyArgsStruct
	{
		public string Kelas;
		public string Materi;
		public string Pertemuan;
	}

}
