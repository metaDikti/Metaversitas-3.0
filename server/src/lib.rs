pub mod tables;
use log;
use spacetimedb::{spacetimedb, ReducerContext, Identity, Timestamp};
use tables::*; 
// Menghapus impor yang tidak digunakan
// use std::time::{SystemTime, Duration, UNIX_EPOCH};

#[spacetimedb(reducer)]
pub fn public_chat_message(ctx: ReducerContext, lobby_id: u32, message: String) -> Result<(), String> {
    let sender_id = ctx.sender;
    let lobby = Lobby::filter_by_id(&lobby_id).ok_or("Lobby not found")?;
    if !lobby.participants.contains(&sender_id) {
        return Err("Sender is not part of the lobby".to_string());
    }

    ChatMessage::insert(ChatMessage {
        chat_entity_id: 0,
        source_entity_id: sender_id,
        lobby_id: lobby_id,
        recipient_id: None,
        chat_text: message,
        timestamp: ctx.timestamp,
    }).expect("Failed to insert chat message.");

    Ok(())
}

#[spacetimedb(reducer)]
pub fn send_private_message(ctx: ReducerContext, lobby_id: u32, sender_id: u64, recipient_id: u64, message: String) -> Result<(), String> {
    let lobby = Lobby::filter_by_id(&lobby_id).ok_or("Lobby not found")?;

    let sender_identity = Identity::from_hashing_bytes(sender_id.to_be_bytes());
    let recipient_identity = Identity::from_hashing_bytes(recipient_id.to_be_bytes());

    if !lobby.participants.contains(&sender_identity) || !lobby.participants.contains(&recipient_identity) {
        return Err("One or both players are not part of the lobby".to_string());
    }

    ChatMessage::insert(ChatMessage {
        chat_entity_id: 0,
        source_entity_id: sender_identity,
        lobby_id: lobby_id,
        recipient_id: Some(recipient_identity),
        chat_text: message,
        timestamp: ctx.timestamp,
    }).expect("Failed to insert chat message.");

    Ok(())
}

fn verify_password(provided_password: &str, stored_password: &str) -> bool {
    provided_password == stored_password
}

fn auth_header_value(token: &str) -> String {
    let username_and_password = format!("token:{}", token);
    let base64_encoded = base64::encode(username_and_password);
    let base64_string: String = base64_encoded.into();
    format!("Basic {}", base64_string)
}

#[spacetimedb(reducer)]
pub fn create_lobby(ctx: ReducerContext, id: u32, Kelas: String, Materi: String, Pertemuan: String, participants: Vec<u64>) -> Result<(), String> {
    spacetimedb::dbg!(&id, &Kelas, &Materi, &Pertemuan, &participants);
    let existing_lobbies = Lobby::iter().filter(|l| 
        l.Kelas == Kelas && l.Materi == Materi && l.Pertemuan == Pertemuan
    ).collect::<Vec<Lobby>>();

    if !existing_lobbies.is_empty() {
        return Err("A lobby with the same attributes already exists".to_string());
    }

    let new_lobby = Lobby {
        id: 0,
        identity: ctx.sender,
        Kelas,
        Materi,
        Pertemuan,
        TimeLimit: 10800,
        participants: Vec::new(),
    };
    Lobby::insert(new_lobby)?;
    Ok(())
}

#[spacetimedb(reducer)]
pub fn filter_lobbies(_ctx: ReducerContext, kelas: String, materi: String, pertemuan: String) -> Result<(), String> {
    let mut found = false;
    for lobby in Lobby::iter().filter(|l| 
        l.Kelas == kelas && l.Materi == materi && l.Pertemuan == pertemuan
    ) {
        found = true;
        log::info!("Mencoba Filtering Lobby: ID: {}, Kelas: {}, Materi: {}, Pertemuan: {}",
                   lobby.id, lobby.Kelas, lobby.Materi, lobby.Pertemuan);
    }
    if !found {
        log::info!("No lobby found matching the criteria");
    }
    Ok(())
}

#[spacetimedb(reducer)]
pub fn check_and_update_lobbies() -> Result<(), String> {
    for lobby in Lobby::iter() {
        let time_remaining = lobby.TimeLimit - 1;
        update_lobby_time_limit(lobby.id, time_remaining)?;
        log::info!("Checking Update Lobby: ID: {}, Kelas: {}, Materi: {}, Pertemuan: {}, TimeLimit {}",
                   lobby.id, lobby.Kelas, lobby.Materi, lobby.Pertemuan, time_remaining);
        if time_remaining == 0 {
            Lobby::delete_by_id(&lobby.id);
        }
    }
    Ok(())
}

#[spacetimedb(reducer)]
pub fn update_lobby_time_limit(lobby_id: u32, new_time_limit: u64) -> Result<(), String> {
    let lobby = Lobby::filter_by_id(&lobby_id).expect("Lobby not found");
    let mut updated_lobby = lobby.clone();
    updated_lobby.TimeLimit = new_time_limit;
    Lobby::update_by_id(&lobby.id, updated_lobby);
    Ok(())
}

