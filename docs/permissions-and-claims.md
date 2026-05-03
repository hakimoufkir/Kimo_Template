# Berechtigungen und Claims

In diesem Abschnitt werden die Berechtigungen und weiteren Claims beschrieben, die in den JWTs enthalten sein können.

## Active Directory Groups

| Umgebung   | Rolle                               |
|------------|-------------------------------------|
| Test       | KIMO\\LG_SPT1_Apps_ZLApp       |
| Test       | KIMO\\LG_SPT1_Apps_ZLApp_Admin |
| Staging    | KIMO\\LG_SPK1_Apps_ZLApp       |
| Staging    | KIMO\\LG_SPK1_Apps_ZLApp_Admin |
| Production | KIMO\\LG_SPP1_Apps_ZLApp       |
| Production | KIMO\\LG_SPP1_Apps_ZLApp_Admin |

## Berechtigungen

Der Claim-Typ für Berechtigungen ist `zlapp_permission`.
Die möglichen Berechtigungswerte sind:

| Name     | Beschreibung                                              |
|----------|-----------------------------------------------------------|
| `read`   | Der Benutzer kann auf die Ressource lesend zugreifen.     |
| `write`  | Der Benutzer kann auf die Ressource schreibend zugreifen. |
| `delete` | Der Benutzer kann die Ressource löschen.                  |

## Claims

| Claim Type                           | Scopes                    | Type   | Example Value | Description                       |
|--------------------------------------|---------------------------|--------|---------------|-----------------------------------|
| `zlapp_permission` | `zlapp` | String | `read`        | Die Rolle des Benutzers.          |
| `email`                              |                           | String |               | Die E-Mail-Adresse des Benutzers. |             
