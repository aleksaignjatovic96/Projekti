#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define FAKTORBAKETIRANJA 5
#define BROJBAKETA 9

#define Lokacija_aktivna          1
#define Lokacija_slobodna         0

#define FiksniKorakK              1

//SLOG//
typedef struct slog {
	unsigned int evidencionibroj;
	char nazivkeksa[40];
	char datum[11];
	char vreme[6];
	unsigned int roktrajanja;
	unsigned int tezina;
	char status;
} Keks;

//DATOTEKA//
typedef struct datotekakeks {
	FILE* file;
	char NazivDatoteke[20];
	char Otvorena;
} DatotekaKeks;

//BAKET//
typedef struct baket {
	Keks keks[FAKTORBAKETIRANJA];
	unsigned char SlobodnaMesta;
	unsigned char Prekoracioci;
	unsigned char AdresaBaketa;
} Baket;

DatotekaKeks trenutnoaktivna;

//KLJUC//
int transformacijaKljuca(int kljuc){
	return kljuc%BROJBAKETA + 1;
}

//1//
void formiranjePrazneDatoteke()
{
	FILE* file;
	Baket prazniBaketi[BROJBAKETA];

	unsigned int i, j;
	char NazivDatoteke[20];

	printf("Unesite naziv datoteke: ");
	scanf("%s", NazivDatoteke);

	if ((file = fopen(NazivDatoteke, "wb")) != NULL)
	{
		printf("Datoteka <%s> je kreirana. \n\n", NazivDatoteke);
	}
	else
	{
		printf("Greska pri kreiranju datoteke '%s'. \n\n", NazivDatoteke);
		return;
	}


	// Rezervisanje memorije (staticko)
	for (i = 0; i < BROJBAKETA; i++)
	{
		for (j = 0; j < FAKTORBAKETIRANJA; j++)
		{
			prazniBaketi[i].keks[j].evidencionibroj = 0;
			strcpy(prazniBaketi[i].keks[j].nazivkeksa, "\0");
			strcpy(prazniBaketi[i].keks[j].datum, "\0");
			strcpy(prazniBaketi[i].keks[j].vreme, "\0");
			prazniBaketi[i].keks[j].roktrajanja = 0;
			prazniBaketi[i].keks[j].tezina = 0;
			prazniBaketi[i].keks[j].status = Lokacija_slobodna;
		}
		prazniBaketi[i].AdresaBaketa = i;
		prazniBaketi[i].SlobodnaMesta = FAKTORBAKETIRANJA;
		prazniBaketi[i].Prekoracioci = 0;

		//Snimi u Fajl
		fwrite(&prazniBaketi[i], sizeof(Baket), 1, file);
	}

	fclose(file);
}

//2//
void izboraktivnedatoteke()
{
	if (trenutnoaktivna.Otvorena == 1)
	{
		// Zatvori trenutno otvorenu datoteku
		fclose(trenutnoaktivna.file);
		trenutnoaktivna.Otvorena = 0;
	}

	printf("Unesite naziv datoteke: ");
	scanf("%s", trenutnoaktivna.NazivDatoteke);


	if ((trenutnoaktivna.file = fopen(trenutnoaktivna.NazivDatoteke, "rb+")) == NULL)
	{
		printf("Datoteka <%s> nije pronadjena. \n\n", trenutnoaktivna.NazivDatoteke);
	}
	else
	{
		printf("Datoteka <%s> otvorena. \n\n", trenutnoaktivna.NazivDatoteke);
		trenutnoaktivna.Otvorena = 1;
	}
}

//3//
void prikazuaktivnudatoteku()
{
	if (trenutnoaktivna.Otvorena == 1)
	{
		printf("Trenutno aktivna datoteka: %s \n\n", trenutnoaktivna.NazivDatoteke);
	}
	else
	{
		printf("Nema aktivne datoteke.\n\n");
	}
}

