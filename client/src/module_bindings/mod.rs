// THIS FILE IS AUTOMATICALLY GENERATED BY SPACETIMEDB. EDITS TO THIS FILE
// WILL NOT BE SAVED. MODIFY TABLES IN RUST INSTEAD.

use spacetimedb_sdk::callbacks::{DbCallbacks, ReducerCallbacks};
use spacetimedb_sdk::client_api_messages::{Event, TableUpdate};
use spacetimedb_sdk::client_cache::{ClientCache, RowCallbackReminders};
use spacetimedb_sdk::global_connection::with_connection_mut;
use spacetimedb_sdk::identity::Credentials;
use spacetimedb_sdk::reducer::AnyReducerEvent;
use spacetimedb_sdk::spacetime_module::SpacetimeModule;
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
use std::sync::Arc;

pub mod config;
pub mod create_lobby_reducer;
pub mod create_player_reducer;
pub mod destroy_lobby_reducer;
pub mod entity_component;
pub mod exit;
pub mod filter_lobbies_reducer;
pub mod join_lobby_reducer;
pub mod leave_lobby_reducer;
pub mod lobby;
pub mod player_component;
pub mod stdb_vector_3;
pub mod update_player_position_reducer;

pub use config::*;
pub use create_lobby_reducer::*;
pub use create_player_reducer::*;
pub use destroy_lobby_reducer::*;
pub use entity_component::*;
pub use exit::*;
pub use filter_lobbies_reducer::*;
pub use join_lobby_reducer::*;
pub use leave_lobby_reducer::*;
pub use lobby::*;
pub use player_component::*;
pub use stdb_vector_3::*;
pub use update_player_position_reducer::*;

#[allow(unused)]
#[derive(Serialize, Deserialize, Clone, PartialEq, Debug)]
pub enum ReducerEvent {
    CreateLobby(create_lobby_reducer::CreateLobbyArgs),
    CreatePlayer(create_player_reducer::CreatePlayerArgs),
    DestroyLobby(destroy_lobby_reducer::DestroyLobbyArgs),
    FilterLobbies(filter_lobbies_reducer::FilterLobbiesArgs),
    JoinLobby(join_lobby_reducer::JoinLobbyArgs),
    LeaveLobby(leave_lobby_reducer::LeaveLobbyArgs),
    UpdatePlayerPosition(update_player_position_reducer::UpdatePlayerPositionArgs),
}

