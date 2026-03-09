# Feature: Manage Permissions

## Description
Provide a user interface to manage system permissions directly from the frontend, rather than requiring direct database inserts. The backend `PermissoesController` already supports full CRUD operations (GET, POST, PUT, DELETE, PATCH).

## Requirements
- Display a list of all existing permissions (Module, Action, Description, Status).
- Allow the creation of new permissions (POST).
- Allow editing of existing permissions (PUT).
- Allow deleting or activating/inactivating permissions.
- Integrate this visually into the "Profiles and Permissions" routine, renaming the sidebar item to "Perfis e Permissões" and splitting the `Perfis.tsx` screen into two tabs (or adding a separate page).
