// THIS FILE IS AUTOMATICALLY GENERATED BY SPACETIMEDB. EDITS TO THIS FILE
// WILL NOT BE SAVED. MODIFY TABLES IN RUST INSTEAD.

using System;
using ClientApi;
using Newtonsoft.Json.Linq;
using SpacetimeDB;

namespace SpacetimeDB.Types
{
	public enum ReducerType
	{
		None,
		CheckAndUpdateLobbies,
		CreateLobby,
		CreatePlayer,
		DestroyLobby,
		FilterLobbies,
		Interact,
		JoinLobby,
		LeaveLobby,
		Matchmaking,
		MovePlayer,
		PublicChatMessage,
		RotatePlayer,
		SendPrivateMessage,
		StopPlayer,
		StopRotatePlayer,
		UpdateLobbyTimeLimit,
		UpdatePlayerInGameStatus,
	}

	public partial class ReducerEvent : ReducerEventBase
	{
		public ReducerType Reducer { get; private set; }

		public ReducerEvent(ReducerType reducer, string reducerName, ulong timestamp, SpacetimeDB.Identity identity, SpacetimeDB.Address? callerAddress, string errMessage, ClientApi.Event.Types.Status status, object args)
			: base(reducerName, timestamp, identity, callerAddress, errMessage, status, args)
		{
			Reducer = reducer;
		}

