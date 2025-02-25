// THIS FILE IS AUTOMATICALLY GENERATED BY SPACETIMEDB. EDITS TO THIS FILE
// WILL NOT BE SAVED. MODIFY TABLES IN RUST INSTEAD.

using System;
using System.Collections.Generic;
using SpacetimeDB;

namespace SpacetimeDB.Types
{
	[Newtonsoft.Json.JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
	public partial class AnimationComponent : IDatabaseTable
	{
		[Newtonsoft.Json.JsonProperty("entity_id")]
		public ulong EntityId;
		[Newtonsoft.Json.JsonProperty("interact_start_timestamp")]
		public ulong InteractStartTimestamp;
		[Newtonsoft.Json.JsonProperty("action_target_entity_id")]
		public ulong ActionTargetEntityId;

		private static Dictionary<ulong, AnimationComponent> EntityId_Index = new Dictionary<ulong, AnimationComponent>(16);

		private static void InternalOnValueInserted(object insertedValue)
		{
			var val = (AnimationComponent)insertedValue;
			EntityId_Index[val.EntityId] = val;
		}

		private static void InternalOnValueDeleted(object deletedValue)
		{
			var val = (AnimationComponent)deletedValue;
			EntityId_Index.Remove(val.EntityId);
		}

		public static SpacetimeDB.SATS.AlgebraicType GetAlgebraicType()
		{
			return SpacetimeDB.SATS.AlgebraicType.CreateProductType(new SpacetimeDB.SATS.ProductTypeElement[]
			{
				new SpacetimeDB.SATS.ProductTypeElement("entity_id", SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.U64)),
				new SpacetimeDB.SATS.ProductTypeElement("interact_start_timestamp", SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.U64)),
				new SpacetimeDB.SATS.ProductTypeElement("action_target_entity_id", SpacetimeDB.SATS.AlgebraicType.CreatePrimitiveType(SpacetimeDB.SATS.BuiltinType.Type.U64)),
			});
		}

		public static explicit operator AnimationComponent(SpacetimeDB.SATS.AlgebraicValue value)
		{
			if (value == null) {
				return null;
			}
			var productValue = value.AsProductValue();
			return new AnimationComponent
			{
				EntityId = productValue.elements[0].AsU64(),
				InteractStartTimestamp = productValue.elements[1].AsU64(),
				ActionTargetEntityId = productValue.elements[2].AsU64(),
			};
		}

		public static System.Collections.Generic.IEnumerable<AnimationComponent> Iter()
		{
			foreach(var entry in SpacetimeDBClient.clientDB.GetEntries("AnimationComponent"))
			{
				yield return (AnimationComponent)entry.Item2;
			}
		}
		public static int Count()
		{
			return SpacetimeDBClient.clientDB.Count("AnimationComponent");
		}
		public static AnimationComponent FilterByEntityId(ulong value)
		{
			EntityId_Index.TryGetValue(value, out var r);
			return r;
		}

		public static System.Collections.Generic.IEnumerable<AnimationComponent> FilterByInteractStartTimestamp(ulong value)
		{
			foreach(var entry in SpacetimeDBClient.clientDB.GetEntries("AnimationComponent"))
			{
				var productValue = entry.Item1.AsProductValue();
				var compareValue = (ulong)productValue.elements[1].AsU64();
				if (compareValue == value) {
					yield return (AnimationComponent)entry.Item2;
				}
			}
		}

		public static System.Collections.Generic.IEnumerable<AnimationComponent> FilterByActionTargetEntityId(ulong value)
		{
			foreach(var entry in SpacetimeDBClient.clientDB.GetEntries("AnimationComponent"))
			{
				var productValue = entry.Item1.AsProductValue();
				var compareValue = (ulong)productValue.elements[2].AsU64();
				if (compareValue == value) {
					yield return (AnimationComponent)entry.Item2;
				}
			}
		}

		public static bool ComparePrimaryKey(SpacetimeDB.SATS.AlgebraicType t, SpacetimeDB.SATS.AlgebraicValue v1, SpacetimeDB.SATS.AlgebraicValue v2)
		{
			var primaryColumnValue1 = v1.AsProductValue().elements[0];
			var primaryColumnValue2 = v2.AsProductValue().elements[0];
			return SpacetimeDB.SATS.AlgebraicValue.Compare(t.product.elements[0].algebraicType, primaryColumnValue1, primaryColumnValue2);
		}

		public static SpacetimeDB.SATS.AlgebraicValue GetPrimaryKeyValue(SpacetimeDB.SATS.AlgebraicValue v)
		{
			return v.AsProductValue().elements[0];
		}

		public static SpacetimeDB.SATS.AlgebraicType GetPrimaryKeyType(SpacetimeDB.SATS.AlgebraicType t)
		{
			return t.product.elements[0].algebraicType;
		}

		public delegate void InsertEventHandler(AnimationComponent insertedValue, SpacetimeDB.Types.ReducerEvent dbEvent);
		public delegate void UpdateEventHandler(AnimationComponent oldValue, AnimationComponent newValue, SpacetimeDB.Types.ReducerEvent dbEvent);
		public delegate void DeleteEventHandler(AnimationComponent deletedValue, SpacetimeDB.Types.ReducerEvent dbEvent);
		public delegate void RowUpdateEventHandler(SpacetimeDBClient.TableOp op, AnimationComponent oldValue, AnimationComponent newValue, SpacetimeDB.Types.ReducerEvent dbEvent);
		public static event InsertEventHandler OnInsert;
		public static event UpdateEventHandler OnUpdate;
		public static event DeleteEventHandler OnBeforeDelete;
		public static event DeleteEventHandler OnDelete;
		public static event RowUpdateEventHandler OnRowUpdate;

		public static void OnInsertEvent(object newValue, ClientApi.Event dbEvent)
		{
			OnInsert?.Invoke((AnimationComponent)newValue,(ReducerEvent)dbEvent?.FunctionCall.CallInfo);
		}

		public static void OnUpdateEvent(object oldValue, object newValue, ClientApi.Event dbEvent)
		{
			OnUpdate?.Invoke((AnimationComponent)oldValue,(AnimationComponent)newValue,(ReducerEvent)dbEvent?.FunctionCall.CallInfo);
		}

		public static void OnBeforeDeleteEvent(object oldValue, ClientApi.Event dbEvent)
		{
			OnBeforeDelete?.Invoke((AnimationComponent)oldValue,(ReducerEvent)dbEvent?.FunctionCall.CallInfo);
		}

		public static void OnDeleteEvent(object oldValue, ClientApi.Event dbEvent)
		{
			OnDelete?.Invoke((AnimationComponent)oldValue,(ReducerEvent)dbEvent?.FunctionCall.CallInfo);
		}

		public static void OnRowUpdateEvent(SpacetimeDBClient.TableOp op, object oldValue, object newValue, ClientApi.Event dbEvent)
		{
			OnRowUpdate?.Invoke(op, (AnimationComponent)oldValue,(AnimationComponent)newValue,(ReducerEvent)dbEvent?.FunctionCall.CallInfo);
		}
	}
}
