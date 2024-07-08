mod module_bindings;
use module_bindings::*;

use spacetimedb_sdk::{
    Address,
    disconnect,
    identity::{load_credentials, once_on_connect, save_credentials, Credentials, Identity},
    on_disconnect, on_subscription_applied,
    reducer::Status,
    subscribe,
    table::{TableType, TableWithPrimaryKey},
};

fn main() {

}