//4//
void dodajNoviArtikal()
{
	Keks qnoviKeks;
	Baket qnoviBaket;
	unsigned short adresaBaketa;
	unsigned int i, j;

	if (!trenutnoaktivna.Otvorena)
	{
		printf("Nije izabrana aktivna datoteka!\n\n");
		return;
	}

	printf("Unesite evidencioni broj: \t\t");
	scanf("%u", &qnoviKeks.evidencionibroj);
	if (qnoviKeks.evidencionibroj > 999999999999)
	{
		printf("\nEvidencioni broj nije ispravan!\n\n");
		return;
	}

	adresaBaketa = transformacijaKljuca(qnoviKeks.evidencionibroj);
	//printf("\n\tmmmmmmmmmm:\t%u\n", adresaBaketa);

	// Provera artikla da li postoji
	///////////////////////////////
	fseek(trenutnoaktivna.file, sizeof(Baket) * adresaBaketa, SEEK_SET);
	fread(&qnoviBaket, sizeof(Baket), 1, trenutnoaktivna.file);

	for (i = 0; i < FAKTORBAKETIRANJA; i++)
	{
		if (qnoviBaket.keks[i].evidencionibroj == qnoviKeks.evidencionibroj)
		{

            if (qnoviBaket.keks[i].status == Lokacija_aktivna)
            {
                printf("\nSlog sa tim evidencionim brojem postoji.\n");
                return;
            }

		}
	}
	/////////////////////////////////


    //Ispitivanje da li je baza puna
	if (qnoviBaket.Prekoracioci > 0)
	{
        int krajTrazenja = 0;

		for (i = 1; i <= BROJBAKETA; ++i)
		{
			unsigned short adresaTrenutnogBaketa = (i * FiksniKorakK + adresaBaketa) % BROJBAKETA;

			Baket qtrenutniBaket;

			fseek(trenutnoaktivna.file, sizeof(Baket) * adresaTrenutnogBaketa, SEEK_SET);
			fread(&qtrenutniBaket, sizeof(Baket), 1, trenutnoaktivna.file);


			for (j = 0; j < FAKTORBAKETIRANJA; ++j)
			{
				if (qtrenutniBaket.keks[j].status == Lokacija_slobodna)
				{
					krajTrazenja = 1;
					break;
				}
				if (qtrenutniBaket.keks[j].evidencionibroj == qnoviKeks.evidencionibroj)
				{
					if (qtrenutniBaket.keks[j].status == Lokacija_aktivna)
					{
						printf("\nSlog sa tim evidencionim brojem postoji.\n");
						return;
					}

				}
			}
			if (krajTrazenja == 1)
			{
				break;
			}
		}
		if (krajTrazenja == 0)
        {
            printf("\nBaza je puna. Obrisite neki artikal.\n");
            return;
        }
	}

	// Unos ostalih polja
	///////////////////////////////////
	printf("Naziv keksa (maksimalno 40 karaktera): \t\t");
	scanf("%s", qnoviKeks.nazivkeksa);
	if (strlen(qnoviKeks.nazivkeksa) > 40)
	{
		printf("\nMozete uneti najvise 40 karaktera! \n\n");
		return;
	}

	printf("Datum proizvodnje (dd.mm.yyyy): \t\t\t");
	scanf("%s", qnoviKeks.datum);

	printf("Vreme proizvodnje (hh:mm): \t\t\t\t");
	scanf("%s", qnoviKeks.vreme);

	printf("Rok trajanja: \t\t");
	scanf("%u", &qnoviKeks.roktrajanja);


	printf("Tezina u gramima: \t\t");
	scanf("%u", &qnoviKeks.tezina);
	/////////////////////////////////////


	// Upis u datoteku
	////////////////////////////////////
	if (qnoviBaket.SlobodnaMesta != 0)
	{
		for (i = 0; i < FAKTORBAKETIRANJA; ++i)
		{
			if (qnoviBaket.keks[i].status == Lokacija_slobodna)
			{
				qnoviBaket.keks[i].evidencionibroj = qnoviKeks.evidencionibroj;
				strcpy( qnoviBaket.keks[i].nazivkeksa, qnoviKeks.nazivkeksa);
				strcpy(qnoviBaket.keks[i].datum, qnoviKeks.datum);
				strcpy(qnoviBaket.keks[i].vreme, qnoviKeks.vreme);
				qnoviBaket.keks[i].roktrajanja = qnoviKeks.roktrajanja;
				qnoviBaket.keks[i].tezina = qnoviKeks.tezina;
				qnoviBaket.keks[i].status = Lokacija_aktivna;

				qnoviBaket.SlobodnaMesta--;

				fseek(trenutnoaktivna.file, sizeof(Baket) * adresaBaketa, SEEK_SET);
				fwrite(&qnoviBaket, sizeof(Baket), 1, trenutnoaktivna.file);

				return;
			}
		}
	}
	///////////////////////////////////////


	// Upis prekoracilaca
	//////////////////////////////////////
	for (i = 1; i <= BROJBAKETA; ++i)
	{
		int adresaTrenutnogBaketa = (i * FiksniKorakK + adresaBaketa) % BROJBAKETA;

		//printf("\n\tggggggggggggggggggggggggg:\t%u\n", adresaTrenutnogBaketa);

		Baket qtrenutniBaket;

		fseek(trenutnoaktivna.file, sizeof(Baket) * adresaTrenutnogBaketa, SEEK_SET);
		fread(&qtrenutniBaket, sizeof(Baket), 1, trenutnoaktivna.file);

		short slogUpisan = 0;

		for (j = 0; j < FAKTORBAKETIRANJA; ++j)
		{
			if (qtrenutniBaket.keks[j].status == Lokacija_slobodna)
			{
				qtrenutniBaket.keks[j].evidencionibroj = qnoviKeks.evidencionibroj;
				strcpy(qtrenutniBaket.keks[j].nazivkeksa, qnoviKeks.nazivkeksa);
				strcpy(qtrenutniBaket.keks[j].datum, qnoviKeks.datum);
				strcpy(qtrenutniBaket.keks[j].vreme, qnoviKeks.vreme);
				qtrenutniBaket.keks[j].roktrajanja = qnoviKeks.roktrajanja;
				qtrenutniBaket.keks[j].tezina = qnoviKeks.tezina;
				qtrenutniBaket.keks[j].status = Lokacija_aktivna;

				fseek(trenutnoaktivna.file, sizeof(Baket) * adresaTrenutnogBaketa, SEEK_SET);
				fwrite(&qtrenutniBaket, sizeof(Baket), 1, trenutnoaktivna.file);

				qnoviBaket.Prekoracioci++;

				fseek(trenutnoaktivna.file, sizeof(Baket) * adresaBaketa, SEEK_SET);
				fwrite(&qnoviBaket, sizeof(Baket), 1, trenutnoaktivna.file);

				slogUpisan = 1;

				break;
			}
		}
		////////////////////////////////////

		if (slogUpisan == 1)
		{
			break;
		}
	}
}

