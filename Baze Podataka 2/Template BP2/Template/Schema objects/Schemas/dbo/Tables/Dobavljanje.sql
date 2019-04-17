CREATE TABLE [dbo].[Dobavljanje]
(
	Kolicina              INTEGER NOT NULL ,
    Cena_Dobavljaca       INTEGER NOT NULL ,
    Marza_Procenat        INTEGER NOT NULL ,
    Marza_Vrednost        INTEGER NOT NULL ,
    Porez_Vrednost        INTEGER NOT NULL ,
    Maloprodajna_Vrednost INTEGER NOT NULL ,
    Maloprodajna_Cena     INTEGER NOT NULL ,
    Artikal_SIF_ART       INTEGER NOT NULL ,
    Kalkulacija_SIF_KALK  INTEGER NOT NULL
)