#[allow(unused)]
pub struct Module;
impl SpacetimeModule for Module {
    fn handle_table_update(
        &self,
        table_update: TableUpdate,
        client_cache: &mut ClientCache,
        callbacks: &mut RowCallbackReminders,
    ) {
        let table_name = &table_update.table_name[..];
        match table_name {
            "Config" => client_cache
                .handle_table_update_with_primary_key::<config::Config>(callbacks, table_update),
            "EntityComponent" => client_cache
                .handle_table_update_with_primary_key::<entity_component::EntityComponent>(
                    callbacks,
                    table_update,
                ),
            "Lobby" => client_cache
                .handle_table_update_with_primary_key::<lobby::Lobby>(callbacks, table_update),
            "PlayerComponent" => client_cache
                .handle_table_update_with_primary_key::<player_component::PlayerComponent>(
                    callbacks,
                    table_update,
                ),
            _ => {
                spacetimedb_sdk::log::error!("TableRowOperation on unknown table {:?}", table_name)
            }
        }
    }
    fn invoke_row_callbacks(
        &self,
        reminders: &mut RowCallbackReminders,
        worker: &mut DbCallbacks,
        reducer_event: Option<Arc<AnyReducerEvent>>,
        state: &Arc<ClientCache>,
    ) {
        reminders.invoke_callbacks::<config::Config>(worker, &reducer_event, state);
        reminders.invoke_callbacks::<entity_component::EntityComponent>(
            worker,
            &reducer_event,
            state,
        );
        reminders.invoke_callbacks::<lobby::Lobby>(worker, &reducer_event, state);
        reminders.invoke_callbacks::<player_component::PlayerComponent>(
            worker,
            &reducer_event,
            state,
        );
    }
    fn handle_event(
        &self,
        event: Event,
        _reducer_callbacks: &mut ReducerCallbacks,
        _state: Arc<ClientCache>,
    ) -> Option<Arc<AnyReducerEvent>> {
        let Some(function_call) = &event.function_call else {
            spacetimedb_sdk::log::warn!("Received Event with None function_call");
            return None;
        };
        #[allow(clippy::match_single_binding)]
match &function_call.reducer[..] {
						"create_lobby" => _reducer_callbacks.handle_event_of_type::<create_lobby_reducer::CreateLobbyArgs, ReducerEvent>(event, _state, ReducerEvent::CreateLobby),
			"create_player" => _reducer_callbacks.handle_event_of_type::<create_player_reducer::CreatePlayerArgs, ReducerEvent>(event, _state, ReducerEvent::CreatePlayer),
			"destroy_lobby" => _reducer_callbacks.handle_event_of_type::<destroy_lobby_reducer::DestroyLobbyArgs, ReducerEvent>(event, _state, ReducerEvent::DestroyLobby),
			"filter_lobbies" => _reducer_callbacks.handle_event_of_type::<filter_lobbies_reducer::FilterLobbiesArgs, ReducerEvent>(event, _state, ReducerEvent::FilterLobbies),
			"join_lobby" => _reducer_callbacks.handle_event_of_type::<join_lobby_reducer::JoinLobbyArgs, ReducerEvent>(event, _state, ReducerEvent::JoinLobby),
			"leave_lobby" => _reducer_callbacks.handle_event_of_type::<leave_lobby_reducer::LeaveLobbyArgs, ReducerEvent>(event, _state, ReducerEvent::LeaveLobby),
			"update_player_position" => _reducer_callbacks.handle_event_of_type::<update_player_position_reducer::UpdatePlayerPositionArgs, ReducerEvent>(event, _state, ReducerEvent::UpdatePlayerPosition),
			unknown => { spacetimedb_sdk::log::error!("Event on an unknown reducer: {:?}", unknown); None }
}
    }
    fn handle_resubscribe(
        &self,
        new_subs: TableUpdate,
        client_cache: &mut ClientCache,
        callbacks: &mut RowCallbackReminders,
    ) {
        let table_name = &new_subs.table_name[..];
        match table_name {
            "Config" => {
                client_cache.handle_resubscribe_for_type::<config::Config>(callbacks, new_subs)
            }
            "EntityComponent" => client_cache
                .handle_resubscribe_for_type::<entity_component::EntityComponent>(
                    callbacks, new_subs,
                ),
            "Lobby" => {
                client_cache.handle_resubscribe_for_type::<lobby::Lobby>(callbacks, new_subs)
            }
            "PlayerComponent" => client_cache
                .handle_resubscribe_for_type::<player_component::PlayerComponent>(
                    callbacks, new_subs,
                ),
            _ => {
                spacetimedb_sdk::log::error!("TableRowOperation on unknown table {:?}", table_name)
            }
        }
    }
}

/// Connect to a database named `db_name` accessible over the internet at the URI `spacetimedb_uri`.
///
/// If `credentials` are supplied, they will be passed to the new connection to
/// identify and authenticate the user. Otherwise, a set of `Credentials` will be
/// generated by the server.
pub fn connect<IntoUri>(
    spacetimedb_uri: IntoUri,
    db_name: &str,
    credentials: Option<Credentials>,
) -> Result<()>
where
    IntoUri: TryInto<spacetimedb_sdk::http::Uri>,
    <IntoUri as TryInto<spacetimedb_sdk::http::Uri>>::Error:
        std::error::Error + Send + Sync + 'static,
{
    with_connection_mut(|connection| {
        connection.connect(spacetimedb_uri, db_name, credentials, Arc::new(Module))?;
        Ok(())
    })
}
