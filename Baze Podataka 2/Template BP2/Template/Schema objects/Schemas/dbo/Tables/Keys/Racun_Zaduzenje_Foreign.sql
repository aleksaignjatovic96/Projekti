ALTER TABLE Racun ADD CONSTRAINT Racun_Zaduzenje_FK FOREIGN KEY ( Zaduzenje_Kasa_SIF_KAS, Zaduzenje_Radnik_SIF_RAD ) REFERENCES Zaduzenje ( Kasa_SIF_KAS, Radnik_SIF_RAD ) ;
