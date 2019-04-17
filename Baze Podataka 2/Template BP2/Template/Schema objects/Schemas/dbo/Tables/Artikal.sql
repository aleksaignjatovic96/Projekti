CREATE TABLE [dbo].[Artikal]
(
	SIF_ART                 INTEGER NOT NULL ,
    Naziv                   VARCHAR(30) NOT NULL ,
    Cena                    INTEGER NOT NULL ,
    Min_Kolicina            INTEGER NOT NULL ,
    Kolicina                INTEGER NOT NULL ,
    BARCODE                 VARCHAR(30) NOT NULL ,
    JedinicaMere_SIF_JEDMER INTEGER NOT NULL ,
    Tarifa_SIF_TAR          INTEGER NOT NULL ,
    Grupa_SIF_GRP           INTEGER NOT NULL
)
