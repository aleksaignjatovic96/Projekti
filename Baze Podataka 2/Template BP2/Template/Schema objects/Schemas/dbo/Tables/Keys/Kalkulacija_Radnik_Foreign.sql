﻿ALTER TABLE Kalkulacija ADD CONSTRAINT Kalkulacija_Radnik_FK FOREIGN KEY ( Radnik_SIF_RAD ) REFERENCES Radnik ( SIF_RAD ) ;