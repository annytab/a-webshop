BEGIN TRANSACTION;

/* WEBSHOP SETTINGS */
INSERT INTO dbo.webshop_settings (id, value) VALUES ('SEND-EMAIL-USE-SSL','false');
INSERT INTO dbo.webshop_settings (id, value) VALUES ('REDIRECT-HTTPS','false');

/* EXCECUTE THE TRANSACTION */
COMMIT;