// THIS FILE IS AUTOMATICALLY GENERATED BY SPACETIMEDB. EDITS TO THIS FILE
// WILL NOT BE SAVED. MODIFY TABLES IN RUST INSTEAD.

use super::stdb_vector_3::StdbVector3;
#[allow(unused)]
use spacetimedb_sdk::{
    anyhow::{anyhow, Result},
    identity::Identity,
    reducer::{Reducer, ReducerCallbackId, Status},
    sats::{de::Deserialize, ser::Serialize},
    spacetimedb_lib,
    table::{TableIter, TableType, TableWithPrimaryKey},
    Address,
};

#[derive(Serialize, Deserialize, Clone, PartialEq, Debug)]
pub struct EntityComponent {
    pub entity_id: u64,
    pub position: StdbVector3,
    pub direction: f32,
    pub moving: bool,
}

impl TableType for EntityComponent {
    const TABLE_NAME: &'static str = "EntityComponent";
    type ReducerEvent = super::ReducerEvent;
}

impl TableWithPrimaryKey for EntityComponent {
    type PrimaryKey = u64;
    fn primary_key(&self) -> &Self::PrimaryKey {
        &self.entity_id
    }
}

impl EntityComponent {
    #[allow(unused)]
    pub fn filter_by_entity_id(entity_id: u64) -> Option<Self> {
        Self::find(|row| row.entity_id == entity_id)
    }
    #[allow(unused)]
    pub fn filter_by_position(position: StdbVector3) -> TableIter<Self> {
        Self::filter(|row| row.position == position)
    }
    #[allow(unused)]
    pub fn filter_by_direction(direction: f32) -> TableIter<Self> {
        Self::filter(|row| row.direction == direction)
    }
    #[allow(unused)]
    pub fn filter_by_moving(moving: bool) -> TableIter<Self> {
        Self::filter(|row| row.moving == moving)
    }
}
