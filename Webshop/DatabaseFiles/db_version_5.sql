BEGIN TRANSACTION;

/* STATIC TEXTS */
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'discount' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('discount',1,'Rabatt')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'discount' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('discount',2,'Discount')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'regular_price_short' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('regular_price_short',1,'Ord.')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'regular_price_short' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('regular_price_short',2,'Reg.')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'click_count' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('click_count',1,'Antal klick')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'click_count' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('click_count',2,'Click count')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'reset' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('reset',1,'Återställ')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'reset' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('reset',2,'Reset')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'discount_codes' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('discount_codes',1,'Rabattkoder')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'discount_codes' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('discount_codes',2,'Discount codes')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'discount_code' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('discount_code',1,'Rabattkod')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'discount_code' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('discount_code',2,'Discount code')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'end_date' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('end_date',1,'Slutdatum')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'end_date' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('end_date',2,'End date')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'free_freight' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('free_freight',1,'Fri frakt')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'free_freight' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('free_freight',2,'Free freight')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'once_per_customer' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('once_per_customer',1,'En gång per kund')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'once_per_customer' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('once_per_customer',2,'Once per customer')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'statistics' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('statistics',1,'Statistik')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'statistics' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('statistics',2,'Statistics')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'exclude_products_on_sale' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('exclude_products_on_sale',1,'Exkludera produkter med rabatt')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'exclude_products_on_sale' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('exclude_products_on_sale',2,'Exclude products with discount')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'invalid_discount_code' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('invalid_discount_code',1,'Ogiltig rabattkod')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'invalid_discount_code' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('invalid_discount_code',2,'Invalid discount code')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'minimum_order_value' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('minimum_order_value',1,'Minsta ordervärde')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'minimum_order_value' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('minimum_order_value',2,'Minimum order value')
END;

/* CAMPAIGNS */
ALTER TABLE [dbo].[campaigns] ADD [click_count] [int] NOT NULL DEFAULT(0);

/* PRODUCTS */
ALTER TABLE [dbo].[products] ADD [discount] [decimal](4,3) NOT NULL DEFAULT(0);

/* DISCOUNT CODES */
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[discount_codes](
	[id] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[language_id] [int] NOT NULL,
	[currency_code] [nchar](3) COLLATE Latin1_General_CI_AI NOT NULL,
	[discount_value] [decimal](4,3) NOT NULL,
	[free_freight] [tinyint] NOT NULL,
	[end_date] [datetime] NOT NULL,
	[once_per_customer] [tinyint] NOT NULL,
	[exclude_products_on_sale] [tinyint] NOT NULL,
	[minimum_order_value] [decimal](14,2) NOT NULL);

/* CREATE PRIMARY KEYS */
ALTER TABLE [dbo].[discount_codes] ADD CONSTRAINT [PK_discount_codes] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);

/* CREATE CLUSTERED INDEXES */
CREATE CLUSTERED INDEX [CDX_discount_codes] ON [dbo].[discount_codes] ([language_id] ASC);

/* CREATE FOREIGN KEYS */
ALTER TABLE [dbo].[discount_codes] WITH CHECK ADD CONSTRAINT [FK_discount_codes_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[discount_codes] CHECK CONSTRAINT [FK_discount_codes_languages];
ALTER TABLE [dbo].[discount_codes] WITH CHECK ADD CONSTRAINT [FK_discount_codes_currencies] FOREIGN KEY([currency_code]) REFERENCES [dbo].[currencies] ([currency_code]);
ALTER TABLE [dbo].[discount_codes] CHECK CONSTRAINT [FK_discount_codes_currencies];

/* ORDER */
ALTER TABLE [dbo].[orders] ADD [discount_code] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL DEFAULT('');

/* EXCECUTE THE TRANSACTION */
COMMIT;