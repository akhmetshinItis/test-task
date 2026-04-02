-- Подготовка тестовых данных для Postman collection.
-- Перед запуском создайте пользователей через запросы из папки "Подготовка данных" в Postman:
--   admin@vitacore.test
--   bidder1@vitacore.test
--   bidder2@vitacore.test
--   buyer1@vitacore.test

BEGIN;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "AspNetUsers" WHERE email = 'bidder1@vitacore.test') THEN
        RAISE EXCEPTION 'Пользователь bidder1@vitacore.test не найден. Сначала выполните запросы из папки "Подготовка данных".';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM "AspNetUsers" WHERE email = 'bidder2@vitacore.test') THEN
        RAISE EXCEPTION 'Пользователь bidder2@vitacore.test не найден. Сначала выполните запросы из папки "Подготовка данных".';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM "AspNetUsers" WHERE email = 'buyer1@vitacore.test') THEN
        RAISE EXCEPTION 'Пользователь buyer1@vitacore.test не найден. Сначала выполните запросы из папки "Подготовка данных".';
    END IF;
END $$;

DELETE FROM bids
WHERE lot_id IN (
    '10000000-0000-0000-0000-000000000001'::uuid,
    '10000000-0000-0000-0000-000000000002'::uuid,
    '10000000-0000-0000-0000-000000000003'::uuid,
    '10000000-0000-0000-0000-000000000004'::uuid,
    '10000000-0000-0000-0000-000000000005'::uuid
);

DELETE FROM tangerine_lots
WHERE id IN (
    '10000000-0000-0000-0000-000000000001'::uuid,
    '10000000-0000-0000-0000-000000000002'::uuid,
    '10000000-0000-0000-0000-000000000003'::uuid,
    '10000000-0000-0000-0000-000000000004'::uuid,
    '10000000-0000-0000-0000-000000000005'::uuid
);

DELETE FROM outbox_messages;

INSERT INTO tangerine_lots
(
    id,
    title,
    description,
    image_url,
    start_price,
    current_price,
    buyout_price,
    auction_start_at,
    auction_end_at,
    expiration_at,
    status,
    current_leader_user_id,
    buyer_id,
    purchase_type,
    created_at,
    closed_at
)
VALUES
(
    '10000000-0000-0000-0000-000000000001',
    'Seed Active Bid Lot',
    'Лот для проверки ставок и уведомления о перебитой ставке.',
    'https://cdn.example.com/tangerines/seed-active-bid.png',
    100.00,
    100.00,
    350.00,
    now() - interval '1 hour',
    now() + interval '7 day',
    now() + interval '14 day',
    1,
    NULL,
    NULL,
    NULL,
    now() - interval '1 hour',
    NULL
),
(
    '10000000-0000-0000-0000-000000000002',
    'Seed Active Buyout Lot',
    'Лот для проверки выкупа и отправки чека.',
    'https://cdn.example.com/tangerines/seed-active-buyout.png',
    120.00,
    120.00,
    260.00,
    now() - interval '1 hour',
    now() + interval '7 day',
    now() + interval '14 day',
    1,
    NULL,
    NULL,
    NULL,
    now() - interval '1 hour',
    NULL
),
(
    '10000000-0000-0000-0000-000000000003',
    'Seed Ended Lot With Winner',
    'Лот для проверки завершения аукциона и письма победителю.',
    'https://cdn.example.com/tangerines/seed-ended-winner.png',
    90.00,
    170.00,
    300.00,
    now() - interval '3 day',
    now() - interval '1 hour',
    now() + interval '3 day',
    1,
    (SELECT id FROM "AspNetUsers" WHERE email = 'bidder1@vitacore.test' LIMIT 1),
    NULL,
    NULL,
    now() - interval '3 day',
    NULL
),
(
    '10000000-0000-0000-0000-000000000004',
    'Seed Ended Lot Without Winner',
    'Лот для проверки завершения аукциона без победителя.',
    'https://cdn.example.com/tangerines/seed-ended-no-winner.png',
    95.00,
    95.00,
    240.00,
    now() - interval '3 day',
    now() - interval '1 hour',
    now() + interval '3 day',
    1,
    NULL,
    NULL,
    NULL,
    now() - interval '3 day',
    NULL
),
(
    '10000000-0000-0000-0000-000000000005',
    'Seed Expired Spoiled Lot',
    'Лот для проверки ежедневной очистки просроченных мандаринок.',
    'https://cdn.example.com/tangerines/seed-expired-spoiled.png',
    50.00,
    50.00,
    110.00,
    now() - interval '10 day',
    now() - interval '8 day',
    now() - interval '1 day',
    4,
    NULL,
    NULL,
    NULL,
    now() - interval '10 day',
    now() - interval '8 day'
);

COMMIT;