//5//
void listaSvihArtikala()
{
	Baket tempBaket;
	size_t i, j;
	int adresa;

	if (!trenutnoaktivna.Otvorena)
	{
		printf("Morate izabrati aktivnu datoteku!\n\n");
		return;
	}


	rewind(trenutnoaktivna.file);
	for (i = 0; i < BROJBAKETA; i++)
	{
		adresa = i + 1;
		fread(&tempBaket, sizeof(Baket), 1, trenutnoaktivna.file);
		printf("\n----------------------------------------------");
		printf("\n-Baket %d: ", adresa);
		printf("\n-Broj prekoracilaca: %d", tempBaket.Prekoracioci);
		for (j = 0; j < FAKTORBAKETIRANJA; j++)
		{
			if (tempBaket.keks[j].status == Lokacija_aktivna)
			{
				printf("\n\nSlog %d:\n", j + 1);
				printf("\n\tEvidencioni broj:\t%u", tempBaket.keks[j].evidencionibroj);
				printf("\n\tNaziv keksa:\t\t%s", tempBaket.keks[j].nazivkeksa);
				printf("\n\tDatum proizvodnje:\t%s", tempBaket.keks[j].datum);
				printf("\n\tVreme proizvodnje:\t%s", tempBaket.keks[j].vreme);
				printf("\n\tRok trajanja:\t\t%u", tempBaket.keks[j].roktrajanja);
				printf("\n\tNeto tezina:\t\t%u\n", tempBaket.keks[j].tezina);
			}
		}
	}
}

