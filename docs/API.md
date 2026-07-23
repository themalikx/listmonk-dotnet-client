# Listmonk API Coverage

This SDK implements the documented Listmonk API areas that are most commonly used in application integration.

## Authentication

- Basic Auth
- `Authorization: token ...`

## Supported endpoints

### Lists

- `GET /api/lists`
- `GET /api/public/lists`
- `GET /api/lists/{list_id}`
- `POST /api/lists`
- `PUT /api/lists/{list_id}`
- `DELETE /api/lists/{list_id}`
- `DELETE /api/lists`

### Subscribers

- `GET /api/subscribers`
- `GET /api/subscribers/{subscriber_id}`
- `GET /api/subscribers/{subscriber_id}/activity`
- `GET /api/subscribers/{subscriber_id}/export`
- `GET /api/subscribers/{subscriber_id}/bounces`
- `POST /api/subscribers`
- `POST /api/subscribers/{subscriber_id}/optin`
- `POST /api/public/subscription`
- `PUT /api/subscribers/lists`
- `PUT /api/subscribers/query/lists`
- `PUT /api/subscribers/{subscriber_id}`
- `PATCH /api/subscribers/{subscriber_id}`
- `PUT /api/subscribers/{subscriber_id}/blocklist`
- `PUT /api/subscribers/blocklist`
- `PUT /api/subscribers/query/blocklist`
- `DELETE /api/subscribers/{subscriber_id}`
- `DELETE /api/subscribers/{subscriber_id}/bounces`
- `DELETE /api/subscribers`
- `POST /api/subscribers/query/delete`

### Campaigns

- `GET /api/campaigns`
- `GET /api/campaigns/{campaign_id}`
- `GET /api/campaigns/{campaign_id}/preview`
- `GET /api/campaigns/running/stats`
- `GET /api/campaigns/analytics/{type}`
- `POST /api/campaigns`
- `POST /api/campaigns/{campaign_id}/test`
- `PUT /api/campaigns/{campaign_id}`
- `PUT /api/campaigns/{campaign_id}/status`
- `PUT /api/campaigns/{campaign_id}/archive`
- `DELETE /api/campaigns/{campaign_id}`
- `DELETE /api/campaigns`

### Templates

- `GET /api/templates`
- `GET /api/templates/{template_id}`
- `GET /api/templates/{template_id}/preview`
- `POST /api/templates`
- `POST /api/templates/preview`
- `PUT /api/templates/{template_id}`
- `PUT /api/templates/{template_id}/default`
- `DELETE /api/templates/{template_id}`

### Media

- `GET /api/media`
- `GET /api/media/{media_id}`
- `POST /api/media`
- `DELETE /api/media/{media_id}`

### Import

- `GET /api/import/subscribers`
- `GET /api/import/subscribers/logs`
- `POST /api/import/subscribers`
- `DELETE /api/import/subscribers`

### Bounces

- `GET /api/bounces`
- `PUT /api/bounces/blocklist`
- `DELETE /api/bounces`
- `DELETE /api/bounces/{bounce_id}`

### Transactional

- `POST /api/tx`

## Implementation notes

- All JSON models use snake_case serialization.
- Envelope responses are deserialized from `{ "data": ... }`.
- Multipart helpers are included for media upload, import, and transactional attachments.