#[spacetimedb(reducer)]
pub fn destroy_lobby(_ctx: ReducerContext, kelas: String, materi: String, pertemuan: String) -> Result<(), String> {
    for lobby in Lobby::iter().filter(|l| 
        l.Kelas == kelas && l.Materi == materi && l.Pertemuan == pertemuan
    ) {
        Lobby::delete_by_id(&lobby.id);
    }
    Ok(())
}

#[spacetimedb(reducer)]
pub fn join_lobby(ctx: ReducerContext, kelas: String, materi: String, pertemuan: String) -> Result<(), String> {
    let player_entity_id = ctx.sender;
    let mut filtered_lobbies = Lobby::iter()
        .filter(|l| l.Kelas == kelas && l.Materi == materi && l.Pertemuan == pertemuan)
        .collect::<Vec<Lobby>>();

    if let Some(mut lobby) = filtered_lobbies.pop() {
        let lobby_id = lobby.id;
        if !lobby.participants.contains(&player_entity_id) {
            lobby.participants.push(player_entity_id);
            if !Lobby::update_by_id(&lobby_id, lobby) {
                return Err("Failed to update the lobby".to_string());
            }
            Ok(())
        } else {
            Err("Player already joined this lobby".to_string())
        }
    } else {
        Err("Lobby not found or not open".to_string())
    }
}

#[spacetimedb(reducer)]
pub fn matchmaking(ctx: ReducerContext, kelas: String, materi: String, pertemuan: String) -> Result<(), String> {
    let mut existing_lobbies = Lobby::iter()
        .filter(|l| l.Kelas == kelas && l.Materi == materi && l.Pertemuan == pertemuan)
        .collect::<Vec<Lobby>>();

    if let Some(mut lobby) = existing_lobbies.pop() {
        if !lobby.participants.contains(&ctx.sender) {
            lobby.participants.push(ctx.sender);
            let lobby_id = lobby.id.clone();
            if !Lobby::update_by_id(&lobby_id, lobby.clone()) {
                return Err("Gagal memperbarui lobby".to_string());
            }
        } else {
            return Err("Pemain sudah bergabung dalam lobby ini".to_string());
        }
    } else {
        let new_lobby = Lobby {
            id: 0,
            identity: ctx.sender,
            Kelas: kelas.clone(),
            Materi: materi.clone(),
            Pertemuan: pertemuan.clone(),
            TimeLimit: 10800,
            participants: vec![ctx.sender],
        };
        if let Err(_) = Lobby::insert(new_lobby.clone()) {
            return Err("Gagal membuat lobby baru".to_string());
        }
    }

    Ok(())
}

#[spacetimedb(reducer)]
pub fn leave_lobby(ctx: ReducerContext, kelas: String, materi: String, pertemuan: String) -> Result<(), String> {
    let player_entity_id = ctx.sender;
    let mut filtered_lobbies = Lobby::iter()
        .filter(|l| l.Kelas == kelas && l.Materi == materi && l.Pertemuan == pertemuan)
        .collect::<Vec<Lobby>>();

    if let Some(mut lobby) = filtered_lobbies.pop() {
        if let Some(pos) = lobby.participants.iter().position(|&id| id == player_entity_id) {
            lobby.participants.remove(pos);
            let lobby_id = lobby.id;
            if Lobby::update_by_id(&lobby_id, lobby) {
                Ok(())
            } else {
                Err("Failed to update the lobby".to_string())
            }
        } else {
            Err("Player not found in this lobby".to_string())
        }
    } else {
        Err("Lobby with the specified attributes not found".to_string())
    }
}

#[spacetimedb(reducer)]
pub fn create_player(ctx: ReducerContext, role: String, gender: String, uuid: String, full_name: String, nickname: String, universitas: String, kodeuniv: String, fakultas: String,
    jurusan: String,) -> Result<(), String> {
    let owner_id = ctx.sender;

    if PlayerComponent::filter_by_owner_id(&owner_id).is_some() {
        log::info!("Player already exists");
        return Err("Player already exists".to_string());
    }

    let entity_id = SpawnableEntityComponent::insert(SpawnableEntityComponent { entity_id: 0 })
        .expect("Failed to create player spawnable entity component.")
        .entity_id;

    PlayerComponent::insert(PlayerComponent {
        entity_id,
        owner_id,
        role,
        gender,
        uuid,
        full_name,
        nickname: nickname.clone(),
        universitas,
        kodeuniv,
        fakultas,
        jurusan,
        in_game: false,
        logged_in: true,
    }).expect("Failed to insert player component.");

    MobileEntityComponent::insert(MobileEntityComponent {
        entity_id,
        position: StdbVector3 { x: 0.0, y: 0.0, z: 0.0 },
        direction: StdbVector2::ZERO,
        rotation: StdbQuaternion {x: 0.0, y: 0.0, z: 0.0, w: 0.0},
        rotvalue: 0.0,
        move_start_timestamp: Timestamp::UNIX_EPOCH,
        rotate_start_timestamp: Timestamp::UNIX_EPOCH,
    }).expect("Failed to create MobileEntityComponent.");

    AnimationComponent::insert(AnimationComponent {
        entity_id,
        interact_start_timestamp: Timestamp::UNIX_EPOCH,
        action_target_entity_id: 0,
    })
    .expect("Failed to insert player animation component.");

    log::info!("Player created: {}({})", nickname, entity_id);

    Ok(())
}

