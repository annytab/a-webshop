BEGIN TRANSACTION;

/* STATIC TEXTS */
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'vat_code_intra_union_trade' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('vat_code_intra_union_trade',1,'Unionsintern försäljning')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'vat_code_intra_union_trade' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('vat_code_intra_union_trade',2,'Intra-Union Trade')
END;

/* EXCECUTE THE TRANSACTION */
COMMIT;