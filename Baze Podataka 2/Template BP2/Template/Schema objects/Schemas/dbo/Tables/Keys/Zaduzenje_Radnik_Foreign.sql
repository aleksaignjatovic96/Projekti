﻿ALTER TABLE Zaduzenje ADD CONSTRAINT Zaduženje_Radnik_FK FOREIGN KEY ( Radnik_SIF_RAD ) REFERENCES Radnik ( SIF_RAD ) ;