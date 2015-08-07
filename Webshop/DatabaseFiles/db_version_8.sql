BEGIN TRANSACTION;

/* COOKIE CONSENT */
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'cookie_consent' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('cookie_consent',1,'Cookies hjälper oss att tillhandahålla våra tjänster. Genom att använda våra tjänster samtycker du till att vi använder cookies. Läs mer i vår sekretesspolicy.')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'cookie_consent' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('cookie_consent',2,'Cookies help us deliver our services. By using our services you agree to our use of cookies. Read more in our Privacy Policy.')
END;

/* MARKDOWN SUPPORT */
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'markdown_supported' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('markdown_supported',1,'Stöd för markdown')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'markdown_supported' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('markdown_supported',2,'Markdown supported')
END;

/* EXCECUTE THE TRANSACTION */
COMMIT;