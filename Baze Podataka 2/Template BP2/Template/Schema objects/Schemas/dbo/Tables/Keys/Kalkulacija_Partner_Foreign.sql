﻿ALTER TABLE Kalkulacija ADD CONSTRAINT Kalkulacija_Partner_FK FOREIGN KEY ( Partner_SIF_PART ) REFERENCES Partner ( SIF_PART ) ;