		public CheckAndUpdateLobbiesArgsStruct CheckAndUpdateLobbiesArgs
		{
			get
			{
				if (Reducer != ReducerType.CheckAndUpdateLobbies) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "CheckAndUpdateLobbies");
				return (CheckAndUpdateLobbiesArgsStruct)Args;
			}
		}
		public CreateLobbyArgsStruct CreateLobbyArgs
		{
			get
			{
				if (Reducer != ReducerType.CreateLobby) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "CreateLobby");
				return (CreateLobbyArgsStruct)Args;
			}
		}
		public CreatePlayerArgsStruct CreatePlayerArgs
		{
			get
			{
				if (Reducer != ReducerType.CreatePlayer) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "CreatePlayer");
				return (CreatePlayerArgsStruct)Args;
			}
		}
		public DestroyLobbyArgsStruct DestroyLobbyArgs
		{
			get
			{
				if (Reducer != ReducerType.DestroyLobby) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "DestroyLobby");
				return (DestroyLobbyArgsStruct)Args;
			}
		}
		public FilterLobbiesArgsStruct FilterLobbiesArgs
		{
			get
			{
				if (Reducer != ReducerType.FilterLobbies) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "FilterLobbies");
				return (FilterLobbiesArgsStruct)Args;
			}
		}
		public InteractArgsStruct InteractArgs
		{
			get
			{
				if (Reducer != ReducerType.Interact) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "Interact");
				return (InteractArgsStruct)Args;
			}
		}
		public JoinLobbyArgsStruct JoinLobbyArgs
		{
			get
			{
				if (Reducer != ReducerType.JoinLobby) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "JoinLobby");
				return (JoinLobbyArgsStruct)Args;
			}
		}
		public LeaveLobbyArgsStruct LeaveLobbyArgs
		{
			get
			{
				if (Reducer != ReducerType.LeaveLobby) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "LeaveLobby");
				return (LeaveLobbyArgsStruct)Args;
			}
		}
		public MatchmakingArgsStruct MatchmakingArgs
		{
			get
			{
				if (Reducer != ReducerType.Matchmaking) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "Matchmaking");
				return (MatchmakingArgsStruct)Args;
			}
		}
		public MovePlayerArgsStruct MovePlayerArgs
		{
			get
			{
				if (Reducer != ReducerType.MovePlayer) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "MovePlayer");
				return (MovePlayerArgsStruct)Args;
			}
		}
		public PublicChatMessageArgsStruct PublicChatMessageArgs
		{
			get
			{
				if (Reducer != ReducerType.PublicChatMessage) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "PublicChatMessage");
				return (PublicChatMessageArgsStruct)Args;
			}
		}
		public RotatePlayerArgsStruct RotatePlayerArgs
		{
			get
			{
				if (Reducer != ReducerType.RotatePlayer) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "RotatePlayer");
				return (RotatePlayerArgsStruct)Args;
			}
		}
		public SendPrivateMessageArgsStruct SendPrivateMessageArgs
		{
			get
			{
				if (Reducer != ReducerType.SendPrivateMessage) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "SendPrivateMessage");
				return (SendPrivateMessageArgsStruct)Args;
			}
		}
		public StopPlayerArgsStruct StopPlayerArgs
		{
			get
			{
				if (Reducer != ReducerType.StopPlayer) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "StopPlayer");
				return (StopPlayerArgsStruct)Args;
			}
		}
		public StopRotatePlayerArgsStruct StopRotatePlayerArgs
		{
			get
			{
				if (Reducer != ReducerType.StopRotatePlayer) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "StopRotatePlayer");
				return (StopRotatePlayerArgsStruct)Args;
			}
		}
		public UpdateLobbyTimeLimitArgsStruct UpdateLobbyTimeLimitArgs
		{
			get
			{
				if (Reducer != ReducerType.UpdateLobbyTimeLimit) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "UpdateLobbyTimeLimit");
				return (UpdateLobbyTimeLimitArgsStruct)Args;
			}
		}
		public UpdatePlayerInGameStatusArgsStruct UpdatePlayerInGameStatusArgs
		{
			get
			{
				if (Reducer != ReducerType.UpdatePlayerInGameStatus) throw new SpacetimeDB.ReducerMismatchException(Reducer.ToString(), "UpdatePlayerInGameStatus");
				return (UpdatePlayerInGameStatusArgsStruct)Args;
			}
		}

		public object[] GetArgsAsObjectArray()
		{
			switch (Reducer)
			{
				case ReducerType.CheckAndUpdateLobbies:
				{
					var args = CheckAndUpdateLobbiesArgs;
					return new object[] {
						args.PrevTime,
					};
				}
				case ReducerType.CreateLobby:
				{
					var args = CreateLobbyArgs;
					return new object[] {
						args.Id,
						args.Kelas,
						args.Materi,
						args.Pertemuan,
						args.Participants,
					};
				}
				case ReducerType.CreatePlayer:
				{
					var args = CreatePlayerArgs;
					return new object[] {
						args.Role,
						args.Gender,
						args.Uuid,
						args.FullName,
						args.Nickname,
						args.Universitas,
						args.Kodeuniv,
						args.Fakultas,
						args.Jurusan,
					};
				}
				case ReducerType.DestroyLobby:
				{
					var args = DestroyLobbyArgs;
					return new object[] {
						args.Kelas,
						args.Materi,
						args.Pertemuan,
					};
				}
				case ReducerType.FilterLobbies:
				{
					var args = FilterLobbiesArgs;
					return new object[] {
						args.Kelas,
						args.Materi,
						args.Pertemuan,
					};
				}
				case ReducerType.Interact:
				{
					var args = InteractArgs;
					return new object[] {
						args.EntityId,
					};
				}
				case ReducerType.JoinLobby:
				{
					var args = JoinLobbyArgs;
					return new object[] {
						args.Kelas,
						args.Materi,
						args.Pertemuan,
					};
				}
				case ReducerType.LeaveLobby:
				{
					var args = LeaveLobbyArgs;
					return new object[] {
						args.Kelas,
						args.Materi,
						args.Pertemuan,
					};
				}
				case ReducerType.Matchmaking:
				{
					var args = MatchmakingArgs;
					return new object[] {
						args.Kelas,
						args.Materi,
						args.Pertemuan,
					};
				}
				case ReducerType.MovePlayer:
				{
					var args = MovePlayerArgs;
					return new object[] {
						args.Position,
						args.Direction,
						args.Moving,
						args.Sprinting,
					};
				}
				case ReducerType.PublicChatMessage:
				{
					var args = PublicChatMessageArgs;
					return new object[] {
						args.LobbyId,
						args.Message,
					};
				}
				case ReducerType.RotatePlayer:
				{
					var args = RotatePlayerArgs;
					return new object[] {
						args.Rotation,
						args.Rotvalue,
					};
				}
				case ReducerType.SendPrivateMessage:
				{
					var args = SendPrivateMessageArgs;
					return new object[] {
						args.LobbyId,
						args.SenderId,
						args.RecipientId,
						args.Message,
					};
				}
				case ReducerType.StopPlayer:
				{
					var args = StopPlayerArgs;
					return new object[] {
						args.Position,
						args.Direction,
					};
				}
				case ReducerType.StopRotatePlayer:
				{
					var args = StopRotatePlayerArgs;
					return new object[] {
						args.Rotation,
						args.Rotvalue,
					};
				}
				case ReducerType.UpdateLobbyTimeLimit:
				{
					var args = UpdateLobbyTimeLimitArgs;
					return new object[] {
						args.LobbyId,
						args.NewTimeLimit,
					};
				}
				case ReducerType.UpdatePlayerInGameStatus:
				{
					var args = UpdatePlayerInGameStatusArgs;
					return new object[] {
						args.InGame,
					};
				}
				default: throw new System.Exception($"Unhandled reducer case: {Reducer}. Please run SpacetimeDB code generator");
			}
		}
	}
}
