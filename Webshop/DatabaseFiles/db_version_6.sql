BEGIN TRANSACTION;

/* STATIC TEXTS */
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'spin_image_360' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('spin_image_360',1,'360 graders bild')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'spin_image_360' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('spin_image_360',2,'360 degree image')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'inspiration' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('inspiration',1,'Inspiration')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'inspiration' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('inspiration',2,'Inspiration')
END;

IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'image_map' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('image_map',1,'Bildkarta')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'image_map' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('image_map',2,'Image map')
END;

/* INSPIRATION IMAGE MAPS */
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[inspiration_image_maps](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[language_id] [int] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[image_name] [nvarchar](100) NOT NULL,
	[image_map_points] [nvarchar](max) NOT NULL,
	[category_id] [int] NOT NULL);

/* CREATE PRIMARY KEYS */
ALTER TABLE [dbo].[inspiration_image_maps] ADD CONSTRAINT [PK_inspiration_image_maps] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);

/* CREATE CLUSTERED INDEXES */
CREATE CLUSTERED INDEX [CDX_inspiration_image_maps] ON [dbo].[inspiration_image_maps] ([language_id] ASC);

/* CREATE FOREIGN KEYS */
ALTER TABLE [dbo].[inspiration_image_maps] WITH CHECK ADD CONSTRAINT [FK_inspiration_image_maps_categories] FOREIGN KEY([category_id]) REFERENCES [dbo].[categories] ([id]);
ALTER TABLE [dbo].[inspiration_image_maps] CHECK CONSTRAINT [FK_inspiration_image_maps_categories];

/* EXCECUTE THE TRANSACTION */
COMMIT;