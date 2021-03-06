﻿BEGIN TRANSACTION;

/* DROP FOREIGN KEYS */
ALTER TABLE [dbo].[discount_codes] DROP CONSTRAINT [FK_discount_codes_currencies];
ALTER TABLE [dbo].[gift_cards] DROP CONSTRAINT [FK_gift_cards_currencies];
ALTER TABLE [dbo].[web_domains] DROP CONSTRAINT [FK_web_domains_currencies];

/* DROP PRIMARY KEYS */
ALTER TABLE [dbo].[currencies] DROP CONSTRAINT [PK_currencies];
ALTER TABLE [dbo].[custom_themes_templates] DROP CONSTRAINT [PK_custom_themes_templates];
ALTER TABLE [dbo].[discount_codes] DROP CONSTRAINT [PK_discount_codes];
ALTER TABLE [dbo].[gift_cards] DROP CONSTRAINT [PK_gift_cards];
ALTER TABLE [dbo].[order_rows] DROP CONSTRAINT [PK_order_rows];
ALTER TABLE [dbo].[orders_gift_cards] DROP CONSTRAINT [PK_orders_gift_cards];
ALTER TABLE [dbo].[static_texts] DROP CONSTRAINT [PK_static_texts];
ALTER TABLE [dbo].[webshop_sessions] DROP CONSTRAINT [PK_webshop_sessions];
ALTER TABLE [dbo].[webshop_settings] DROP CONSTRAINT [PK_webshop_settings];

/* DROP UNIQUE KEYS */
ALTER TABLE [dbo].[administrators] DROP CONSTRAINT [UK_administrators_user_name];
ALTER TABLE [dbo].[categories_detail] DROP CONSTRAINT [UK_categories_detail_page_name];
ALTER TABLE [dbo].[customers] DROP CONSTRAINT [UK_customers_email];
ALTER TABLE [dbo].[products_detail] DROP CONSTRAINT [UK_products_detail_page_name];
ALTER TABLE [dbo].[static_pages_detail] DROP CONSTRAINT [UK_static_pages_detail_page_name];
ALTER TABLE [dbo].[web_domains] DROP CONSTRAINT [UK_web_domains_domain_name];

/* DROP INDEXES */
DROP INDEX [IX_campaigns_language] ON [dbo].[campaigns];
DROP INDEX [IX_customers_org_number] ON [dbo].[customers];
DROP INDEX [IX_customers_facebook_id] ON [dbo].[customers];
DROP INDEX [IX_customers_google_id] ON [dbo].[customers];
DROP INDEX [IX_orders_document_type] ON [dbo].[orders];
DROP INDEX [IX_orders_exported_to_erp] ON [dbo].[orders];
DROP INDEX [IX_orders_discount_code] ON [dbo].[orders];
DROP INDEX [IX_orders_gift_cards_gift_card_id] ON [dbo].[orders_gift_cards];
DROP INDEX [IX_categories_parent_id] ON [dbo].[categories];
DROP INDEX [IX_categories_detail_language] ON [dbo].[categories_detail];
DROP INDEX [IX_products_category_id] ON [dbo].[products];
DROP INDEX [IX_products_detail_language] ON [dbo].[products_detail];
DROP INDEX [IX_product_reviews_customer_id] ON [dbo].[product_reviews];
DROP INDEX [IX_product_reviews_product_id] ON [dbo].[product_reviews];

