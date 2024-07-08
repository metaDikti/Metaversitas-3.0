// THIS FILE IS AUTOMATICALLY GENERATED BY SPACETIMEDB. EDITS TO THIS FILE
// WILL NOT BE SAVED. MODIFY TABLES IN RUST INSTEAD.

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
pub struct JoinLobbyArgs {
    pub kelas: String,
    pub materi: String,
    pub pertemuan: String,
}

impl Reducer for JoinLobbyArgs {
    const REDUCER_NAME: &'static str = "join_lobby";
}

#[allow(unused)]
pub fn join_lobby(kelas: String, materi: String, pertemuan: String) {
    JoinLobbyArgs {
        kelas,
        materi,
        pertemuan,
    }
    .invoke();
}

#[allow(unused)]
pub fn on_join_lobby(
    mut __callback: impl FnMut(&Identity, Option<Address>, &Status, &String, &String, &String)
        + Send
        + 'static,
) -> ReducerCallbackId<JoinLobbyArgs> {
    JoinLobbyArgs::on_reducer(move |__identity, __addr, __status, __args| {
        let JoinLobbyArgs {
            kelas,
            materi,
            pertemuan,
        } = __args;
        __callback(__identity, __addr, __status, kelas, materi, pertemuan);
    })
}

#[allow(unused)]
pub fn once_on_join_lobby(
    __callback: impl FnOnce(&Identity, Option<Address>, &Status, &String, &String, &String)
        + Send
        + 'static,
) -> ReducerCallbackId<JoinLobbyArgs> {
    JoinLobbyArgs::once_on_reducer(move |__identity, __addr, __status, __args| {
        let JoinLobbyArgs {
            kelas,
            materi,
            pertemuan,
        } = __args;
        __callback(__identity, __addr, __status, kelas, materi, pertemuan);
    })
}

#[allow(unused)]
pub fn remove_on_join_lobby(id: ReducerCallbackId<JoinLobbyArgs>) {
    JoinLobbyArgs::remove_on_reducer(id);
}
