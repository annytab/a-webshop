BEGIN TRANSACTION;

/* DROP FOREIGN KEYS */
ALTER TABLE [dbo].[order_rows] DROP CONSTRAINT [FK_order_rows_orders];
ALTER TABLE [dbo].[order_rows] DROP CONSTRAINT [FK_order_rows_units];

/* DROP PRIMARY KEYS */
ALTER TABLE [dbo].[order_rows] DROP CONSTRAINT [PK_order_rows];

/* CREATE PRIMARY KEYS */
ALTER TABLE [dbo].[order_rows] ADD CONSTRAINT [PK_order_rows] PRIMARY KEY CLUSTERED ([order_id] ASC, [product_code] ASC, [sort_order] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);

/* CREATE FOREIGN KEYS */
ALTER TABLE [dbo].[order_rows] WITH CHECK ADD CONSTRAINT [FK_order_rows_orders] FOREIGN KEY([order_id]) REFERENCES [dbo].[orders] ([id]);
ALTER TABLE [dbo].[order_rows] CHECK CONSTRAINT [FK_order_rows_orders];
ALTER TABLE [dbo].[order_rows] WITH CHECK ADD CONSTRAINT [FK_order_rows_units] FOREIGN KEY([unit_id]) REFERENCES [dbo].[units] ([id]);
ALTER TABLE [dbo].[order_rows] CHECK CONSTRAINT [FK_order_rows_units];

/* EXCECUTE THE TRANSACTION */
COMMIT;