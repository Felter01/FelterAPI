
-- FelterAPI Ecommerce Cleanup (snake_case padrão)
-- Rode com cuidado no PostgreSQL (schema ecommerce_felter)

BEGIN;

-- =========================
-- USERS
-- Mantém: id, client_id, name, email, password_hash, role, permissions, is_active, created_at
-- Remove duplicadas
-- =========================
ALTER TABLE ecommerce_felter.users
    DROP COLUMN IF EXISTS "Id",
    DROP COLUMN IF EXISTS "Name",
    DROP COLUMN IF EXISTS "Email",
    DROP COLUMN IF EXISTS "PasswordHash",
    DROP COLUMN IF EXISTS "IsActive",
    DROP COLUMN IF EXISTS "CreatedAt",
    DROP COLUMN IF EXISTS "IsSuperAdmin",
    DROP COLUMN IF EXISTS clientid,
    DROP COLUMN IF EXISTS password,
    DROP COLUMN IF EXISTS isactive,
    DROP COLUMN IF EXISTS createdat;

-- Garante colunas padrão
ALTER TABLE ecommerce_felter.users
    ADD COLUMN IF NOT EXISTS client_id uuid,
    ADD COLUMN IF NOT EXISTS name text,
    ADD COLUMN IF NOT EXISTS email text,
    ADD COLUMN IF NOT EXISTS password_hash text,
    ADD COLUMN IF NOT EXISTS role text,
    ADD COLUMN IF NOT EXISTS permissions text,
    ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT true,
    ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

-- =========================
-- CLIENTS
-- Mantém snake_case padrão
-- =========================
ALTER TABLE ecommerce_felter.clients
    DROP COLUMN IF EXISTS "Id",
    DROP COLUMN IF EXISTS "Name",
    DROP COLUMN IF EXISTS "Email",
    DROP COLUMN IF EXISTS "Plan",
    DROP COLUMN IF EXISTS "TradeName",
    DROP COLUMN IF EXISTS "Cnpj",
    DROP COLUMN IF EXISTS "Phone",
    DROP COLUMN IF EXISTS "Slug",
    DROP COLUMN IF EXISTS "CustomUrl",
    DROP COLUMN IF EXISTS "FirebaseApiKey",
    DROP COLUMN IF EXISTS "FirebaseAuthDomain",
    DROP COLUMN IF EXISTS "FirebaseProjectId",
    DROP COLUMN IF EXISTS "FirebaseStorageBucket",
    DROP COLUMN IF EXISTS "FirebaseSenderId",
    DROP COLUMN IF EXISTS "FirebaseAppId",
    DROP COLUMN IF EXISTS "FirebaseMeasurementId",
    DROP COLUMN IF EXISTS "FirebaseServiceAccountJson",
    DROP COLUMN IF EXISTS "CreatedAt";

ALTER TABLE ecommerce_felter.clients
    ADD COLUMN IF NOT EXISTS master_id uuid NULL,
    ADD COLUMN IF NOT EXISTS name text,
    ADD COLUMN IF NOT EXISTS email text,
    ADD COLUMN IF NOT EXISTS plan text DEFAULT 'basic',
    ADD COLUMN IF NOT EXISTS trade_name text NULL,
    ADD COLUMN IF NOT EXISTS cnpj text NULL,
    ADD COLUMN IF NOT EXISTS phone text NULL,
    ADD COLUMN IF NOT EXISTS slug text,
    ADD COLUMN IF NOT EXISTS custom_url text NULL,
    ADD COLUMN IF NOT EXISTS firebase_api_key text NULL,
    ADD COLUMN IF NOT EXISTS firebase_auth_domain text NULL,
    ADD COLUMN IF NOT EXISTS firebase_project_id text NULL,
    ADD COLUMN IF NOT EXISTS firebase_storage_bucket text NULL,
    ADD COLUMN IF NOT EXISTS firebase_sender_id text NULL,
    ADD COLUMN IF NOT EXISTS firebase_app_id text NULL,
    ADD COLUMN IF NOT EXISTS firebase_measurement_id text NULL,
    ADD COLUMN IF NOT EXISTS firebase_service_account_json text NULL,
    ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

-- =========================
-- MODULES
-- Mantém: id, client_id, key, name, is_enabled, created_at
-- =========================
ALTER TABLE ecommerce_felter.modules
    DROP COLUMN IF EXISTS "Enabled",
    DROP COLUMN IF EXISTS enabled;

ALTER TABLE ecommerce_felter.modules
    ADD COLUMN IF NOT EXISTS is_enabled boolean DEFAULT true;

-- =========================
-- DB_CONFIG
-- Mantém: id, client_id, firebase_json, status, created_at
-- =========================
ALTER TABLE ecommerce_felter.db_config
    DROP COLUMN IF EXISTS "FirebaseJson",
    DROP COLUMN IF EXISTS "Status",
    DROP COLUMN IF EXISTS "CreatedAt";

ALTER TABLE ecommerce_felter.db_config
    ADD COLUMN IF NOT EXISTS firebase_json text,
    ADD COLUMN IF NOT EXISTS status text DEFAULT 'active',
    ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

COMMIT;
