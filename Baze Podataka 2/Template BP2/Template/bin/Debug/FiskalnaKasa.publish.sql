﻿/*
Deployment script for FiskalnaKasa

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "FiskalnaKasa"
:setvar DefaultFilePrefix "FiskalnaKasa"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET PAGE_VERIFY NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
PRINT N'Creating [dbo].[Artikal]...';


GO
CREATE TABLE [dbo].[Artikal] (
    [SIF_ART]                 INT          NOT NULL,
    [Naziv]                   VARCHAR (30) NOT NULL,
    [Cena]                    INT          NOT NULL,
    [Min_Kolicina]            INT          NOT NULL,
    [Kolicina]                INT          NOT NULL,
    [BARCODE]                 VARCHAR (30) NOT NULL,
    [JedinicaMere_SIF_JEDMER] INT          NOT NULL,
    [Tarifa_SIF_TAR]          INT          NOT NULL,
    [Grupa_SIF_GRP]           INT          NOT NULL,
    CONSTRAINT [Artikal_PK] PRIMARY KEY CLUSTERED ([SIF_ART] ASC)
);


GO
PRINT N'Creating [dbo].[Dobavljanje]...';


GO
CREATE TABLE [dbo].[Dobavljanje] (
    [Kolicina]              INT NOT NULL,
    [Cena_Dobavljaca]       INT NOT NULL,
    [Marza_Procenat]        INT NOT NULL,
    [Marza_Vrednost]        INT NOT NULL,
    [Porez_Vrednost]        INT NOT NULL,
    [Maloprodajna_Vrednost] INT NOT NULL,
    [Maloprodajna_Cena]     INT NOT NULL,
    [Artikal_SIF_ART]       INT NOT NULL,
    [Kalkulacija_SIF_KALK]  INT NOT NULL,
    CONSTRAINT [Dobavljanje_PK] PRIMARY KEY CLUSTERED ([Artikal_SIF_ART] ASC, [Kalkulacija_SIF_KALK] ASC)
);


GO
PRINT N'Creating [dbo].[Grupa]...';


GO
CREATE TABLE [dbo].[Grupa] (
    [SIF_GRP]     INT          NOT NULL,
    [Naziv_Grupe] VARCHAR (30) NOT NULL,
    CONSTRAINT [Grupa_PK] PRIMARY KEY CLUSTERED ([SIF_GRP] ASC)
);


GO
PRINT N'Creating [dbo].[JedinicaMere]...';


GO
CREATE TABLE [dbo].[JedinicaMere] (
    [SIF_JEDMER]         INT          NOT NULL,
    [Naziv_JediniceMere] VARCHAR (30) NOT NULL,
    CONSTRAINT [JedinicaMere_PK] PRIMARY KEY CLUSTERED ([SIF_JEDMER] ASC)
);


GO
PRINT N'Creating [dbo].[Kalkulacija]...';


GO
CREATE TABLE [dbo].[Kalkulacija] (
    [SIF_KALK]          INT  NOT NULL,
    [Datum_Kalkulacije] DATE NOT NULL,
    [Partner_SIF_PART]  INT  NOT NULL,
    [Radnik_SIF_RAD]    INT  NOT NULL,
    CONSTRAINT [Kalkulacija_PK] PRIMARY KEY CLUSTERED ([SIF_KALK] ASC)
);


GO
PRINT N'Creating [dbo].[Kasa]...';


GO
CREATE TABLE [dbo].[Kasa] (
    [SIF_KAS]       INT           NOT NULL,
    [Opis_Kase]     VARCHAR (200) NOT NULL,
    [Datum_Servisa] DATE          NOT NULL,
    CONSTRAINT [Kasa_PK] PRIMARY KEY CLUSTERED ([SIF_KAS] ASC)
);


GO
PRINT N'Creating [dbo].[Partner]...';


GO
CREATE TABLE [dbo].[Partner] (
    [SIF_PART]     INT          NOT NULL,
    [Ime_Partnera] VARCHAR (30) NOT NULL,
    [Adresa]       VARCHAR (70) NULL,
    [Telefon]      VARCHAR (20) NULL,
    [Ziro_Racun]   VARCHAR (40) NOT NULL,
    CONSTRAINT [Partner_PK] PRIMARY KEY CLUSTERED ([SIF_PART] ASC)
);


GO
PRINT N'Creating [dbo].[Placanje]...';


GO
CREATE TABLE [dbo].[Placanje] (
    [VrstaPlacanja_SIF_PLC] INT NOT NULL,
    [Racun_SIF_RAC]         INT NOT NULL,
    CONSTRAINT [Placanje_PK] PRIMARY KEY CLUSTERED ([VrstaPlacanja_SIF_PLC] ASC, [Racun_SIF_RAC] ASC)
);


GO
PRINT N'Creating [dbo].[Prodato]...';


GO
CREATE TABLE [dbo].[Prodato] (
    [Racun_SIF_RAC]               INT NOT NULL,
    [Dobavljanje_Artikal_SIF_ART] INT NOT NULL,
    [Dobavljanje_Kalk_SIF_KALK]   INT NOT NULL,
    CONSTRAINT [Prodato_PK] PRIMARY KEY CLUSTERED ([Racun_SIF_RAC] ASC, [Dobavljanje_Artikal_SIF_ART] ASC, [Dobavljanje_Kalk_SIF_KALK] ASC)
);


GO
PRINT N'Creating [dbo].[Racun]...';


GO
CREATE TABLE [dbo].[Racun] (
    [SIF_RAC]                  INT  NOT NULL,
    [Datum_Izd]                DATE NOT NULL,
    [Zaduzenje_Kasa_SIF_KAS]   INT  NOT NULL,
    [Zaduzenje_Radnik_SIF_RAD] INT  NOT NULL,
    CONSTRAINT [Racun_PK] PRIMARY KEY CLUSTERED ([SIF_RAC] ASC)
);


GO
PRINT N'Creating [dbo].[Radnik]...';


GO
CREATE TABLE [dbo].[Radnik] (
    [SIF_RAD]  INT          NOT NULL,
    [Ime]      VARCHAR (20) NOT NULL,
    [Prezime]  VARCHAR (20) NOT NULL,
    [Username] VARCHAR (20) NOT NULL,
    [Password] VARCHAR (20) NOT NULL,
    CONSTRAINT [Radnik_PK] PRIMARY KEY CLUSTERED ([SIF_RAD] ASC)
);


GO
PRINT N'Creating [dbo].[Tarifa]...';


GO
CREATE TABLE [dbo].[Tarifa] (
    [SIF_TAR]     INT           NOT NULL,
    [Opis_Tarife] VARCHAR (100) NOT NULL,
    [Stopa]       FLOAT (53)    NOT NULL,
    CONSTRAINT [Tarifa_PK] PRIMARY KEY CLUSTERED ([SIF_TAR] ASC)
);


GO
PRINT N'Creating [dbo].[VrstaPlacanja]...';


GO
CREATE TABLE [dbo].[VrstaPlacanja] (
    [SIF_PLC]        INT          NOT NULL,
    [Naziv_Placanja] VARCHAR (20) NOT NULL,
    CONSTRAINT [VrstaPlacanja_PK] PRIMARY KEY CLUSTERED ([SIF_PLC] ASC)
);


GO
PRINT N'Creating [dbo].[Zaduzenje]...';


GO
CREATE TABLE [dbo].[Zaduzenje] (
    [Kasa_SIF_KAS]   INT NOT NULL,
    [Radnik_SIF_RAD] INT NOT NULL,
    CONSTRAINT [Zaduzenje_PK] PRIMARY KEY CLUSTERED ([Kasa_SIF_KAS] ASC, [Radnik_SIF_RAD] ASC)
);


GO
PRINT N'Creating [dbo].[Artikal_Grupa_FK]...';


GO
ALTER TABLE [dbo].[Artikal] WITH NOCHECK
    ADD CONSTRAINT [Artikal_Grupa_FK] FOREIGN KEY ([Grupa_SIF_GRP]) REFERENCES [dbo].[Grupa] ([SIF_GRP]);


GO
PRINT N'Creating [dbo].[Artikal_JedinicaMere_FK]...';


GO
ALTER TABLE [dbo].[Artikal] WITH NOCHECK
    ADD CONSTRAINT [Artikal_JedinicaMere_FK] FOREIGN KEY ([JedinicaMere_SIF_JEDMER]) REFERENCES [dbo].[JedinicaMere] ([SIF_JEDMER]);


GO
PRINT N'Creating [dbo].[Artikal_Tarifa_FK]...';


GO
ALTER TABLE [dbo].[Artikal] WITH NOCHECK
    ADD CONSTRAINT [Artikal_Tarifa_FK] FOREIGN KEY ([Tarifa_SIF_TAR]) REFERENCES [dbo].[Tarifa] ([SIF_TAR]);


GO
PRINT N'Creating [dbo].[Dobavljanje_Artikal_FK]...';


GO
ALTER TABLE [dbo].[Dobavljanje] WITH NOCHECK
    ADD CONSTRAINT [Dobavljanje_Artikal_FK] FOREIGN KEY ([Artikal_SIF_ART]) REFERENCES [dbo].[Artikal] ([SIF_ART]);


GO
PRINT N'Creating [dbo].[Dobavljanje_Kalkulacija_FK]...';


GO
ALTER TABLE [dbo].[Dobavljanje] WITH NOCHECK
    ADD CONSTRAINT [Dobavljanje_Kalkulacija_FK] FOREIGN KEY ([Kalkulacija_SIF_KALK]) REFERENCES [dbo].[Kalkulacija] ([SIF_KALK]);


GO
PRINT N'Creating [dbo].[Kalkulacija_Partner_FK]...';


GO
ALTER TABLE [dbo].[Kalkulacija] WITH NOCHECK
    ADD CONSTRAINT [Kalkulacija_Partner_FK] FOREIGN KEY ([Partner_SIF_PART]) REFERENCES [dbo].[Partner] ([SIF_PART]);


GO
PRINT N'Creating [dbo].[Kalkulacija_Radnik_FK]...';


GO
ALTER TABLE [dbo].[Kalkulacija] WITH NOCHECK
    ADD CONSTRAINT [Kalkulacija_Radnik_FK] FOREIGN KEY ([Radnik_SIF_RAD]) REFERENCES [dbo].[Radnik] ([SIF_RAD]);


GO
PRINT N'Creating [dbo].[Placanje_VrstaPlacanja_FK]...';


GO
ALTER TABLE [dbo].[Placanje] WITH NOCHECK
    ADD CONSTRAINT [Placanje_VrstaPlacanja_FK] FOREIGN KEY ([VrstaPlacanja_SIF_PLC]) REFERENCES [dbo].[VrstaPlacanja] ([SIF_PLC]);


GO
PRINT N'Creating [dbo].[Placanje_Racun_FK]...';


GO
ALTER TABLE [dbo].[Placanje] WITH NOCHECK
    ADD CONSTRAINT [Placanje_Racun_FK] FOREIGN KEY ([Racun_SIF_RAC]) REFERENCES [dbo].[Racun] ([SIF_RAC]);


GO
PRINT N'Creating [dbo].[Prodato_Racun_FK]...';


GO
ALTER TABLE [dbo].[Prodato] WITH NOCHECK
    ADD CONSTRAINT [Prodato_Racun_FK] FOREIGN KEY ([Racun_SIF_RAC]) REFERENCES [dbo].[Racun] ([SIF_RAC]);


GO
PRINT N'Creating [dbo].[Prodato_Dobavljanje_FK]...';


GO
ALTER TABLE [dbo].[Prodato] WITH NOCHECK
    ADD CONSTRAINT [Prodato_Dobavljanje_FK] FOREIGN KEY ([Dobavljanje_Artikal_SIF_ART], [Dobavljanje_Kalk_SIF_KALK]) REFERENCES [dbo].[Dobavljanje] ([Artikal_SIF_ART], [Kalkulacija_SIF_KALK]);


GO
PRINT N'Creating [dbo].[Racun_Zaduzenje_FK]...';


GO
ALTER TABLE [dbo].[Racun] WITH NOCHECK
    ADD CONSTRAINT [Racun_Zaduzenje_FK] FOREIGN KEY ([Zaduzenje_Kasa_SIF_KAS], [Zaduzenje_Radnik_SIF_RAD]) REFERENCES [dbo].[Zaduzenje] ([Kasa_SIF_KAS], [Radnik_SIF_RAD]);


GO
PRINT N'Creating [dbo].[Zaduženje_Kasa_FK]...';


GO
ALTER TABLE [dbo].[Zaduzenje] WITH NOCHECK
    ADD CONSTRAINT [Zaduženje_Kasa_FK] FOREIGN KEY ([Kasa_SIF_KAS]) REFERENCES [dbo].[Kasa] ([SIF_KAS]);


GO
PRINT N'Creating [dbo].[Zaduženje_Radnik_FK]...';


GO
ALTER TABLE [dbo].[Zaduzenje] WITH NOCHECK
    ADD CONSTRAINT [Zaduženje_Radnik_FK] FOREIGN KEY ([Radnik_SIF_RAD]) REFERENCES [dbo].[Radnik] ([SIF_RAD]);


GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[Artikal] WITH CHECK CHECK CONSTRAINT [Artikal_Grupa_FK];

ALTER TABLE [dbo].[Artikal] WITH CHECK CHECK CONSTRAINT [Artikal_JedinicaMere_FK];

ALTER TABLE [dbo].[Artikal] WITH CHECK CHECK CONSTRAINT [Artikal_Tarifa_FK];

ALTER TABLE [dbo].[Dobavljanje] WITH CHECK CHECK CONSTRAINT [Dobavljanje_Artikal_FK];

ALTER TABLE [dbo].[Dobavljanje] WITH CHECK CHECK CONSTRAINT [Dobavljanje_Kalkulacija_FK];

ALTER TABLE [dbo].[Kalkulacija] WITH CHECK CHECK CONSTRAINT [Kalkulacija_Partner_FK];

ALTER TABLE [dbo].[Kalkulacija] WITH CHECK CHECK CONSTRAINT [Kalkulacija_Radnik_FK];

ALTER TABLE [dbo].[Placanje] WITH CHECK CHECK CONSTRAINT [Placanje_VrstaPlacanja_FK];

ALTER TABLE [dbo].[Placanje] WITH CHECK CHECK CONSTRAINT [Placanje_Racun_FK];

ALTER TABLE [dbo].[Prodato] WITH CHECK CHECK CONSTRAINT [Prodato_Racun_FK];

ALTER TABLE [dbo].[Prodato] WITH CHECK CHECK CONSTRAINT [Prodato_Dobavljanje_FK];

ALTER TABLE [dbo].[Racun] WITH CHECK CHECK CONSTRAINT [Racun_Zaduzenje_FK];

ALTER TABLE [dbo].[Zaduzenje] WITH CHECK CHECK CONSTRAINT [Zaduženje_Kasa_FK];

ALTER TABLE [dbo].[Zaduzenje] WITH CHECK CHECK CONSTRAINT [Zaduženje_Radnik_FK];


GO
PRINT N'Update complete.';


GO