//6//
void zamenaVrednosti()
{

	Keks qKeks;
	Baket qBaket;
	unsigned short adresaBaketa;
	unsigned int i;

	if (!trenutnoaktivna.Otvorena)
	{
		printf("Nije izabrana aktivna datoteka!\n\n");
		return;
	}

	printf("Unesite evidencioni broj: \t\t");
	scanf("%u", &qKeks.evidencionibroj);
	if (qKeks.evidencionibroj > 999999999999)
	{
		printf("\nEvidencioni broj nije ispravan!\n\n");
		return;
	}

	adresaBaketa = transformacijaKljuca(qKeks.evidencionibroj);


	// Izmena keksa
	///////////////////////////////
	fseek(trenutnoaktivna.file, sizeof(Baket) * adresaBaketa, SEEK_SET);
	fread(&qBaket, sizeof(Baket), 1, trenutnoaktivna.file);

	for (i = 0; i < FAKTORBAKETIRANJA; i++)
	{
		if (qBaket.keks[i].evidencionibroj == qKeks.evidencionibroj)
		{

			printf("Datum proizvodnje (dd.mm.yyyy): \t\t\t");
			scanf("%s", qKeks.datum);

			printf("Vreme proizvodnje (hh:mm): \t\t\t\t");
			scanf("%s", qKeks.vreme);


			strcpy(qBaket.keks[i].datum, qKeks.datum);
			strcpy(qBaket.keks[i].vreme, qKeks.vreme);


			fseek(trenutnoaktivna.file, sizeof(Baket) * adresaBaketa, SEEK_SET);
			fwrite(&qBaket, sizeof(Baket), 1, trenutnoaktivna.file);


			return;

		}
	}

	/////////////////////////////////

    int pp = 1;
	//Ako je prekoracioc
    while (qBaket.Prekoracioci > 0 && pp < 9)
	{
	    adresaBaketa = (pp * FiksniKorakK + adresaBaketa) % BROJBAKETA;

	    fseek(trenutnoaktivna.file, sizeof(Baket) * adresaBaketa, SEEK_SET);
        fread(&qBaket, sizeof(Baket), 1, trenutnoaktivna.file);

        for (i = 0; i < FAKTORBAKETIRANJA; i++)
        {
            if (qBaket.keks[i].evidencionibroj == qKeks.evidencionibroj)
            {

                printf("Datum proizvodnje (dd.mm.yyyy): \t\t\t");
                scanf("%s", qKeks.datum);

                printf("Vreme proizvodnje (hh:mm): \t\t\t\t");
                scanf("%s", qKeks.vreme);


                strcpy(qBaket.keks[i].datum, qKeks.datum);
                strcpy(qBaket.keks[i].vreme, qKeks.vreme);


                fseek(trenutnoaktivna.file, sizeof(Baket) * adresaBaketa, SEEK_SET);
                fwrite(&qBaket, sizeof(Baket), 1, trenutnoaktivna.file);


                return;

            }
        }

        pp++;
	}

	printf("\nEvidencioni broj nije ispravan!\n\n");

}