/* CHANGE COLLATIONS */
ALTER TABLE [dbo].[additional_services] ALTER COLUMN [product_code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[additional_services_detail] ALTER COLUMN [account_code] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[additional_services_detail] ALTER COLUMN [name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[administrators] ALTER COLUMN [admin_password] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[administrators] ALTER COLUMN [admin_role] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[administrators] ALTER COLUMN [admin_user_name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[campaigns] ALTER COLUMN [category_name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[campaigns] ALTER COLUMN [image_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[campaigns] ALTER COLUMN [link_url] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[campaigns] ALTER COLUMN [name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[categories] ALTER COLUMN [meta_robots] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[categories_detail] ALTER COLUMN [main_content] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[categories_detail] ALTER COLUMN [meta_description] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[categories_detail] ALTER COLUMN [meta_keywords] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[categories_detail] ALTER COLUMN [page_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[categories_detail] ALTER COLUMN [title] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[companies] ALTER COLUMN [email] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[companies] ALTER COLUMN [mobile_phone_number] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[companies] ALTER COLUMN [name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[companies] ALTER COLUMN [org_number] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[companies] ALTER COLUMN [phone_number] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[companies] ALTER COLUMN [post_address_1] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[companies] ALTER COLUMN [post_address_2] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[companies] ALTER COLUMN [post_city] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[companies] ALTER COLUMN [post_code] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[companies] ALTER COLUMN [post_country] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[companies] ALTER COLUMN [registered_office] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[companies] ALTER COLUMN [vat_number] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[countries] ALTER COLUMN [country_code] [nchar] (2) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[countries_detail] ALTER COLUMN [name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[currencies] ALTER COLUMN [currency_code] [nchar] (3) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[custom_themes] ALTER COLUMN [name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[custom_themes_templates] ALTER COLUMN [comment] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[custom_themes_templates] ALTER COLUMN [master_file_url] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[custom_themes_templates] ALTER COLUMN [user_file_content] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[custom_themes_templates] ALTER COLUMN [user_file_name] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [contact_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [customer_password] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [delivery_address_1] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [delivery_address_2] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [delivery_city] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [delivery_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [delivery_post_code] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [email] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [facebook_user_id] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [google_user_id] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [invoice_address_1] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [invoice_address_2] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [invoice_city] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [invoice_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [invoice_post_code] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [mobile_phone_number] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [org_number] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [phone_number] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[customers] ALTER COLUMN [vat_number] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[discount_codes] ALTER COLUMN [currency_code] [nchar] (3) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[discount_codes] ALTER COLUMN [id] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[gift_cards] ALTER COLUMN [currency_code] [nchar] (3) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[gift_cards] ALTER COLUMN [id] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[inspiration_image_maps] ALTER COLUMN [image_map_points] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[inspiration_image_maps] ALTER COLUMN [image_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[inspiration_image_maps] ALTER COLUMN [name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[languages] ALTER COLUMN [country_code] [nchar] (2) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[languages] ALTER COLUMN [language_code] [nchar] (2) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[languages_detail] ALTER COLUMN [name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[newsletters] ALTER COLUMN [content] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[newsletters] ALTER COLUMN [title] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[option_types] ALTER COLUMN [google_name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[option_types_detail] ALTER COLUMN [title] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[options] ALTER COLUMN [product_code_suffix] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[options_detail] ALTER COLUMN [title] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[order_rows] ALTER COLUMN [account_code] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[order_rows] ALTER COLUMN [gtin] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[order_rows] ALTER COLUMN [manufacturer_code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[order_rows] ALTER COLUMN [product_code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[order_rows] ALTER COLUMN [product_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[order_rows] ALTER COLUMN [supplier_erp_id] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [country_code] [nchar] (2) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [currency_code] [nchar] (3) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [customer_email] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [customer_mobile_phone] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [customer_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [customer_org_number] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [customer_phone] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [customer_vat_number] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [delivery_address_1] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [delivery_address_2] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [delivery_city] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [delivery_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [delivery_post_code] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [discount_code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [invoice_address_1] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [invoice_address_2] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [invoice_city] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [invoice_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [invoice_post_code] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [language_code] [nchar] (2) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [order_status] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [payment_status] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders] ALTER COLUMN [payment_token] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[orders_gift_cards] ALTER COLUMN [gift_card_id] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[payment_options] ALTER COLUMN [payment_term_code] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[payment_options] ALTER COLUMN [product_code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[payment_options_detail] ALTER COLUMN [account_code] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[payment_options_detail] ALTER COLUMN [name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[product_options] ALTER COLUMN [mpn_suffix] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[product_reviews] ALTER COLUMN [review_text] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products] ALTER COLUMN [age_group] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products] ALTER COLUMN [brand] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products] ALTER COLUMN [condition] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products] ALTER COLUMN [energy_efficiency_class] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products] ALTER COLUMN [gender] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products] ALTER COLUMN [gtin] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products] ALTER COLUMN [manufacturer_code] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products] ALTER COLUMN [meta_robots] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products] ALTER COLUMN [product_code] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products] ALTER COLUMN [supplier_erp_id] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products] ALTER COLUMN [variant_image_filename] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products_detail] ALTER COLUMN [account_code] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products_detail] ALTER COLUMN [affiliate_link] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products_detail] ALTER COLUMN [availability_status] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products_detail] ALTER COLUMN [delivery_time] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products_detail] ALTER COLUMN [extra_content] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products_detail] ALTER COLUMN [google_category] [nvarchar] (300) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products_detail] ALTER COLUMN [main_content] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products_detail] ALTER COLUMN [meta_description] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products_detail] ALTER COLUMN [meta_keywords] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products_detail] ALTER COLUMN [page_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products_detail] ALTER COLUMN [size_system] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products_detail] ALTER COLUMN [size_type] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[products_detail] ALTER COLUMN [title] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages] ALTER COLUMN [meta_robots] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages_detail] ALTER COLUMN [link_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages_detail] ALTER COLUMN [main_content] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages_detail] ALTER COLUMN [meta_description] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages_detail] ALTER COLUMN [meta_keywords] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages_detail] ALTER COLUMN [page_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages_detail] ALTER COLUMN [title] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_texts] ALTER COLUMN [id] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_texts] ALTER COLUMN [value] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[units] ALTER COLUMN [erp_code] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[units] ALTER COLUMN [unit_code_si] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[units_detail] ALTER COLUMN [name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[units_detail] ALTER COLUMN [unit_code] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [analytics_tracking_id] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [currency] [nchar] (3) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [domain_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [facebook_app_id] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [facebook_app_secret] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [google_app_id] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [google_app_secret] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [web_address] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [webshop_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[webshop_sessions] ALTER COLUMN [application_name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[webshop_sessions] ALTER COLUMN [id] [nvarchar] (80) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[webshop_sessions] ALTER COLUMN [session_items] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[webshop_settings] ALTER COLUMN [id] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[webshop_settings] ALTER COLUMN [value] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;

/* CREATE PRIMARY KEYS */
ALTER TABLE [dbo].[currencies] ADD CONSTRAINT [PK_currencies] PRIMARY KEY CLUSTERED ([currency_code] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[custom_themes_templates] ADD CONSTRAINT [PK_custom_themes_templates] PRIMARY KEY CLUSTERED ([custom_theme_id] ASC, [user_file_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[discount_codes] ADD CONSTRAINT [PK_discount_codes] PRIMARY KEY CLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[gift_cards] ADD CONSTRAINT [PK_gift_cards] PRIMARY KEY CLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[order_rows] ADD CONSTRAINT [PK_order_rows] PRIMARY KEY CLUSTERED ([order_id] ASC, [product_code] ASC, [sort_order] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[orders_gift_cards] ADD CONSTRAINT [PK_orders_gift_cards] PRIMARY KEY CLUSTERED ([order_id] ASC, [gift_card_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[static_texts] ADD CONSTRAINT [PK_static_texts] PRIMARY KEY CLUSTERED ([id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[webshop_sessions] ADD CONSTRAINT [PK_webshop_sessions] PRIMARY KEY CLUSTERED ([id] ASC, [application_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[webshop_settings] ADD CONSTRAINT [PK_webshop_settings] PRIMARY KEY CLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);

/* CREATE FOREIGN KEYS */
ALTER TABLE [dbo].[discount_codes] WITH CHECK ADD CONSTRAINT [FK_discount_codes_currencies] FOREIGN KEY([currency_code]) REFERENCES [dbo].[currencies] ([currency_code]);
ALTER TABLE [dbo].[discount_codes] CHECK CONSTRAINT [FK_discount_codes_currencies];
ALTER TABLE [dbo].[gift_cards] WITH CHECK ADD CONSTRAINT [FK_gift_cards_currencies] FOREIGN KEY([currency_code]) REFERENCES [dbo].[currencies] ([currency_code]);
ALTER TABLE [dbo].[gift_cards] CHECK CONSTRAINT [FK_gift_cards_currencies];
ALTER TABLE [dbo].[web_domains] WITH CHECK ADD CONSTRAINT [FK_web_domains_currencies] FOREIGN KEY([currency]) REFERENCES [dbo].[currencies] ([currency_code]);
ALTER TABLE [dbo].[web_domains] CHECK CONSTRAINT [FK_web_domains_currencies];

/* CREATE UNIQUE KEYS */
ALTER TABLE [dbo].[administrators] ADD CONSTRAINT [UK_administrators_user_name] UNIQUE NONCLUSTERED ([admin_user_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[categories_detail] ADD CONSTRAINT [UK_categories_detail_page_name] UNIQUE NONCLUSTERED ([page_name] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[customers] ADD CONSTRAINT [UK_customers_email] UNIQUE NONCLUSTERED ([email] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[products_detail] ADD CONSTRAINT [UK_products_detail_page_name] UNIQUE NONCLUSTERED ([page_name] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[static_pages_detail] ADD CONSTRAINT [UK_static_pages_detail_page_name] UNIQUE NONCLUSTERED ([page_name] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[web_domains] ADD CONSTRAINT [UK_web_domains_domain_name] UNIQUE NONCLUSTERED ([domain_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);

/* CREATE INDEXES */
CREATE NONCLUSTERED INDEX [IX_campaigns_language] ON [dbo].[campaigns] ([language_id] ASC, [category_name] ASC, [inactive] ASC);
CREATE NONCLUSTERED INDEX [IX_customers_org_number] ON [dbo].[customers] ([org_number] ASC);
CREATE NONCLUSTERED INDEX [IX_customers_facebook_id] ON [dbo].[customers] ([facebook_user_id] ASC);
CREATE NONCLUSTERED INDEX [IX_customers_google_id] ON [dbo].[customers] ([google_user_id] ASC);
CREATE NONCLUSTERED INDEX [IX_orders_document_type] ON [dbo].[orders] ([document_type] ASC, [country_code] ASC);
CREATE NONCLUSTERED INDEX [IX_orders_exported_to_erp] ON [dbo].[orders] ([exported_to_erp] ASC, [order_status] ASC, [company_id] ASC);
CREATE NONCLUSTERED INDEX [IX_orders_discount_code] ON [dbo].[orders] ([discount_code] ASC);
CREATE NONCLUSTERED INDEX [IX_orders_gift_cards_gift_card_id] ON [dbo].[orders_gift_cards] ([gift_card_id] ASC);
CREATE NONCLUSTERED INDEX [IX_categories_parent_id] ON [dbo].[categories] ([parent_category_id] ASC) INCLUDE ([id], [date_added], [page_views]);
CREATE NONCLUSTERED INDEX [IX_categories_detail_language] ON [dbo].[categories_detail] ([language_id] ASC, [inactive] ASC) INCLUDE ([title]);
CREATE NONCLUSTERED INDEX [IX_products_category_id] ON [dbo].[products] ([category_id] ASC) INCLUDE ([id], [unit_price], [page_views], [buys], [added_in_basket], [date_added]);
CREATE NONCLUSTERED INDEX [IX_products_detail_language] ON [dbo].[products_detail] ([language_id] ASC, [inactive] ASC) INCLUDE ([title], [rating]);
CREATE NONCLUSTERED INDEX [IX_product_reviews_customer_id] ON [dbo].[product_reviews] ([customer_id] ASC, [product_id] ASC, [id] ASC) INCLUDE ([language_id], [review_date], [rating]);
CREATE NONCLUSTERED INDEX [IX_product_reviews_product_id] ON [dbo].[product_reviews] ([product_id] ASC, [language_id] ASC) INCLUDE ([id], [customer_id], [review_date], [rating]);

/* REBUILD INDEXES */
ALTER INDEX ALL ON [dbo].[additional_services] REBUILD;
ALTER INDEX ALL ON [dbo].[additional_services_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[administrators] REBUILD;
ALTER INDEX ALL ON [dbo].[campaigns] REBUILD;
ALTER INDEX ALL ON [dbo].[categories] REBUILD;
ALTER INDEX ALL ON [dbo].[categories_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[companies] REBUILD;
ALTER INDEX ALL ON [dbo].[countries] REBUILD;
ALTER INDEX ALL ON [dbo].[countries_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[currencies] REBUILD;
ALTER INDEX ALL ON [dbo].[custom_themes] REBUILD;
ALTER INDEX ALL ON [dbo].[custom_themes_templates] REBUILD;
ALTER INDEX ALL ON [dbo].[customers] REBUILD;
ALTER INDEX ALL ON [dbo].[customers_files] REBUILD;
ALTER INDEX ALL ON [dbo].[discount_codes] REBUILD;
ALTER INDEX ALL ON [dbo].[gift_cards] REBUILD;
ALTER INDEX ALL ON [dbo].[inspiration_image_maps] REBUILD;
ALTER INDEX ALL ON [dbo].[languages] REBUILD;
ALTER INDEX ALL ON [dbo].[languages_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[newsletters] REBUILD;
ALTER INDEX ALL ON [dbo].[option_types] REBUILD;
ALTER INDEX ALL ON [dbo].[option_types_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[options] REBUILD;
ALTER INDEX ALL ON [dbo].[options_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[order_rows] REBUILD;
ALTER INDEX ALL ON [dbo].[orders] REBUILD;
ALTER INDEX ALL ON [dbo].[orders_gift_cards] REBUILD;
ALTER INDEX ALL ON [dbo].[payment_options] REBUILD;
ALTER INDEX ALL ON [dbo].[payment_options_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[product_accessories] REBUILD;
ALTER INDEX ALL ON [dbo].[product_bundles] REBUILD;
ALTER INDEX ALL ON [dbo].[product_option_types] REBUILD;
ALTER INDEX ALL ON [dbo].[product_options] REBUILD;
ALTER INDEX ALL ON [dbo].[product_reviews] REBUILD;
ALTER INDEX ALL ON [dbo].[products] REBUILD;
ALTER INDEX ALL ON [dbo].[products_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[static_pages] REBUILD;
ALTER INDEX ALL ON [dbo].[static_pages_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[static_texts] REBUILD;
ALTER INDEX ALL ON [dbo].[units] REBUILD;
ALTER INDEX ALL ON [dbo].[units_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[value_added_taxes] REBUILD;
ALTER INDEX ALL ON [dbo].[web_domains] REBUILD;
ALTER INDEX ALL ON [dbo].[webshop_sessions] REBUILD;
ALTER INDEX ALL ON [dbo].[webshop_settings] REBUILD;

/* EXCECUTE THE TRANSACTION */
COMMIT TRANSACTION;