BEGIN TRANSACTION;

/* STATIC TEXTS */
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'comparison_unit' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('comparison_unit',1,'Enhet för jämförelse')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'comparison_unit' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('comparison_unit',2,'Comparison unit')
END;

/* PRODUCTS */
ALTER TABLE [dbo].[products] ADD [comparison_unit_id] [int] NOT NULL DEFAULT(0);

/* EXCECUTE THE TRANSACTION */
COMMIT;