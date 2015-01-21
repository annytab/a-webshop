BEGIN TRANSACTION;

/* STATIC TEXTS */
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'do_not_index' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('do_not_index',1,'Indexera inte')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'do_not_index' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('do_not_index',2,'Do not index')
END;

/* WEB DOMAINS */
ALTER TABLE [dbo].[web_domains] ADD [noindex] [tinyint] NOT NULL DEFAULT(0);

/* EXCECUTE THE TRANSACTION */
COMMIT;