#[spacetimedb(reducer)]
pub fn move_player(
    ctx: ReducerContext,
    position: StdbVector3,
    direction: StdbVector2,
    _moving: bool,
    _sprinting: bool,
) -> Result<(), String> {
    let owner_id = ctx.sender;
    if let Some(player) = PlayerComponent::filter_by_owner_id(&owner_id) {
        if let Some(mut entity) = MobileEntityComponent::filter_by_entity_id(&player.entity_id) {
            entity.position = position;
            entity.direction = direction;
            entity.move_start_timestamp = ctx.timestamp;
            MobileEntityComponent::update_by_entity_id(&player.entity_id, entity);
            return Ok(());
        }
    }
    Err("Player not found".to_string())
}

#[spacetimedb(reducer)]
pub fn stop_player(
    ctx: ReducerContext,
    position: StdbVector3,
    direction: StdbVector2,
) -> Result<(), String> {
    let owner_id = ctx.sender;
    if let Some(player) = PlayerComponent::filter_by_owner_id(&owner_id) {
        if let Some(mut entity) = MobileEntityComponent::filter_by_entity_id(&player.entity_id) {
            entity.position = position;
            entity.direction = direction;
            MobileEntityComponent::update_by_entity_id(&player.entity_id, entity);
            return Ok(());
        }
    }
    Err("Player not found".to_string())
}

#[spacetimedb(reducer)]
pub fn rotate_player(
    ctx: ReducerContext,
    rotation: StdbQuaternion,
    rotvalue: f32,
) -> Result<(), String> {
    let owner_id = ctx.sender;
    if let Some(player) = PlayerComponent::filter_by_owner_id(&owner_id) {
        if let Some(mut entity) = MobileEntityComponent::filter_by_entity_id(&player.entity_id) {
            entity.rotation = rotation;
            entity.rotvalue = rotvalue; 
            entity.rotate_start_timestamp = ctx.timestamp;
            MobileEntityComponent::update_by_entity_id(&player.entity_id, entity);
            return Ok(());
        }
    }
    Err("Player not found".to_string())
}

#[spacetimedb(reducer)]
pub fn stop_rotate_player(
    ctx: ReducerContext,
    rotation: StdbQuaternion,
    _rotvalue: f32,
) -> Result<(), String> {
    let owner_id = ctx.sender;
    if let Some(player) = PlayerComponent::filter_by_owner_id(&owner_id) {
        if let Some(mut entity) = MobileEntityComponent::filter_by_entity_id(&player.entity_id) {
            entity.rotation = rotation;
            entity.rotvalue = 0.0;
            MobileEntityComponent::update_by_entity_id(&player.entity_id, entity);
            return Ok(());
        }
    }
    Err("Player not found".to_string())
}

#[spacetimedb(init)]
pub fn init() {
    Config::insert(Config {
        version: 0,
        message_of_the_day: "Hello, World!".to_string(),
    }).expect("Failed to insert config.");
    
    spacetimedb::schedule!("1s", check_and_update_lobbies());
}

#[spacetimedb(connect)]
pub fn client_connected(ctx: ReducerContext) {
    update_player_login_state(ctx, true);
}

#[spacetimedb(disconnect)]
pub fn client_disconnected(ctx: ReducerContext) {
    update_player_login_state(ctx, false);
    update_player_in_game_status(ctx, false);
}

pub fn update_player_login_state(ctx: ReducerContext, logged_in: bool) {
    if let Some(player) = PlayerComponent::filter_by_owner_id(&ctx.sender) {
        let entity_id = player.entity_id;
        let mut player = player.clone();
        player.logged_in = logged_in;
        PlayerComponent::update_by_entity_id(&entity_id, player);
    }
}

#[spacetimedb(reducer)]
pub fn update_player_in_game_status(ctx: ReducerContext, in_game: bool){
    if let Some(player) = PlayerComponent::filter_by_owner_id(&ctx.sender) {
        let entity_id = player.entity_id;
        let mut player = player.clone();
        player.in_game = in_game;
        PlayerComponent::update_by_entity_id(&entity_id, player);
    }
}

#[spacetimedb(reducer)]
pub fn interact(ctx: ReducerContext, entity_id: u64) -> Result<(), String> {
    let player = PlayerComponent::filter_by_entity_id(&entity_id).expect("This player doesn't exist!");

    if player.owner_id != ctx.sender {
        log::info!("This identity doesn't own this player! (allowed for now)");
    }

    if let Some(mut anim_comp) = AnimationComponent::filter_by_entity_id(&entity_id) {
        anim_comp.interact_start_timestamp = ctx.timestamp;

        AnimationComponent::update_by_entity_id(&entity_id, anim_comp);
        return Ok(());
    }

    Err("AnimationComponent not found".to_string())
}