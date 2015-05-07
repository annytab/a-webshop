BEGIN TRANSACTION;

/* STATIC TEXTS */
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'gift_cards' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('gift_cards',1,'Presentkort')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'gift_cards' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('gift_cards',2,'Gift cards')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'gift_card' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('gift_card',1,'Presentkort')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'gift_card' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('gift_card',2,'Gift card')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'invalid_gift_card' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('invalid_gift_card',1,'Ogiltigt presentkort')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'invalid_gift_card' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('invalid_gift_card',2,'Invalid gift card')
END;

/* DELETE STATIC TEXT FOR MOBILE DISPLAY VIEW */
DELETE FROM dbo.static_texts WHERE id = 'mobile_display_view';

/* REMOVE THE CATEGORY CONSTRAINT ON INSPIRATION MAPS */
ALTER TABLE [dbo].[inspiration_image_maps] DROP CONSTRAINT [FK_inspiration_image_maps_categories];

/* REMOVE THE MOBILE_DISPLAY_VIEW COLUMN IN DOMAINS */
ALTER TABLE [dbo].[web_domains] DROP COLUMN [mobile_display_view];

/* GIFT CARDS */
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[gift_cards](
	[id] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[language_id] [int] NOT NULL,
	[currency_code] [nchar](3) COLLATE Latin1_General_CI_AI NOT NULL,
	[amount] [decimal](14,0) NOT NULL,
	[end_date] [datetime] NOT NULL);

/* CREATE PRIMARY KEYS */
ALTER TABLE [dbo].[gift_cards] ADD CONSTRAINT [PK_gift_cards] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);

/* CREATE CLUSTERED INDEXES */
CREATE CLUSTERED INDEX [CDX_gift_cards] ON [dbo].[gift_cards] ([language_id] ASC);

/* CREATE FOREIGN KEYS */
ALTER TABLE [dbo].[gift_cards] WITH CHECK ADD CONSTRAINT [FK_gift_cards_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[gift_cards] CHECK CONSTRAINT [FK_gift_cards_languages];
ALTER TABLE [dbo].[gift_cards] WITH CHECK ADD CONSTRAINT [FK_gift_cards_currencies] FOREIGN KEY([currency_code]) REFERENCES [dbo].[currencies] ([currency_code]);
ALTER TABLE [dbo].[gift_cards] CHECK CONSTRAINT [FK_gift_cards_currencies];

/* ORDER */
ALTER TABLE [dbo].[orders] ADD [gift_cards_amount] [decimal](14,0) NOT NULL DEFAULT(0);

/* ORDERS GIFT CARDS */
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[orders_gift_cards](
	[order_id] [int] NOT NULL,
	[gift_card_id] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[amount] [decimal](14,0) NOT NULL);

/* CREATE PRIMARY KEYS */
ALTER TABLE [dbo].[orders_gift_cards] ADD CONSTRAINT [PK_orders_gift_cards] PRIMARY KEY NONCLUSTERED ([order_id] ASC, [gift_card_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);

/* CREATE CLUSTERED INDEXES */
CREATE CLUSTERED INDEX [CDX_orders_gift_cards] ON [dbo].[orders_gift_cards] ([order_id] ASC);

/* CREATE FOREIGN KEYS */
ALTER TABLE [dbo].[orders_gift_cards] WITH CHECK ADD CONSTRAINT [FK_orders_gift_cards_orders] FOREIGN KEY([order_id]) REFERENCES [dbo].[orders] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[orders_gift_cards] CHECK CONSTRAINT [FK_orders_gift_cards_orders];

/* EXCECUTE THE TRANSACTION */
COMMIT;