use spacetimedb::{spacetimedb, Identity, SpacetimeType, Timestamp};

// We're using this table as a singleton, so there should typically only be one element where the version is 0.
#[spacetimedb(table)]
#[derive(Clone)]
pub struct Config {
    #[primarykey]
    pub version: u32,
    pub message_of_the_day: String,
}

#[spacetimedb(table(public))]
#[derive(Clone)]
pub struct Lobby {
    #[primarykey]
    #[autoinc]
    pub id: u32, // This field will auto-increment, ensuring each Lobby has a unique primary key
    pub identity: Identity,
    pub Kelas: String,
    pub Materi: String,
    pub Pertemuan: String,
    pub TimeLimit: u64,
    pub participants: Vec<Identity>,
    //pub games_played: u32,
}

#[derive(SpacetimeType, Clone)]
pub struct Exit {
    pub direction: String,
    pub examine: String,
    pub destination_room_id: String,
}

// This allows us to store 3D points in tables.
#[derive(SpacetimeType, Clone)]
pub struct StdbVector3 {
    pub x: f32,
    pub y: f32,
    pub z: f32,
}

#[derive(SpacetimeType, Clone)]
pub struct StdbVector2 {
    // A spacetime type which can be used in tables and reducers to represent
    // a 2d position.
    pub x: f32,
    pub z: f32,
}

impl StdbVector2 {
    // this allows us to use StdbVector2::ZERO in reducers
    pub const ZERO: StdbVector2 = StdbVector2 { x: 0.0, z: 0.0 };
}

#[derive(SpacetimeType, Clone)]
pub struct StdbQuaternion {
    pub x: f32,
    pub y: f32,
    pub z: f32,
    pub w: f32,
}

#[spacetimedb(table(public))]
pub struct SpawnableEntityComponent {
    // All entities that can be spawned in the world will have this component.
    // This allows us to find all objects in the world by iterating through
    // this table. It also ensures that all world objects have a unique
    // entity_id.
    #[primarykey]
    #[autoinc]
    pub entity_id: u64,
}

// This stores information related to all entities in our game. In this tutorial
// all entities must at least have an entity_id, a position, a direction and they
// must specify whether or not they are moving.
#[spacetimedb(table(public))]
#[derive(Clone)]
pub struct MobileEntityComponent {
    #[primarykey]
    pub entity_id: u64,
    // The autoinc macro here just means every time we insert into this table
    // we will receive a new row where this value will be increased by one. This
    // allows us to easily get rows where `entity_id` is unique.
    pub position: StdbVector3,
    pub direction: StdbVector2,
    pub rotation: StdbQuaternion,
    pub rotvalue: f32,
    pub move_start_timestamp: Timestamp,
    pub rotate_start_timestamp: Timestamp,
}

// All players have this component and it associates an entity with the user's 
// Identity. It also stores their username and whether or not they're logged in.
#[derive(Clone)]
#[spacetimedb(table(public))]
pub struct PlayerComponent {
    // An entity_id that matches an entity_id in the `EntityComponent` table.
    #[primarykey]
    pub entity_id: u64,
    // The user's identity, which is unique to each player
    #[unique]
    pub owner_id: Identity,
    pub role: String,
    pub uuid: String,
    pub full_name: String,
    pub nickname: String,
    pub gender: String,
    pub universitas: String,
    pub kodeuniv: String,
    pub fakultas: String,
    pub jurusan: String,
    pub in_game : bool,
    pub logged_in: bool,
}

#[spacetimedb(table(public))]
pub struct AnimationComponent {
    #[primarykey]
    pub entity_id: u64,
    pub interact_start_timestamp: Timestamp,
    pub action_target_entity_id: u64,
}

// table  for chat message
#[spacetimedb(table(public))]
pub struct ChatMessage {
    #[primarykey]
    #[autoinc]
    pub chat_entity_id: u64, // Auto-incrementing primary key for each chat message.
    pub source_entity_id: Identity, // Entity ID of the player sending the message.
    pub lobby_id: u32, // Lobby ID to ensure the message is associated with a specific lobby.
    pub recipient_id: Option<Identity>, // Optional. Entity ID of the player receiving the message. None if the message is public.
    pub chat_text: String, // The content of the chat message.
    pub timestamp: Timestamp, // Timestamp when the message was sent.
}