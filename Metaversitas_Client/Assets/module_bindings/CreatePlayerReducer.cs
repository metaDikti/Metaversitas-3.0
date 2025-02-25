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
		public delegate void CreatePlayerHandler(ReducerEvent reducerEvent, string role, string gender, string uuid, string fullName, string nickname, string universitas, string kodeuniv, string fakultas, string jurusan);
		public static event CreatePlayerHandler OnCreatePlayerEvent;

		public static void CreatePlayer(string role, string gender, string uuid, string fullName, string nickname, string universitas, string kodeuniv, string fakultas, string jurusan)
		{
			var _argArray = new object[] {role, gender, uuid, fullName, nickname, universitas, kodeuniv, fakultas, jurusan};
			var _message = new SpacetimeDBClient.ReducerCallRequest {
				fn = "create_player",
				args = _argArray,
			};
			SpacetimeDBClient.instance.InternalCallReducer(Newtonsoft.Json.JsonConvert.SerializeObject(_message, _settings));
		}

		[ReducerCallback(FunctionName = "create_player")]
		public static bool OnCreatePlayer(ClientApi.Event dbEvent)
		{
			if(OnCreatePlayerEvent != null)
			{
				var args = ((ReducerEvent)dbEvent.FunctionCall.CallInfo).CreatePlayerArgs;
				OnCreatePlayerEvent((ReducerEvent)dbEvent.FunctionCall.CallInfo
					,(string)args.Role
					,(string)args.Gender
					,(string)args.Uuid
					,(string)args.FullName
					,(string)args.Nickname
					,(string)args.Universitas
					,(string)args.Kodeuniv
					,(string)args.Fakultas
					,(string)args.Jurusan
				);
				return true;
			}
			return false;
		}

		[DeserializeEvent(FunctionName = "create_player")]
		public static void CreatePlayerDeserializeEventArgs(ClientApi.Event dbEvent)
		{
			var args = new CreatePlayerArgsStruct();
			var bsatnBytes = dbEvent.FunctionCall.ArgBytes;
			using var ms = new System.IO.MemoryStream();
			ms.SetLength(bsatnBytes.Length);
			bsatnBytes.CopyTo(ms.GetBuffer(), 0);
			ms.Position = 0;
			using var reader = new System.IO.BinaryReader(ms);
			var args_0_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.String), reader);
			args.Role = args_0_value.AsString();
			var args_1_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.String), reader);
			args.Gender = args_1_value.AsString();
			var args_2_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.String), reader);
			args.Uuid = args_2_value.AsString();
			var args_3_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.String), reader);
			args.FullName = args_3_value.AsString();
			var args_4_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.String), reader);
			args.Nickname = args_4_value.AsString();
			var args_5_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.String), reader);
			args.Universitas = args_5_value.AsString();
			var args_6_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.String), reader);
			args.Kodeuniv = args_6_value.AsString();
			var args_7_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.String), reader);
			args.Fakultas = args_7_value.AsString();
			var args_8_value = SpacetimeDB.SATS.AlgebraicValue.Deserialize(SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.String), reader);
			args.Jurusan = args_8_value.AsString();
			dbEvent.FunctionCall.CallInfo = new ReducerEvent(ReducerType.CreatePlayer, "create_player", dbEvent.Timestamp, Identity.From(dbEvent.CallerIdentity.ToByteArray()), Address.From(dbEvent.CallerAddress.ToByteArray()), dbEvent.Message, dbEvent.Status, args);
		}
	}

	public partial class CreatePlayerArgsStruct
	{
		public string Role;
		public string Gender;
		public string Uuid;
		public string FullName;
		public string Nickname;
		public string Universitas;
		public string Kodeuniv;
		public string Fakultas;
		public string Jurusan;
	}

}
