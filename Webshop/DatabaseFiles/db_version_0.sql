BEGIN TRANSACTION;

/* CREATE TABLES */
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[additional_services]([id] [int] IDENTITY(1,1) NOT NULL, 
	[product_code] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL, 
	[fee] [decimal](12,2) NOT NULL, 
	[unit_id] [int] NOT NULL, 
	[price_based_on_mount_time] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[additional_services_detail](
	[additional_service_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[value_added_tax_id] [int] NOT NULL, 
	[account_code] [nvarchar](10) COLLATE Latin1_General_CI_AI NOT NULL,
	[inactive] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
 CREATE TABLE [dbo].[administrators](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[admin_user_name] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[admin_password] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[admin_role] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[campaigns](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[language_id] [int] NOT NULL,
	[name] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[category_name] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[image_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[link_url] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[inactive] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent_category_id] [int] NOT NULL,
	[meta_robots] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[date_added] [datetime] NOT NULL,
	[page_views] [int] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[categories_detail](
	[category_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[title] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[main_content] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[meta_description] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[meta_keywords] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[page_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[use_local_images] [tinyint] NOT NULL,
	[inactive] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[companies](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[registered_office] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[org_number] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[vat_number] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[phone_number] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[mobile_phone_number] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[email] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[post_address_1] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[post_address_2] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[post_code] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[post_city] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[post_country] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[countries](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[country_code] [nchar](2) COLLATE Latin1_General_CI_AI NOT NULL,
	[vat_code] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[countries_detail](
	[country_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[name] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[currencies](
	[currency_code] [nchar](3) COLLATE Latin1_General_CI_AI NOT NULL,
	[conversion_rate] [decimal](10,6) NOT NULL,
	[currency_base] [smallint] NOT NULL,
	[decimals] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[custom_themes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[custom_themes_templates](
	[custom_theme_id] [int] NOT NULL,
	[user_file_name] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[master_file_url] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[user_file_content] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[comment] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[customers_files](
	[customer_id] [int] NOT NULL,
	[product_id] [int] NOT NULL, 
	[language_id] [int] NOT NULL, 
	[order_date] [datetime] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[customers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[language_id] [int] NOT NULL,
	[email] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[customer_password] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[customer_type] [tinyint] NOT NULL,
	[org_number] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[vat_number] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[contact_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[phone_number] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[mobile_phone_number] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[invoice_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[invoice_address_1] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[invoice_address_2] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[invoice_post_code] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[invoice_city] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[invoice_country] [int] NOT NULL,
	[delivery_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[delivery_address_1] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[delivery_address_2] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[delivery_post_code] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[delivery_city] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[delivery_country] [int] NOT NULL,
	[newsletter] [tinyint] NOT NULL,
	[facebook_user_id] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[google_user_id] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[languages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[language_code] [nchar](2) COLLATE Latin1_General_CI_AI NOT NULL,
	[country_code] [nchar](2) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[languages_detail](
	[language_id] [int] NOT NULL,
	[translation_language_id] [int] NOT NULL,
	[name] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[newsletters](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[language_id] [int] NOT NULL,
	[title] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[content] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[sent_date] [datetime] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[option_types](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[google_name] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[option_count] [int] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[option_types_detail](
	[option_type_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[title] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[options](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[product_code_suffix] [nvarchar](10) COLLATE Latin1_General_CI_AI NOT NULL,
	[sort_order] [smallint] NOT NULL,
	[option_type_id] [int] NOT NULL);
 
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[options_detail](
	[option_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[title] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[order_rows](
	[order_id] [int] NOT NULL,
	[product_code] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[manufacturer_code] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[product_id] [int] NOT NULL,
	[gtin] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[product_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[vat_percent] [decimal](6,5) NOT NULL,
	[quantity] [decimal](8,2) NOT NULL,
	[unit_id] [int] NOT NULL,
	[unit_price] [decimal](12,2) NOT NULL,
	[account_code] [nvarchar](10) COLLATE Latin1_General_CI_AI NOT NULL,
	[supplier_erp_id] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[sort_order] [smallint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[orders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[document_type] [tinyint] NOT NULL,
	[order_date] [datetime] NOT NULL,
	[company_id] [int] NOT NULL,
	[country_code] [nchar](2) COLLATE Latin1_General_CI_AI NOT NULL,
	[language_code] [nchar](2) COLLATE Latin1_General_CI_AI NOT NULL,
	[currency_code] [nchar](3) COLLATE Latin1_General_CI_AI NOT NULL,
	[conversion_rate] [decimal](10,6) NOT NULL,
	[customer_id] [int] NOT NULL,
	[customer_type] [tinyint] NOT NULL,
	[customer_org_number] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[customer_vat_number] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[customer_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[customer_phone] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[customer_mobile_phone] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[customer_email] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[invoice_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[invoice_address_1] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[invoice_address_2] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[invoice_post_code] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[invoice_city] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[invoice_country_id] [int] NOT NULL,
	[delivery_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[delivery_address_1] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[delivery_address_2] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[delivery_post_code] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[delivery_city] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[delivery_country_id] [int] NOT NULL,
	[net_sum] [decimal](14,2) NOT NULL,
	[vat_sum] [decimal](14,2) NOT NULL,
	[rounding_sum] [decimal](6,3) NOT NULL,
	[total_sum] [decimal](14,2) NOT NULL,
	[vat_code] [tinyint] NOT NULL,
	[payment_option] [int] NOT NULL,
	[payment_token] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[payment_status] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[exported_to_erp] [tinyint] NOT NULL,
	[order_status] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[desired_date_of_delivery] [datetime] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[payment_options](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[product_code] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[payment_term_code] [nvarchar](10) COLLATE Latin1_General_CI_AI NOT NULL,
	[fee] [decimal](12,2) NOT NULL,
	[unit_id] [int] NOT NULL,
	[connection] [int] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[payment_options_detail](
	[payment_option_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[value_added_tax_id] [int] NOT NULL,
	[account_code] [nvarchar](10) COLLATE Latin1_General_CI_AI NOT NULL,
	[inactive] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[product_accessories](
	[product_id] [int] NOT NULL,
	[accessory_id] [int] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[product_bundles](
	[bundle_product_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[quantity] [decimal](8,2) NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[product_option_types](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[product_id] [int] NOT NULL,
	[option_type_id] [int] NOT NULL,
	[sort_order] [smallint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[product_options](
	[product_option_type_id] [int] NOT NULL,
	[option_id] [int] NOT NULL,
	[mpn_suffix] [nvarchar](10) COLLATE Latin1_General_CI_AI NOT NULL,
	[price_addition] [decimal](12,2) NOT NULL,
	[freight_addition] [decimal](12,2) NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[product_reviews](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[product_id] [int] NOT NULL,
	[customer_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[review_date] [datetime] NOT NULL,
	[review_text] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[rating] [decimal](8,2) NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[products](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[product_code] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[manufacturer_code] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[gtin] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[unit_price] [decimal](12,2) NOT NULL,
	[unit_freight] [decimal](12,2) NOT NULL,
	[unit_id] [int] NOT NULL,
	[mount_time_hours] [decimal](6,2) NOT NULL,
	[from_price] [tinyint] NOT NULL,
	[category_id] [int] NOT NULL,
	[brand] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[supplier_erp_id] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[meta_robots] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[page_views] [int] NOT NULL,
	[buys] [decimal](12,2) NOT NULL,
	[added_in_basket] [int] NOT NULL,
	[condition] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[variant_image_filename] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[gender] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[age_group] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[adult_only] [tinyint] NOT NULL,
	[unit_pricing_measure] [decimal](10,5) NOT NULL,
	[unit_pricing_base_measure] [int] NOT NULL,
	[energy_efficiency_class] [nvarchar](10) COLLATE Latin1_General_CI_AI NOT NULL,
	[downloadable_files] [tinyint] NOT NULL,
	[date_added] [datetime] NOT NULL);
 
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[products_detail](
	[product_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[title] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[main_content] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[extra_content] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[meta_description] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[meta_keywords] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[page_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[delivery_time] [nvarchar] (50) COLLATE Latin1_General_CI_AI NOT NULL,
	[affiliate_link] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[rating] [decimal](8,2) NOT NULL,
	[toll_freight_addition] [decimal](12,2) NOT NULL,
	[value_added_tax_id] [int] NOT NULL,
	[account_code] [nvarchar](10) COLLATE Latin1_General_CI_AI NOT NULL,
	[google_category] [nvarchar](300) COLLATE Latin1_General_CI_AI NOT NULL,
	[use_local_images] [tinyint] NOT NULL,
	[use_local_files] [tinyint] NOT NULL,
	[availability_status] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[availability_date] [datetime] NOT NULL,
	[size_type] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL,
	[size_system] [nvarchar](10) COLLATE Latin1_General_CI_AI NOT NULL,
	[inactive] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[static_pages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[connected_to_page] [tinyint] NOT NULL,
	[meta_robots] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[static_pages_detail](
	[static_page_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[link_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[title] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[main_content] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[meta_description] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[meta_keywords] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[page_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[inactive] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[static_texts](
	[id] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[language_id] [int] NOT NULL,
	[value] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[units](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[unit_code_si] [nvarchar](10) COLLATE Latin1_General_CI_AI NOT NULL,
	[erp_code] [nvarchar](10) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[units_detail](
	[unit_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[unit_code] [nvarchar](10) COLLATE Latin1_General_CI_AI NOT NULL,
	[name] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[value_added_taxes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[value] [decimal](6,5) NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[web_domains](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[webshop_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[domain_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[web_address] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[country_id] [int] NOT NULL,
	[front_end_language] [int] NOT NULL,
	[back_end_language] [int] NOT NULL,
	[currency] [nchar](3) COLLATE Latin1_General_CI_AI NOT NULL,
	[company_id] [int] NOT NULL,
	[default_display_view] [tinyint] NOT NULL,
	[mobile_display_view] [tinyint] NOT NULL,
	[custom_theme_id] [int] NOT NULL,
	[prices_includes_vat] [tinyint] NOT NULL,
	[analytics_tracking_id] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[facebook_app_id] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[facebook_app_secret] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[google_app_id] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[google_app_secret] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[webshop_sessions](
	[id] [nvarchar](80) COLLATE Latin1_General_CI_AI NOT NULL,
	[application_name] [nvarchar](255) COLLATE Latin1_General_CI_AI NOT NULL,
	[created_date] [datetime] NOT NULL,
	[expires_date] [datetime] NOT NULL,
	[lock_date] [datetime] NOT NULL,
	[lock_id] [int] NOT NULL,
	[timeout_limit] [int] NOT NULL,
	[locked] [tinyint] NOT NULL,
	[session_items] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[flags] [int] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[webshop_settings](
	[id] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[value] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL);

/* CREATE PRIMARY KEYS */
ALTER TABLE [dbo].[additional_services] ADD CONSTRAINT [PK_additional_services] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[additional_services_detail] ADD CONSTRAINT [PK_additional_services_detail] PRIMARY KEY NONCLUSTERED ([additional_service_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[administrators] ADD CONSTRAINT [PK_administrators] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[campaigns] ADD CONSTRAINT [PK_campaigns] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[categories] ADD CONSTRAINT [PK_categories] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[categories_detail] ADD CONSTRAINT [PK_categories_detail] PRIMARY KEY NONCLUSTERED ([category_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[companies] ADD CONSTRAINT [PK_companies] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[countries] ADD CONSTRAINT [PK_countries] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[countries_detail] ADD CONSTRAINT [PK_countries_detail] PRIMARY KEY NONCLUSTERED ([country_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[currencies] ADD CONSTRAINT [PK_currencies] PRIMARY KEY NONCLUSTERED ([currency_code] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[custom_themes] ADD CONSTRAINT [PK_custom_themes] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[custom_themes_templates] ADD CONSTRAINT [PK_custom_themes_templates] PRIMARY KEY NONCLUSTERED ([custom_theme_id] ASC, [user_file_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[customers_files] ADD CONSTRAINT [PK_customers_downloadable_files] PRIMARY KEY NONCLUSTERED ([customer_id] ASC, [product_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[customers] ADD CONSTRAINT [PK_customers] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[languages] ADD CONSTRAINT [PK_languages] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[languages_detail] ADD CONSTRAINT [PK_languages_detail] PRIMARY KEY NONCLUSTERED ([language_id] ASC, [translation_language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[option_types] ADD CONSTRAINT [PK_option_types] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[option_types_detail] ADD CONSTRAINT [PK_option_types_detail] PRIMARY KEY NONCLUSTERED ([option_type_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[options] ADD CONSTRAINT [PK_options] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[options_detail] ADD CONSTRAINT [PK_options_detail] PRIMARY KEY NONCLUSTERED ([option_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[order_rows] ADD CONSTRAINT [PK_order_rows] PRIMARY KEY NONCLUSTERED ([order_id] ASC, [product_code] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[orders] ADD CONSTRAINT [PK_orders] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[newsletters] ADD CONSTRAINT [PK_newsletters] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[payment_options] ADD CONSTRAINT [PK_payment_options] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[payment_options_detail] ADD CONSTRAINT [PK_payment_options_detail] PRIMARY KEY NONCLUSTERED ([payment_option_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[product_accessories] ADD CONSTRAINT [PK_product_accessories] PRIMARY KEY NONCLUSTERED ([product_id] ASC, [accessory_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[product_bundles] ADD CONSTRAINT [PK_product_bundles] PRIMARY KEY NONCLUSTERED ([bundle_product_id] ASC, [product_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[product_option_types] ADD CONSTRAINT [PK_product_option_types] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[product_options] ADD CONSTRAINT [PK_product_options] PRIMARY KEY NONCLUSTERED ([product_option_type_id] ASC, [option_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[product_reviews] ADD CONSTRAINT [PK_product_reviews] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[products] ADD CONSTRAINT [PK_products] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[products_detail] ADD CONSTRAINT [PK_products_detail] PRIMARY KEY NONCLUSTERED ([product_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[static_pages] ADD CONSTRAINT [PK_static_pages] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[static_pages_detail] ADD CONSTRAINT [PK_static_pages_detail] PRIMARY KEY NONCLUSTERED ([static_page_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[static_texts] ADD CONSTRAINT [PK_static_texts] PRIMARY KEY NONCLUSTERED ([id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[units] ADD CONSTRAINT [PK_units] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[units_detail] ADD CONSTRAINT [PK_units_detail] PRIMARY KEY NONCLUSTERED ([unit_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[value_added_taxes] ADD CONSTRAINT [PK_value_added_taxes] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[web_domains] ADD CONSTRAINT [PK_web_domains] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[webshop_sessions] ADD CONSTRAINT [PK_webshop_sessions] PRIMARY KEY NONCLUSTERED ([id] ASC, [application_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[webshop_settings] ADD CONSTRAINT [PK_webshop_settings] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);

/* CREATE CLUSTERED INDEXES */
CREATE CLUSTERED INDEX [CDX_additional_services] ON [dbo].[additional_services] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_additional_services_detail] ON [dbo].[additional_services_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_administrators] ON [dbo].[administrators] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_campaigns] ON [dbo].[campaigns] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_categories] ON [dbo].[categories] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_categories_detail] ON [dbo].[categories_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_companies] ON [dbo].[companies] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_countries] ON [dbo].[countries] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_countries_detail] ON [dbo].[countries_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_currencies] ON [dbo].[currencies] ([currency_code] ASC);
CREATE CLUSTERED INDEX [CDX_custom_themes] ON [dbo].[custom_themes] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_custom_themes_templates] ON [dbo].[custom_themes_templates] ([custom_theme_id] ASC);
CREATE CLUSTERED INDEX [CDX_customers_files] ON [dbo].[customers_files] ([customer_id] ASC);
CREATE CLUSTERED INDEX [CDX_customers] ON [dbo].[customers] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_languages] ON [dbo].[languages] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_languages_detail] ON [dbo].[languages_detail] ([translation_language_id] ASC);
CREATE CLUSTERED INDEX [CDX_option_types] ON [dbo].[option_types] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_option_types_detail] ON [dbo].[option_types_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_options] ON [dbo].[options] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_options_detail] ON [dbo].[options_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_order_rows] ON [dbo].[order_rows] ([order_id] ASC);
CREATE CLUSTERED INDEX [CDX_orders] ON [dbo].[orders] ([country_code] ASC);
CREATE CLUSTERED INDEX [CDX_newsletters] ON [dbo].[newsletters] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_payment_options] ON [dbo].[payment_options] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_payment_options_detail] ON [dbo].[payment_options_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_product_accessories] ON [dbo].[product_accessories] ([product_id] ASC);
CREATE CLUSTERED INDEX [CDX_product_bundles] ON [dbo].[product_bundles] ([bundle_product_id] ASC);
CREATE CLUSTERED INDEX [CDX_product_option_types] ON [dbo].[product_option_types] ([product_id] ASC);
CREATE CLUSTERED INDEX [CDX_product_options] ON [dbo].[product_options] ([product_option_type_id] ASC);
CREATE CLUSTERED INDEX [CDX_product_reviews] ON [dbo].[product_reviews] ([product_id] ASC);
CREATE CLUSTERED INDEX [CDX_products] ON [dbo].[products] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_products_detail] ON [dbo].[products_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_static_pages] ON [dbo].[static_pages] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_static_pages_detail] ON [dbo].[static_pages_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_static_texts] ON [dbo].[static_texts] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_units] ON [dbo].[units] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_units_detail] ON [dbo].[units_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_value_added_taxes] ON [dbo].[value_added_taxes] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_web_domains] ON [dbo].[web_domains] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_webshop_sessions] ON [dbo].[webshop_sessions] ([application_name] ASC);
CREATE CLUSTERED INDEX [CDX_webshop_settings] ON [dbo].[webshop_settings] ([id] ASC);

/* CREATE UNIQUE KEYS */
ALTER TABLE [dbo].[administrators] ADD CONSTRAINT [UK_administrators_user_name] UNIQUE NONCLUSTERED ([admin_user_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[categories_detail] ADD CONSTRAINT [UK_categories_detail_page_name] UNIQUE NONCLUSTERED ([page_name] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[customers] ADD CONSTRAINT [UK_customers_email] UNIQUE NONCLUSTERED ([email] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[products_detail] ADD CONSTRAINT [UK_products_detail_page_name] UNIQUE NONCLUSTERED ([page_name] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[static_pages_detail] ADD CONSTRAINT [UK_static_pages_detail_page_name] UNIQUE NONCLUSTERED ([page_name] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[web_domains] ADD CONSTRAINT [UK_web_domains_domain_name] UNIQUE NONCLUSTERED ([domain_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);

/* CREATE DEFAULT VALUES */
ALTER TABLE [dbo].[administrators] ADD CONSTRAINT [DF_administrators_password]  DEFAULT (N'1000:BS3ZEKeB3ZOuI1LL:dSYvveOPOyJgAtKb') FOR [admin_password];
ALTER TABLE [dbo].[currencies] ADD CONSTRAINT [DF_currencies_conversion_rate]  DEFAULT ((1)) FOR [conversion_rate];
ALTER TABLE [dbo].[currencies] ADD CONSTRAINT [DF_currencies_currency_base]  DEFAULT ((1)) FOR [currency_base];

/* CREATE FOREIGN KEYS */
ALTER TABLE [dbo].[additional_services] WITH CHECK ADD CONSTRAINT [FK_additional_services_units] FOREIGN KEY([unit_id]) REFERENCES [dbo].[units] ([id]);
ALTER TABLE [dbo].[additional_services] CHECK CONSTRAINT [FK_additional_services_units];
ALTER TABLE [dbo].[additional_services_detail] WITH CHECK ADD CONSTRAINT [FK_additional_services_detail_additional_services] FOREIGN KEY([additional_service_id]) REFERENCES [dbo].[additional_services] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[additional_services_detail] CHECK CONSTRAINT [FK_additional_services_detail_additional_services];
ALTER TABLE [dbo].[additional_services_detail] WITH CHECK ADD CONSTRAINT [FK_additional_services_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[additional_services_detail] CHECK CONSTRAINT [FK_additional_services_detail_languages];
ALTER TABLE [dbo].[additional_services_detail] WITH CHECK ADD CONSTRAINT [FK_additional_services_detail_value_added_taxes] FOREIGN KEY([value_added_tax_id]) REFERENCES [dbo].[value_added_taxes] ([id]);
ALTER TABLE [dbo].[additional_services_detail] CHECK CONSTRAINT [FK_additional_services_detail_value_added_taxes];
ALTER TABLE [dbo].[campaigns] WITH CHECK ADD CONSTRAINT [FK_campaigns_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[campaigns] CHECK CONSTRAINT [FK_campaigns_languages];
ALTER TABLE [dbo].[categories_detail] WITH CHECK ADD CONSTRAINT [FK_categories_detail_categories] FOREIGN KEY([category_id]) REFERENCES [dbo].[categories] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[categories_detail] CHECK CONSTRAINT [FK_categories_detail_categories];
ALTER TABLE [dbo].[categories_detail] WITH CHECK ADD CONSTRAINT [FK_categories_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[categories_detail] CHECK CONSTRAINT [FK_categories_detail_languages];
ALTER TABLE [dbo].[countries_detail] WITH CHECK ADD CONSTRAINT [FK_countries_detail_countries] FOREIGN KEY([country_id]) REFERENCES [dbo].[countries] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[countries_detail] CHECK CONSTRAINT [FK_countries_detail_countries];
ALTER TABLE [dbo].[countries_detail] WITH CHECK ADD CONSTRAINT [FK_countries_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[countries_detail] CHECK CONSTRAINT [FK_countries_detail_languages];
ALTER TABLE [dbo].[custom_themes_templates] WITH CHECK ADD CONSTRAINT [FK_custom_themes_templates_custom_themes] FOREIGN KEY([custom_theme_id]) REFERENCES [dbo].[custom_themes] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[custom_themes_templates] CHECK CONSTRAINT [FK_custom_themes_templates_custom_themes];
ALTER TABLE [dbo].[customers] WITH CHECK ADD CONSTRAINT [FK_customers_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[customers] CHECK CONSTRAINT [FK_customers_languages];
ALTER TABLE [dbo].[customers] WITH CHECK ADD CONSTRAINT [FK_customers_countries] FOREIGN KEY([delivery_country]) REFERENCES [dbo].[countries] ([id]);
ALTER TABLE [dbo].[customers] CHECK CONSTRAINT [FK_customers_countries];
ALTER TABLE [dbo].[customers] WITH CHECK ADD CONSTRAINT [FK_customers_invoice_countries] FOREIGN KEY([invoice_country]) REFERENCES [dbo].[countries] ([id]);
ALTER TABLE [dbo].[customers] CHECK CONSTRAINT [FK_customers_invoice_countries];
ALTER TABLE [dbo].[customers_files] WITH CHECK ADD CONSTRAINT [FK_customers_files_customers] FOREIGN KEY([customer_id]) REFERENCES [dbo].[customers] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[customers_files] CHECK CONSTRAINT [FK_customers_files_customers];
ALTER TABLE [dbo].[customers_files] WITH CHECK ADD CONSTRAINT [FK_customers_files_products] FOREIGN KEY([product_id]) REFERENCES [dbo].[products] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[customers_files] CHECK CONSTRAINT [FK_customers_files_products];
ALTER TABLE [dbo].[customers_files] WITH CHECK ADD CONSTRAINT [FK_customers_files_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[customers_files] CHECK CONSTRAINT [FK_customers_files_languages];
ALTER TABLE [dbo].[languages_detail] WITH CHECK ADD CONSTRAINT [FK_languages_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[languages_detail] CHECK CONSTRAINT [FK_languages_detail_languages];
ALTER TABLE [dbo].[languages_detail] WITH CHECK ADD CONSTRAINT [FK_languages_detail_translation_languages] FOREIGN KEY([translation_language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[languages_detail] CHECK CONSTRAINT [FK_languages_detail_translation_languages];
ALTER TABLE [dbo].[option_types_detail] WITH CHECK ADD CONSTRAINT [FK_option_types_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[option_types_detail] CHECK CONSTRAINT [FK_option_types_detail_languages];
ALTER TABLE [dbo].[option_types_detail] WITH CHECK ADD CONSTRAINT [FK_option_types_detail_option_types] FOREIGN KEY([option_type_id]) REFERENCES [dbo].[option_types] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[option_types_detail] CHECK CONSTRAINT [FK_option_types_detail_option_types];
ALTER TABLE [dbo].[options] WITH CHECK ADD CONSTRAINT [FK_options_option_types] FOREIGN KEY([option_type_id]) REFERENCES [dbo].[option_types] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[options] CHECK CONSTRAINT [FK_options_option_types];
ALTER TABLE [dbo].[options_detail] WITH CHECK ADD CONSTRAINT [FK_options_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[options_detail] CHECK CONSTRAINT [FK_options_detail_languages];
ALTER TABLE [dbo].[options_detail] WITH CHECK ADD CONSTRAINT [FK_options_detail_options] FOREIGN KEY([option_id]) REFERENCES [dbo].[options] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[options_detail] CHECK CONSTRAINT [FK_options_detail_options];
ALTER TABLE [dbo].[order_rows] WITH CHECK ADD CONSTRAINT [FK_order_rows_orders] FOREIGN KEY([order_id]) REFERENCES [dbo].[orders] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[order_rows] CHECK CONSTRAINT [FK_order_rows_orders];
ALTER TABLE [dbo].[order_rows] WITH CHECK ADD CONSTRAINT [FK_order_rows_units] FOREIGN KEY([unit_id]) REFERENCES [dbo].[units] ([id]);
ALTER TABLE [dbo].[order_rows] CHECK CONSTRAINT [FK_order_rows_units];
ALTER TABLE [dbo].[orders] WITH CHECK ADD CONSTRAINT [FK_orders_customers] FOREIGN KEY([customer_id]) REFERENCES [dbo].[customers] ([id]);
ALTER TABLE [dbo].[orders] CHECK CONSTRAINT [FK_orders_customers];
ALTER TABLE [dbo].[orders] WITH CHECK ADD CONSTRAINT [FK_orders_delivery_countries] FOREIGN KEY([delivery_country_id]) REFERENCES [dbo].[countries] ([id]);
ALTER TABLE [dbo].[orders] CHECK CONSTRAINT [FK_orders_delivery_countries];
ALTER TABLE [dbo].[orders] WITH CHECK ADD CONSTRAINT [FK_orders_invoice_countries] FOREIGN KEY([invoice_country_id]) REFERENCES [dbo].[countries] ([id]);
ALTER TABLE [dbo].[orders] CHECK CONSTRAINT [FK_orders_invoice_countries];
ALTER TABLE [dbo].[orders] WITH CHECK ADD CONSTRAINT [FK_orders_payment_options] FOREIGN KEY([payment_option]) REFERENCES [dbo].[payment_options] ([id]);
ALTER TABLE [dbo].[orders] CHECK CONSTRAINT [FK_orders_payment_options];
ALTER TABLE [dbo].[orders] WITH CHECK ADD CONSTRAINT [FK_orders_companies] FOREIGN KEY([company_id]) REFERENCES [dbo].[companies] ([id]);
ALTER TABLE [dbo].[orders] CHECK CONSTRAINT [FK_orders_companies];
ALTER TABLE [dbo].[newsletters] WITH CHECK ADD CONSTRAINT [FK_newsletters_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[newsletters] CHECK CONSTRAINT [FK_newsletters_languages];
ALTER TABLE [dbo].[payment_options] WITH CHECK ADD CONSTRAINT [FK_payment_options_units] FOREIGN KEY([unit_id]) REFERENCES [dbo].[units] ([id]);
ALTER TABLE [dbo].[payment_options] CHECK CONSTRAINT [FK_payment_options_units];
ALTER TABLE [dbo].[payment_options_detail] WITH CHECK ADD CONSTRAINT [FK_payment_options_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[payment_options_detail] CHECK CONSTRAINT [FK_payment_options_detail_languages];
ALTER TABLE [dbo].[payment_options_detail] WITH CHECK ADD CONSTRAINT [FK_payment_options_detail_payment_options] FOREIGN KEY([payment_option_id]) REFERENCES [dbo].[payment_options] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[payment_options_detail] CHECK CONSTRAINT [FK_payment_options_detail_payment_options];
ALTER TABLE [dbo].[payment_options_detail] WITH CHECK ADD CONSTRAINT [FK_payment_options_detail_value_added_taxes] FOREIGN KEY([value_added_tax_id]) REFERENCES [dbo].[value_added_taxes] ([id]);
ALTER TABLE [dbo].[payment_options_detail] CHECK CONSTRAINT [FK_payment_options_detail_value_added_taxes];
ALTER TABLE [dbo].[product_accessories] WITH CHECK ADD CONSTRAINT [FK_product_accessories_accessory_product] FOREIGN KEY([accessory_id]) REFERENCES [dbo].[products] ([id]);
ALTER TABLE [dbo].[product_accessories] CHECK CONSTRAINT [FK_product_accessories_accessory_product];
ALTER TABLE [dbo].[product_accessories] WITH CHECK ADD CONSTRAINT [FK_product_accessories_products] FOREIGN KEY([product_id]) REFERENCES [dbo].[products] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[product_accessories] CHECK CONSTRAINT [FK_product_accessories_products];
ALTER TABLE [dbo].[product_bundles] WITH CHECK ADD CONSTRAINT [FK_product_bundles_product] FOREIGN KEY([product_id]) REFERENCES [dbo].[products] ([id]);
ALTER TABLE [dbo].[product_bundles] CHECK CONSTRAINT [FK_product_bundles_product];
ALTER TABLE [dbo].[product_bundles] WITH CHECK ADD CONSTRAINT [FK_product_bundles_bundle_product] FOREIGN KEY([bundle_product_id]) REFERENCES [dbo].[products] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[product_bundles] CHECK CONSTRAINT [FK_product_bundles_bundle_product];
ALTER TABLE [dbo].[product_option_types] WITH CHECK ADD CONSTRAINT [FK_product_option_types_option_types] FOREIGN KEY([option_type_id]) REFERENCES [dbo].[option_types] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[product_option_types] CHECK CONSTRAINT [FK_product_option_types_option_types];
ALTER TABLE [dbo].[product_option_types] WITH CHECK ADD CONSTRAINT [FK_product_option_types_products] FOREIGN KEY([product_id]) REFERENCES [dbo].[products] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[product_option_types] CHECK CONSTRAINT [FK_product_option_types_products];
ALTER TABLE [dbo].[product_options] WITH CHECK ADD CONSTRAINT [FK_product_options_options] FOREIGN KEY([option_id]) REFERENCES [dbo].[options] ([id]);
ALTER TABLE [dbo].[product_options] CHECK CONSTRAINT [FK_product_options_options];
ALTER TABLE [dbo].[product_options] WITH CHECK ADD CONSTRAINT [FK_product_options_product_option_types] FOREIGN KEY([product_option_type_id]) REFERENCES [dbo].[product_option_types] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[product_options] CHECK CONSTRAINT [FK_product_options_product_option_types];
ALTER TABLE [dbo].[product_reviews] WITH CHECK ADD CONSTRAINT [FK_product_reviews_customers] FOREIGN KEY([customer_id]) REFERENCES [dbo].[customers] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[product_reviews] CHECK CONSTRAINT [FK_product_reviews_customers];
ALTER TABLE [dbo].[product_reviews] WITH CHECK ADD CONSTRAINT [FK_product_reviews_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[product_reviews] CHECK CONSTRAINT [FK_product_reviews_languages];
ALTER TABLE [dbo].[product_reviews] WITH CHECK ADD CONSTRAINT [FK_product_reviews_products] FOREIGN KEY([product_id]) REFERENCES [dbo].[products] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[product_reviews] CHECK CONSTRAINT [FK_product_reviews_products];
ALTER TABLE [dbo].[products] WITH CHECK ADD CONSTRAINT [FK_products_categories] FOREIGN KEY([category_id]) REFERENCES [dbo].[categories] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[products] CHECK CONSTRAINT [FK_products_categories];
ALTER TABLE [dbo].[products] WITH CHECK ADD CONSTRAINT [FK_products_units] FOREIGN KEY([unit_id]) REFERENCES [dbo].[units] ([id]);
ALTER TABLE [dbo].[products] CHECK CONSTRAINT [FK_products_units];
ALTER TABLE [dbo].[products_detail] WITH CHECK ADD CONSTRAINT [FK_products_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[products_detail] CHECK CONSTRAINT [FK_products_detail_languages];
ALTER TABLE [dbo].[products_detail] WITH CHECK ADD CONSTRAINT [FK_products_detail_products] FOREIGN KEY([product_id]) REFERENCES [dbo].[products] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[products_detail] CHECK CONSTRAINT [FK_products_detail_products];
ALTER TABLE [dbo].[products_detail] WITH CHECK ADD CONSTRAINT [FK_products_detail_value_added_taxes] FOREIGN KEY([value_added_tax_id]) REFERENCES [dbo].[value_added_taxes] ([id]);
ALTER TABLE [dbo].[products_detail] CHECK CONSTRAINT [FK_products_detail_value_added_taxes];
ALTER TABLE [dbo].[static_pages_detail] WITH CHECK ADD CONSTRAINT [FK_static_pages_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[static_pages_detail] CHECK CONSTRAINT [FK_static_pages_detail_languages];
ALTER TABLE [dbo].[static_pages_detail] WITH CHECK ADD CONSTRAINT [FK_static_pages_detail_pages] FOREIGN KEY([static_page_id]) REFERENCES [dbo].[static_pages] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[static_pages_detail] CHECK CONSTRAINT [FK_static_pages_detail_pages];
ALTER TABLE [dbo].[static_texts] WITH CHECK ADD CONSTRAINT [FK_static_texts_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[static_texts] CHECK CONSTRAINT [FK_static_texts_languages];
ALTER TABLE [dbo].[units_detail] WITH CHECK ADD CONSTRAINT [FK_units_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[units_detail] CHECK CONSTRAINT [FK_units_detail_languages];
ALTER TABLE [dbo].[units_detail] WITH CHECK ADD CONSTRAINT [FK_units_detail_units] FOREIGN KEY([unit_id]) REFERENCES [dbo].[units] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[units_detail] CHECK CONSTRAINT [FK_units_detail_units];
ALTER TABLE [dbo].[web_domains] WITH CHECK ADD CONSTRAINT [FK_web_domains_back_end_languages] FOREIGN KEY([back_end_language]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[web_domains] CHECK CONSTRAINT [FK_web_domains_back_end_languages];
ALTER TABLE [dbo].[web_domains] WITH CHECK ADD CONSTRAINT [FK_web_domains_currencies] FOREIGN KEY([currency]) REFERENCES [dbo].[currencies] ([currency_code]);
ALTER TABLE [dbo].[web_domains] CHECK CONSTRAINT [FK_web_domains_currencies];
ALTER TABLE [dbo].[web_domains] WITH CHECK ADD CONSTRAINT [FK_web_domains_front_end_languages] FOREIGN KEY([front_end_language]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[web_domains] CHECK CONSTRAINT [FK_web_domains_front_end_languages];
ALTER TABLE [dbo].[web_domains] WITH CHECK ADD CONSTRAINT [FK_web_domains_countries] FOREIGN KEY([country_id]) REFERENCES [dbo].[countries] ([id]);
ALTER TABLE [dbo].[web_domains] CHECK CONSTRAINT [FK_web_domains_countries];
ALTER TABLE [dbo].[web_domains] WITH CHECK ADD CONSTRAINT [FK_web_domains_companies] FOREIGN KEY([company_id]) REFERENCES [dbo].[companies] ([id]);
ALTER TABLE [dbo].[web_domains] CHECK CONSTRAINT [FK_web_domains_companies];

/* EXCECUTE THE TRANSACTION */
COMMIT;