# Feature: Token Expiration and Visual Timer

## Description
Modify the token persistence mechanism to include an expiration time limit, replacing the current indefinite persistence.
Additionally, create a visual timer in the `AppLayout` component, located next to the logged-in user's initials icon, to indicate the remaining time before the token expires.

## Classification
- Type: Feature
- Component: Frontend (DPManagement.Web)
- Areas: Authentication Context, AppLayout Component

## Requirements
1. Token should not persist indefinitely.
2. Token must have a defined expiration time.
3. A visual timer must be added to `AppLayout` next to the user initials icon.
4. Timer should clearly show remaining time before token expiration.