//7//
void brisanjeSloga()
{

	Keks qKeks;
	Baket qBaket;
	unsigned short adresaBaketa;
	unsigned int i;
	int qpredhodni = -1;

	if (!trenutnoaktivna.Otvorena)
	{
		printf("Nije izabrana aktivna datoteka!\n\n");
		return;
	}

	printf("Unesite evidencioni broj: \t\t");
	scanf("%u", &qKeks.evidencionibroj);
	if (qKeks.evidencionibroj > 999999999999)
	{
		printf("\nEvidencioni broj nije ispravan!\n\n");
		return;
	}

	adresaBaketa = transformacijaKljuca(qKeks.evidencionibroj);


	// Brisanje keksa
	///////////////////////////////
	fseek(trenutnoaktivna.file, sizeof(Baket) * adresaBaketa, SEEK_SET);
	fread(&qBaket, sizeof(Baket), 1, trenutnoaktivna.file);

	for (i = 0; i < FAKTORBAKETIRANJA; i++)
	{
		if (qBaket.keks[i].evidencionibroj == qKeks.evidencionibroj)
		{
			qBaket.keks[i].evidencionibroj = 0;
			strcpy(qBaket.keks[i].nazivkeksa, "\0");
			strcpy(qBaket.keks[i].datum, "\0");
			strcpy(qBaket.keks[i].vreme, "\0");
			qBaket.keks[i].roktrajanja = 0;
			qBaket.keks[i].tezina = 0;
			qBaket.keks[i].status = Lokacija_slobodna;

			qBaket.SlobodnaMesta++;

			// Upamti redni broj sloga
			qpredhodni = i;
		}
		else
		{
			if (qpredhodni != -1)
			{

				// Prebaci slog u prethodni
				qBaket.keks[qpredhodni].evidencionibroj = qBaket.keks[i].evidencionibroj;
				strcpy(qBaket.keks[qpredhodni].nazivkeksa, qBaket.keks[i].nazivkeksa);
				strcpy(qBaket.keks[qpredhodni].datum, qBaket.keks[i].datum);
				strcpy(qBaket.keks[qpredhodni].vreme, qBaket.keks[i].vreme);
				qBaket.keks[qpredhodni].roktrajanja = qBaket.keks[i].roktrajanja;
				qBaket.keks[qpredhodni].tezina = qBaket.keks[i].tezina;
				qBaket.keks[qpredhodni].status = qBaket.keks[i].status;

				// Obrisi slog
				qBaket.keks[i].evidencionibroj = 0;
				strcpy(qBaket.keks[i].nazivkeksa, "\0");
				strcpy(qBaket.keks[i].datum, "\0");
				strcpy(qBaket.keks[i].vreme, "\0");
				qBaket.keks[i].roktrajanja = 0;
				qBaket.keks[i].tezina = 0;
				qBaket.keks[i].status = Lokacija_slobodna;

				// Upamti redni broj sloga
				qpredhodni = i;

			}

		}


	}
	/////////////////////////////////

	if (qpredhodni != -1)
	{
		fseek(trenutnoaktivna.file, sizeof(Baket) * adresaBaketa, SEEK_SET);
		fwrite(&qBaket, sizeof(Baket), 1, trenutnoaktivna.file);
	}
	else
    {
        printf("\nEvidencioni broj nije ispravan!\n\n");
    }

}

//Main - MENI//
int main(int argc, char* argv[])
{

	int odg;
    char odgChar[10];

	do
	{
	    printf("\n==================================");
		printf("\n1. Formiranje prazne datoteke ");
		printf("\n2. Izbor aktivne datoteke ");
		printf("\n3. Prikaz aktivne datoteke ");
		printf("\n4. Dodavanje novog artikla ");
		printf("\n5. Lista svih artikala  ");
		printf("\n6. Izmena vrednosti datuma i vremena ");
		printf("\n7. Fizicko brisanje artikla ");
		printf("\n9. Kraj ");
		printf("\n==================================");
		printf("\n\nOdaberite opciju: ");
		scanf("%s", odgChar);
		system("@cls||clear");
		printf("\n");

		odg = atoi(odgChar);
		if (odg == 0)
        {
            printf("Pogresan unos.\n\n");
            continue;
        }

		switch (odg)
		{
		case 1:
			formiranjePrazneDatoteke();
			break;
		case 2:
			izboraktivnedatoteke();
			break;
		case 3:
			prikazuaktivnudatoteku();
			break;
		case 4:
			dodajNoviArtikal();
			break;
		case 5:
			listaSvihArtikala();
			break;
		case 6:
			zamenaVrednosti();
			break;
		case 7:
			brisanjeSloga();
			break;
		case 9:
			break;
		default:
			printf("Pogresan unos.\n\n");
			break;
		}
	} while (odg != 9);

	if (trenutnoaktivna.Otvorena)
		fclose(trenutnoaktivna.file);



	return 0;
}
